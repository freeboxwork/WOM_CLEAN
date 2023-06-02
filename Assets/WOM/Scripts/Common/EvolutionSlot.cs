using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvolutionSlot : MonoBehaviour
{
    public Image imgBlock;
    public TextMeshProUGUI txtStatName;
    public Button btnLock;
    public Image imgLock;
    public Sprite sprUnLock;
    public Sprite sprLock;
    public bool isUnlock = false;

    public Image imgSymbol;
    public EnumDefinition.EvolutionRewardGrade evolutionRewardGrade;  

    // 능력치 오픈 되어 있는지 판단
    public bool statOpend = false;

    void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        btnLock.onClick.AddListener(LockEvent);
    }

    public void LockEvent()
    {
        isUnlock = !isUnlock;
        var sprit = isUnlock ? sprUnLock : sprLock;
        imgLock.sprite = sprit;

        // 주사위 굴리기 버튼 활성화
        // GlobalData.instance.uiController.EanbleBtnEvolutionRollDice();
        
        // 사용에 필요한 주사위 개수 변경
        GlobalData.instance.evolutionManager.SetTxtUsingDiceCount();
    }

    public void UnLock()
    {
        isUnlock = true;
        imgLock.sprite = sprUnLock;
    }
         

    public void UnLockSlot()
    {
        statOpend = true;
        imgBlock.gameObject.SetActive(false);
    }

    public void SettxtStatName(string value)
    {
        txtStatName.text = value;
    }
   

    public void SetEvolutionRewardGrade(EnumDefinition.EvolutionRewardGrade gradeType)
    {
        evolutionRewardGrade = gradeType;
    }

    public EnumDefinition.EvolutionRewardGrade GetEvolutionRewardGrade()
    {
        return evolutionRewardGrade;
    }

    public void SetSymbol(Sprite symbol)
    {
        imgSymbol.sprite = symbol;  
    }

    public void SetGradeTxtColor(string hexCode)
    {
        ColorUtility.TryParseHtmlString("#"+hexCode, out Color color);
        txtStatName.color = color;
    }
}
