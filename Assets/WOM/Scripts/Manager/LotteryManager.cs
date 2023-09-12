using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjectGraphics;
using System.Linq;
using Unity.Mathematics;
/// <summary>
/// 유니온 뽑기 , 진화 주사위 뽑기
/// </summary>
public class LotteryManager : MonoBehaviour
{
    public int curSummonGrade = 0; // 소환 등급 데이터 
    public int lottreyRoundCount = 0;     // 뽑기 시도 카운트
    public UnionGambleData curGambleData;
    public SummonGradeData curSummonGradeData;
    public Transform trCardsParent;
    public LotteryCard prefabLotteryCard;
    public List<LotteryCard> lotteryCards = new List<LotteryCard>();
    public float cardOpenDeayTime = 0.2f;

    public LotteryAnimationController lotteryAnimationController;
    EnumDefinition.LotteryPageType curLotteryPageType;

    public List<Sprite> unionFaceNormal = new List<Sprite>();
    public List<Sprite> unionFaceHigh = new List<Sprite>();
    public List<Sprite> unionFaceRare = new List<Sprite>();
    public List<Sprite> unionFaceHero = new List<Sprite>();
    public List<Sprite> unionFaceLegend = new List<Sprite>();
    public List<Sprite> unionFaceUnique = new List<Sprite>();
    List<List<Sprite>> unionFaceDatas = new List<List<Sprite>>();


    // 테스트 용도
    //string[][] unionNameData;
    float[] randomGradeValues;
    //public List<Color> cardColors = new List<Color>();
    public List<EnumDefinition.UnionGradeType> openedUnionTypeCards;



    public int unionGradeLevel = 0;
    // 현재 레벨에서 뽑기 진행 수
    public int curLotteryCount = 0;
    // 현재 레벨에서 전체 뽑기 수
    public int totalLotteryCount = 0;
    /// <summary> 리워드 획득전 전체 뽑기 진행 수 </summary>
    public int totalDrawCount = 0;

    // union gamble 새로 만듦....
    public int totalGambleCount = 0; // 현재 레벨에서 전체 뽑기 수 ( 레벨 올라가면 초기화)
    public int summonGradeLevel = 0; // 뽑기 등급 레벨
     // public int totalGamblePlayCount = 0; // 게임 전체 뽑기 수 ( 리워드 획득시 해당 레벨 count 만큼 차감 )

    void Start()
    {
        // SetUnionNmaeData();
        SetUnionFaceList();
    }

    void SetUnionFaceList()
    {
        unionFaceDatas.Add(unionFaceNormal);
        unionFaceDatas.Add(unionFaceHigh);
        unionFaceDatas.Add(unionFaceRare);
        unionFaceDatas.Add(unionFaceHero);
        unionFaceDatas.Add(unionFaceLegend);
        unionFaceDatas.Add(unionFaceUnique);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            //StartCoroutine(CardOpen(10));
        }
    }

    public void SetGambleData(UnionGambleData unionGambleData)
    {
        curGambleData = unionGambleData;
    }
    public void SetSummonGradeData(SummonGradeData summonGradeData)
    {
        curSummonGradeData = summonGradeData;
        curLotteryCount = 0;
        totalLotteryCount = curSummonGradeData.count;
        //획득할 유니온 저장
        if (curSummonGradeData.rewardUnionIndex > 0)
        {
            var index = curSummonGradeData.rewardUnionIndex;
            if (!GlobalData.instance.saveDataManager.saveDataTotal.saveDataUnionSummonGrade.rewaedUnionIds_Remove.Contains(index))
                GlobalData.instance.rewardManager.AddUnionReward(curSummonGradeData.rewardUnionIndex);

            var campPopup = (CampPopup)GlobalData.instance.castleManager.GetCastlePopupByType(EnumDefinition.CastlePopupType.camp);
            campPopup.SetTxtGradeLevel(curSummonGradeData.level);
        }

    }

    /// <summary> 뽑기 필요 데이터 최초 세팅 </summary>
    public IEnumerator Init()
    {

        // TODO : 저장된 데이터에서 불러와야 함
        var data = GlobalData.instance.saveDataManager.saveDataTotal.saveDataUnionSummonGrade;
        summonGradeLevel = data.summonGradeLevel;
        totalGambleCount = data.totalGambleCount;


        //소환 레벨UI세팅
        SetSummonGradeData(GlobalData.instance.dataManager.GetSummonGradeDataByLevel(summonGradeLevel));
        SetGambleData(GlobalData.instance.dataManager.GetUnionGambleDataBySummonGrade(summonGradeLevel));
        randomGradeValues = GetRandomArrayValue();
        // set ui
        PopupUIUpdate();
        //CreateCards();
        yield return null;
    }

    public void LotteryStart(int roundCount, int payValue, UnityAction gameEndEvent, EnumDefinition.RewardType rewardType)
    {
        if (lotteryAnimationController.toggleRepeatGame.isOn)
            StartCoroutine(RepeatGame(roundCount, payValue, gameEndEvent, rewardType));
        else
            StartCoroutine(CardOpen(roundCount, payValue, gameEndEvent, rewardType));
    }

    IEnumerator RepeatGame(int roundCount, int payValue, UnityAction gameEndEvent, EnumDefinition.RewardType rewardType)
    {
        while (lotteryAnimationController.toggleRepeatGame.isOn)
        {
            yield return StartCoroutine(CardOpen(roundCount, payValue, gameEndEvent, rewardType));
            yield return new WaitForSeconds(0.4f);
        }
    }

    public IEnumerator CardOpen(int roundCount, int payValue, UnityAction gameEndEvent, EnumDefinition.RewardType rewardType)
    {
        //보석이 충분한지 체크
        if (IsValidGemCount(payValue, rewardType))
        {

            if (rewardType == EnumDefinition.RewardType.unionTicket)
            {
                //유니온 티켓 차감
                GlobalData.instance.player.PayUnionTicket(payValue);
            }
            else
            {
                // pay gem
                GlobalData.instance.player.PayGem(payValue);
            }


            // 닫기 버튼 비활성화
            UtilityMethod.GetCustomTypeBtnByID(44).interactable = false;

            // 다시 뽑기 버튼 비활성화
            UtilityMethod.SetBtnsInteractableEnable(new List<int> { 17, 18, 19 }, false);

            // 스킵 버튼 활성화
            UtilityMethod.SetBtnInteractableEnable(33, true);

            yield return StartCoroutine(MakeCardOption(roundCount));

            yield return StartCoroutine(CardsOpenEffect());

            //yield return new WaitForSeconds(0.3f); // 05초 대기 ( 연출 )
            //curLotteryCount += roundCount;
            //totalDrawCount += roundCount;
            //var data = GlobalData.instance.dataManager.GetSummonGradeDataByLevel(summonGradeLevel);

            var maxData = GlobalData.instance.dataManager.summonGradeDatas.data.Last();
            if (summonGradeLevel < maxData.level)
            {
                totalGambleCount += roundCount;

                // save data
                GlobalData.instance.saveDataManager.SaveDataUnionTotalGambleCount(totalGambleCount);

                // 소환등급 레벨업 체크 및 UI 업데이트
                if (totalGambleCount >= curSummonGradeData.count)
                {
                    summonGradeLevel++;
                    // save data
                    GlobalData.instance.saveDataManager.SaveDataUnionSummonGradeLevel(summonGradeLevel);

                    totalGambleCount = totalGambleCount - curSummonGradeData.count;

                    GlobalData.instance.saveDataManager.SaveDataUnionTotalGambleCount(totalGambleCount);
                    //소환레벨 UI세팅
                    SetSummonGradeData(GlobalData.instance.dataManager.GetSummonGradeDataByLevel(summonGradeLevel));
                    SetGambleData(GlobalData.instance.dataManager.GetUnionGambleDataBySummonGrade(summonGradeLevel));
                }

                PopupUIUpdate();
            }

            //Debug.Log("뽑기 진행 수 : " + curLotteryCount);

            yield return new WaitForSeconds(0.6f);

            //연속 뽑기중이 아닐때만 버튼 활성화
            if (!lotteryAnimationController.toggleRepeatGame.isOn)
            {
                // 뽑기 버튼 활성화
                UtilityMethod.SetBtnsInteractableEnable(new List<int> { 17, 18, 19 }, true);
            }

            // 스킵 버튼 비활성화
            UtilityMethod.SetBtnInteractableEnable(33, false);

            // 닫기 버튼 활성화
            UtilityMethod.GetCustomTypeBtnByID(44).interactable = true;



            gameEndEvent.Invoke();

            if (GlobalData.instance.tutorialManager.isUnionGamblingTutorial)
                EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTutorialUnionGamblingEnd);

        }
        else
        {
            // message popup (보석이 부족합니다)
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 3);
            StopAllCoroutines();
            yield break;
        }

    }

    public void CustomLevelUp(int level)
    {
        summonGradeLevel = level;
        SetSummonGradeData(GlobalData.instance.dataManager.GetSummonGradeDataByLevel(summonGradeLevel));
        SetGambleData(GlobalData.instance.dataManager.GetUnionGambleDataBySummonGrade(summonGradeLevel));
        PopupUIUpdate();
    }

    bool IsValidGemCount(int payValue, EnumDefinition.RewardType rewardType)
    {
        var goods = GlobalData.instance.player.GetGoodsByRewardType(rewardType);
        return goods >= payValue;
    }

    // UI 업데이트
    void PopupUIUpdate()
    {
        CampPopup popup = (CampPopup)GlobalData.instance.castleManager.GetCastlePopupByType(EnumDefinition.CastlePopupType.camp);
        
        if(summonGradeLevel >= 8)
        {
            popup.SetSummonCountProgress(1, 1);
            popup.SetTxtSummonCount(9999, 9999);
            return;
        }
        
        //FillAmount 세팅
        popup.SetSummonCountProgress(totalGambleCount, curSummonGradeData.count);
        //뽑기 카운트 UI 세팅
        popup.SetTxtSummonCount(totalGambleCount, curSummonGradeData.count);

    }

    //유니온 리워드 사용 획득 버튼 이벤트 실행 후 호출 
    public void TotalDrawCountUiUpdate(int subCount)
    {
        //totalGamblePlayCount -= subCount;
        var campPopup = (CampPopup)GlobalData.instance.castleManager.GetCastlePopupByType(EnumDefinition.CastlePopupType.camp);
        campPopup.SetTxtSummonCount(totalGambleCount, curSummonGradeData.count);
    }

    void LotteryClose()
    {

    }

    public IEnumerator MakeCardOption(int gameCount)
    {
        openedUnionTypeCards = new List<EnumDefinition.UnionGradeType>();
        for (int i = 0; i < gameCount; i++)
        {
            var union = (EnumDefinition.UnionGradeType)UtilityMethod.GetWeightRandomValue(randomGradeValues);
            //Debug.Log("유니온뽑기확률:"+randomGradeValues[5]);
            openedUnionTypeCards.Add(union);
            //일일 퀘스트 완료 : 유니온 소환
            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.summonUnion);
        }
        yield return null;

    }


    public IEnumerator CardsOpenEffect()
    {
        List<int> unionIndexList = new List<int>();
        for (int i = 0; i < openedUnionTypeCards.Count; i++)
        {
            if (i == 0 && GlobalData.instance.tutorialManager.isUnionGamblingTutorial)
            {
                // 투토리얼 일때 무조건 0번 뽑기
                unionIndexList.Add(0);
                continue;
            }

            //유니온
            var unionType = openedUnionTypeCards[i];
            var faceIndex = GetRandomFaceIndex();
            var unionIdex = GetUnionIndex(unionType, faceIndex);
            unionIndexList.Add(unionIdex);
            yield return null;
        }


        lotteryAnimationController.gameObject.SetActive(true);
        EnableLotteryBtnsSet(EnumDefinition.LotteryPageType.UNION);
        lotteryAnimationController.StartLotteryAnimation();
        yield return StartCoroutine(lotteryAnimationController.ShowUnionSlotCardOpenProcess(unionIndexList.ToArray()));
        GlobalData.instance.unionManager.AddUnions(unionIndexList);
    }

    void EnableLotteryBtnsSet(EnumDefinition.LotteryPageType lotteryPageType)
    {
        var unionPage = lotteryPageType == EnumDefinition.LotteryPageType.UNION;
        UtilityMethod.GetCustomTypeGMById(8).SetActive(unionPage);
        UtilityMethod.GetCustomTypeGMById(9).SetActive(!unionPage);
    }

    int GetRandomFaceIndex()
    {
        return UnityEngine.Random.Range(0, 8);
    }

    int GetUnionIndex(EnumDefinition.UnionGradeType unionGradeType, int faceIndex)
    {
        return faceIndex + (8 * (int)unionGradeType);
    }

    // curGambleData 의 각 랜덤 범위 적용 
    float[] GetRandomArrayValue()
    {
        var data = curGambleData;
        float[] array = new float[6];
        array[0] = data.normal;
        array[1] = data.high;
        array[2] = data.rare;
        array[3] = data.hero;
        array[4] = data.legend;
        array[5] = data.unique;
        return array;
    }

    //float ChooseUnionType(float[] probs)
    //{
    //    float total = 0;
    //    foreach (float elem in probs)
    //    {
    //        total += elem;
    //    }
    //    float randomPoint = Random.value * total;

    //    for (int i = 0; i < probs.Length; i++)
    //    {
    //        if (randomPoint < probs[i])
    //        {
    //            return i;
    //        }
    //        else
    //        {
    //            randomPoint -= probs[i];
    //        }
    //    }
    //    return probs.Length - 1;
    //}


    private void OnDisable()
    {
        ResetValues();
    }

    void ResetValues()
    {

    }


}
