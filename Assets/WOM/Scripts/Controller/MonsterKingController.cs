using System.Collections;
using UnityEngine;


public class MonsterKingController : MonoBehaviour
{

    public AnimationController animController;
    public Transform startPoint;
    public Transform endPoint;
    public Transform trMonsterKing;


    public float hitTimingMin = 0.05f;
    public float hitTimingMax = 0.2f;
    public int totalHitCount = 5;

    void Start()
    {
        AddEvent();
    }

    void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnMonsterKingHit, HitMonster);
    }

    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnMonsterKingHit, HitMonster);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MonsterKingMove();
        }
    }

    public void MonsterKingMove()
    {
        StartCoroutine(animController.AnimPositionEndEvent(startPoint.position, endPoint.position, () =>
        {
            trMonsterKing.gameObject.SetActive(false);
            Debug.Log("MonsterKingMove End");
        }));
    }

    void HitMonster()
    {
        if (animController.isAnimPlay == false)
            StartCoroutine(HitMonsterCot());
    }
    //5 회 가격
    public IEnumerator HitMonsterCot()
    {
        for (int i = 0; i < totalHitCount; i++)
        {
            AttackMonster();
            var waitTime = RaddomHitTiming();
            yield return new WaitForSeconds(waitTime);
        }
    }

    void AttackMonster()
    {

    }

    float RaddomHitTiming()
    {
        // 0.05 초 에서 0.2 초 사이
        return Random.Range(hitTimingMin, hitTimingMax);
    }

}
