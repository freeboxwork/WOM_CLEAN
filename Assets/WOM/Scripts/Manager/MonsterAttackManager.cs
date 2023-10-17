using UnityEngine;

public class MonsterAttackManager : MonoBehaviour
{

    int attackPercent = 10;
    public MonsterBoss monsterBoss;
    float insectKillRadius = 2f;

    void Start()
    {

    }

    public void AttackMotion()
    {
        // attackPercent 의 확률로 공격
        //if (Random.Range(0, 100) < attackPercent)
        if (true)
        {
            // 곤충들 공격 불가능 상태로 전환
            //GlobalData.instance.attackController.SetAttackableState(false);
            // 보스 피격 안되게 전환
            GlobalData.instance.eventController.SetBossMonsterAttackMotion(true);
            monsterBoss.AnimPlayAttack();
        }
    }


    public void KillInsects(Transform tr)
    {
        // tr 반경 insectKillRadius 안에 있는 모든 곤충을 죽인다.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(tr.position, insectKillRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("insectBullet"))
            {
                // try getcomponent
                if (collider.TryGetComponent<InsectBullet>(out var insectBullet))
                {
                    insectBullet.DieInsect();
                }
            }
        }
    }

    public void AttackMotionEnd()
    {
        // 곤충들 공격 가능 상태로 전환
        // GlobalData.instance.attackController.SetAttackableState(true);

        // 보스 피격 가능 상태로 전환
        GlobalData.instance.eventController.SetBossMonsterAttackMotion(false);
    }


}
