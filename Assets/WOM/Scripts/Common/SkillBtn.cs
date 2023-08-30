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
    //버튼 UI가 비활성화 되기 전에 스킬이 사용되어 지속시간이 남았는지 체크
    public bool skillAddValue = false;
    public bool isCoolTime = false;

    float coolTimeWait;
    float skillLeftTime;
    float skillLeftCoolTime;

    //TEST CODE
    public void SkipCoolTime()
    {
        animCont.animData.animTime = 1;
        coolTimeWait = 0;
    }

    void Start()
    {
        SetBtnEvent();
    }

    void OnEnable()
    {
        if (skillAddValue)
        {
            StartCoroutine(ReloadStillUseingSkill_Cor());
        }
        if (skillAddValue == false && isCoolTime)
        {
            StartCoroutine(ReloadStillWaitCoolTime());
        }
    }
    
    void SetBtnEvent()
    {
        btnSkill.onClick.AddListener(() => { UsingSkillByButton(); });
    }
    
    public void UsingSkillByButton()
    {
        StartCoroutine(UsingSkillByButton_Cor());
    }
    //버튼이 눌렸을때 진입
    IEnumerator UsingSkillByButton_Cor()
    {
        //스킬 사용 연출
        GlobalData.instance.effectManager.EnableTransitionEffSkillOnByType(skillType);
        //스킬 데이터 가져오기
        var data = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
        //기본 스킬 [재사용 대기 시간] - DNA [재사용 대기 시간] 감소 적용
        float totalCoolTime = data.coolTime - GlobalData.instance.statManager.SkillCoolTime();
        //기본 스킬 [지속시간] - DNA [지속시간] 감소 적용
        var skillDuration = data.duaration + GlobalData.instance.statManager.SkillDuration();

        //Debug.Log("재사용 시간 : "+data.coolTime);
        //Debug.Log("재지속시간 : "+data.duaration);
        
        //animDataUsingSkill.animDuration = data.duaration;
        //스킬 지속시간 타이머
        animDataUsingSkill.animDuration = skillDuration;
        //스킬 재사용 시간 타이머
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
            txtTimeAnim.enabled = true;

            //스킬 [지속시간] UI Update
            StartCoroutine(animCont.UI_TextAnim(txtTimeAnim, skillDuration, 0));
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 0, 1));

            // skill effect on!
            SkillEffectBySkillType(true);

            // 스킬 사용 대기
            float calcSkillTime = skillDuration;
            var skillStartTime = Time.time;
            skillLeftTime = calcSkillTime;

            // ui 전환 효과가 발동되면 대기
            //yield return new WaitUntil(() =>  GlobalData.instance.statManager.transitionUI == false);
            skillAddValue = true;

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

            // [재사용 대기 시간] 데이터 저장
            GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, true);
            //스킬 [재사용 대기 시간] UI Update
            StartCoroutine(animCont.UI_TextAnim(txtTime, animDataReloadSkill.animDuration, 0, () => coolTimeWait = 0));
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 1, 0));

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

            //Debug.Log("스킬 재사용 대기 시간 종료");
            GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);
            GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, false);

            imgSkillFront.fillAmount = 0;
            isCoolTime = false;
            txtTime.enabled = false;
            coolTimeWait = 0;
            btnSkill.enabled = true;
            skillReady = true;

        }
        yield return null;
    }
    
    public void SetTxtCoolTime(int time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        string answer = string.Format("{0:D0}:{1:D0}", t.Minutes, t.Seconds);
        txtTime.text = answer;
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
    
    //캐슬을 갔다온 후 스킬의 지속시간(Duration)이 아직 남아 있다면
    IEnumerator ReloadStillUseingSkill_Cor()
    {
        Debug.Log("캐슬에서 빠져나와 스킬 지속시간이 남아 있음");

        //StartCoroutine(animCont.UI_TextAnim(txtTimeAnim, skillLeftTime, 0));
        StartCoroutine(animCont.UI_TextAnim_Reload(txtTimeAnim, skillLeftTime, 0, skillLeftTime));
        StartCoroutine(animCont.UI_ImageFillAnim(imgSkillFront, skillLeftTime/animDataUsingSkill.animDuration, 1, skillLeftTime));

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
        StartCoroutine(animCont.UI_TextAnim(txtTime, animDataReloadSkill.animDuration, 0,() => coolTimeWait = 0));

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

    //캐슬을 갔다온 후 스킬이 Duration이 끝났지만 쿨타임이 남아 있다면
    IEnumerator ReloadStillWaitCoolTime()
    {
        yield return null;

        var data = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
        //기본 스킬 [재사용 대기 시간] - DNA [재사용 대기 시간] 감소 적용
        float totalCoolTime = data.coolTime - GlobalData.instance.statManager.SkillCoolTime();

        //Debug.Log("재사용 시간 : "+data.coolTime);

        //Debug.Log("재지속시간 : "+data.duaration);

        //스킬 재사용 시간 타이머
        animDataReloadSkill.animDuration = totalCoolTime;

        //Debug.Log("캐슬에서 빠져나와 아직 재사용 대기시간입니다. coolTimeWait:" + coolTimeWait);
        // [재사용 대기 시간]  UI Update
        txtTime.enabled = true;
        StartCoroutine(animCont.UI_TextAnim_Reload(txtTime, coolTimeWait, 0, coolTimeWait));
        StartCoroutine(animCont.UI_ImageFillAnim(imgSkillFront, coolTimeWait/animDataReloadSkill.animDuration, 0, coolTimeWait));

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
        imgSkillFront.fillAmount = 0;

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
    // 게임 재 접속시 [재사용 대기 시간]일 남아 있는 경우
    IEnumerator ReloadCoolTimeCor()
    {
        var sheetData = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
        var saveData = GlobalData.instance.saveDataManager.GetSaveDataSkill(skillType);
        //Debug.Log("저장된 데이터 LEFT TIME 불러오기 : " + saveData.leftCoolTime);

        //무조건 이곳에 들어옴
        if (saveData.isCooltime)
        {
            isCoolTime = true;

            //var pervTime = "2023-08-16 오후 9:13:23";
            //var calcTime = System.DateTime.Parse(pervTime);
            var currentTime = System.DateTime.Now;
            //게임을 종료한시간 불러오기
            var lastTime = GlobalData.instance.saveDataManager.saveDataTotal.saveDataSystem.quitTime;

            TimeSpan timeSpan = currentTime - System.DateTime.Parse(lastTime);

            //Debug.Log("Skill curretn time " + currentTime + " last time " + lastTime + " time span " + timeSpan + " seconds " + timeSpan.Seconds);

            var second = timeSpan.Seconds;

            if (second <= 0)
            {
                // 쿨타임이 끝난경우
                //saveData.isCooltime = false;
                //saveData.leftCoolTime = 0;
                //Debug.Log("게임을 종료한지 몇초 안되었음");
            }
            else
            {
                // 쿨타임이 필요한 경우
                btnSkill.enabled = false;
                skillReady = false;

                //TODO: 계산식 맞는지 확인 필요
                //Debug.Log("저장되어 있는 쿨타임 시간 : "+ saveData.leftCoolTime);
                //Debug.Log("감소한 시간 : "+ second);
                
                var loadCooltimeToSaveData = saveData.leftCoolTime - second;
                //Debug.Log("쿨타임 시간 결과 : "+ loadCooltimeToSaveData);

                if (loadCooltimeToSaveData < 1f)
                {
                    loadCooltimeToSaveData = 0;
                    //Debug.Log("미접속 시간동안 쿨타임 종료");
                }
                else
                {
                    float animDuration = loadCooltimeToSaveData;
                    imgSkillBack.color = colorWhite;
                    imgSkillFront.color = colorDeem;
                    imgSkillFront.fillClockwise = false;

                    //스킬을 버튼을 눌러 사용할때만 animData세팅이 되므로 게임을 재접속하였을때 다시 세팅해줘야함
                    var skillData = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);
                    //기본 스킬 [재사용 대기 시간] - DNA [재사용 대기 시간] 감소 적용
                    //float totalCoolTime = skillData.coolTime - GlobalData.instance.statManager.SkillCoolTime();//데이터 불러올때 타이밍 문제로 버그남 DNA 연산은 뺌
                    float defaultCoolTimeValue = skillData.coolTime;
                    //기본 스킬 [지속시간] - DNA [지속시간] 감소 적용
                    //var skillDuration = data.duaration + GlobalData.instance.statManager.SkillDuration();//데이터 불러올때 타이밍 문제로 버그남 DNA 연산은 뺌
                    //var skillDuration = sheetData.duaration;
                    //animDataUsingSkill.animDuration = data.duaration;
                    //스킬 지속시간 타이머
                    //animDataUsingSkill.animDuration = skillDuration;
                    //스킬 재사용 시간 타이머
                    animDataReloadSkill.animDuration = loadCooltimeToSaveData;
                    // 쿨타임
                    StartCoroutine(animCont.UI_ImageFillAnim(imgSkillFront, animDuration/defaultCoolTimeValue, 0, animDuration));
                    txtTime.enabled = true;
                    StartCoroutine(animCont.UI_TextAnim_Reload(txtTime, animDuration, 0, animDuration));

                    // 쿨타임 대기
                    float calcCooltime = animDataReloadSkill.animDuration;
                    var startTime = Time.time;
                    coolTimeWait = calcCooltime;

                    while (coolTimeWait > 0)
                    {
                        coolTimeWait = calcCooltime - (Time.time - startTime);
                        GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, coolTimeWait);
                        //Debug.Log("LEFT TIME에 저장되고 있음 : " + coolTimeWait);
                        yield return null;
                    }

                }

                coolTimeWait = 0;
                btnSkill.enabled = true;
                skillReady = true;
                txtTime.enabled = false;
                imgSkillFront.fillAmount = 0;
                isCoolTime = false;

                GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);
                GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, false);
            }
        }

        yield return null;

    }



}
