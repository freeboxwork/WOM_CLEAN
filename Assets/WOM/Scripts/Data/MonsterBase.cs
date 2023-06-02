using ProjectGraphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    public int id;
    public float hp;
    public float exp;
    public int bone;
    public int gold;
    public int boneCount;
    public int goldCount;
    public int imageId;
    public int bgId;
    public EnumDefinition.MonsterType monsterType;
    public EnumDefinition.AttackType attackType;

    public SpriteLibraryChanged spriteLibraryChanged;
    public MonsterInOutAnimator inOutAnimator;

    void Start()
    {

    }
    
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.transform.name);
    //}


}

