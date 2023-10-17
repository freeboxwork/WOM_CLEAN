using UnityEngine;
public class MonsterBoss : MonsterBase
{
    public Animator animator;
    public Transform attackPoint;

    void Start()
    {

    }
    public void AnimPlayAttack()
    {
        GlobalData.instance.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.BossAttack);

        animator.Play("Attack");
    }


    public void Attack() // <---  animator event
    {
        // kill insect
        GlobalData.instance.monsterAttackManager.KillInsects(attackPoint);
    }

    public void AttackMotionEnd() // <---  animator event
    {
        GlobalData.instance.monsterAttackManager.AttackMotionEnd();
    }


}
