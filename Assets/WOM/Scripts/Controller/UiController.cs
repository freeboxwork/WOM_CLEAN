using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static EnumDefinition;
using ProjectGraphics;
using DG.Tweening;

public class UiController : MonoBehaviour
{
    public CustomTypeDataManager customTypeDataManager;

    [Header("Monster 관련 UI 항목")]
    public TextMeshProUGUI txtMonsterHp;
    public TextMeshProUGUI txtPhaseCount;
    public TextMeshProUGUI txtGold;
    public Button btnBossChallenge;

    [Header("보스몬스터 도전 관련 UI 항목")]
    public TextMeshProUGUI txtBossMonChallengeTimer;
    public Image imgBossMonTimer;
    public Image imgBossMonTimerParent;

    //[Header("뽑기 관련 UI 항목")]
    //public Transform trLotteryGameSet;

    [SerializeField]
    List<GameObject> mainPanels = new List<GameObject>();
    public List<MainBtnSlot> mainButtons = new List<MainBtnSlot>();

    //[Header("진화 UI 관련 항목")]
    //public List<Sprite> evolutionGradeBadgeImages = new List<Sprite>();
    //public List<EvolutionSlot> evolutionSlots = new List<EvolutionSlot>();

    public float menuPannelScrollView_posY_traning;
    public float menuPannelScrollView_posY_union;
    public float menuPannelScrollView_posY_dna;
    public float menuPannelScrollView_posY_shop;

    public FloatingTextValues floatingTextGold;
    public FloatingTextValues floatingTextBone;
    public FloatingTextValues floatingTextGem;

    //public Button btnMainMenuClose;
    MenuPanelType curMenuPanelType = MenuPanelType.none;

    public bool isCastleOpen = false;

    public List<ButtonInteractableCheck> btnInteractableCheckList = new List<ButtonInteractableCheck>();

    public List<GameObject> disablePopups = new List<GameObject>();

    // 추가 재화 표시 항목 
    public TextMeshProUGUI txtUnionTicketCount;
    public TextMeshProUGUI txtDnaTicketCount;
    public TextMeshProUGUI txtCoalCount;

    // 던전 추가 재화 표시 항목
    public TextMeshProUGUI txtDungeonGoldKeyCount;
    public TextMeshProUGUI txtDungeonBoneKeyCount;
    public TextMeshProUGUI txtDungeonDiceKeyCount;
    public TextMeshProUGUI txtDungeonCoalKeyCount;
    public TextMeshProUGUI txtDungeonClearTicketCount;

    public GameObject castleButtonObj;


    public GameObject menuGameObject;
    [SerializeField] CanvasGroup[] canvasGroups;

    bool isMenuHide = false;
    void Start()
    {
    }

    void SetMainPanels()
    {
        foreach (MenuPanelType type in Enum.GetValues(typeof(MenuPanelType)))
        {
            var panel = UtilityMethod.GetCustomTypeGMById((int)type);
            mainPanels.Add(panel);
        }
    }

    /// <summary> UI 관련 정보 세팅 </summary>
    public IEnumerator Init()
    {
        // set panel gameObjcts
        // SetMainPanels();

        // set ui data
        SetUiData();

        // set btn event
        SetBtnEvent();

        // Disable Ui Elements
        DisableUiElements();

        // 재화 UI 세팅 ( 골드 , 뼈조각 , 보석 , 소탕권 UI 초기 세팅 )
        SetGoodsUI();

        // 메인 판넬 스크롤뷰 시작 위치 가져오기 ( 리셋을 위해 )
        GetMainPannelsScrollViewPosY();

        yield return null;

    }

    // 투토리얼 진행시 UI 초기화
    public void AllDisableUI()
    {
        CloseAllMenuPanel();
        // // Disable main menu
        // if (curMenuPanelType != MenuPanelType.none)
        // {
        //     EnableMenuPanel(curMenuPanelType);
        // }
        // disable popup object
        foreach (var popup in disablePopups)
        {
            popup.SetActive(false);
        }

    }

    public void ButtonInteractableCheck(EnumDefinition.RewardType rewardType)
    {
        foreach (var btn in btnInteractableCheckList)
        {
            if (btn.gameObject.activeSelf == false)
                continue;
            if (btn.type == rewardType)
                btn.InteractableCheck();
        }
    }

    // SET PLAYER UI
    void SetGoodsUI()
    {
        //TODO: 저장된 데이터에서 불어와야 함
        var player = GlobalData.instance.player;
        SetTxtGold(player.gold, 0);
        SetTxtBone(player.bone, 0);
        SetTxtGem(player.gem, 0);
        SetTxtDice(player.diceCount);  // 현재 남은 진화 주사위 개수 UI 적용
        SetTxtDnaTicket(player.dnaTicket);
        SetTxtUnionTicket(player.unionTicket);
        SetTxtCoal(player.coal);

        // 던전 추가 UI
        SetTxtDungeonGoldKeyCount(player.dungeonKeys[GoodsType.gold]);
        SetTxtDungeonBoneKeyCount(player.dungeonKeys[GoodsType.bone]);
        SetTxtDungeonDiceKeyCount(player.dungeonKeys[GoodsType.dice]);
        SetTxtDungeonCoalKeyCount(player.dungeonKeys[GoodsType.coal]);
        SetTxtDungeonClearTicketCount(player.clearTicket);

        // 소탕권
        //SetTxtClearTicket();
    }

    // void SetTxtClearTicket()
    // {
    //     var ticket = GlobalData.instance.player.clearTicket;
    //     GlobalData.instance.dungeonEnterPopup.SetTxtClierTicket(ticket);
    // }



    /* SET MONSTER UI */
    public void SetTxtMonsterHp(float value)
    {
        if(value < 0) value = 0;

        //txtMonsterHp.text = txtHP.ToString();
        txtMonsterHp.text = UtilityMethod.ChangeSymbolNumber(value);
    }

    public void SetTxtPhaseCount(int value)
    {
        var countValue = GlobalData.instance.player.pahseCountOriginalValue - value;
        txtPhaseCount.text = countValue.ToString();


    }

    public void SetSliderMonsterHp(float value)
    {
        if(value < 0) value = 0;

        var currentFillAmountValue = UtilityMethod.GetCustomTypeImageById(41).fillAmount;
        var sliderValue = value / GlobalData.instance.player.currentMonsterHp;
        UtilityMethod.GetCustomTypeImageById(41).fillAmount = sliderValue;

        // 0.1초 뒤에 실행
        StartCoroutine(SetSliderBgWithDelay(UtilityMethod.GetCustomTypeImageById(41).fillAmount, currentFillAmountValue));
    }

    public void SetSliderDungeonMonsterHP(float value)
    {
        if(value < 0) value = 0;
        
        var currentFillAmountValue = UtilityMethod.GetCustomTypeImageById(41).fillAmount;
        var sliderValue = value / GlobalData.instance.monsterManager.GetMonsterDungeon().curMonsterHP;
        UtilityMethod.GetCustomTypeImageById(41).fillAmount = sliderValue;

        // 0.1초 뒤에 실행
        StartCoroutine(SetSliderBgWithDelay(UtilityMethod.GetCustomTypeImageById(41).fillAmount, currentFillAmountValue));
    }

    public void SetSliderBg(float value)
    {
        UtilityMethod.GetCustomTypeImageById(44).fillAmount = value;
    }

    private IEnumerator SetSliderBgWithDelay(float target, float current)
    {
        float animationDuration = 0.3f;
        float startTime = Time.time;

        while (Time.time < startTime + animationDuration)
        {
            float elapsed = Time.time - startTime;
            float progress = elapsed / animationDuration;
            float value = Mathf.Lerp(current, target, progress);

            SetSliderBg(value);

            yield return null;
        }

        SetSliderBg(target);

    }


    public void SetSliderPhaseValue(float value)
    {
        var calcValue = (float)value / GlobalData.instance.player.pahseCountOriginalValue;
        var sliderValue = 1 - calcValue;
        UtilityMethod.GetCustomTypeImageById(42).fillAmount = sliderValue;

        EnableGlodMonsterIconOutlineEffect(sliderValue >= 1);
    }

    public void SetTxtGold(float totalValue, float flotingValue, float bonusValue = 0)
    {
        var totalGold = UtilityMethod.ChangeSymbolNumber(totalValue);
        string totalTxt;

        if(bonusValue > 0)
        {
            totalTxt = string.Format("{0}<color=#00FF00> +{1}</color>", UtilityMethod.ChangeSymbolNumber(flotingValue), UtilityMethod.ChangeSymbolNumber(bonusValue));
        }
        else
        {
            totalTxt = UtilityMethod.ChangeSymbolNumber(flotingValue);
        }

        // floting text effect
        if (flotingValue > 0)
        {
            floatingTextGold.gameObject.SetActive(true);

            floatingTextGold.SetText(totalTxt, FloatingTextValues.ValueType.Gold);
        }
        //최종 골드량 표시
        txtGold.text = totalGold;
    }

    public void SetTxtBone(float value, float flotingValue, float bonusValue = 0)
    {
        var changeValue = UtilityMethod.ChangeSymbolNumber(value);
        string totalTxt;

        if(bonusValue > 0)
        {
            totalTxt = string.Format("{0}<color=#00FF00> +{1}</color>", UtilityMethod.ChangeSymbolNumber(flotingValue), UtilityMethod.ChangeSymbolNumber(bonusValue));
        }
        else
        {
            totalTxt = UtilityMethod.ChangeSymbolNumber(flotingValue);
        }

        // floting text effect
        if (flotingValue > 0)
        {
            floatingTextBone.gameObject.SetActive(true);
            floatingTextBone.SetText(totalTxt, FloatingTextValues.ValueType.Bone);
        }
        UtilityMethod.SetTxtCustomTypeByID(60, changeValue);
    }

    public void SetTxtGem(float value, float flotingValue)
    {
        // GEM TEXT 는 단위 변환 없음
        // var changeValue = UtilityMethod.ChangeSymbolNumber(value);
        // var flotingValueChange = UtilityMethod.ChangeSymbolNumber(flotingValue);

        // floting text effect
        if (flotingValue > 0)
        {
            floatingTextGem.gameObject.SetActive(true);
            floatingTextGem.SetText(Mathf.Round(flotingValue).ToString(), FloatingTextValues.ValueType.jewel);
        }


        UtilityMethod.SetTxtCustomTypeByID(79, Mathf.Round(value).ToString());
    }

    public void SetTxtDice(float value)
    {
        var changeValue = UtilityMethod.ChangeSymbolNumber(value);
        UtilityMethod.SetTxtCustomTypeByID(65, changeValue);
    }

    public void SetTxtUnionTicket(float value)
    {
        txtUnionTicketCount.text = value.ToString();
    }
    public void SetTxtDnaTicket(float value)
    {
        txtDnaTicketCount.text = value.ToString();
    }
    public void SetTxtCoal(float value)
    {
        txtCoalCount.text = value.ToString();
    }

    public void SetTxtDungeonGoldKeyCount(float value)
    {
        txtDungeonGoldKeyCount.text = value.ToString();
    }
    public void SetTxtDungeonBoneKeyCount(float value)
    {
        txtDungeonBoneKeyCount.text = value.ToString();
    }
    public void SetTxtDungeonDiceKeyCount(float value)
    {
        txtDungeonDiceKeyCount.text = value.ToString();
    }
    public void SetTxtDungeonCoalKeyCount(float value)
    {
        txtDungeonCoalKeyCount.text = value.ToString();
    }
    public void SetTxtDungeonClearTicketCount(float value)
    {
        txtDungeonClearTicketCount.text = value.ToString();
    }
    public void SetTxtBossChallengeTimer(int value)
    {
        txtBossMonChallengeTimer.text = value.ToString();
    }
    public void SetImgTimerFilledRaidal(float value)
    {
        var sliderValue = 1 - value;
        imgBossMonTimer.fillAmount = sliderValue;
    }
    public void EnableGlodMonsterIconOutlineEffect(bool value)
    {
        UtilityMethod.GetCustomTypeImageById(43).enabled = value;
    }
    void SetUiData()
    {
        txtMonsterHp = customTypeDataManager.GetCustomTypeData_Text(1);
        txtPhaseCount = customTypeDataManager.GetCustomTypeData_Text(2);
        txtGold = customTypeDataManager.GetCustomTypeData_Text(4);
        btnBossChallenge = customTypeDataManager.GetCustomTypeData_Button(0);
        txtBossMonChallengeTimer = customTypeDataManager.GetCustomTypeData_Text(3);
        imgBossMonTimer = customTypeDataManager.GetCustomTypeData_Image(0);
        imgBossMonTimerParent = customTypeDataManager.GetCustomTypeData_Image(1);
        //trLotteryGameSet         = customTypeDataManager.GetCustomTypeData_Transform(0);
    }
    void SetBtnEvent()
    {
        // 보스 몬스터 도전 버튼
        btnBossChallenge.onClick.AddListener(() =>
        {
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnBossMonsterChallenge);
        });

        // 던전 몬스터 - 골드 도전 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(45, () =>
        {
            EnableDungeonEnterPopup(MonsterType.dungeonGold);
        });
        UtilityMethod.SetBtnEventCustomTypeByID(46, () =>
        {
            EnableDungeonEnterPopup(MonsterType.dungeonDice);
        });
        UtilityMethod.SetBtnEventCustomTypeByID(47, () =>
        {
            EnableDungeonEnterPopup(MonsterType.dungeonBone);
        });
        UtilityMethod.SetBtnEventCustomTypeByID(48, () =>
        {
            EnableDungeonEnterPopup(MonsterType.dungeonCoal);
        });

        // 캐슬 빠저 나가기.
        UtilityMethod.SetBtnEventCustomTypeByID(55, () =>
        {
            StartCoroutine(ExitCastlePanel());
        });

        // 메인 판넬 열기
        //foreach (MenuPanelType type in Enum.GetValues(typeof(MenuPanelType)))
        //{
        //    UtilityMethod.SetBtnEventCustomTypeByID(((int)type + 1), () => { EnableMenuPanel(type); });
        //}

        // 메인메뉴 판넬 닫기 버튼
        // btnMainMenuClose.onClick.AddListener(() =>
        // {
        //     EnableMenuPanel(curMenuPanelType);
        //     EnableMainMenuCloseBtn(false);
        // });


        for (int i = 0; i < mainButtons.Count; i++)
        {
            var index = i;
            var btn = mainButtons[index];
            if (btn.menuPanelType == MenuPanelType.castle)
            {
                btn.btnMain.onClick.AddListener(() =>
                {
                    if(isCastleOpen) return;
                    StartCoroutine(EnableCastlePanel());
                });
            }
            else
            {
                btn.btnMain.onClick.AddListener(() =>
                {
                    EnableMenuPanel(btn.menuPanelType);
                });
            }

        }


        // foreach (var btn in mainButtons)
        // {
        //     btn.btnMain.onClick.AddListener(() =>
        //     {

        //     EnableMenuPanel(btn.menuPanelType);

        //         // 무빙캐슬 트랜지션
        //         // if (btn.menuPanelType == MenuPanelType.castle)
        //         // {
        //         //     StartCoroutine(EnableCastlePanel());
        //         // }
        //         // else
        //         // {
        //         //     EnableMenuPanel(btn.menuPanelType);
        //         // }
        //     });
        // }

    }

    public bool CheckOpenMenuPanel()
    {
        foreach (var panel in mainPanels)
        {
            if (panel.activeSelf)
                return true;
        }
        return false;
    }

    public void CloseAllMenuPanel()
    {
        for (int i = 0; i < mainPanels.Count; i++)
        {
            mainPanels[i].SetActive(false);
            mainButtons[i].Select(false);
        }
        //EnableMainMenuCloseBtn(false);
    }
    IEnumerator EnableCastlePanel()
    {
        // isActiveBossChallengeBtn = GlobalData.instance.uiController.btnBossChallenge.gameObject.activeSelf;
        // if (isActiveBossChallengeBtn)
        //     ToggleChallengeBossButton(false);

        isCastleOpen = true;
        //곤충 생성 타이머 종료
        GlobalData.instance.insectSpwanManager.AllTimerStop();
        // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
        StartCoroutine(GlobalData.instance.effectManager.goldPoolingCont.DisableGoldEffects());
        //보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
        StartCoroutine(GlobalData.instance.effectManager.bonePoolingCont.DisableGoldEffects());
        // 공격 불가능 상태 전환
        GlobalData.instance.attackController.SetAttackableState(false);
        // 황금돼지 비활성화
        GlobalData.instance.goldPigController.EnterOtherView();
        // 버튼 클릭 안되게 수정
        UtilityMethod.EnableUIEventSystem(false);

        GlobalData.instance.eventController.IsMonsterDead = true;    

        GlobalData.instance.eventController.StopAllCoroutine();
        // 트렌지션 효과
        GlobalData.instance.effectManager.EnableTransition(EnumDefinition.TransitionTYPE.Castle);

        // 화면전환 효과
        yield return StartCoroutine(GlobalData.instance.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            // BGM CHANGE
            GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Castle);
            // 현재 몬스터 OUT
            // StartCoroutine(GlobalData.instance.player.currentMonster.inOutAnimator.MonsterKillMatAnim());
            var monster = GlobalData.instance.player.currentMonster;
            monster.gameObject.SetActive(false);
            // 활성화된 곤충 모두 비활성화
            GlobalData.instance.insectManager.DisableAllAvtiveInsects();
            
            mainPanels[(int)MenuPanelType.castle].SetActive(true);

            // UI 비활성화
            //UtilityMethod.GetCustomTypeGMById(6).gameObject.SetActive(false);
            EnableCanvadGroup(EnumDefinition.CanvasTYPE.Castle);


        }));


       //StartCoroutine(EnableCastle());

        isCastleOpen = false;

        UtilityMethod.EnableUIEventSystem(true);

    }


    public void EnableCanvadGroup(CanvasTYPE type)
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].alpha = 0;
            canvasGroups[i].interactable = false;
            canvasGroups[i].blocksRaycasts = false;
        }

        canvasGroups[(int)type].alpha = 1;
        canvasGroups[(int)type].interactable = true;
        canvasGroups[(int)type].blocksRaycasts = true;
    }
    

    IEnumerator ExitCastlePanel()
    {
        // if (isActiveBossChallengeBtn)
        // {
        //     isActiveBossChallengeBtn = false;
        //     GlobalData.instance.uiController.btnBossChallenge.gameObject.SetActive(true);
        // }
        if(isCastleOpen) yield break;
        GlobalData.instance.insectSpwanManager.AllTimerStart();

        UtilityMethod.EnableUIEventSystem(false);
        isCastleOpen = true;
        // 화면전환 효과
        yield return StartCoroutine(GlobalData.instance.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            // BGM CHANGE
            GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

            // 캐슬창 비활성화
            mainPanels[(int)MenuPanelType.castle].gameObject.SetActive(false);

            // UI 활성화
            //UtilityMethod.GetCustomTypeGMById(6).gameObject.SetActive(true);
            EnableCanvadGroup(EnumDefinition.CanvasTYPE.Main);

            var monster = GlobalData.instance.player.currentMonster;
            monster.gameObject.SetActive(true);
            // Monster IN
            StartCoroutine(GlobalData.instance.eventController.AppearMonster(MonsterType.normal));

        }));

        isCastleOpen = false;

        // 황금 돼지 출현 타이머 활성
        GlobalData.instance.goldPigController.ExitOtherView();

        // 공격 가능 상태 전환
        GlobalData.instance.attackController.SetAttackableState(true);

        UtilityMethod.EnableUIEventSystem(true);
    }

    void EnableDungeonEnterPopup(MonsterType monsterType)
    {
        // AllDisableMenuPanels();
        // AllUnSelectMenuBtns();
        //if (IsValidDungeonKeyCount(monsterType))
        GlobalData.instance.dungeonEnterPopup.EnablePopup(monsterType);
    }

    public void EnableMenuPanel(MenuPanelType type)
    {
        for (int i = 0; i < mainPanels.Count; i++)
        {
            if (i == (int)type)
            {
                curMenuPanelType = type;
                //Debug.Log(type);
                var enableValue = !mainPanels[i].activeSelf;

                //btnMainMenuClose.interactable = enableValue;

                mainPanels[i].SetActive(enableValue);
                mainButtons[i].Select(enableValue);
                ResetMainPannelScrollViewPosY(type);

                if (type == MenuPanelType.training)
                {
                    GlobalData.instance.traningManager.EnableFirstSubMenuPanel();
                }

                //Debug.Log("EnableMenuPanel : " + type + " / " + enableValue);
                if (enableValue == false)
                {
                    curMenuPanelType = MenuPanelType.none;
                }
            }
            else
            {
                mainButtons[i].Select(false);
                mainPanels[i].SetActive(false);
            }
        }
    }
    void ResetMainPannelScrollViewPosY(MenuPanelType type)
    {
        switch (type)
        {
            case MenuPanelType.training:
                SetMenuPannelScrollView_Pos(0, menuPannelScrollView_posY_traning);
                break;
            case MenuPanelType.union:
                SetMenuPannelScrollView_Pos(1, menuPannelScrollView_posY_union);
                break;
            case MenuPanelType.dna:
                SetMenuPannelScrollView_Pos(2, menuPannelScrollView_posY_dna);
                break;
            case MenuPanelType.shop:
                SetMenuPannelScrollView_Pos(3, menuPannelScrollView_posY_shop);
                break;
        }
    }

    void SetMenuPannelScrollView_Pos(int id, float changePosY)
    {
        var curPos = UtilityMethod.GetCustomTypeTrById(id).localPosition;
        var changePos = new Vector3(curPos.x, changePosY, curPos.z);
        UtilityMethod.GetCustomTypeTrById(id).localPosition = changePos;
    }

    void GetMainPannelsScrollViewPosY()
    {
        menuPannelScrollView_posY_traning = UtilityMethod.GetCustomTypeTrById(0).localPosition.y;
        menuPannelScrollView_posY_union = UtilityMethod.GetCustomTypeTrById(1).localPosition.y;
        menuPannelScrollView_posY_dna = UtilityMethod.GetCustomTypeTrById(2).localPosition.y;
        menuPannelScrollView_posY_shop = UtilityMethod.GetCustomTypeTrById(3).localPosition.y;
    }


    public void AllDisableMenuPanels()
    {
        foreach (var panel in mainPanels)
            panel.SetActive(false);
    }

    public void AllUnSelectMenuBtns()
    {
        foreach (var mainBtn in mainButtons)
            mainBtn.Select(false);
    }



    // 초기화시 UI 오브젝트 비활성화 
    void DisableUiElements()
    {
        btnBossChallenge.gameObject.SetActive(false);
        imgBossMonTimerParent.gameObject.SetActive(false);
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(false);
    }

    public void SetEnablePhaseCountUI(bool value)
    {
        UtilityMethod.GetCustomTypeGMById(7).SetActive(value);
    }


    public void MainMenuHide()
    {
        AllDisableUI();
        MainMenuAllUnSelect();
        menuGameObject.SetActive(false);
        //menuRectTrans.DOKill();
        //menuRectTrans.DOAnchorPos(new Vector2(0f, -150f), 0.5f).SetEase(Ease.InOutExpo);
    }

    public void MainMenuShow()
    {
        MainMenuAllUnSelect();
        menuGameObject.SetActive(true);

        //menuRectTrans.DOKill();
        //menuRectTrans.DOAnchorPos(new Vector2(0f, 0f), 0.5f).SetEase(Ease.InOutExpo);
    }

    public void MainMenuAllUnSelect()
    {
        foreach (var mainBtn in mainButtons)
            mainBtn.Select(false);
    }


    public void ToggleChallengeBossButton(bool isActive)
    {
        btnBossChallenge.gameObject.SetActive(isActive);
    }

    public void ToggleBossTimer(bool isActive)
    {
        imgBossMonTimerParent.gameObject.SetActive(isActive);
    }

    public void ToggleEscapeBossButton(bool isActive)
    {
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(isActive);
    }
    void ToggleSideButtons(bool isActive)
    {
        UtilityMethod.GetCustomTypeGMById(15).SetActive(isActive);
        UtilityMethod.GetCustomTypeGMById(16).SetActive(isActive);

        if (isActive)
        {
            MainMenuShow();
        }
        else
        {
            MainMenuHide();
        }
    }

    public void ShowUI(bool isActive)
    {
        if(isActive)
        {
            MainMenuShow();
        }
        else
        {
            MainMenuHide();
        }
        //이벤트,광고
        UtilityMethod.GetCustomTypeGMById(15).SetActive(isActive);
        //캐슬
        UtilityMethod.GetCustomTypeGMById(16).SetActive(isActive);
        MainMenuAllUnSelect();
    }
}