using ProjectGraphics;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public int id;
    public float hp;
    public float exp;
    public float bone;
    public float gold;
    public int boneCount;
    public int goldCount;
    public int imageId;
    public int bgId;
    public string stageName;
    public EnumDefinition.MonsterType monsterType;
    public EnumDefinition.AttackType attackType;

    public SpriteLibraryChanged spriteLibraryChanged;
    public MonsterInOutAnimator inOutAnimator;

    void Start()
    {

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.transform.name);
    //}
    public void SetSkinById(int id)
    {
        spriteLibraryChanged.ChangedSpriteAllImage(id);
    }


}

