using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class LotteryCard : MonoBehaviour
{
    public Image imgCard;
    public TextMeshProUGUI txtCard;
    
    void Start()
    {
        
    }

    public void Effect(EnumDefinition.UnionGradeType unionGradeType)
    {
        Debug.Log(unionGradeType + " Card Open Effect!!!");
    }

    public IEnumerator EffectCor(EnumDefinition.UnionGradeType unionGradeType)
    {
        Debug.Log(unionGradeType + " Card Open Effect!!! - Corrutin");
        yield return null;
    }

    public void SetCardFace(Sprite cardImage)
    {
        imgCard.sprite = cardImage;
    }

    public void SetTxtName(string value)
    {
        txtCard.text = value;   
    }

    private void OnDisable()
    {
        ResetValues();
    }


    void ResetValues()
    {
        imgCard.sprite = null;
        gameObject.SetActive(false);
    }
}
 