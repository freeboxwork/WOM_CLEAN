using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DNASlot : MonoBehaviour
{

    public EnumDefinition.DNAType DNAType;

    public Image imgDnaFace;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtMaxLevel;
    public TextMeshProUGUI txtInfo;
    public TextMeshProUGUI txtHasCount;

    public DNAInGameData inGameData;

    void Start()
    {
        
    }

    public void SetTxtName(string value)
    {
        txtName.text = value;
    }
    
    public void SetTxtMaxLevel(int vlaue)
    {
        txtMaxLevel.text = $"Max Lv : {vlaue}";
    }

    public void SetTxtInfo(string front ,string color, float power, string back)
    {
        txtInfo.text = $"{front} <{color}> {power}% </color> {back}";
    }

    public void SetTxtHasCount(int level, int maxCount)
    {
        txtHasCount.text = $"{level}/{maxCount}";
    }

    public void SetFace(Sprite sprite)
    {
        imgDnaFace.sprite = sprite;
    }
 
    public void SetDnaType(EnumDefinition.DNAType type)
    {
        DNAType = type;
    }

}
