using UnityEngine.UI;
using TMPro;
using UnityEngine;
using ProjectGraphics;
using System.Collections;
using System.Collections.Generic;

public class CampPopup : MonoBehaviour
{

    public Image imgSummonCountProgress;
    public TextMeshProUGUI txtSummonCount;
    public TextMeshProUGUI txtGradeLevel;

    public Button btnGetReward;

    public Toggle toggleUnionEffSkip;
    public Toggle toggleUnionRepeatGame;
    public Toggle toggleDnaRepeatGame;

    bool isToggleEffSkipUnion;
    bool isToggleRepeatUnion;
    bool isToggleRepeatDNA;
    public LotteryAnimationController lotteryAnimationController;

    public List<GameObject> particle;


    void Start()
    {
        SetBtnEvent();
        ToggleReset();
    }

    void OnEnable()
    {
        foreach (var item in particle)
        {
            item.SetActive(true);
        }
        GlobalData.instance.uiController.ButtonInteractableCheck();
    }
    void OnDisable()
    {
       foreach (var item in particle)
        {
            item.SetActive(false);
        } 
    }

    public bool GetIsOnToggleSkipUnionIsOn()
    {
        return toggleUnionEffSkip.isOn;
    }
    public bool GetIsOnToggleRepeatUnion()
    {
        return toggleUnionRepeatGame.isOn;
    }
    public bool GetIsOnToggleRepeatDNA()
    {
        return toggleDnaRepeatGame.isOn;
    }

    public void ToggleReset()
    {
        toggleUnionEffSkip.isOn = false;
        toggleUnionRepeatGame.isOn = false;
        toggleDnaRepeatGame.isOn = false;
    }

    public void SetSummonCountProgress(int curValue, int totalValue)
    {
        var value = (float)curValue / (float)totalValue;
//        Debug.Log($"value : {value}" + $"curValue : {curValue}" + $"totalValue : {totalValue}");
        imgSummonCountProgress.fillAmount = value;
    }

    public void SetTxtSummonCount(int curValue, int totalValue)
    {
        txtSummonCount.text = $"{curValue}/{totalValue}";
    }

    public void SetTxtGradeLevel(int level)
    {
        txtGradeLevel.text = $"{level}";
    }

    void SetBtnEvent()
    {
        btnGetReward.onClick.AddListener(() =>
        {
            GlobalData.instance.rewardManager.UnionReward();

        });

    }
}
