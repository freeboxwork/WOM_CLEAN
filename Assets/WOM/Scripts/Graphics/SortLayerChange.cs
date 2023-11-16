using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayerChange : MonoBehaviour
{
    [Header("���̵庸�� ���� : 14,  1 ���� ���")]
    public SpriteRenderer foreGroundSpriteObject;
    [SerializeField] int startOrder = 20;
    //[SerializeField] int finalOrder = 1;
    private int order = 0;

    private void Start()
    {
        foreGroundSpriteObject.sortingOrder = startOrder;
    }

    public void SetChangeSpriteOrder(int i)
    {
        foreGroundSpriteObject.sortingOrder = i;
    }
    
}
