using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopSlot : MonoBehaviour
{
    public TextMeshProUGUI txtTitle;
    public Image imgIcon;
    public EnumDefinition.ShopSlotType shopSlotType;

    public void SetTxtTitle( string value)
    {
        txtTitle.text = value;  
    }    

    public void SetImageIcon(Sprite sprite)
    {
        imgIcon.sprite = sprite;    
    }

    public EnumDefinition.ShopSlotType GetShopType()
    {
        return shopSlotType;
    }

}
