using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayerChange : MonoBehaviour
{
    [Header("레이드보스 전용 : 14,  1 번만 사용")]
    public SpriteRenderer foreGroundSpriteObject;
    [SerializeField] int startOrder = 20;
    //[SerializeField] int finalOrder = 1;
    private int order = 0;

    [Header("0,1 : 꼬리, 2, 3 : 앞다리, 4 : 입")]
    public GameObject[] particleEffects = new GameObject[5];    

    private void Start()
    {
        foreGroundSpriteObject.sortingOrder = startOrder;
    }

    public void SetChangeSpriteOrder(int i)
    {
        foreGroundSpriteObject.sortingOrder = i;
    }

    public void PlayEffectTails()
    {
        particleEffects[0].SetActive(true);
        particleEffects[1].SetActive(true);
    }

    public void PlayEffectHands()
    {
        particleEffects[2].SetActive(true);
        particleEffects[3].SetActive(true);
    }
    
    public void PlayEffectHead()
    {
        particleEffects[4].SetActive(true);
    }
}
