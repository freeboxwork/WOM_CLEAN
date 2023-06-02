using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoldAnimController : MonoBehaviour
{
    public AnimData goldInAnimData;
    public AnimData goldOutAnimData;

    // RV : RANDOM VALUE
    float startPointRV = 1f;

    float randomRotMin = 1f;
    float randomRotMax = 720f;
    float randomRotValue;
    Vector3 startRotValue;
    Vector3 targetRotValue;
    Quaternion startRot;
    Quaternion targetRot;

    Vector2 pos_startPoint;
    Vector2 pos_targetPoint;
    float rot_randomValue;

    // 재화 타입
    public EnumDefinition.GoodsType goodsType;


    void Start()
    {

    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    StartCoroutine(GoldOutAnim());
        //}
    }


    public void GoldInAnimStart()
    {
        SetRandomRandomPositions();
        SetRandomRotValue();
        StartCoroutine(GoldInAnim());
    }

    public void GoldOutAnimStart(UnityAction goalEvent)
    {
        StartCoroutine(GoldOutAnim(goalEvent));
    }


    void SetRandomRotValue()
    {
        startRotValue = Vector3.zero;
        randomRotValue = Random.Range(randomRotMin, randomRotMax);
        targetRotValue = new Vector3(0, 0, randomRotValue);

        startRot = Quaternion.Euler(startRotValue);
        targetRot = Quaternion.Euler(targetRotValue);
    }

    void SetRandomRandomPositions()
    {
        var startPoint = GlobalData.instance.effectManager.GetGoldSfxRandomPoint(EnumDefinition.GoldPosType.START_POINT_Y).position;
        var xMin = GlobalData.instance.effectManager.GetGoldSfxRandomPoint(EnumDefinition.GoldPosType.END_POOINT_X_MIN).position;
        var xMax = GlobalData.instance.effectManager.GetGoldSfxRandomPoint(EnumDefinition.GoldPosType.END_POOINT_X_MAX).position;
        var yMin = GlobalData.instance.effectManager.GetGoldSfxRandomPoint(EnumDefinition.GoldPosType.END_POOINT_Y_MIN).position;
        var yMax = GlobalData.instance.effectManager.GetGoldSfxRandomPoint(EnumDefinition.GoldPosType.END_POOINT_Y_MAX).position;

        var startX = UtilityMethod.GetRandomRangeValue_X(startPoint, startPointRV);
        var startY = GlobalData.instance.effectManager.GetGoldSfxRandomPoint(EnumDefinition.GoldPosType.START_POINT_Y).position.y;
        pos_startPoint = new Vector2(startX, startY);
        pos_targetPoint = UtilityMethod.GetTargetRandomRangeValue_V2(xMin, xMax, yMin, yMax);
    }



    IEnumerator GoldInAnim()
    {
        goldInAnimData.ResetAnimData();

        while (goldInAnimData.animValue < 0.99f)
        {
            goldInAnimData.animTime = ((Time.time - goldInAnimData.animStartTime) / goldInAnimData.animDuration);
            goldInAnimData.animValue = EaseValues.instance.GetAnimCurve(goldInAnimData.animCurveType, goldInAnimData.animTime);// EaseValues.instance.EaseOutQuint(0f, 1f, goldInAnimData.animTime);
            transform.position = Vector2.Lerp(pos_startPoint, pos_targetPoint, goldInAnimData.animValue);
            //transform.rotation = Quaternion.Lerp(startRot, targetRot, goldInAnimData.animValue);

            yield return null;
        }
    }


    IEnumerator GoldOutAnim(UnityAction goalEvent)
    {
        goldOutAnimData.ResetAnimData();
        var curPos = transform.position;
        var posType = goodsType == EnumDefinition.GoodsType.gold ? EnumDefinition.GoldPosType.SCREEN_UI_POINT_GOLD : EnumDefinition.GoldPosType.SCREEN_UI_POINT_BONE;
        var uiPoint = GlobalData.instance.effectManager.GetGoldSfxRandomPoint(posType);
        var targetPos = Camera.main.ScreenToWorldPoint(uiPoint.position);
        var centerPos = (curPos + targetPos) * 0.5f;
        float centerRandomX = Random.Range(-2f, 2f);
        centerPos = new Vector2(centerPos.x + centerRandomX, centerPos.y);

        while (goldOutAnimData.animValue < 0.99f)
        {
            goldOutAnimData.animTime = ((Time.time - goldOutAnimData.animStartTime) / goldOutAnimData.animDuration);
            goldOutAnimData.animValue = EaseValues.instance.GetAnimCurve(goldOutAnimData.animCurveType, goldOutAnimData.animTime);// EaseValues.instance.EaseInQuad(0f, 1f, goldOutAnimData.animTime);
            transform.position = CalculateBezierPoint(goldOutAnimData.animValue, curPos, centerPos, targetPos);
            //transform.position = Vector2.Lerp(curPos, targetPos, goldOutAnimData.animValue);
            //transform.rotation = Quaternion.Lerp(targetRot, startRot, goldOutAnimData.animValue);

            

            yield return null;
        }

        goalEvent.Invoke();

        yield return new WaitForEndOfFrame();
        
        gameObject.SetActive(false);
    }

   


    Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var a = Vector2.Lerp(p0, p1, t);
        var b = Vector2.Lerp(p1, p2, t);
        var c = Vector2.Lerp(a, b, t);
        var d = Vector2.Lerp(b, c, t);
        var e = Vector2.Lerp(c, d, t);

        return e;
    }

}
