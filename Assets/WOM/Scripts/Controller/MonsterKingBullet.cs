using UnityEngine;

public class MonsterKingBullet : MonoBehaviour
{

    public MonsterKingController monsterKingController;

    void Start()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GlobalData.instance.attackController.GetAttackableState() == true)
        {
            var tag = collision.tag;
            if (tag.Contains("monster"))
            {
                monsterKingController.HitMonster();
                //Debug.Log("Hit Monster - by monstreKingBullet");
            }
        }
    }

}
