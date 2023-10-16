using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class EvolutionSlot : MonoBehaviour
{
    public int slotId;
    public Image imgBlock;
    public TextMeshProUGUI txtStatName;
    public Button btnLock;
    public Image imgLock;
    public Color lockColor;
    public Color unLockColor;
    

    public bool isUnlock = false;

    public Image imgSymbol;
    public EnumDefinition.EvolutionRewardGrade evolutionRewardGrade;

    // ?ä•?†•Ïπ? ?ò§?îà ?êò?ñ¥ ?ûà?äîÏß? ?åê?ã®
    public bool statOpend = false;

    public Image imgLightSweepAnim;


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
        var color = isUnlock ? unLockColor : lockColor;
        imgLock.color = color;

        // Ï£ºÏÇ¨?úÑ Íµ¥Î¶¨Í∏? Î≤ÑÌäº ?ôú?Ñ±?ôî
        // GlobalData.instance.uiController.EanbleBtnEvolutionRollDice();

        // ?Ç¨?ö©?óê ?ïÑ?öî?ïú Ï£ºÏÇ¨?úÑ Í∞úÏàò Î≥?Í≤?
        GlobalData.instance.evolutionManager.SetTxtUsingDiceCount();
    }

    public void UnLock()
    {
        isUnlock = true;
        imgLock.color = unLockColor;
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

        // setSaveData

    }

    public void SetGradeImgColor(string hexCode)
    {
        ColorUtility.TryParseHtmlString("#" + hexCode, out Color color);
        imgLightSweepAnim.color = color;
    }

    public void EnableSlotLightSweepAnim()
    {
        StopCoroutine("EnableSlotLightSweepAnimCoroutine");
        StartCoroutine(EnableSlotLightSweepAnimCoroutine());
    }

    IEnumerator EnableSlotLightSweepAnimCoroutine()
    {
        imgLightSweepAnim.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        imgLightSweepAnim.gameObject.SetActive(true);
    }
}
