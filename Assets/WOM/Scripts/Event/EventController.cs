using System.Collections;
using UnityEngine;
using static EnumDefinition;
using DG.Tweening;

/// <summary>
/// 몬스터전 이벤트 관리
/// </summary>
public class EventController : MonoBehaviour
{
    public IconSpriteFileData iconSpriteFileData;

    GlobalData globalData;
    public bool evalGradeEffectShow = false;
    float dungeonMonsterLeftDamage = 0;
    bool isInsectMovementStop = false;
    bool isMonsterDead = false;
    bool isDungeonMonsterNextLevel = false;
    bool dungeonMonsterPopupClose = false;

    bool isRememberActiveBossButton = false;

    bool isEnterFail = false;

    public bool debugTimeScale = false;
    public float addTimeScale = 0;

    bool bossMonsterAttack = false;
    bool isBossMonsterAttackMotion = false;

    void Start()
    {
        globalData = GlobalData.instance;
        AddEvents();

        if (debugTimeScale)
        {
            Time.timeScale = addTimeScale;
        }
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    void AddEvents()
    {
        globalData.dungeonRewardPopup.OnButtonClick += DungeonPopupApplyEvent;
        EventManager.instance.AddCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnMonsterHit, EvnOnMonsterHit);
        EventManager.instance.AddCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnDungeonMonsterHit, EvnOnDungeonMonsterHit);
        EventManager.instance.AddCallBackEvent<Transform>(CallBackEventType.TYPES.OnMonsterKingHit, EvnOnMonsterHitMonsterKing);
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallengeTimeOut, EvnTimeOut);
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallenge, EvnOnBossMonsterChalleng);
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnEvolutionMonsterChallenge, EvnOnEvolutionMonsterChallenge);
        EventManager.instance.AddCallBackEvent<MonsterType>(CallBackEventType.TYPES.OnDungeonMonsterChallenge, EvnOnDungenMonsterChallenge);

    }

    void RemoveEvents()
    {
        EventManager.instance.RemoveCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnMonsterHit, EvnOnMonsterHit);
        EventManager.instance.RemoveCallBackEvent<EnumDefinition.InsectType, int, Transform>(CallBackEventType.TYPES.OnDungeonMonsterHit, EvnOnDungeonMonsterHit);
        EventManager.instance.RemoveCallBackEvent<Transform>(CallBackEventType.TYPES.OnMonsterKingHit, EvnOnMonsterHitMonsterKing);
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallengeTimeOut, EvnTimeOut);
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnBossMonsterChallenge, EvnOnBossMonsterChalleng);
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnEvolutionMonsterChallenge, EvnOnEvolutionMonsterChallenge);
        EventManager.instance.RemoveCallBackEvent<MonsterType>(CallBackEventType.TYPES.OnDungeonMonsterChallenge, EvnOnDungenMonsterChallenge);
    }

    public void StopAllCoroutine()
    {
        StopAllCoroutines();
        // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.goldPoolingCont.DisableGoldEffects());

        // 보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.bonePoolingCont.DisableGoldEffects());
    }

    //===================================================================================================================================================================================
    #region MONSTER HIT

    void EvnOnMonsterHit(EnumDefinition.InsectType insectType, int unionIndex = 0, Transform tr = null)
    {
        if (IsMonsterDead) return;
        if (isBossMonsterAttackMotion) return;

        var damage = globalData.insectManager.GetInsectDamage(insectType, insectType == InsectType.union ? unionIndex : 0, out bool isCritical);
        // GET MONSTER
        var currentMonster = globalData.player.currentMonster;
        var curDamage = damage;
        if (IsBossTypeEliteMonster())
            curDamage = damage * (1 + globalData.statManager.BossDamage() * 0.01f);
        // set monster damage
        currentMonster.hp -= curDamage;

        // monster hit animation 
        currentMonster.inOutAnimator.monsterAnim.SetBool("Hit", true);

        // monster hit shader effect
        currentMonster.inOutAnimator.MonsterHitAnim();
        // ENABLE Floting Text Effect 
        globalData.effectManager.EnableFloatingText(damage, isCritical, tr, insectType);

        // 몬스터 체력이 0보다 작으면 죽음 처리
        if (IsMonseterKill(currentMonster.hp))
        {
            bossMonsterAttack = false;
            StartCoroutine(MonsterKill(currentMonster));
        }
        // 몬스터 단순 피격시
        else
        {
            // 몬스터 hp text
            globalData.uiController.SetTxtMonsterHp(currentMonster.hp);

            // 몬스터 hp slider
            globalData.uiController.SetSliderMonsterHp(currentMonster.hp);

            // 보스 몬스터 hp 50% 이하일때 단 한번 공격 발동
            if (IsMonsterHpHalfLess(currentMonster.hp) && bossMonsterAttack == false && IsBossMonster())
            {
                bossMonsterAttack = true;
                globalData.monsterAttackManager.AttackMotion();
            }
        }
    }

    public void SetBossMonsterAttackMotion(bool value)
    {
        isBossMonsterAttackMotion = value;
    }
    bool IsBossMonster()
    {
        return GlobalData.instance.player.currentMonster.monsterType == EnumDefinition.MonsterType.boss;
    }

    // monster hp 50% less
    bool IsMonsterHpHalfLess(float hp)
    {
        var origionalHp = GlobalData.instance.player.currentMonsterHp;
        var curHp = hp;
        //if monster hp is less than 50%
        var returnValue = curHp <= (origionalHp * 0.5f);
        //Debug.Log($"IsMonsterHpHalfLess : {returnValue}  " + $"curHp : {curHp}  " + $"origionalHp : {origionalHp}");
        return returnValue;
    }

    void EvnOnMonsterHitMonsterKing(Transform tr = null)
    {
        if (IsMonsterDead) return;

        // skill data 의 power 를 가져온다.
        var damage = GlobalData.instance.skillManager.GetSkillInGameDataByType(SkillType.monsterKing).power;

        //곤충의 데미지를 가져온다
        var insectDamage = globalData.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);

        var totalDamage = insectDamage * damage;
        // 보스형 몬스터인지 아닌지 체크
        if (IsBossTypeEliteMonster())
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
        if (isDungeonMonsterNextLevel || IsMonsterDead) return;

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


            GlobalData.instance.uiController.monsterHPSlider.value = 1f;

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
    #endregion

    //===================================================================================================================================================================================
    #region Monster DEAD
    IEnumerator MonsterKill(MonsterBase currentMonster)
    {
        IsMonsterDead = true;

        // 몬스터 타입이 보스,진화라면 포기 버튼 비활성화
        if (currentMonster.monsterType > MonsterType.gold)
        {
            //보스 타이머 중지
            globalData.bossChallengeTimer.StopBossTimer(true);
            //포기 버튼 게임오브젝트 비활성화
            globalData.uiController.ToggleEscapeBossButton(false);
            //골드 몬스터 제거 퀘스트 이벤트 실행
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnQuestPattern_002);
        }

        // HP Text 및 Slider 0으로 표시
        globalData.uiController.SetTxtMonsterHp(0);
        globalData.uiController.SetSliderMonsterHp(0);

        // sfx monster die
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.BossDie);
        // monster kill animation 사망 애니메이션 대기
        yield return StartCoroutine(currentMonster.inOutAnimator.MonsterKillMatAnim());

        //골드 뿌리는 이펙트
        yield return StartCoroutine(globalData.effectManager.goldPoolingCont.EnableGoldEffects(currentMonster.goldCount));

        //금광 보스 인지 체크
        if (currentMonster.monsterType == MonsterType.gold)
        {
            //골드 2배획득 확률 계산
            currentMonster.gold = CalculateGoldFromGoldMonster(currentMonster.gold);
        }
        // 보스일경우 뼈조각 추가 획득
        else if (currentMonster.monsterType == MonsterType.boss)
        {

            // 타이머 종료
            globalData.bossChallengeTimer.StopBossTimer(true);
            yield return StartCoroutine(globalData.effectManager.bonePoolingCont.EnableGoldEffects(currentMonster.boneCount));
            //곤충들 슬로우 모션
            yield return StartCoroutine(BossDefeatSlowMotionEffect());
            // tutorial event ( 보스 몬스터 사망 )
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterKillBossMonster);
        }
        else if (currentMonster.monsterType == MonsterType.evolution)
        {
            // 진화전 포기 버튼 비활성화
            globalData.uiController.ToggleEscapeBossButton(false);
            // 타이머 종료
            globalData.bossChallengeTimer.StopBossTimer(true);
            //곤충들 슬로우 모션
            yield return StartCoroutine(BossDefeatSlowMotionEffect());
        }

        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.CoinPickUp);

        //재화획득
        GainGold(currentMonster);
        if (currentMonster.monsterType == MonsterType.boss) GainBone(currentMonster);

        // 하프라인 위쪽 곤충들 제거
        globalData.insectManager.DisableHalfLineInsects();
        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();

        // tutorial event ( 골드 몬스터 사망 )
        if (currentMonster.monsterType == MonsterType.gold)
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterKillGoldMonster);
        // tutorial event ( 몬스터 골드 드랍 획득 )
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTutorialAddGold);



        IsInsectMovementStop = false;

        //몬스터가 죽은 후 몬스터 타입에 따른 후처리
        DieMonster(currentMonster.monsterType);
    }




    IEnumerator BossDefeatSlowMotionEffect()
    {
        IsInsectMovementStop = true;
        if (debugTimeScale == false)
        {
            Time.timeScale = 0.2f;
            yield return new WaitForSecondsRealtime(0.9f);
            Time.timeScale = 1;
        }
        // 보스 사냥 성공 전환 이펙트FAppr
        GlobalData.instance.effectManager.EnableTransition(EnumDefinition.TransitionTYPE.ClearBoss);
    }

    float CalculateGoldFromGoldMonster(float gold)
    {
        float dropGold = gold;
        //금광 몬스터 2배 받을 확률 가져오기
        float pbb = globalData.statManager.GoldMonsterBonus();
        //확률 계산
        float ran = UnityEngine.Random.Range(0f, 100f);

        if (ran <= pbb)
        {
            return dropGold = dropGold * 2;
        }
        return dropGold;
    }


    void DieMonster(MonsterType type)
    {
        switch (type)
        {
            case MonsterType.normal: StartCoroutine(MonsterDie_Normal()); break;
            case MonsterType.gold: StartCoroutine(MonsterDie_Gold()); break;
            case MonsterType.boss: StartCoroutine(MonsterDie_Boss()); break;
            case MonsterType.evolution: StartCoroutine(MonsterDie_Evolution()); break;
        }
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
            if (IsMonsterDead == false) yield break;

            yield return StartCoroutine(AppearMonster(MonsterType.gold));
        }
        else
        {
            if (IsMonsterDead == false) yield break;

            yield return StartCoroutine(AppearMonster(MonsterType.normal));
        }
    }
    // 골드 몬스터 사망시
    IEnumerator MonsterDie_Gold()
    {
        if (IsMonsterDead == false) yield break;

        // BG Scroll Animation
        globalData.stageManager.PlayAnimBgScroll(0.2f);

        // phaseCount 리셋
        PhaseCountReset();

        // 일일 퀘스트 완료 : 금광보스
        EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.killGoldBoss);


        isRememberActiveBossButton = true;
        globalData.uiController.ToggleChallengeBossButton(true);

        // 일반 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.normal));
        // 보스 도전 버튼 활성화 
        // 보스 도전 가능 상태 설정
        globalData.player.isBossMonsterChllengeEnable = true;
    }
    //보스 몬스터 사망시
    IEnumerator MonsterDie_Boss()
    {
        if (IsMonsterDead == false) yield break;

        // BG Scroll Animation
        globalData.stageManager.PlayAnimBgScroll(1f);

        //하단 메인 메뉴 활성화
        globalData.uiController.MainMenuShow();
        // 타이머 UI 리셋
        globalData.uiController.SetImgTimerFilledRaidal(0);

        globalData.uiController.SetTxtBossChallengeTimer(0);
        // 타이머 UI Disable
        globalData.uiController.ToggleBossTimer(false);

        isRememberActiveBossButton = false;
        // 보스 도전 가능 상태 설정
        globalData.player.isBossMonsterChllengeEnable = false;

        // SET STAGE DATA ( 다음 스테이지로 변경 )
        var newStageIdx = globalData.player.stageIdx + 1;

        // QUEST EVENT - 스테이지 완료
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnQuestPattern_003);

        if (newStageIdx >= StaticDefine.MAX_STAGE)
        {
            newStageIdx = StaticDefine.MAX_STAGE;
        }

        globalData.player.stageIdx = newStageIdx;

        // set save data
        globalData.saveDataManager.SaveStageDataLevel(newStageIdx);

        // current stage setting
        globalData.player.SetCurrentStageData(globalData.player.stageIdx);

        // phaseCount 리셋
        PhaseCountReset();
        //Change BGM
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        //Debug.Log("globalData.player.stageIdx:"+globalData.player.stageIdx);
        // stage setting - stage manager 스테이지 데이터와 배경 이미지 전환 애니메이션
        yield return StartCoroutine(globalData.stageManager.SetStageById(globalData.player.stageIdx));

        // set monster data and monster skin
        yield return StartCoroutine(globalData.monsterManager.Init(globalData.player.stageIdx));
        // camera zoom In
        globalData.stageManager.bgAnimController.CameraZoomIn();

        //만약 자동 보스 도전 버튼토글이 On이라면
        if (globalData.uiController.IsToggleAutoBossChallengeIsOn())
        {
            // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
            StartCoroutine(globalData.effectManager.goldPoolingCont.DisableGoldEffects());
            //보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
            StartCoroutine(globalData.effectManager.bonePoolingCont.DisableGoldEffects());
            EvnOnBossMonsterChalleng();

        }
        else
        {
            yield return StartCoroutine(AppearMonster(MonsterType.normal));

        }
        // 황금돼지 활성화
        globalData.goldPigController.ExitOtherView();
        // 퀘스트 - 배틀 패스 스테이지 완료 블록 이미지 해제
        globalData.questManager.questPopup.UnlockBattlePassSlot(globalData.player.stageIdx);

    }
    //진화 몬스터 사망시
    IEnumerator MonsterDie_Evolution()
    {
        // 타이머 UI 리셋
        globalData.uiController.SetImgTimerFilledRaidal(0);
        globalData.uiController.SetTxtBossChallengeTimer(0);

        // 타이머 UI Disable
        globalData.uiController.imgBossMonTimerParent.gameObject.SetActive(false);

        var evalutionLeveld = globalData.evolutionManager.evalutionLeveldx + 1;

        // set save data
        globalData.saveDataManager.SetEvolutionLevel(evalutionLeveld);
        globalData.saveDataManager.SaveDataToFile();

        // 진화 보상 지급 및 UI 세팅
        globalData.evolutionManager.SetUI_Pannel_Evolution(evalutionLeveld);

        // 능력치 슬롯 오픈
        globalData.evolutionManager.SetUI_EvolutionSlots(evalutionLeveld);

        // 유니온 장착 슬롯 오픈
        globalData.unionManager.UnlockEquipSlots(evalutionLeveld);

        // 상단의 몬스터 정보([ CANVAS UI SET ])와 재화 정보([ TOP_UI_CANVAS ]) UI 활성화 → 비활성화
        GlobalData.instance.uiController.ShowMainCanvas(false);

        UtilityMethod.GetCustomTypeGMById(11).SetActive(false);

        // 등급 업그레이드 연출 등장
        globalData.gradeAnimCont.gradeIndex = evalutionLeveld;
        globalData.gradeAnimCont.gameObject.SetActive(true);
        evalGradeEffectShow = true;

        // notify icon 활성화
        UtilityMethod.GetCustomTypeGMById(17).SetActive(true);

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

        // sfx 진화 성공
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.Evolution_Victory);

        // 스테이지 텍스트 변경
        GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);

        yield return new WaitUntil(() => evalGradeEffectShow == false); // 등급업그레이드 연출이 끝날때까지 대기
                                                                        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();
        // 하프 라인 위 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();
        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        // 상단의 몬스터 정보([ CANVAS UI SET ])와 재화 정보([ TOP_UI_CANVAS ]) 비활성화 → UI 활성화
        GlobalData.instance.uiController.ShowMainCanvas(true);

        UtilityMethod.GetCustomTypeGMById(11).SetActive(true);

        // 메인 메뉴 활성화
        globalData.uiController.MainMenuAllUnSelect();
        //UI 활성화
        globalData.uiController.ShowUI(true);
        //yield return new WaitForSeconds(0.5f);// 메인메뉴 등장 애니메이션 연출이 끝날때까지 대기
        // 등급 업그레이트 연출 시간 후 몬스터 등장

        // 진화 메뉴 활성화
        // globalData.uiController.EnableMenuPanel(MenuPanelType.evolution);

        // camera zoom In
        globalData.stageManager.bgAnimController.CameraZoomIn();
        // 일반 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.normal));

        // 메뉴 판넬 활성/비활성화 버튼 보임
        // UtilityMethod.GetCustomTypeImageById(47).raycastTarget = true;


        // 황금돼지 활성화
        globalData.goldPigController.ExitOtherView();


    }
    #endregion
    //===================================================================================================================================================================================
    #region 몬스터 생성 및 등장
    // 몬스터 등장
    public IEnumerator AppearMonster(EnumDefinition.MonsterType monsterType)
    {

        IsMonsterDead = false;
        isInsectMovementStop = false;

        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);

        if (monsterType > EnumDefinition.MonsterType.gold)
        {
            //보스 몬스터 등장시 사이드 메뉴 비활성화
            globalData.uiController.ShowUI(false);
        }
        else
        {
            //일반, 골드 몬스터 등장시 사이드 메뉴 활성화
            globalData.uiController.ShowUI(true);
        }


        // 몬스터 타입에 따른 데이터 불러오기
        var monsterData = globalData.monsterManager.GetMonsterData(monsterType);

        // 몬스터 체력 세팅
        monsterData.hp = monsterData.hp * (1 - (GlobalData.instance.statManager.MonsterHpLess() * 0.01f));

        // 현재 몬스터 Player에 세팅
        globalData.player.SetCurrentMonster(monsterData);

        // 현재 몬스터 타입 Player에 세팅
        globalData.player.SetCurrentMonsterType(monsterType);

        // 사용하지 않는 몬스터 숨기기
        globalData.monsterManager.ShowMonsterByType(monsterType);

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

        if (monsterType == MonsterType.normal || monsterType == MonsterType.gold)
        {
            // BG Color Change
            globalData.stageManager.bgAnimController.spriteColorAnim.ColorNormalAnim();
            //globalData.uiController.SetEnablePhaseCountUI(true);
        }
        // 금광보스 카운트 UI 활성

        SetEnablePhaseCountUI(monsterType);
        // set current monster hp
        // TODO: 이펙트 연출 추가
        globalData.player.SetCurrentMonsterHP(monsterData.hp);
        // 몬스터 UI 세팅
        MonsterUiSetting();

        // Monster In Animation
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionIn());

        // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.goldPoolingCont.DisableGoldEffects());

        // 보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.bonePoolingCont.DisableGoldEffects());
        // 공격 가능 상태 변경
        if (globalData.uiController.isCastleOpen == false)
            globalData.attackController.SetAttackableState(true);

        // Tutorial Event ( 골드 몬스터 등장 )
        if (monsterType == MonsterType.gold)
        {
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnStageInGoldMonster);
        }


    }

    //몬스터 타입에 따라 금광 카운트 ui 활성/비활성
    void SetEnablePhaseCountUI(EnumDefinition.MonsterType type)
    {
        switch (type)
        {
            case MonsterType.normal:
                globalData.uiController.SetEnablePhaseCountUI(true);
                globalData.uiController.ShowToggleAutoBossChallenge(true);
                break;

            case MonsterType.gold:
                globalData.uiController.SetEnablePhaseCountUI(true);
                globalData.uiController.ShowToggleAutoBossChallenge(true);

                break;
            case MonsterType.boss:
                globalData.uiController.SetEnablePhaseCountUI(false);
                globalData.uiController.ShowToggleAutoBossChallenge(false);

                break;

            case MonsterType.evolution:
                globalData.uiController.SetEnablePhaseCountUI(false);
                globalData.uiController.ShowToggleAutoBossChallenge(false);

                break;

            case MonsterType.dungeon:
                globalData.uiController.SetEnablePhaseCountUI(false);
                globalData.uiController.ShowToggleAutoBossChallenge(false);


                break;

        }
    }

    #endregion
    //===================================================================================================================================================================================
    #region 보스 몬스터 프로세스 (도전/시간종료/포기)
    void EvnOnBossMonsterChalleng()
    {
        StartCoroutine(ChallengeBoss());
    }
    //도전
    IEnumerator ChallengeBoss()
    {
        IsMonsterDead = false;
        // 황금돼지 비활성화
        globalData.goldPigController.EnterOtherView();

        globalData.uiController.ShowUI(false);
        // 보스 도전 버튼 숨김
        globalData.uiController.ToggleChallengeBossButton(false);

        GlobalData.instance.effectManager.EnableTransition(EnumDefinition.TransitionTYPE.Boss);
        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();
        // 하프 라인 위 곤충 모두 제거
        globalData.insectManager.DisableHalfLineInsects();

        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_DungeonBoss);

        // 일반 몬스터 OUT
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.MonsterKillMatAnim());
        // BG Color Change
        globalData.stageManager.bgAnimController.spriteColorAnim.ColorChangeAnim();

        // camera zoom out
        globalData.stageManager.bgAnimController.CameraZoomOut_Boss();

        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.BossIn);

        // 보스 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.boss));

        // 보스 도전 타이머 활성화
        globalData.uiController.ToggleBossTimer(true);

        // 포기 버튼 활성화
        globalData.uiController.ToggleEscapeBossButton(true);

        // 타이머 계산 시작
        globalData.bossChallengeTimer.StartTimer();

        // TODO: 구조적인 변경 필요함. ( MONSTER HIT 이벤트 막는 처리 )
    }
    //시간종료 / 포기
    public IEnumerator FailedChallengBoss()
    {
        isEnterFail = true;

        IsMonsterDead = true;
        //하단 메인 메뉴 활성화
        globalData.uiController.MainMenuShow();
        globalData.bossChallengeTimer.StopBossTimer(true);
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);

        // 포기 버튼 비활성화
        globalData.uiController.ToggleEscapeBossButton(false);
        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();
        // 활성화 된 모든 곤충 모두 제거
        globalData.insectManager.DisableHalfLineInsects();

        isEnterFail = false;

        //Change BGM
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        // 보스 도전 타이머 비활성화
        globalData.uiController.ToggleBossTimer(false);

        // 보스 몬스터 OUT
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionOut());

        // camera zoom In
        globalData.stageManager.bgAnimController.CameraZoomIn();
        globalData.uiController.SetToggleAutoBossChallenge(false);
        // 기존 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.normal));
        // 도전 버튼 활성화
        globalData.uiController.ToggleChallengeBossButton(true);
        // 황금돼지 활성화
        globalData.goldPigController.ExitOtherView();
        // tutorial event ( 보스 몬스터 도전 실패 )
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterKillFailedBossMonster);
    }
    #endregion
    //===================================================================================================================================================================================
    #region 진화 몬스터 프로세스 (도전/시간종료/포기)
    //진화 몬스터 도전 버튼 클릭시
    void EvnOnEvolutionMonsterChallenge()
    {
        StartCoroutine(ChallengeEvolution());
    }
    //도전
    IEnumerator ChallengeEvolution()
    {
        PhaseCountReset();
        SetBeforeChallengeTransition();
        globalData.uiController.SetToggleAutoBossChallenge(false);

        // 트랜지션 효과
        globalData.effectManager.EnableTransition(EnumDefinition.TransitionTYPE.Evolution);
        // 현재 몬스터 OUT
        StartCoroutine(globalData.player.currentMonster.inOutAnimator.MonsterKillMatAnim());
        // 진화전 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            // UI 숨김
            globalData.uiController.ShowUI(false);
            SetEnablePhaseCountUI(MonsterType.evolution);

            // 배경 교체
            var monsterData = globalData.monsterManager.GetMonsterData(MonsterType.evolution, globalData.evolutionManager.evalutionLeveldx);
            globalData.stageManager.SetDungeonBgImage(monsterData.bgId);
            globalData.stageManager.bgAnimController.ResetBg();

            // 스테이지 텍스트 변경
            var stageName = monsterData.stageName;
            GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.evolution, stageName);
            GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.evolution);
        }));

        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_EvolutionBoss);

        // camera zoom out
        globalData.stageManager.bgAnimController.CameraZoomOut_Evolution();
        // 진화 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.evolution));

        SetAfterChallengeTransition();
    }
    //시간종료 / 포기
    public IEnumerator FailedChallengEvolution()
    {
        isEnterFail = true;

        IsMonsterDead = true;
        // 타이머 시간 멈춤
        globalData.bossChallengeTimer.StopBossTimer(true);
        // 포기 버튼 비활성화
        globalData.uiController.ToggleEscapeBossButton(false);
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);
        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();
        // 활성화 된 모든 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();

        UtilityMethod.EnableUIEventSystem(false);

        isEnterFail = false;

        // 보스 몬스터 OUT
        yield return StartCoroutine(globalData.player.currentMonster.inOutAnimator.AnimPositionOut());
        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            // 보스 타이머 게임 오브젝트 비활성화
            globalData.uiController.ToggleBossTimer(false);
            // 배경 이미지 변경
            globalData.stageManager.SetBgImage();
            // 스테이지 텍스트 변경
            GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.normal);
            // 메뉴 판넬 활성화
            globalData.uiController.ShowUI(true);
        }));

        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);
        // camera zoom In
        globalData.stageManager.bgAnimController.CameraZoomIn();
        // 미리 저장된 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.normal));
        //만약 진화전 시작전에 보스전 버튼이 활성화 되어 있었다면
        if (isRememberActiveBossButton) globalData.uiController.ToggleChallengeBossButton(true);
        // 진화 몬스터 도전 버튼 활성화
        globalData.evolutionManager.EnableBtnEvolutionMonsterChange(true);
        // 황금 돼지 활성화
        globalData.goldPigController.ExitOtherView();
        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);

        UtilityMethod.EnableUIEventSystem(true);


    }
    #endregion
    //===================================================================================================================================================================================
    #region 던전 몬스터 프로세스 (도전/시간종료/포기)
    private void DungeonPopupApplyEvent()
    {
        dungeonMonsterPopupClose = true;
    }
    //던전 포기를 수락한 경우
    void AllowGiveUp(EnumDefinition.GoodsType g, float l)
    {
        StartCoroutine(GiveUpDungeonMonster(g, l));
    }
    //던전 포기 버튼을 통한 전투 종료
    IEnumerator GiveUpDungeonMonster(EnumDefinition.GoodsType g, float l)
    {
        isEnterFail = true;

        IsMonsterDead = true;
        globalData.bossChallengeTimer.StopBossTimer(true);
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);
        // 포기 버튼 비활성화
        globalData.uiController.ToggleEscapeBossButton(false);
        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();
        // 활성화 된 모든 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();
        // sfx dungeon monster out
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.End_Batle);

        isEnterFail = false;

        RewardGoods(g, l);
        // 타이머 시간 멈춤
        globalData.bossChallengeTimer.StopBossTimer(true);
        // 보스 몬스터 OUT
        StartCoroutine(globalData.monsterManager.GetMonsterDungeon().inOutAnimator.AnimPositionOut());

        globalData.monsterManager.GetMonsterDungeon().DungeonMonsterOut();
        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            // 보스 타이머 게임 오브젝트 비활성화
            globalData.uiController.ToggleBossTimer(false);
            globalData.uiController.EnableCanvadGroup(EnumDefinition.CanvasTYPE.Castle);
            globalData.uiController.castleButton.gameObject.SetActive(true);
            globalData.uiController.castlePanel.gameObject.SetActive(true);

            //DUNGEON_BOX_ICON_BTN 박스아이콘 비활성화
            UtilityMethod.GetCustomTypeGMById(10).gameObject.SetActive(false);
            // BGM CHANGE
            GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);
            // 활성화 된 모든 곤충 모두 제거
            globalData.insectManager.DisableAllAvtiveInsects();
            // UI 숨김
            globalData.uiController.ShowUI(true);
            globalData.stageManager.SetBgImage();
        }));
        dungeonMonsterPopupClose = false;
        // 미리 저장된 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.normal));

    }
    //던전 포기 버튼을 눌렀을 때
    public void BtnEventDungeonGiveUp()
    {
        if (globalData.popUpGiveUpDungeon.isShowPopup) return;
        //보스 타이머 시간을 멈추고
        //globalData.bossChallengeTimer.StopBossTimer(true);

        // 총 보상 재화 가지고 오기
        var monster = globalData.monsterManager.GetMonsterDungeon();
        var monsterType = monster.curMonsterData.monsterType;
        var goodsType = monster.curMonsterData.goodsType;

        //var totalCurrencyAmount = globalData.dataManager.GetDungeonMonsterDataByTypeLevel(monsterType, monster.curLevel);
        var dungeonMonsterData = globalData.dataManager.GetDungeonMonsterDataByTypeLevel(monsterType, monster.curLevel);

        // 캐슬 -> 연구소에 따른 던전 추가보상량
        var addValue = globalData.labBuildingManager.GetInLabBuildGameData(goodsType).value;
        var totalCurrencyAmount = dungeonMonsterData.currencyAmount + (dungeonMonsterData.currencyAmount * addValue * 0.01f);
        Debug.Log($"totalCurrencyAmount:{totalCurrencyAmount}");
        globalData.popUpGiveUpDungeon.ShowGiveUpDungeonPopup(goodsType, totalCurrencyAmount, AllowGiveUp);

    }
    void EvnOnDungenMonsterChallenge(MonsterType monsterType)
    {
        globalData.uiController.SetToggleAutoBossChallenge(false);

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

        StartCoroutine(ChallengeDungeonMonster(monsterType));
    }
    //도전
    IEnumerator ChallengeDungeonMonster(MonsterType monsterType)
    {
        PhaseCountReset();

        IsMonsterDead = false;
        IsInsectMovementStop = false;
        // 전체 UI 비활성 
        UtilityMethod.EnableUIEventSystem(false);
        // 황금돼지 비활성화
        //globalData.goldPigController.EnterOtherView();
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);

        //SetBeforeChallengeTransition();

        // transition effect
        globalData.effectManager.SetDungeonKeyImage(monsterType);

        GlobalData.instance.effectManager.EnableTransition(EnumDefinition.TransitionTYPE.Dungeon);

        // 현재 몬스터 타입 세팅 ( 타이머 종료 이벤트를 위한...)
        globalData.player.curMonsterType = MonsterType.dungeon;
        // 현재 몬스터 OUT
        //StartCoroutine(globalData.player.currentMonster.inOutAnimator.MonsterKillMatAnim());

        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            // 활성화 된 모든 곤충 모두 제거
            globalData.insectManager.DisableAllAvtiveInsects();
            globalData.uiController.EnableCanvadGroup(EnumDefinition.CanvasTYPE.Main);
            // 버튼 패널 및 캐슬화면 숨김
            globalData.uiController.castleButton.gameObject.SetActive(false);
            globalData.uiController.castlePanel.gameObject.SetActive(false);
            // UI 숨김
            globalData.uiController.ShowUI(false);
            // 보스 도전 버튼 활성화
            globalData.uiController.ToggleChallengeBossButton(false);
            SetEnablePhaseCountUI(MonsterType.dungeon);

            // 배경 교체
            var monsterData = GlobalData.instance.monsterManager.GetMonsterDungeon();
            globalData.stageManager.SetDungeonBgImage(monsterData.monsterToDataMap[monsterType].bgID);
            globalData.stageManager.bgAnimController.ResetBg();
            // 스테이지 텍스트 변경
            var stageName = monsterData.stageName;
            GlobalData.instance.stageNameSetManager.SetTxtStageName(EnumDefinition.StageNameType.dungeon, stageName);
            GlobalData.instance.stageNameSetManager.EnableStageName(EnumDefinition.StageNameType.dungeon);

            //DUNGEON_BOX_ICON_BTN 박스아이콘 활성화
            UtilityMethod.GetCustomTypeGMById(10).gameObject.SetActive(true);
            // 던전 몬스터 레벨 표시
            UtilityMethod.SetTxtCustomTypeByID(107, $"X{0}");
            UtilityMethod.GetCustomTypeImageById(46).sprite = iconSpriteFileData.GetBoxIcon(monsterType);
            // Show Dungeon Monster
            globalData.monsterManager.ShowMonsterByType(MonsterType.dungeon);
        }));
        // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
        //StartCoroutine(globalData.effectManager.goldPoolingCont.DisableGoldEffects());

        // 보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
        //StartCoroutine(globalData.effectManager.bonePoolingCont.DisableGoldEffects());
        // BGM CHANGE
        GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_DungeonBoss);
        var monster = globalData.monsterManager.GetMonsterDungeon();

        // 던전 몬스터 데이터 세팅
        yield return StartCoroutine(monster.Init(monsterType));
        // 던전 몬스터 등장
        yield return StartCoroutine(monster.inOutAnimator.AnimPositionIn());

        // 전체 UI 활성
        UtilityMethod.EnableUIEventSystem(true);
        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);
        // 보스 타이머 게임 오브젝트 활성화
        globalData.uiController.ToggleBossTimer(true);
        // 타이머 계산 시작
        globalData.bossChallengeTimer.StartTimer();
        //보스 포기 버튼 활성화
        globalData.uiController.ToggleEscapeBossButton(true);

    }
    //시간 제한에 따른 전투 종료
    IEnumerator FailedChallengDungeon()
    {
        isEnterFail = true;

        IsMonsterDead = true;
        // 타이머 시간 멈춤
        globalData.bossChallengeTimer.StopBossTimer(true);
        // 포기 버튼 비활성화
        globalData.uiController.ToggleEscapeBossButton(false);
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);
        // 활성화 된 모든 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();
        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();
        // sfx dungeon monster out
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.End_Batle);

        isEnterFail = false;

        //만약 포기 팝업이 떠있는 상태라면
        if (globalData.popUpGiveUpDungeon.isShowPopup)
        {
            globalData.popUpGiveUpDungeon.ButtonRefuse();
        }

        // 총 보상 재화 가지고 오기
        var monster = globalData.monsterManager.GetMonsterDungeon();
        var monsterType = monster.curMonsterData.monsterType;
        var goodsType = monster.curMonsterData.goodsType;

        //var totalCurrencyAmount = globalData.dataManager.GetDungeonMonsterDataByTypeLevel(monsterType, monster.curLevel);
        var dungeonMonsterData = globalData.dataManager.GetDungeonMonsterDataByTypeLevel(monsterType, monster.curLevel);

        // 캐슬 -> 연구소에 따른 던전 추가보상량
        var addValue = globalData.labBuildingManager.GetInLabBuildGameData(goodsType).value;
        var totalCurrencyAmount = dungeonMonsterData.currencyAmount + (dungeonMonsterData.currencyAmount * addValue * 0.01f);

        // sfx dungeon monster out
        globalData.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.End_Batle);

        // 던전 몬스터 팝업 
        globalData.dungeonRewardPopup.SetDungeonPopup(goodsType, totalCurrencyAmount);

        //팝업 닫기 버튼을 누를때까지 대기
        yield return new WaitUntil(() => dungeonMonsterPopupClose);
        // 타이머 시간 멈춤
        globalData.bossChallengeTimer.StopBossTimer(true);
        // 재화 획득 TODO: 재화 획득 연출
        RewardGoods(goodsType, totalCurrencyAmount);
        // 보스 몬스터 OUT
        StartCoroutine(globalData.monsterManager.GetMonsterDungeon().inOutAnimator.AnimPositionOut());

        globalData.monsterManager.GetMonsterDungeon().DungeonMonsterOut();
        // 화면전환 이펙트
        yield return StartCoroutine(globalData.effectManager.EffTransitioEvolutionUpgrade(() =>
        {
            globalData.stageManager.SetBgImage();

            // 활성화 된 모든 곤충 모두 제거
            globalData.insectManager.DisableAllAvtiveInsects();
            globalData.uiController.EnableCanvadGroup(EnumDefinition.CanvasTYPE.Castle);
            globalData.uiController.castlePanel.gameObject.SetActive(true);

            globalData.uiController.castleButton.gameObject.SetActive(true);
            // 보스 타이머 게임 오브젝트 비활성화
            globalData.uiController.ToggleBossTimer(false);
            // UI 활성화
            globalData.uiController.ShowUI(true);
            //DUNGEON_BOX_ICON_BTN 박스아이콘 비활성화
            UtilityMethod.GetCustomTypeGMById(10).gameObject.SetActive(false);
            // BGM CHANGE
            GlobalData.instance.soundManager.PlayBGM(EnumDefinition.BGM_TYPE.BGM_Main);

        }));
        // 미리 저장된 몬스터 등장
        yield return StartCoroutine(AppearMonster(MonsterType.normal));
        dungeonMonsterPopupClose = false;


    }

    #endregion
    //===================================================================================================================================================================================


    //몬스터가 죽었는지 체크
    public bool IsMonsterDead
    {
        get { return isMonsterDead; }
        set { isMonsterDead = value; }
    }
    //곤충 이동 속도 멈추기 위한 플레그
    public bool IsInsectMovementStop
    {
        get { return isInsectMovementStop; }
        set { isInsectMovementStop = value; }
    }

    // 몬스터 타입이 보스 or 진화보스인지 체크
    bool IsBossTypeEliteMonster()
    {
        var elite = globalData.player.curMonsterType >= MonsterType.gold;
        return elite;
    }

    // 도전할 때 화면전환이 이루어 지기 전 세팅
    void SetBeforeChallengeTransition()
    {
        IsMonsterDead = false;
        IsInsectMovementStop = false;
        // 골드 OUT EFFECT ( 골드 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.goldPoolingCont.DisableGoldEffects());
        //보스의 경우 뼈조각 OUT EFF 추가 ( 뼈조각 화면에 뿌려진 경우에만 )
        StartCoroutine(globalData.effectManager.bonePoolingCont.DisableGoldEffects());
        // 전체 UI 비활성 
        UtilityMethod.EnableUIEventSystem(false);
        // 버튼 패널 및 캐슬화면 숨김
        GlobalData.instance.uiController.AllDisableMenuPanels();
        // 황금돼지 비활성화
        globalData.goldPigController.EnterOtherView();
        // 공격 불가능 상태로 전환
        globalData.attackController.SetAttackableState(false);
        //몬스터 죽음 해골 이펙트
        globalData.effectManager.EnableMonsterDieBeforeEffect();
        // 하프 라인 위 곤충 모두 제거
        globalData.insectManager.DisableAllAvtiveInsects();
        // 보스 도전 버튼 숨김
        globalData.uiController.ToggleChallengeBossButton(false);
    }
    // 도전할 때 화면 전환 이후 세팅
    void SetAfterChallengeTransition()
    {
        // 전체 UI 활성
        UtilityMethod.EnableUIEventSystem(true);
        //보스 포기 버튼 활성화
        globalData.uiController.ToggleEscapeBossButton(true);
        // 공격 가능 상태로 전환
        globalData.attackController.SetAttackableState(true);
        // 보스 타이머 게임 오브젝트 활성화
        globalData.uiController.ToggleBossTimer(true);
        // 타이머 계산 시작
        globalData.bossChallengeTimer.StartTimer();
    }
    //시간내에 보스몬스터를 잡지 못하였을때 콜백 이벤트를 받아 호출됨
    void EvnTimeOut()
    {
        if (isEnterFail) return;
        // 현재(보스 몬스터 도전 전) phaseCount 몬스터 재등장 ?? -> 노멀 몬스터 등장하면 됨 phase count 는 따로 카운팅 되고 있으며 하나의 스테이지에 노멀 몬스터 데이터는 모두 동일함.
        switch (globalData.player.curMonsterType)
        {
            case MonsterType.boss:
                StartCoroutine(FailedChallengBoss());
                break;
            case MonsterType.evolution:
                StartCoroutine(FailedChallengEvolution());
                break;
            case MonsterType.dungeon:
                StartCoroutine(FailedChallengDungeon());
                break;
        }
    }
    //던전 보스 전투시간이 종료되어보상을 받는다
    void RewardGoods(GoodsType goodsType, float totalCurrencyAmount)
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

    //몬스터 체력 및 슬라이드바 세팅
    void MonsterUiSetting()
    {
        var monsterData = globalData.player.currentMonster;

        // SET HP
        globalData.uiController.SetTxtMonsterHp(monsterData.hp);
        // SLIDE BAR
        globalData.uiController.SetSliderMonsterHp(monsterData.hp);
    }

    #region  GAIN GOLD & BONE
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
    #endregion
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


