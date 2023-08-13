using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjectGraphics;
using System.Linq;
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
    public int cardsBirthCount = 50; // 풀링으로 사용할 카드 이미지 - 50회 연속 뽑기 기능으로 최초 50개 생성

    public LotteryCard prefabLotteryCard;
    public List<LotteryCard> lotteryCards = new List<LotteryCard>();
    public float cardOpenDeayTime = 0.2f;

    public LotteryAnimationController lotteryAnimationController;

    //// 테스트 용도 데이터
    //string[] unionNameNormal = { "normal-1", "normal-2", "normal-3", "normal-4", "normal-5", "normal-6", "normal-7", "normal-8" };
    //string[] unionNameHigh = { "high-1", "high-2", "high-3", "high-4", "high-5", "high-6", "high-7", "high-8" };
    //string[] unionNameRare = { "rare-1", "rare-2", "rare-3", "rare-4", "rare-5", "rare-6", "rare-7", "rare-8" };
    //string[] unionNameHero = { "hero-1", "hero-2", "hero-3", "hero-4", "hero-5", "hero-6", "hero-7", "hero-8" };
    //string[] unionNameLegend = { "legend-1", "legend-2", "legend-3", "legend-4", "legend-5", "legend-6", "legend-7", "legend-8" };
    //string[] unionNameUnique = { "unique-1", "unique-2", "unique-3", "unique-4", "unique-5", "unique-6", "unique-7", "unique-8" };

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

    //// 테스트 용도
    //void SetUnionNmaeData()
    //{
    //    unionNameData = new string[6][];
    //    unionNameData[0] = unionNameNormal;
    //    unionNameData[1] = unionNameHigh;
    //    unionNameData[2] = unionNameRare;
    //    unionNameData[3] = unionNameHero;
    //    unionNameData[4] = unionNameLegend;
    //    unionNameData[5] = unionNameUnique;
    //}


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
        PopupUIUpdate();

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

        // // 저장된 데이터 세팅
        // if (data.rewaedUnionIds.Count > 0)
        // {
        //     foreach (var id in data.rewaedUnionIds)
        //         GlobalData.instance.rewardManager.unionRewardQueue.Enqueue(id);
        // }


        // set ui
        PopupUIUpdate();


        SetSummonGradeData(GlobalData.instance.dataManager.GetSummonGradeDataByLevel(summonGradeLevel));
        SetGambleData(GlobalData.instance.dataManager.GetUnionGambleDataBySummonGrade(summonGradeLevel));
        randomGradeValues = GetRandomArrayValue();
        //CreateCards();
        yield return null;
    }

    // 풀링으로 사용할 카드 생성
    void CreateCards()
    {
        for (int i = 0; i < cardsBirthCount; i++)
        {
            var card = Instantiate(prefabLotteryCard, trCardsParent);
            card.SetTxtName("name");
            card.gameObject.SetActive(false);
            lotteryCards.Add(card);
        }
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
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator CardOpen(int roundCount, int payValue, UnityAction gameEndEvent, EnumDefinition.RewardType rewardType)
    {


        if (IsValidGemCount(payValue, rewardType))
        {

            if (rewardType == EnumDefinition.RewardType.unionTicket)
            {
                // pay union ticket
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

            //yield return new WaitForSeconds(0.3f); // 05초 대기 ( 연출 )

            yield return StartCoroutine(CardsOpenEffect());



            // 뽑기 버튼 활성화
            UtilityMethod.SetBtnsInteractableEnable(new List<int> { 17, 18, 19 }, true);

            // 스킵 버튼 비활성화
            UtilityMethod.SetBtnInteractableEnable(33, false);

            // 닫기 버튼 활성화
            UtilityMethod.GetCustomTypeBtnByID(44).interactable = true;

            if (GlobalData.instance.tutorialManager.isUnionGamblingTutorial)
                EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTutorialUnionGamblingEnd);

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
                    //++unionGradeLevel;
                    SetSummonGradeData(GlobalData.instance.dataManager.GetSummonGradeDataByLevel(summonGradeLevel));
                    SetGambleData(GlobalData.instance.dataManager.GetUnionGambleDataBySummonGrade(summonGradeLevel));
                }

                PopupUIUpdate();
            }



            Debug.Log("뽑기 진행 수 : " + curLotteryCount);

            gameEndEvent.Invoke();
        }
        else
        {
            // message popup (보석이 부족합니다)
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 3);
            StopAllCoroutines();
            yield break;
        }

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
        popup.SetSummonCountProgress(totalGambleCount, curSummonGradeData.count);
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
            openedUnionTypeCards.Add(union);
            lottreyRoundCount++;

            //일일 퀘스트 완료 : 유니온 소환
            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.summonUnion);
            yield return null;
        }
    }


    public IEnumerator CardsOpenEffect()
    {
        List<int> unionIndexList = new List<int>();
        if (GlobalData.instance.tutorialManager.isUnionGamblingTutorial)
        {
            // 투토리얼 일때 무조건 0번 뽑기
            unionIndexList.Add(0);
        }
        else
        {
            for (int i = 0; i < openedUnionTypeCards.Count; i++)
            {
                var unionType = openedUnionTypeCards[i];
                var faceIndex = GetRandomFaceIndex();
                var unionIdex = GetUnionIndex(unionType, faceIndex);
                unionIndexList.Add(unionIdex);
                yield return null;
            }
        }


        lotteryAnimationController.gameObject.SetActive(true);
        lotteryAnimationController.StartLotteryAnimation();
        yield return StartCoroutine(lotteryAnimationController.ShowUnionSlotCardOpenProcess(unionIndexList.ToArray()));
        GlobalData.instance.unionManager.AddUnions(unionIndexList);
    }

    int GetRandomFaceIndex()
    {
        return Random.Range(0, 7);
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
