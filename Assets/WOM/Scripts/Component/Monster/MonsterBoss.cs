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
        animator.SetTrigger("Attack");
    }

    public void Attack()
    {
        // kill insect
    }



}
