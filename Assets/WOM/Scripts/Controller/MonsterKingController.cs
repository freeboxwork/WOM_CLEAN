using System.Collections;
using UnityEngine;


public class MonsterKingController : MonoBehaviour
{

    public AnimationController animController;
    public Transform startPoint;
    public Transform endPoint;
    public Transform trMonsterKing;

    public Transform trHitPoint;

    public float hitTimingMin = 0.01f;
    public float hitTimingMax = 0.05f;
    private int totalHitCount = 10;

    void Start()
    {

    }





    // Update is called once per frame
    void Update()
    {

    }

    public void MonsterKingMove()
    {
        trMonsterKing.gameObject.SetActive(true);
        StartCoroutine(animController.AnimPositionEndEvent(startPoint.position, endPoint.position, () =>
        {
            trMonsterKing.gameObject.SetActive(false);
            Debug.Log("MonsterKingMove End");
        }));
    }

    public void HitMonster()
    {
        StartCoroutine(HitMonsterCor());
    }
    //5 회 가격
    public IEnumerator HitMonsterCor()
    {
        for (int i = 0; i < totalHitCount; i++)
        {
            AttackMonster(i);
            var waitTime = RaddomHitTiming();
            yield return new WaitForSeconds(waitTime);
            Debug.Log("HitCount : " + i);
            // SFX PLAY
            GlobalData.instance.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.MonsterHit);
            if (GlobalData.instance.attackController.GetAttackableState() == false)
                yield break;
            if (IsMonsterDead() == true)
                yield break;
        }
    }

    bool IsMonsterDead()
    {
        var hp = GlobalData.instance.player.currentMonster.hp;
        return hp < 0;
    }

    void AttackMonster(int hitIndex)
    {
        // enable Hit Effect pariicle
        GlobalData.instance.effectManager.EnableKingMonsterHitEffect(trHitPoint, hitIndex);
        EventManager.instance.RunEvent<Transform>(CallBackEventType.TYPES.OnMonsterKingHit, trHitPoint);
    }

    float RaddomHitTiming()
    {
        // 0.01초 에서 0.05 초 사이
        return Random.Range(hitTimingMin, hitTimingMax);
    }

}
