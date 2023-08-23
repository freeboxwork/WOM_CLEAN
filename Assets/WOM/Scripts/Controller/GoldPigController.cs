using System.Collections;
using UnityEngine;

public class GoldPigController : MonoBehaviour
{

    public GoldPig goldPig;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform radomPointLeft;
    public Transform radomPointRight;
    public AnimData animData;

    public float randomRangeA = 2f;
    public float randomRangeB = 1f;


    Transform[] pointLeftSide;
    Transform[] pointRightSide;

    float startPointYRandVal;
    float endPointYRandVal;

    public LineRenderer lineRenderer;

    public GoldPigPopup goldPigPopup;

    public int[] enableGoldPigRange;

    //현재 골드 피그 재생성이 될때까지 딜레이 시간
    public float currentEnableGoldPigWaitTime;

    float spawnProbability;

    void Start()
    {

    }

    public IEnumerator Init()
    {
        SetPointSides();
        AddEvents();
        if (GlobalData.instance.tutorialManager.isTutorial == false)
            EnableGoldPig();
        yield return null;
    }

    void OnDestroy()
    {
        RemoveEvents();
    }

    void SetPointSides()
    {
        // left
        pointLeftSide = new Transform[] { leftPoint, radomPointLeft, radomPointRight, rightPoint };

        // right
        pointRightSide = new Transform[] { rightPoint, radomPointRight, radomPointLeft, leftPoint };
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // if (Input.GetKeyDown(KeyCode.Space))
    //     // {
    //     //     StartCoroutine(MoveGoldPig());
    //     // }
    // }

    void AddEvents()
    {
        UtilityMethod.SetBtnEventCustomTypeByID(49, () =>
        {
            EnterOtherView();
        });

        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnGoldPigEvent, GetGoldPig);
    }

    void RemoveEvents()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnGoldPigEvent, GetGoldPig);
    }

    // 골드피그 획득
    void GetGoldPig()
    {
        // 애니메이션 종료 ( 코루틴 종료 )
        StopAllCoroutines();
        goldPig.gameObject.SetActive(false);
        goldPigPopup.gameObject.SetActive(true);
    }

    //다른 화면으로 이동 시 
    public void EnterOtherView()
    {
        // 애니메이션 종료 ( 코루틴 종료 )
        StopAllCoroutines();
        goldPig.gameObject.SetActive(false);
    }

    //다른 화면에서 빠져나올 시
    public void ExitOtherView()
    {
        if (currentEnableGoldPigWaitTime <= 0)
        {
            EnableGoldPig();
        }
        else
        {
            StartCoroutine(EnableGoldPig_ExitCastleView());
        }
    }

    // ramdom pos y points
    Vector3[] GetPointValues()
    {
        var points = new Vector3[4];
        Transform[] sidePoints;
        var sideValue = Random.Range(0, 2);
        if (sideValue == 0)
        {
            sidePoints = pointLeftSide;
        }
        else
        {
            sidePoints = pointRightSide;
        }

        points[0] = GetRandomPos(sidePoints[0], randomRangeA);
        points[1] = GetRandomPos(sidePoints[1], randomRangeB);
        points[2] = GetRandomPos(sidePoints[2], randomRangeB);
        points[3] = GetRandomPos(sidePoints[3], randomRangeA);
        return points;
    }

    public IEnumerator MoveGoldPig()
    {
        animData.ResetAnimData();
        var points = GetPointValues();

        goldPig.gameObject.SetActive(true);

        // debug
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }

        while (animData.animValue < 0.99f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            animData.animValue = Mathf.Lerp(0, 1, animData.animTime);
            var pos = CalculateBezierPoint(animData.animValue, points[0], points[1], points[2], points[3]);
            goldPig.transform.position = pos;
            yield return null;
        }

        goldPig.gameObject.SetActive(false);
        EnableGoldPig();
    }

    public void EnableGoldPig()
    {
        StartCoroutine(EnableGoldPigCor());
    }

    //300~500초 사이 등장
    IEnumerator EnableGoldPigCor()
    {

        var spawnGoldPigDelayTime = (float)Random.Range(enableGoldPigRange[0], enableGoldPigRange[1]);
        
        var startTime = Time.time;
        float waitTime = spawnGoldPigDelayTime;
        while (waitTime > 0)
        {
            waitTime = spawnGoldPigDelayTime - (Time.time - startTime);
            currentEnableGoldPigWaitTime = waitTime;
            yield return null;
        }
        currentEnableGoldPigWaitTime = 0;


        //  확률에 따라 등장
        spawnProbability = 40.0f + GlobalData.instance.statManager.GoldPig();
        float randomNumber = Random.Range(0f, 100f);
        if (randomNumber < spawnProbability)
        {
            StartCoroutine(MoveGoldPig());
        }
        else
        {
            Debug.Log("확률에 따라 골드피그 등장하지 않음");
        }

    }


    public IEnumerator EnableGoldPig_ExitCastleView()
    {
        float enableTime = currentEnableGoldPigWaitTime;

        var startTime = Time.time;
        float waitTime = enableTime;
        while (waitTime > 0)
        {
            waitTime = enableTime - (Time.time - startTime);
            currentEnableGoldPigWaitTime = waitTime;
            yield return null;
        }
        currentEnableGoldPigWaitTime = 0;
        StartCoroutine(MoveGoldPig());
    }

    Vector3 GetRandomPos(Transform point, float range)
    {
        var pos = point.position;
        pos.y = GetRandomRange(pos.y, range);
        return pos;
    }


    float GetRandomRange(float yPos, float range)
    {
        return yPos += Random.Range(-range, range);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0; // (1-t)^3 * p0
        point += 3 * uu * t * p1; // 3 * (1-t)^2 * t * p1
        point += 3 * u * tt * p2; // 3 * (1-t) * t^2 * p2
        point += ttt * p3; // t^3 * p3

        return point;
    }

}
