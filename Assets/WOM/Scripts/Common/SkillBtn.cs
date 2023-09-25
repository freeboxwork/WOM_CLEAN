using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class SkillBtn : MonoBehaviour
{

    public Image imgSkillBack;
    public Image imgSkillFront;
    public TextMeshProUGUI txtTime; // -> cool time
    public TextMeshProUGUI txtTimeAnim;
    public TextMeshProUGUI txtLevel;

    public Button btnSkill;

    public Color colorDeem;
    public Color colorWhite;

    public Image backLightImage;

    public AnimData animDataUsingSkill;
    public AnimData animDataReloadSkill;
    public AnimationController animCont;

    public EnumDefinition.SkillType skillType;

    Color white = Color.white;
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

    public float coolTimeWait;
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
    void SetBtnEvent()
    {
        btnSkill.onClick.AddListener(() => { UsingSkillByButton(); });
    }

    public void Init(bool coolTime, float leftCoolTime)
    {
        isCoolTime = coolTime;

        if (isCoolTime)
        {
            backLightImage.DOKill();
            backLightImage.DOColor(Color.red, 0.2f);
            skillReady = false;
            skillAddValue = false;
            coolTimeWait = leftCoolTime;
            StartCoroutine(UsingSkillByButton_Cor());
        }
        else
        {
            backLightImage.color = white;
            backLightImage.DOColor(new Color(1, 1, 1, 0), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

    }


    public void UsingSkillByButton()
    {
        if (isCoolTime) return;
        StartCoroutine(UsingSkillByButton_Cor());
    }
    //버튼이 눌렸을때 진입
    IEnumerator UsingSkillByButton_Cor()
    {
        if (skillReady == true)
        {
            GlobalData.instance.soundManager.PlaySfxInGame((EnumDefinition.SFX_TYPE)((int)skillType + 12));
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

            btnSkill.enabled = false;
            skillReady = false;

            imgSkillBack.color = colorWhite;
            imgSkillFront.color = colorDeem;
            imgSkillFront.fillClockwise = true;

            animCont.animData = animDataUsingSkill;



            // 일일 퀘스트 완료 : 스킬 사용
            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.useSkill);

            txtTime.enabled = false;
            txtTimeAnim.enabled = true;

            //스킬 [지속시간] UI Update
            StartCoroutine(animCont.UI_TextAnim(txtTimeAnim, 0));
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 0, 1));

            // skill effect on!
            SkillEffectBySkillType(true);

            // 스킬 사용 대기
            float calcSkillTime = skillDuration;
            var skillStartTime = Time.time;
            skillLeftTime = calcSkillTime;

            //backLightImage.DOColor의 Loop를 종료
            backLightImage.DOKill();
            backLightImage.DOColor(Color.red, 0.2f);

            //버프 이펙트 처리를 위한 변수
            skillAddValue = true;
            //스킬 사용 스텟을 적용시키기 위함
            GlobalData.instance.statManager.UsingSkill(skillType);

            while (skillLeftTime > 0)
            {

                skillLeftTime = calcSkillTime - (Time.time - skillStartTime);
                yield return null;
            }
            // skill effect off!
            SkillEffectBySkillType(false);

            txtTimeAnim.enabled = false;
            skillAddValue = false;

            //쿨타임 Text
            txtTime.enabled = true;
            imgSkillBack.color = colorWhite;
            imgSkillFront.color = colorDeem;
            imgSkillFront.fillClockwise = false;
            animCont.animData = animDataReloadSkill;

            // 쿨타임 대기 및 데이터 저장 
            isCoolTime = true;
            float calcCooltime = animDataReloadSkill.animDuration;
            var startTime = Time.time;
            coolTimeWait = calcCooltime;

            // [재사용 대기 시간] 데이터 저장
            GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, true);
            //스킬 [재사용 대기 시간] UI Update
            StartCoroutine(animCont.UI_TextAnim(txtTime, 0, () => coolTimeWait = 0));
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 1, 0));

            while (coolTimeWait > 0)
            {
                coolTimeWait = calcCooltime - (Time.time - startTime);
                GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, coolTimeWait);
                //Debug.Log("skill coolTimeWait : " + coolTimeWait);
                yield return null;
            }

            //Debug.Log("스킬 재사용 대기 시간 종료");
            SetEndCoolTime();



        }
        else
        {
            var data = GlobalData.instance.skillManager.GetSkillInGameDataByType(skillType);

            //쿨타임 Text
            txtTime.enabled = true;
            imgSkillBack.color = colorWhite;
            imgSkillFront.color = colorDeem;
            imgSkillFront.fillClockwise = false;
            animDataReloadSkill.animDuration = coolTimeWait;
            animCont.animData = animDataReloadSkill;
            float calcCooltime = coolTimeWait;
            var startTime = Time.time;

            Debug.Log("ssssssssssssssssss");

            //스킬 [재사용 대기 시간] UI Update
            StartCoroutine(animCont.UI_TextAnim(txtTime, 0, () => coolTimeWait = 0));
            StartCoroutine(animCont.UI_ImageFillAmountAnim(imgSkillFront, 1, 0));

            while (coolTimeWait > 0)
            {
                coolTimeWait = calcCooltime - (Time.time - startTime);
                GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, coolTimeWait);
                //Debug.Log("skill coolTimeWait : " + coolTimeWait);
                yield return null;
            }

            SetEndCoolTime();



        }

        yield return null;
    }

    public void SetTxtCoolTime(int time)
    {
        TimeSpan t = TimeSpan.FromSeconds(time);
        string answer = string.Format("{0:D0}:{1:D0}", t.Minutes, t.Seconds);
        txtTime.text = answer;
    }
    public void SetTxtLevel(int lv)
    {
        txtLevel.text = string.Format("Lv{0}", lv);
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
                    if (insect.insectType == EnumDefinition.InsectType.union)
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
    void SetEndCoolTime()
    {
        coolTimeWait = 0;
        btnSkill.enabled = true;
        skillReady = true;
        txtTime.enabled = false;
        imgSkillFront.fillAmount = 0;
        isCoolTime = false;

        GlobalData.instance.saveDataManager.SetSkillLeftCoolTime(skillType, 0);
        GlobalData.instance.saveDataManager.SetSkillCooltime(skillType, false);

        //이미지의 Color의 Alpha 값을 1초동안 1과 0을 반복한다
        backLightImage.color = white;
        backLightImage.DOColor(new Color(1, 1, 1, 0), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }


}

