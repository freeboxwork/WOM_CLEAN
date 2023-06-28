using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SkillBtn : MonoBehaviour
{

    public Image imgSkillBack;
    public Image imgSkillFront;
    public TextMeshProUGUI txtTime; // -> cool time
    public TextMeshProUGUI txtTimeAnim;
    public Button btnSkill;

    public Color colorDeem;
    public Color colorWhite;

    public AnimData animDataUsingSkill;
    public AnimData animDataReloadSkill;
    public AnimationController animCont;

    public EnumDefinition.SkillType skillType;

    /* PROCESS
    
    1 : BACK COLOR DEEM
    2 : FRONT FILL ANIM ( 0~1 ) usin skill
    3 : FRONT COLOR DEEM
    4 : BACK COLOR WHITE
    5 : FRONT FILL ANIM ( 0~1 ) reload skill
    6 : BACK COLOR DEEM
    7 : FRONT COLOR WHIRE

    */

    public bool skillReady = false;

    void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        btnSkill.onClick.AddListener(() => { UsingSkill(); });

    }

    public void UsingSkill()
    {
        StartCoroutine(UsingKill_Cor());
    }

    public void SetTxtCoolTime(int time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        string answer = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        txtTime.text = answer;
    }

    public bool skillAddValue = false;

    IEnumerator UsingKill_Cor()
    {
        var data = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
        float totalCoolTime = data.coolTime * (1 - GlobalData.instance.statManager.SkillCoolTime());

        animDataUsingSkill.animDuration = data.duaration;
        animDataReloadSkill.animDuration = totalCoolTime;
        if (skillReady == true)
        {
            btnSkill.enabled = false;
            skillReady = false;

            imgSkillBack.color = colorWhite;
            imgSkillFront.color = colorDeem;

            animCont.animData = animDataUsingSkill;

            //스킬 사용
            GlobalData.instance.statManager.UsingSkill(skillType);

            // 일일 퀘스트 완료 : 스킬 사용
            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.useSkill);

            txtTime.enabled = false;
            skillAddValue = true;
            txtTimeAnim.enabled = true;
            StartCoroutine(animCont.UI_TextAnim(txtTimeAnim, data.duaration, 0));
            yield return StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 0, 1));
            txtTimeAnim.enabled = false;
            skillAddValue = false;
            txtTime.enabled = true;

            imgSkillBack.color = colorDeem;
            imgSkillFront.color = colorWhite;
            animCont.animData = animDataReloadSkill;

            // 쿨타임
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 0, 1));

            // 쿨타임 대기 및 데이터 저장 
            float calcCooltime = animDataReloadSkill.animDuration;
            var startTime = Time.time;
            while (calcCooltime <= 0)
            {
                calcCooltime -= Time.time - startTime;
                GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, calcCooltime);
                yield return null;
            }
            GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);

            btnSkill.enabled = true;
            skillReady = true;

        }
        yield return null;
    }

    // 재접속시 쿨타임 여부에 따라 실행
    IEnumerator ReloadCoolTime()
    {
        var data = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
        var saveData = GlobalData.instance.saveDataManager.GetSaveDataSkill(skillType);
        if (saveData.isUsingSkill)
        {
            var currentTime = System.DateTime.Now;
            var lastTime = GlobalData.instance.saveDataManager.saveDataTotal.saveDataSystem.quitTime;
            TimeSpan timeSpan = currentTime - System.DateTime.Parse(lastTime);
            var second = timeSpan.Seconds;
            if (second > data.coolTime)
            {
                // 쿨타임이 끝난경우
            }
            else
            {
                // 쿨타임이 필요한 경우

            }

        }
        yield return null;

    }



}
