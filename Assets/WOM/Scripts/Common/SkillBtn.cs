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
    public bool skillAddValue = false;
    public bool isCoolTime = false;

    float coolTimeWait;
    float skillLeftTime;
    float skillLeftCoolTime;

    void Start()
    {
        SetBtnEvent();
    }

    void OnEnable()
    {
        if (skillAddValue)
        {
            StartCoroutine(ReloadSkill_Cor());
        }
        if (skillAddValue == false && isCoolTime)
        {
            StartCoroutine(ReloadSkillCoolTime());
        }
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
        string answer = string.Format("{0:D0}:{1:D0}", t.Minutes, t.Seconds);
        txtTime.text = answer;
    }



    IEnumerator UsingKill_Cor()
    {
        // effect
        GlobalData.instance.effectManager.EnableTransitionEffSkillOnByType(skillType);
        var data = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
        float totalCoolTime = data.coolTime - GlobalData.instance.statManager.SkillCoolTime();

        var skillDuration = data.duaration + GlobalData.instance.statManager.SkillDuration();
        //animDataUsingSkill.animDuration = data.duaration;
        animDataUsingSkill.animDuration = skillDuration;
        animDataReloadSkill.animDuration = totalCoolTime;

        //Debug.Log("스킬 애니메이션 시간 :  " + skillDuration);
        if (skillReady == true)
        {
            btnSkill.enabled = false;
            skillReady = false;

            imgSkillBack.color = colorWhite;
            imgSkillFront.color = colorDeem;
            imgSkillFront.fillClockwise = true;

            animCont.animData = animDataUsingSkill;

            //스킬 사용
            GlobalData.instance.statManager.UsingSkill(skillType);

            // 일일 퀘스트 완료 : 스킬 사용
            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.useSkill);

            txtTime.enabled = false;
            skillAddValue = true;
            txtTimeAnim.enabled = true;
            StartCoroutine(animCont.UI_TextAnim(txtTimeAnim, skillDuration, 0));
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 0, 1));

            // skill effect on!
            SkillEffectBySkillType(true);


            // 스킬 사용 대기
            float calcSkillTime = skillDuration;
            var skillStartTime = Time.time;
            skillLeftTime = calcSkillTime;
            while (skillLeftTime > 0)
            {
                skillLeftTime = calcSkillTime - (Time.time - skillStartTime);
                yield return null;
            }

            txtTimeAnim.enabled = false;
            skillAddValue = false;
            txtTime.enabled = true;

            imgSkillBack.color = colorWhite;
            imgSkillFront.color = colorDeem;
            imgSkillFront.fillClockwise = false;
            animCont.animData = animDataReloadSkill;

            yield return new WaitForEndOfFrame();

            // skill effect off!
            SkillEffectBySkillType(false);


            // 쿨타임
            GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, true);
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 1, 0));
            StartCoroutine(animCont.UI_TextAnim(txtTime, animDataReloadSkill.animDuration, 0));

            // 쿨타임 대기 및 데이터 저장 
            isCoolTime = true;
            float calcCooltime = animDataReloadSkill.animDuration;
            var startTime = Time.time;
            coolTimeWait = calcCooltime;
            while (coolTimeWait > 0)
            {
                coolTimeWait = calcCooltime - (Time.time - startTime);
                GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, coolTimeWait);
                //Debug.Log("skill coolTimeWait : " + coolTimeWait);
                yield return null;
            }

            GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);
            GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, false);

            isCoolTime = false;
            txtTime.enabled = false;
            coolTimeWait = 0;
            btnSkill.enabled = true;
            skillReady = true;

        }
        yield return null;
    }

    void SkillEffectBySkillType(bool enableValue)
    {
        var insects = GlobalData.instance.insectManager.enableInsects;

        foreach (var insect in insects)
        {
            switch (skillType)
            {
                case EnumDefinition.SkillType.insectDamageUp:
                    insect.effectContoller.AuraEffect(enableValue);
                    break;
                case EnumDefinition.SkillType.unionDamageUp:
                    insect.effectContoller.FireEffect(enableValue);
                    break;
                case EnumDefinition.SkillType.allUnitSpeedUp:
                    insect.effectContoller.TrailEffect(enableValue);
                    break;
                case EnumDefinition.SkillType.glodBonusUp:
                    insect.effectContoller.GoldEffect(enableValue);
                    break;
                case EnumDefinition.SkillType.monsterKing:

                    break;
                case EnumDefinition.SkillType.allUnitCriticalChanceUp:
                    insect.effectContoller.ThunderEffect(enableValue);
                    break;
            }
        }


    }


    IEnumerator ReloadSkill_Cor()
    {

        //StartCoroutine(animCont.UI_TextAnim(txtTimeAnim, skillLeftTime, 0));
        StartCoroutine(animCont.UI_TextAnim_Reload(txtTimeAnim, skillLeftTime, 0, skillLeftTime));
        StartCoroutine(animCont.UI_ImageFillAnim(imgSkillFront, 0, 1, skillLeftTime));

        // 스킬 사용 대기
        float calcSkillTime = skillLeftTime;
        var skillStartTime = Time.time;
        while (skillLeftTime > 0)
        {
            skillLeftTime = calcSkillTime - (Time.time - skillStartTime);
            yield return null;
        }

        txtTimeAnim.enabled = false;
        skillAddValue = false;
        txtTime.enabled = true;

        imgSkillBack.color = colorWhite;
        imgSkillFront.color = colorDeem;
        imgSkillFront.fillClockwise = false;
        animCont.animData = animDataReloadSkill;

        // 쿨타임
        GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, true);
        StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 1, 0));
        StartCoroutine(animCont.UI_TextAnim(txtTime, animDataReloadSkill.animDuration, 0));

        // 쿨타임 대기 및 데이터 저장 
        isCoolTime = true;
        float calcCooltime = animDataReloadSkill.animDuration;
        var startTime = Time.time;
        coolTimeWait = calcCooltime;
        while (coolTimeWait > 0)
        {
            coolTimeWait = calcCooltime - (Time.time - startTime);
            GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, coolTimeWait);
            //Debug.Log("skill coolTimeWait : " + coolTimeWait);
            yield return null;
        }

        GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);
        GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, false);

        isCoolTime = false;
        txtTime.enabled = false;
        coolTimeWait = 0;
        btnSkill.enabled = true;
        skillReady = true;
    }

    IEnumerator ReloadSkillCoolTime()
    {
        yield return null;

        // 쿨타임
        StartCoroutine(animCont.UI_ImageFillAnim(imgSkillFront, 1, 0, coolTimeWait));
        txtTime.enabled = true;
        StartCoroutine(animCont.UI_TextAnim_Reload(txtTime, coolTimeWait, 0, coolTimeWait));

        // 쿨타임 대기 및 데이터 저장 

        float calcCooltime = coolTimeWait;
        var startTime = Time.time;
        while (coolTimeWait > 0)
        {
            coolTimeWait = calcCooltime - (Time.time - startTime);
            GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, coolTimeWait);
            //Debug.Log("skill coolTimeWait : " + coolTimeWait);
            yield return null;
        }

        GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);
        GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, false);

        isCoolTime = false;
        txtTime.enabled = false;
        coolTimeWait = 0;
        btnSkill.enabled = true;
        skillReady = true;
    }


    public void ReloadCoolTime()
    {
        StartCoroutine(ReloadCoolTimeCor());
    }
    // 재접속시 쿨타임 여부에 따라 실행
    IEnumerator ReloadCoolTimeCor()
    {
        var data = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
        var saveData = GlobalData.instance.saveDataManager.GetSaveDataSkill(skillType);
        if (saveData.isCooltime)
        {
            var currentTime = System.DateTime.Now;
            var lastTime = GlobalData.instance.saveDataManager.saveDataTotal.saveDataSystem.quitTime;
            TimeSpan timeSpan = currentTime - System.DateTime.Parse(lastTime);
            var second = timeSpan.Seconds;
            if (second > data.coolTime)
            {
                // 쿨타임이 끝난경우
                saveData.isCooltime = false;
                saveData.leftCoolTime = 0;
                Debug.Log("cooltime end!!!");
            }
            else
            {
                // 쿨타임이 필요한 경우
                btnSkill.enabled = false;
                skillReady = false;

                //TODO: 계산식 맞는지 확인 필요
                var cooltime = saveData.leftCoolTime - second;

                if (cooltime < 1f)
                {
                    cooltime = 0;
                }
                else
                {
                    float animDuration = cooltime;
                    imgSkillBack.color = colorWhite;
                    imgSkillFront.color = colorDeem;
                    imgSkillFront.fillClockwise = false;

                    Debug.Log("reload cooltime : " + cooltime);


                    // 쿨타임
                    StartCoroutine(animCont.UI_ImageFillAnim(imgSkillFront, 1, 0, animDuration));

                    txtTime.enabled = true;
                    StartCoroutine(animCont.UI_TextAnim_Reload(txtTime, animDuration, 0, animDuration));

                    // 쿨타임 대기
                    float calcCooltime = cooltime;
                    var startTime = Time.time;
                    coolTimeWait = calcCooltime;
                    while (coolTimeWait > 0)
                    {
                        coolTimeWait = calcCooltime - (Time.time - startTime);
                        GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, coolTimeWait);
                        //Debug.Log("reload skill coolTimeWait : " + coolTimeWait);
                        yield return null;
                    }

                }

                coolTimeWait = 0;
                btnSkill.enabled = true;
                skillReady = true;
                txtTime.enabled = false;

                GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);
                GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, false);
            }
        }
        yield return null;

    }



}
