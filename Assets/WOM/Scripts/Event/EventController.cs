using System.Collections;
using UnityEngine;
using static EnumDefinition;
using DG.Tweening;
/// <summary>
/// 몬스터전 이벤트 관리
/// </summary>
public class EventController : MonoBehaviour
{
    GlobalData globalData;
    public bool evalGradeEffectShow = false;
    double dungeonMonsterLeftDamage = 0;

    // 죽기전 남은 데미지 + 추가 데미지
    double dungeonMonsterAddDamage;

    public

    void Start()
    {
        GetManagers();
        AddEvents();

    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    void GetManagers()
    {
        globalData = GlobalData.instance;
    }

    void AddEvents()
    {
        globalData.dungeonPopup.OnButtonClick += DungeonPopupApplyEvent;
        //globalData.dungeonPopup.OnFinishParticle += DungeonPopupFinishParticle;

        EventManager.instance.AddCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnMonsterHit, EvnOnMonsterHit);
        EventManager.instance.AddCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnDungeonMonsterHit, EvnOnDungeonMonsterHit);
        EventManager.instance.AddCallBackEvent<Transform>(CallBackEventType.TYPES.OnMonsterKingHit, EvnOnMonsterHitMonsterKing);
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallengeTimeOut, EvnBossMonsterTimeOut);
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallenge, EvnOnBossMonsterChalleng);
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnEvolutionMonsterChallenge, EvnOnEvolutionGradeChallenge);
        EventManager.instance.AddCallBackEvent<MonsterType>(CallBackEventType.TYPES.OnDungeonMonsterChallenge, EvnOnDungenMonsterChalleng);

    }

    void RemoveEvents()
    {
        EventManager.instance.RemoveCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnMonsterHit, EvnOnMonsterHit);
        EventManager.instance.RemoveCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnDungeonMonsterHit, EvnOnDungeonMonsterHit);
        EventManager.instance.RemoveCallBackEvent<Transform>(CallBackEventType.TYPES.OnMonsterKingHit, EvnOnMonsterHitMonsterKing);
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallengeTimeOut, EvnBossMonsterTimeOut);
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallenge, EvnOnBossMonsterChalleng);
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnEvolutionMonsterChallenge, EvnOnEvolutionGradeChallenge);
        EventManager.instance.RemoveCallBackEvent<MonsterType>(CallBackEventType.TYPES.OnDungeonMonsterChallenge, EvnOnDungenMonsterChalleng);
    }

    bool isMonsterDie = false;
    bool isDungeonMonsterNextLevel = false;
    bool dungeonMonsterPopupClose = false;
    //bool dungeonMonsterPopupFinishParticle = false;

    // 몬스터 타입이 보스 or 진화보스인지 체크
    bool IsBossTypeMonster()
    {
        return globalData.player.curMonsterType == MonsterType.boss || globalData.player.curMonsterType == MonsterType.evolution || globalData.player.curMonsterType == MonsterType.dungeon;
    }
    // 몬스터가 피격될 때 이벤트로 받음
    void EvnOnMonsterHit(EnumDefinition.InsectType insectType, int unionIndex = 0, Transform tr = null)
    {
        if (isMonsterDie) return;

        var damage = globalData.insectManager.GetInsectDamage(insectType, insectType == InsectType.union ? unionIndex : 0, out bool isCritical);

        // GET MONSTER
        var currentMonster = globalData.player.currentMonster;

        var curDamage = damage;
        if (IsBossTypeMonster())
            curDamage = damage * (1 + globalData.statManager.BossDamage() * 0.01f);

        // set monster damage
        currentMonster.hp -= curDamage;

        // monster hit animation 
        currentMonster.inOutAnimator.monsterAnim.SetBool("Hit", true);

        // monster hit shader effect
        currentMonster.inOutAnimator.MonsterHitAnim();
        // ENABLE Floting Text Effect 
        globalData.effectManager.EnableFloatingText(damage, isCritical, tr, insectType);
        // 몬스터 제거시 ( hp 로 판단 )
        if (IsMonseterKill(currentMonster.hp))
        {
            StartCoroutine(MonsterKill(currentMonster));
        }
        // 몬스터 단순 피격시
        else
        {
            // 몬스터 hp text
            globalData.uiController.SetTxtMonsterHp(currentMonster.hp);

            // 몬스터 hp slider
            globalData.uiController.SetSliderMonsterHp(currentMonster.hp);

        }
    }

    void EvnOnMonsterHitMonsterKing(Transform tr = null)
    {
        if (isMonsterDie) return;

        // skill data 의 power 를 가져온다.
        var damage = GlobalData.instance.skillManager.GetSkillInGameDataByType(SkillType.monsterKing).power;

        //곤충의 데미지를 가져온다
        var insectDamage = globalData.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);

        var totalDamage = insectDamage * damage;
        // 보스형 몬스터인지 아닌지 체크
        if (IsBossTypeMonster())
            totalDamage = totalDamage * (1 + globalData.statManager.BossDamage() * 0.01f);

        // 크리티컬이 터지지 않는 공격력 타입임
        globalData.effectManager.EnableFloatingText(totalDamage, false, tr, EnumDefinition.InsectType.none);

        //던전 몬스터가 아니라면 일반 적인 공격을 함
        if (globalData.player.curMonsterType != EnumDefinition.MonsterType.dungeon)
        {
            // GET MONSTER
            var currentMonster = globalData.player.currentMonster;
            // set monster damage
            currentMonster.hp -= totalDamage;
            // monster hit animation 
            currentMonster.inOutAnimator.monsterAnim.SetBool("Hit", true);
            // monster hit shader effect
            currentMonster.inOutAnimator.MonsterHitAnim();

            // 몬스터 제거시 ( hp 로 판단 )
            if (IsMonseterKill(currentMonster.hp))
            {
                StartCoroutine(MonsterKill(currentMonster));
            }
            // 몬스터 단순 피격시
            else
            {
                // 몬스터 hp text
                globalData.uiController.SetTxtMonsterHp(currentMonster.hp);

                // 몬스터 hp slider
                globalData.uiController.SetSliderMonsterHp(currentMonster.hp);

            }

        }
        else
        {
            if (isDungeonMonsterNextLevel) return;

            // 던전 몬스터일경우 던전 Hit와 동일한 매커니즘
            DungeonMonster currentMonster = globalData.monsterManager.GetMonsterDungeon();
            // monster hit animation 
            currentMonster.inOutAnimator.monsterAnim.SetBool("Hit", true);
            // monster hit shader effect
            currentMonster.inOutAnimator.MonsterHitAnim();
            // 적에게 피해를 입히고
            currentMonster.curData.monsterHP -= totalDamage;

            // 몬스터의 HP가 0보다 작면
            if (IsMonseterKill(currentMonster.curData.monsterHP))
            {
                // set next level
                currentMonster.SetNextLevelData();
                // level setting
                UtilityMethod.SetTxtCustomTypeByID(107, $"X {currentMonster.curData.level}");
                // Set Stage Name
                GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.dungeon, currentMonster.stageName);
                // 몬스터 hp text
                globalData.uiController.SetTxtMonsterHp(currentMonster.curData.monsterHP);
                // 몬스터 hp slider
                globalData.uiController.SetSliderDungeonMonsterHP(currentMonster.curData.monsterHP);
            }

        }

    }

    void EvnOnDungeonMonsterHit(EnumDefinition.InsectType insectType, int unionIndex = 0, Transform tr = null)
    {
        if (isDungeonMonsterNextLevel) return;

        var damage = globalData.insectManager.GetInsectDamage(insectType, insectType == InsectType.union ? unionIndex : 0, out bool isCritical);
        //보스 추가 데미지 적용
        var curDamage = damage * (1 + globalData.statManager.BossDamage() * 0.01f);

        //[남은 피해]가 있는 상태에서 추가적인 데미지가 들어오게 되면 남은피해에 더해 준다
        if (dungeonMonsterLeftDamage > 0)
        {
            dungeonMonsterLeftDamage += curDamage;
            return;
        }


        // ENABLE Floting Text Effect 
        globalData.effectManager.EnableFloatingText(curDamage, isCritical, tr, insectType);
        // GET MONSTER
        DungeonMonster currentMonster = globalData.monsterManager.GetMonsterDungeon();
        // monster hit animation 
        currentMonster.inOutAnimator.monsterAnim.SetBool("Hit", true);
        // monster hit shader effect
        currentMonster.inOutAnimator.MonsterHitAnim();

        // 만약 입힐 피해가 몬스터 체력보다 크다면
        if (curDamage > currentMonster.curData.monsterHP)
        {
            // [남은 피해]를 세팅한다
            dungeonMonsterLeftDamage = curDamage - currentMonster.curData.monsterHP;
        }


        if (dungeonMonsterLeftDamage > 0)
        {
            // [남은 피해]가 있다는건 현재 레벨의 몬스터의 체력을 다 깍았다는걸 의미
            while (dungeonMonsterLeftDamage > 0)
            {
                //Debug.Log("남은 피해 : " + dungeonMonsterLeftDamage);
                // 다음 레벨의 몬스터 체력을 세팅하고
                currentMonster.SetNextLevelData();
                //맥스 체력 임시 보관
                var tempMaxHp = currentMonster.curData.monsterHP;
                //세팅된 다음 레벨의 몬스터 체력에 남아 있는 피해를 입힌다
                currentMonster.curData.monsterHP -= dungeonMonsterLeftDamage;
                // [남은 피해] 다시 세팅
                dungeonMonsterLeftDamage = dungeonMonsterLeftDamage - tempMaxHp;

            }


            UtilityMethod.GetCustomTypeImageById(41).fillAmount = 1;

            UtilityMethod.SetTxtCustomTypeByID(107, $"X {currentMonster.curData.level}");
            UtilityMethod.GetTxtCustomTypeByID(107).transform.DOScale(Vector3.one * 1.3f, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                //원래 크기로 되돌리기
                UtilityMethod.GetTxtCustomTypeByID(107).transform.DOScale(Vector3.one, 0.1f)
                    .SetEase(Ease.OutQuad);
            });

            // 레벨 Text UI Update
            // Stage UI Update
            GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.dungeon, currentMonster.stageName);

        }
        else
        {
            // 적에게 피해를 입히고
            currentMonster.curData.monsterHP -= curDamage;
        }

        //몬스터 hp text
        globalData.uiController.SetTxtMonsterHp(currentMonster.curData.monsterHP);
        //몬스터 hp slider
        globalData.uiController.SetSliderDungeonMonsterHP(currentMonster.curData.monsterHP);


        //몬스터의 HP가 0보다 작면
        if (IsMonseterKill(currentMonster.curData.monsterHP))
        {
            // set next level
            currentMonster.SetNextLevelData();
            // level setting
            UtilityMethod.SetTxtCustomTypeByID(107, $"X {currentMonster.curData.level}");
            UtilityMethod.GetTxtCustomTypeByID(107).transform.DOScale(Vector3.one * 1.2f, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                //원래 크기로 되돌리기
                UtilityMethod.GetTxtCustomTypeByID(107).transform.DOScale(Vector3.one, 0.1f)
                    .SetEase(Ease.OutQuad);
            });
            // Set Stage Name
            GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.dungeon, currentMonster.stageName);
            // 몬스터 hp text
            globalData.uiController.SetTxtMonsterHp(currentMonster.curData.monsterHP);
            // 몬스터 hp slider
            globalData.uiController.SetSliderDungeonMonsterHP(currentMonster.curData.monsterHP);

        }

    }




    IEnumerator MonsterKill(MonsterBase currentMonster)
    {

        isMonsterDie = true;

        // 공격 막기
        // globalData.attackController.SetAttackableState(false);

        yield return null;

        // hp text 0으로 표시
        globalData.uiController.SetTxtMonsterHp(0);

        // hp slider
        globalData.uiController.SetSliderMonsterHp(0);

        if (currentMonster.monsterType == MonsterType.boss)
        {
            // 보스 사냥 성공 전환 이펙트
            globalData.effectManager.EnableTransitionEffStageClear();

            // BG Color Change
            globalData.stageManager.bgAnimController.spriteColorAnim.ColorNormalAnim();
        }

        // sfx monster die
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.BossDie);

        //금광 보스 인지 체크
        if (currentMonster.monsterType == MonsterType.gold)
        {
            //금광 몬스터 2배 받을 확률 가져오기
            float pbb = (float)globalData.statManager.GoldMonsterBonus();
            //확률 계산
            float ran = Random.Range(0f, 100f);

            if (ran <= pbb)
            {
                //2배 보상
                currentMonster.gold = currentMonster.gold * 2;
                // GOLD 획득 이미지도 2배로 뿌려줍니다
                yield return StartCoroutine(globalData.effectManager.goldPoolingCont.EnableGoldEffects(currentMonster.goldCount * 2));
            }
            else
            {
                // GOLD 획득 애니메이션
                yield return StartCoroutine(globalData.effectManager.goldPoolingCont.EnableGoldEffects(currentMonster.goldCount));
            }
        }
        else
        {
            // GOLD 획득 애니메이션
            yield return StartCoroutine(globalData.effectManager.goldPoolingCont.EnableGoldEffects(currentMonster.goldCount));
        }

        // 골드 획득
        GainGold(currentMonster);

        // tutorial event ( 몬스터 골드 드랍 획득 )
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTutorialAddGold);

        // 보스일경우 뼈조각 추가 획득
        if (currentMonster.monsterType == MonsterType.boss)
        {
            yield return StartCoroutine(globalData.effectManager.bonePoolingCont.EnableGoldEffects(currentMonster.boneCount));
            // 뼈 조각 획득
            GainBone(currentMonster);
        }

        // monster kill animation 사망 애니메이션 대기
        yield return StartCoroutine(currentMonster.inOutAnimator.MonsterKillMatAnim());
        // 하프라인 위쪽 곤충들 제거
        globalData.insectManager.DisableHalfLineInsects();
        // tutorial event ( 골드 몬스터 사망 )
        if (currentMonster.monsterType == MonsterType.gold)
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterKillGoldMonster);

        // tutorial event ( 보스 몬스터 사망 )
        if (currentMonster.monsterType == MonsterType.boss)
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterKillBossMonster);

        switch (currentMonster.monsterType)
        {
            case MonsterType.normal: StartCoroutine(MonsterDie_Normal()); break;
            case MonsterType.gold: StartCoroutine(MonsterDie_Gold()); break;
            case MonsterType.boss: StartCoroutine(MonsterDie_Boss()); break;
            case MonsterType.evolution: StartCoroutine(MonsterDie_Evolution()); break;
        }

        isMonsterDie = false;
    }





    void GainGold(MonsterBase monster)
    {
        var gold = monster.gold;
        globalData.player.AddGold(gold);
    }
    void GainBone(MonsterBase monster)
    {
        var bone = monster.bone;
        globalData.player.AddBone(bone);
    }

    void GainDice(int value)
    {
        globalData.player.AddDice(value);
    }

    void GainCoal(int value)
    {
        globalData.player.AddCoal(value);
    }

    // 아이템 획득
    IEnumerator GainDungeonMonsterGoods(MonsterType monsterType)
    {
        yield return null;

    }

    // 일반 몬스터 사망시
    IEnumerator MonsterDie_Normal()
    {
        // BG Scroll Animation
        globalData.stageManager.PlayAnimBgScroll(0.2f);

        //phaseCount 0 도달시 골드 몬스터 등장.
        PhaseCounting(out int phaseCount);

        if (IsPhaseCountZero(phaseCount))
        {
            // 보스 도전 버튼 숨김 ( 보스 도전 버튼 있다면 숨김 - 무조건 숨김 )
            //globalData.uiController.btnBossChallenge.gameObject.SetActive(false);
            yield return StartCoroutine(MonsterAppearCor(MonsterType.gold));
        }
        else
        {
            yield return StartCoroutine(MonsterAppearCor(MonsterType.normal));
        }
    }


    #region 골드 몬스터 사망시 프로세스
    // 골드 몬스터 사망시
    /* process */
    // 0 : 골드 몬스터 사망  
    // 2 : 보스 도전 버튼 활성화
    // 3 : 일반 몬스터 데이터 세팅
    // 4 : 현재 몬스터 일반 몬스터로 변경
    // 4 : phaaseCount reset
    // 5 : ui 세팅
    // 6 : 몬스터 등장
    #endregion
    IEnumerator MonsterDie_Gold()
    {

        // BG Scroll Animation
        globalData.stageManager.PlayAnimBgScroll(0.2f);

        // 보스 도전 버튼 활성화 
        globalData.uiController.btnBossChallenge.gameObject.SetActive(true);

        // 보스 도전 가능 상태 설정
        globalData.player.isBossMonsterChllengeEnable = true;

        // phaseCount 리셋
        PhaseCountReset();

        // phase count UI 활성화
        globalData.uiController.SetEnablePhaseCountUI(true);

        // 일일 퀘스트 완료 : 금광보스
        EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.killGoldBoss);

        // 일반 몬스터 등장
        yield return StartCoroutine(MonsterAppearCor(MonsterType.normal));
    }

    //보스 몬스터 사망시
    IEnumerator MonsterDie_Boss()
    {
        // BG Scroll Animation
        globalData.stageManager.PlayAnimBgScroll(1f);
        // side menu show
        SideUIMenuHide(false);

        //하단 메인 메뉴 활성화
        globalData.uiController.MainMenuShow();

        // 타이머 종료
        globalData.bossChallengeTimer.StopAllCoroutines();

        // 타이머 UI 리셋
        globalData.uiController.SetImgTimerFilledRaidal(0);
        globalData.uiController.SetTxtBossChallengeTimer(0);

        // 타이머 UI Disable
        globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);

        // 보스 도전 가능 상태 설정
        globalData.player.isBossMonsterChllengeEnable = false;

        // 포기 버튼 비활성화
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(false);

        // SET STAGE DATA ( 다음 스테이지로 변경 )
        var newStageIdx = globalData.player.stageIdx + 1;
        globalData.player.stageIdx = newStageIdx;

        // set save data
        globalData.saveDataManager.SaveStageDataLevel(newStageIdx);

        // current stage setting
        globalData.player.SetCurrentStageData(globalData.player.stageIdx);

        // phaseCount 리셋
        PhaseCountReset();
        //Change BGM
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);
        // 금광보스 카운트 UI 활성
        globalData.uiController.SetEnablePhaseCountUI(true);

        // stage setting - stage manager 스테이지 데이터와 배경 이미지 전환 애니메이션
        yield return StartCoroutine(globalData.stageManager.SetStageById(globalData.player.stageIdx));

        // set monster data and monster skin
        yield return StartCoroutine(globalData.monsterManager.Init(globalData.player.stageIdx));

        yield return StartCoroutine(MonsterAppearCor(MonsterType.normal));

        // camera zoom In
        globalData.stageManager.bgAnimController.CameraZoomIn();

        // 퀘스트 - 배틀 패스 스테이지 완료 블록 이미지 해제
        globalData.questManager.questPopup.UnlockBattlePassSlot(newStageIdx);

    }

    // 던전 몬스터 도전으로 인한 현재 몬스터 잠시 사라지도록
    // 던전 몬스터 등장

    void EvnOnDungenMonsterChalleng(MonsterType monsterType)
    {
        // 전체 UI 비활성 
        UtilityMethod.EnableUIEventSystem(false);

        var usingKeyCount = GlobalData.instance.monsterManager.GetMonsterDungeon().monsterToDataMap[monsterType].usingKeyCount;

        // 열쇠 사용
        // 열쇠 없으면 광고 키 사용
        var keyCount = globalData.player.GetCurrentDungeonKeyCount(monsterType);
        if (keyCount > 0)
        {
            globalData.player.PayDungeonKeyByMonsterType(monsterType, usingKeyCount);
        }
        else
        {
            globalData.player.PayDungeonADKeyByMonsterType(monsterType, usingKeyCount);
        }

        // UI 업데이트
        globalData.dungeonManager.UpdateDunslotKeyUI(monsterType);

        StopAllCoroutines();

        StartCoroutine(DungeonMonsterAppear(monsterType));
    }
    public IconSpriteFileData iconSpriteFileData;

    IEnumerator DungeonMonsterAppear(MonsterType monsterType)
    {

        // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.goldPoolingCont.DisableGoldEffects());

        // 보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.bonePoolingCont.DisableGoldEffects());

        // transition effect
        globalData.effectManager.EnableTransitionEffDungeonInByType(monsterType);

        // 황금돼지 비활성화
        globalData.goldPigController.EnterOtherView();

        // 공격 막기
        globalData.attackController.SetAttackableState(false);

        // 골드 몬스터 도전 버튼 비활성화 TODO: 던전 몬스터별 버튼 비활성화
        UtilityMethod.GetCustomTypeBtnByID(45).interactable = false;
        UtilityMethod.GetCustomTypeBtnByID(46).interactable = false;
        UtilityMethod.GetCustomTypeBtnByID(47).interactable = false;
        UtilityMethod.GetCustomTypeBtnByID(48).interactable = false;

        // 현재 몬스터 타입 세팅 ( 타이머 종료 이벤트를 위한...)
        globalData.player.curMonsterType = MonsterType.dungeon;
        globalData.uiController.EnableMainMenuCloseBtn(false);

        // 메인 메뉴 활성화
        globalData.uiController.MainMenuHide();


        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            // bg sprite id
            var bgSpriteId = GlobalData.instance.monsterManager.GetMonsterDungeon().monsterToDataMap[monsterType].bgID;

            // bg 변경
            globalData.stageManager.SetDungeonBgImage(bgSpriteId);

            // 금광보스 카운트 UI 숨김
            globalData.uiController.SetEnablePhaseCountUI(false);

            // 보스 도전 버튼 숨김
            globalData.uiController.btnBossChallenge.gameObject.SetActive(false);

            // 하프 라인 위 곤충 모두 제거
            globalData.insectManager.DisableHalfLineInsects();

            // 일반 몬스터 OUT
            StartCoroutine(globalData.player.currentMonster.inOutAnimator.MonsterKillMatAnim());

            //DUNGEON_BOX_ICON_BTN 박스아이콘 활성화
            UtilityMethod.GetCustomTypeGMById(10).gameObject.SetActive(true);

            // 던전 몬스터 레벨 표시
            UtilityMethod.SetTxtCustomTypeByID(107, $"X {1}");

            // side menu hide
            SideUIMenuHide(true);

            GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.dungeon);

            UtilityMethod.GetCustomTypeImageById(46).sprite = iconSpriteFileData.GetBoxIcon(monsterType);

            // 배경 리셋
            globalData.stageManager.bgAnimController.ResetBg();


        }));

        // 배경 리셋
        globalData.stageManager.bgAnimController.SetOffsetY(0f);

        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_DungeonBoss);


        var monster = globalData.monsterManager.GetMonsterDungeon();

        // Show Dungeon Monster
        globalData.monsterManager.ShowMonsterByType(MonsterType.dungeon);

        // 던전 몬스터 데이터 세팅
        yield return StartCoroutine(monster.Init(monsterType));

        // Set Stage Name
        var stageName = monster.stageName;
        GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.dungeon, stageName);


        // 던전 몬스터 등장
        yield return StartCoroutine(monster.inOutAnimator.AnimPositionIn());

        // 전체 UI 활성
        UtilityMethod.EnableUIEventSystem(true);

        // 보스 도전 타이머 활성화
        globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(true);

        // 타이머 시간 설정 
        globalData.bossChallengeTimer.SetTimeValue(monster.curMonsterData.battleTime);

        // 타이머 계산 시작
        globalData.bossChallengeTimer.StartTimer();

        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);

        isMonsterDie = false;
    }

    // // 던전 몬스터 사망시
    // public IEnumerator KillDungeonMonsterKill(MonsterType monsterType)
    // {
    //     var monster = globalData.monsterManager.GetMonsterDungeon();

    //     // 던전 몬스터 데이터 셋팅
    //     monster.SetNextLevelData();

    //     // UI 세팅

    //     yield return null;
    // }




    //진화 몬스터 사망시
    IEnumerator MonsterDie_Evolution()
    {
        // 보스 도전 버튼 활성화 
        if (globalData.player.isBossMonsterChllengeEnable)
            globalData.uiController.btnBossChallenge.gameObject.SetActive(true);

        // 타이머 종료
        globalData.bossChallengeTimer.StopAllCoroutines();

        // 타이머 UI 리셋
        globalData.uiController.SetImgTimerFilledRaidal(0);
        globalData.uiController.SetTxtBossChallengeTimer(0);

        // 타이머 UI Disable
        globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);

        var evalutionLeveld = globalData.evolutionManager.evalutionLeveldx + 1;

        // set save data
        globalData.saveDataManager.SetEvolutionLevel(evalutionLeveld);

        // 진화 보상 지급 및 UI 세팅
        globalData.evolutionManager.SetUI_Pannel_Evolution(evalutionLeveld);

        // 능력치 슬롯 오픈
        globalData.evolutionManager.SetUI_EvolutionSlots(evalutionLeveld);

        // 유니온 장착 슬롯 오픈
        globalData.unionManager.UnlockEquipSlots(evalutionLeveld);

        // 상단의 몬스터 정보([ CANVAS UI SET ])와 재화 정보([ TOP_UI_CANVAS ]) UI 활성화 → 비활성화
        UtilityMethod.GetCustomTypeGMById(6).SetActive(false);
        UtilityMethod.GetCustomTypeGMById(11).SetActive(false);

        // 등급 업그레이드 연출 등장
        globalData.gradeAnimCont.gradeIndex = evalutionLeveld;
        globalData.gradeAnimCont.gameObject.SetActive(true);
        evalGradeEffectShow = true;


        // 훈련 메뉴 -> 진화 메뉴 UI 활성화
        // UtilityMethod.GetCustomTypeGMById(0).SetActive(true);


        // notify icon 활성화
        UtilityMethod.GetCustomTypeGMById(17).SetActive(true);

        // 금광보스 카운트 UI 활성화
        globalData.uiController.SetEnablePhaseCountUI(true);

        // 진화 idx 레벨업
        globalData.evolutionManager.evalutionLeveldx = evalutionLeveld;

        // 곤충 페이스 변경
        GlobalData.instance.insectManager.SetAllInsectFace(evalutionLeveld);

        //진화 몬스터 도전 버튼 활성화
        globalData.evolutionManager.EnableBtnEvolutionMonsterChange(true);

        // 프레임 대기 ( evalutionLeveld 업데이트 대기 )
        yield return new WaitForEndOfFrame();

        // 배경 이미지 변경
        globalData.stageManager.SetBgImage();

        // 진화 자물쇠 UnLock 상태로 Enable 및 필요 주사위 개수 계산하여 적용함.
        globalData.evolutionManager.SetUI_EvolutuinSlotsLockerItems(evalutionLeveld);

        // 진화전 포기 버튼 비활성화
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(false);

        // sfx 진화 성공
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.Evolution_Victory);

        // 스테이지 텍스트 변경
        GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);

        yield return new WaitUntil(() => evalGradeEffectShow == false); // 등급업그레이드 연출이 끝날때까지 대기

        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        // 상단의 몬스터 정보([ CANVAS UI SET ])와 재화 정보([ TOP_UI_CANVAS ]) 비활성화 → UI 활성화
        UtilityMethod.GetCustomTypeGMById(6).SetActive(true);
        UtilityMethod.GetCustomTypeGMById(11).SetActive(true);

        // 메인 메뉴 활성화
        globalData.uiController.MainMenuAllUnSelect();
        globalData.uiController.MainMenuShow();

        // 사이드 메뉴 활성화
        SideUIMenuHide(false);

        yield return new WaitForSeconds(0.5f);// 메인메뉴 등장 애니메이션 연출이 끝날때까지 대기
        // 등급 업그레이트 연출 시간 후 몬스터 등장
        //yield return new WaitForSeconds(3f);

        // 진화 메뉴 활성화
        // globalData.uiController.EnableMenuPanel(MenuPanelType.evolution);


        // 일반 몬스터 등장
        yield return StartCoroutine(MonsterAppearCor(MonsterType.normal));

        // 메뉴 판넬 활성/비활성화 버튼 보임
        // UtilityMethod.GetCustomTypeImageById(47).raycastTarget = true;


        // 황금돼지 활성화
        globalData.goldPigController.ExitOtherView();


    }

    // 몬스터 등장
    public IEnumerator MonsterAppearCor(EnumDefinition.MonsterType monsterType)
    {

        // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.goldPoolingCont.DisableGoldEffects());

        // 보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.bonePoolingCont.DisableGoldEffects());

        // get curret monster data
        var monsterData = globalData.monsterManager.GetMonsterData(monsterType);

        // set hp 
        monsterData.hp = monsterData.hp * (1 - (GlobalData.instance.statManager.MonsterHpLess() * 0.01f));


        // set current monster
        globalData.player.SetCurrentMonster(monsterData);


        // 사용하지 않는 몬스터 숨기기
        globalData.monsterManager.ShowMonsterByType(monsterType);

        // set prev monster type
        globalData.player.SetPervMonsterType(monsterType);

        // set current monster type
        globalData.player.SetCurrentMonsterType(monsterType);

        // set monster data
        if (monsterType == MonsterType.evolution)
        {
            globalData.monsterManager.SetMonsterDataOther(monsterType, globalData.evolutionManager.evalutionLeveldx);
            // set monster skin
            globalData.player.currentMonster.SetSkinById(globalData.evolutionManager.evalutionLeveldx);
        }
        else
        {
            globalData.monsterManager.SetMonsterData(monsterType, globalData.player.stageIdx);
        }

        if (monsterType == MonsterType.normal)
        {
            // 금광보스 카운트 UI 활성
            globalData.uiController.SetEnablePhaseCountUI(true);
        }


        // set current monster hp
        // TODO: 이펙트 연출 추가
        globalData.player.SetCurrentMonsterHP(monsterData.hp);
        // 몬스터 UI 리셋 
        MonsterUiReset();

        // Monster In Animation
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionIn());


        // Tutorial Event ( 골드 몬스터 등장 )
        if (monsterType == MonsterType.gold)
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnStageInGoldMonster);

        // 공격 가능 상태 변경
        if (globalData.uiController.isCastleOpen == false)
            globalData.attackController.SetAttackableState(true);
    }



    public void NormalMonsterIn()
    {
        StartCoroutine(MonsterAppearCor(MonsterType.normal));
    }
    #region 진화전 프로세스
    /*
     0 : 트랜지션 인
     1 : 몬스터 및 스테이지 세팅
     2 : 트랜지션 아웃
     3 : 진화전 몬스터 등장
     4 : 몬스터 사냥
     5 : 몬스터 사냥 성공 -> 진화
     6 : 몬스터 사냥 실패 -> 이전 몬스터 등장  
    */
    #endregion

    // 진화전 도전 버튼 눌렀을때 ( 진화 몬스터 사냥 )
    void EvnOnEvolutionGradeChallenge()
    {
        //StopAllCoroutines();
        StartCoroutine(ProcessEvolutionGradeChallenge());
    }

    IEnumerator ProcessEvolutionGradeChallenge()
    {
        // 전체 UI 비활성 
        UtilityMethod.EnableUIEventSystem(false);

        // 황금돼지 비활성화
        globalData.goldPigController.EnterOtherView();

        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);

        // 하단 메인 메뉴 숨김
        globalData.uiController.MainMenuHide();

        // 트랜지션 효과
        globalData.effectManager.EnableTransitionEffEvolution();

        // 진화전 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {

            // 진화전 포기 버튼 활성화
            UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(true);

            // 금광보스 카운트 UI 숨김
            globalData.uiController.SetEnablePhaseCountUI(false);

            // 보스 도전 버튼 숨김
            globalData.uiController.btnBossChallenge.gameObject.SetActive(false);

            // 하프 라인 위 곤충 모두 제거
            globalData.insectManager.DisableHalfLineInsects();

            // 일반 몬스터 OUT
            StartCoroutine(globalData.player.currentMonster.inOutAnimator.MonsterKillMatAnim());

            // 보스 도전 타이머 활성화
            globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(true);

            // 타이머 시간 설정
            globalData.bossChallengeTimer.SetTimeValue(30f);

            // 타이머 계산 시작
            globalData.bossChallengeTimer.StartTimer();

            // 배경 리셋
            globalData.stageManager.bgAnimController.ResetBg();


            // side menu hide
            // 메뉴 판넬 비활성화
            UtilityMethod.GetCustomTypeImageById(47).raycastTarget = false;
            UtilityMethod.GetCustomTypeImageById(47).enabled = false;
            SideUIMenuHide(true);

            // 배경 교체
            var monsterData = globalData.monsterManager.GetMonsterData(MonsterType.evolution, globalData.evolutionManager.evalutionLeveldx);
            globalData.stageManager.SetDungeonBgImage(monsterData.bgId);

            // 스테이지 텍스트 변경
            var stageName = monsterData.stageName;
            GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.evolution, stageName);
            GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.evolution);


        }));
        globalData.stageManager.bgAnimController.SetOffsetY(0f);
        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_EvolutionBoss);



        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);

        // 전체 UI 활성 
        UtilityMethod.EnableUIEventSystem(true);
        // 진화 몬스터 등장
        StartCoroutine(MonsterAppearCor(MonsterType.evolution));

        isMonsterDie = false;
    }

    void SideUIMenuHide(bool isHide)
    {
        UtilityMethod.GetCustomTypeGMById(15).SetActive(!isHide);
        UtilityMethod.GetCustomTypeGMById(16).SetActive(!isHide);
    }

    // 보스 몬스터 도전 버튼 눌렀을때 이벤트
    void EvnOnBossMonsterChalleng()
    {
        StopAllCoroutines();
        StartCoroutine(ProcessBossMonsterChallenge());
    }
    bool isbossMonsterChallenge;
    IEnumerator ProcessBossMonsterChallenge()
    {


        // side menu hide
        SideUIMenuHide(true);

        //하단 메인 메뉴 비활성화
        //globalData.uiController.MainMenuHide();

        // 하프 라인 위 곤충 모두 제거
        globalData.insectManager.DisableHalfLineInsects();
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_DungeonBoss);

        // 일반 몬스터 OUT
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.MonsterKillMatAnim());

        // 보스 도전 버튼 숨김
        globalData.uiController.btnBossChallenge.gameObject.SetActive(false);

        // 보스 도전 타이머 활성화
        globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(true);

        // 타이머 시간 설정
        globalData.bossChallengeTimer.SetTimeValue(30f);

        // 포기 버튼 활성화
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(true);

        // phase count UI 숨김
        globalData.uiController.SetEnablePhaseCountUI(false);

        // BG Color Change
        globalData.stageManager.bgAnimController.spriteColorAnim.ColorChangeAnim();

        // camera zoom out
        globalData.stageManager.bgAnimController.CameraZoomOut();

        // 보스 몬스터 등장
        StartCoroutine(MonsterAppearCor(MonsterType.boss));

        // 타이머 계산 시작
        globalData.bossChallengeTimer.StartTimer();

        // TODO: 구조적인 변경 필요함. ( MONSTER HIT 이벤트 막는 처리 )
        isMonsterDie = false;
    }

    // 보스 몬스터 시간내에 잡지 못했을때.
    void EvnBossMonsterTimeOut()
    {
        // 현재(보스 몬스터 도전 전) phaseCount 몬스터 재등장 ?? -> 노멀 몬스터 등장하면 됨 phase count 는 따로 카운팅 되고 있으며 하나의 스테이지에 노멀 몬스터 데이터는 모두 동일함.
        switch (globalData.player.curMonsterType)
        {
            case MonsterType.boss: StartCoroutine(ProcessBossMonsterTimeOut()); break;
            case MonsterType.evolution: StartCoroutine(ProcessEvolutionMonsterTimeOut()); break;
            case MonsterType.dungeon: StartCoroutine(ProcessDungeonMonsterTimeOut()); break;
        }
    }

    IEnumerator ProcessBossMonsterTimeOut()
    {

        // side menu show
        SideUIMenuHide(false);

        //하단 메인 메뉴 활성화
        globalData.uiController.MainMenuShow();

        // 하프 라인 위 곤충 모두 제거
        globalData.insectManager.DisableHalfLineInsects();
        //Change BGM
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        // BG Color Change
        globalData.stageManager.bgAnimController.spriteColorAnim.ColorNormalAnim();

        // 보스 몬스터 OUT
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionOut());

        // 포기 버튼 비활성화
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(false);

        // 보스 도전 버튼 활성화
        globalData.uiController.btnBossChallenge.gameObject.SetActive(true);

        // 보스 도전 타이머 비활성화
        globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);

        // phase count UI 활성화
        globalData.uiController.SetEnablePhaseCountUI(true);

        // camera zoom In
        globalData.stageManager.bgAnimController.CameraZoomIn();


        // 일반 몬스터 등장
        StartCoroutine(MonsterAppearCor(MonsterType.normal));

        // tutorial event ( 보스 몬스터 도전 실패 )
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterKillFailedBossMonster);

    }

    IEnumerator ProcessEvolutionMonsterTimeOut()
    {
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);

        yield return StartCoroutine(globalData.globalPopupController.EnableGlobalPopupCor("message", 0));

        // 활성화 된 모든 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();

        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
          {

              // 진화전 포기 버튼 비활성화
              UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(false);

              // 보스 몬스터 OUT
              StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionOut());

              // 보스 도전 타이머 비활성화
              globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);

              // 금광보스 카운트 UI 활성화
              globalData.uiController.SetEnablePhaseCountUI(true);

              // side menu show
              SideUIMenuHide(false);

              // 훈련 메뉴 -> 진화 메뉴 UI 활성화
              UtilityMethod.GetCustomTypeGMById(0).SetActive(true);

              // 배경 이미지 변경
              globalData.stageManager.SetBgImage();

              // 스테이지 텍스트 변경
              GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);

          }));

        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        // 메인 메뉴 활성화
        globalData.uiController.MainMenuAllUnSelect();
        globalData.uiController.MainMenuShow();

        yield return new WaitForSeconds(0.5f);

        //진화 메뉴 활성화
        //globalData.uiController.EnableMenuPanel(MenuPanelType.evolution);
        // 진화 몬스터 도전 버튼 활성화
        globalData.evolutionManager.EnableBtnEvolutionMonsterChange(true);

        // 일반 몬스터 등장
        yield return StartCoroutine(MonsterAppearCor(MonsterType.normal));

        // 황금 돼지 활성화
        globalData.goldPigController.ExitOtherView();

        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);


    }
    private void DungeonPopupApplyEvent()
    {
        dungeonMonsterPopupClose = true;
    }
    // private void DungeonPopupFinishParticle()
    // {
    //     dungeonMonsterPopupFinishParticle = true;
    // }

    //던전 몬스터 전투 종료
    IEnumerator ProcessDungeonMonsterTimeOut()
    {

        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);

        // 활성화 된 모든 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();
        // 하프 라인 위 곤충 모두 제거
        // globalData.insectManager.DisableHalfLineInsects();

        // yield return StartCoroutine(globalData.globalPopupController.EnableGlobalPopupCor("message", 0));

        // 총 보상 재화 가지고 오기
        var monster = globalData.monsterManager.GetMonsterDungeon();
        var monsterType = monster.curMonsterData.monsterType;
        var goodsType = monster.curMonsterData.goodsType;

        //var totalCurrencyAmount = globalData.dataManager.GetDungeonMonsterDataByTypeLevel(monsterType, monster.curLevel);
        var dungeonMonsterData = globalData.dataManager.GetDungeonMonsterDataByTypeLevel(monsterType, monster.curLevel);



        // 캐슬 -> 연구소에 따른 던전 추가보상량
        var addValue = globalData.labBuildingManager.GetInLabBuildGameData(goodsType).value;
        long totalCurrencyAmount = (long)(dungeonMonsterData.currencyAmount + (dungeonMonsterData.currencyAmount * addValue * 0.01f));


        // sfx dungeon monster out
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.End_Batle);

        // 던전 몬스터 팝업 
        globalData.dungeonPopup.gameObject.SetActive(true);

        globalData.dungeonPopup.SetDungeonPopup(goodsType, totalCurrencyAmount);

        //

        // 팝업 파티클이 종료될때까지 대기
        //yield return new WaitUntil(() => dungeonMonsterPopupFinishParticle);

        //팝업 닫기 버튼을 누를때까지 대기
        yield return new WaitUntil(() => dungeonMonsterPopupClose);

        // 재화 획득 TODO: 재화 획득 연출
        AddDungeonMonsterKillReward(goodsType, totalCurrencyAmount);





        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {

            // 배경 이미지 변경
            globalData.stageManager.SetBgImage();

            // 금광보스 카운트 UI 활성화
            globalData.uiController.SetEnablePhaseCountUI(true);

            // 보스 몬스터 OUT
            StartCoroutine(globalData.monsterManager.GetMonsterDungeon().inOutAnimator.AnimPositionOut());

            globalData.monsterManager.GetMonsterDungeon().DungeonMonsterOut();

            // 보스 도전 타이머 비활성화
            globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);

            //DUNGEON_BOX_ICON_BTN 박스아이콘 비활성화
            UtilityMethod.GetCustomTypeGMById(10).gameObject.SetActive(false);

            // side menu show
            SideUIMenuHide(false);

            // 스테이지 텍스트 변경
            GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);

        }));

        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        // 메인 메뉴 활성화
        globalData.uiController.MainMenuShow();
        globalData.uiController.EnableMainMenuCloseBtn(true);
        yield return new WaitForSeconds(0.5f);

        //던전 메뉴 활성화
        globalData.uiController.EnableMenuPanel(MenuPanelType.dungeon);

        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);

        // 일반 몬스터 등장
        StartCoroutine(MonsterAppearCor(MonsterType.normal));

        // 던전 몬스터 도전 버튼 활성화 TODO: 모든 던전 몬스터 고려
        // 골드 몬스터 도전 버튼 비활성화 TODO: 던전 몬스터별 버튼 비활성화
        UtilityMethod.GetCustomTypeBtnByID(45).interactable = true;
        UtilityMethod.GetCustomTypeBtnByID(46).interactable = true;
        UtilityMethod.GetCustomTypeBtnByID(47).interactable = true;
        UtilityMethod.GetCustomTypeBtnByID(48).interactable = true;

        // dungeonMonsterPopupFinishParticle = false;
        dungeonMonsterPopupClose = false;

        // 진화 몬스터 도전 버튼 활성화
        // globalData.evolutionManager.EnableBtnEvolutionMonsterChange(true);

        //진화 메뉴 활성화
        // globalData.uiController.EnableMenuPanel(MenuPanelType.evolution);

        // 황금돼지 활성화
        globalData.goldPigController.ExitOtherView();
    }

    public void AddDungeonMonsterKillReward(GoodsType goodsType, long totalCurrencyAmount)
    {
        switch (goodsType)
        {
            case GoodsType.gold: globalData.player.AddGold(totalCurrencyAmount); break;
            case GoodsType.bone: globalData.player.AddBone(totalCurrencyAmount); break;
            case GoodsType.dice: globalData.player.AddDice(totalCurrencyAmount); break;
            case GoodsType.coal: globalData.player.AddCoal(totalCurrencyAmount); break;
            default: return;
        }
    }

    // 진화전 포기 했을때
    public IEnumerator ProcessEvolutionMonsterGiveUp()
    {
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);

        // 진화전 포기 버튼 비활성화
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(false);

        // 활성화 된 모든 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();

        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {

            // 금광보스 카운트 UI 활성화
            globalData.uiController.SetEnablePhaseCountUI(true);

            // 보스 몬스터 OUT
            StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionOut());

            // 보스 도전 타이머 비활성화
            globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);



            // 배경 이미지 변경
            globalData.stageManager.SetBgImage();

            // 스테이지 텍스트 변경
            GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);

        }));

        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        // 메인 메뉴 활성화
        globalData.uiController.MainMenuAllUnSelect();
        globalData.uiController.MainMenuShow();
        yield return new WaitForSeconds(0.5f);// 메인메뉴 등장 애니메이션 연출이 끝날때까지 대기
        //진화 메뉴 활성화
        //globalData.uiController.EnableMenuPanel(MenuPanelType.evolution);

        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);

        // 일반 몬스터 등장
        StartCoroutine(MonsterAppearCor(MonsterType.normal));

        // 진화 몬스터 도전 버튼 활성화
        globalData.evolutionManager.EnableBtnEvolutionMonsterChange(true);

        // side menu show
        SideUIMenuHide(false);
    }

    // 보스 몬스터 포기 했을때
    public IEnumerator ProcessBossMonsterGiveUp()
    {

        //하단 메인 메뉴 활성화
        globalData.uiController.MainMenuShow();

        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);

        // 진화전 포기 버튼 비활성화
        UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(false);

        // 하프 라인 위 곤충 모두 제거
        globalData.insectManager.DisableHalfLineInsects();

        // BG Color Change
        globalData.stageManager.bgAnimController.spriteColorAnim.ColorNormalAnim();

        //Change BGM
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);
        // 보스 몬스터 OUT
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionOut());

        // 보스 도전 타이머 비활성화
        globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);

        // 보스 도전 버튼 활성화
        globalData.uiController.btnBossChallenge.gameObject.SetActive(true);

        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);

        // camera zoom In
        globalData.stageManager.bgAnimController.CameraZoomIn();

        // 일반 몬스터 등장
        StartCoroutine(MonsterAppearCor(MonsterType.normal));

        // side menu show
        SideUIMenuHide(false);

        // tutorial event ( 보스 몬스터 도전 실패 )
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterKillFailedBossMonster);
    }


    void MonsterUiReset()
    {
        var monsterData = globalData.player.currentMonster;

        // SET HP
        globalData.uiController.SetTxtMonsterHp(monsterData.hp);
        // SLIDE BAR
        globalData.uiController.SetSliderMonsterHp(monsterData.hp);
    }


    /* UTILITY METHOD */
    #region UTILITY MEHTOD
    // 몬스터 제거 판단
    bool IsMonseterKill(double monster_hp)
    {
        return monster_hp <= 0;
    }

    // 골드 몬스터 진입 단계 판단
    bool IsPhaseCountZero(int phaseCount)
    {
        return phaseCount <= 0;
    }

    // 골드 몬스터 진입 단계 카운팅
    void PhaseCounting(out int value)
    {
        value = globalData.player.currentStageData.phaseCount -= 1;
        globalData.uiController.SetTxtPhaseCount(value);
        globalData.uiController.SetSliderPhaseValue(value);
    }

    // 골드 몬스터 진입 단계 리셋
    void PhaseCountReset()
    {
        var resetValue = globalData.player.pahseCountOriginalValue;
        globalData.player.currentStageData.phaseCount = resetValue;
        globalData.uiController.SetTxtPhaseCount(resetValue);
        globalData.uiController.SetSliderPhaseValue(resetValue);
    }

    #endregion
}


