using ProjectGraphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DamageNumbersPro;


public class EffectManager : MonoBehaviour
{
    [Header("곤충 이펙트 관련 항목")]
    public int insectDisableEffectsBirthCount = 25;
    public int insectAttackEffectsBrithCount = 10;
    public ParticleSystem prefabInsectDisableEff;
    public List<ParticleSystem> insectDisableEffects = new List<ParticleSystem>();

    /// <summary> 보스 공격 했을때 나타나는 파티클 이펙트 </summary>
    // mentis , bee , beetle 
    public List<ParticleRoate> prefabInsectAttackEff = new List<ParticleRoate>();
    public List<ParticleRoate> insectAttackEffMentis = new List<ParticleRoate>();
    public List<ParticleRoate> insectAttackEffBee = new List<ParticleRoate>();
    public List<ParticleRoate> insectAttackEffBeetle = new List<ParticleRoate>();
    public List<ParticleRoate> insectAttackEffUnion = new List<ParticleRoate>();
    [Header("=====================================================================================================================")]


    [Header("이펙트들 부모 게임 오브젝트")]
    // 프리팹 생성될 부모 모드젝트
    public Transform trEffects;
    [Header("=====================================================================================================================")]


    [Header("골드 관련 항목")]
    // 골드 생성 포인트들
    public List<Transform> goldSFX_RandomPoints = new List<Transform>();
    public GoodsPoolingConrtoller goldPoolingCont;
    public GoodsPoolingConrtoller bonePoolingCont;

    [Header("=====================================================================================================================")]
    [Header("진화전 이펙트 관련 항목")]
    public AnimationController animContTransition;
    public AnimData animDataTranIn;
    public AnimData animDataTranOut;

    [Header("=====================================================================================================================")]
    [Header("몬스터 타격시 나타나는 텍스트")]
    int flotingTextPoolCount = 20;
    public FloatingText prefabFloatingText;
    public List<FloatingText> floatingTextPool = new List<FloatingText>();

    [Header("=====================================================================================================================")]
    [Header("화면전환 이펙트")]
    public GameObject transitionAnimEffBossAttack;
    public GameObject transitionAnimEffSkillOn;
    public Image transitionAnimEffSkillOnImage;
    public SerializableDictionary<EnumDefinition.SkillType, Sprite> skillTypeToIconMap;


    // Dungeon tranition effectg
    public GameObject transitionAnimEffDungeonIn;
    public Image imgDungeonKey;
    public Image imgDungeonKeyShadow;
    public SerializableDictionary<EnumDefinition.MonsterType, Sprite> dungeonTypeToIconMap;

    // evolution tranition effect
    public GameObject transitionAnimEffEvolutionIn;

    public GameObject transitionAnimEffStageClear;

    public DamageNumber numberPrefab;


    // castle tranition effect
    public GameObject transitionAnimEffCastleIn;

    // king monster effect
    public GameObject[] effectKingMonsterHits;

    public GameObject monsterDieEffect;

    void Start()
    {

    }

    public IEnumerator Init()
    {
        yield return null;
        //DamageNumber Number Pooling
        numberPrefab.PrewarmPool();
        // disable effect
        CreateInsectDisableEffects();

        // attack effect mentis
        CreateInstanceAttackEffects(prefabInsectAttackEff[(int)EnumDefinition.InsectType.mentis], insectAttackEffMentis);
        // attack effect bee
        CreateInstanceAttackEffects(prefabInsectAttackEff[(int)EnumDefinition.InsectType.bee], insectAttackEffBee);
        // attack effect beetle
        CreateInstanceAttackEffects(prefabInsectAttackEff[(int)EnumDefinition.InsectType.beetle], insectAttackEffBeetle);
        // attack effect Union
        CreateInstanceAttackEffects(prefabInsectAttackEff[(int)EnumDefinition.InsectType.union], insectAttackEffUnion);

        // 플로팅 텍스트 오브젝트 풀 생성
        CreateFloatingTextPool();

        // 골드 풀링 오브젝트 생성
        goldPoolingCont.Init();
        bonePoolingCont.Init();
        //CreateInstanceGolds();
    }


    void CreateInstanceAttackEffects(ParticleRoate prefabParticle, List<ParticleRoate> list)
    {
        for (int i = 0; i < insectAttackEffectsBrithCount; i++)
        {
            var effect = Instantiate(prefabParticle, trEffects);
            list.Add(effect);
            effect.gameObject.SetActive(false);
        }
    }


    void CreateInsectDisableEffects()
    {
        for (int i = 0; i < insectDisableEffectsBirthCount; i++)
        {
            var effect = Instantiate(prefabInsectDisableEff, trEffects);
            insectDisableEffects.Add(effect);
            effect.gameObject.SetActive(false);
        }
    }

    public void EnableInsectDisableEffect(Transform tr)
    {
        var effect = GetInsectDisableEff();
        effect.transform.position = tr.position;
        effect.gameObject.SetActive(true);
    }

    public void EnableAttackEffectByInsectType(EnumDefinition.InsectType insectType, Transform tr)
    {
        switch (insectType)
        {
            case EnumDefinition.InsectType.mentis: EnableAttackEffect(insectAttackEffMentis, tr, insectType); break;
            case EnumDefinition.InsectType.bee: EnableAttackEffect(insectAttackEffBee, tr, insectType); break;
            case EnumDefinition.InsectType.beetle: EnableAttackEffect(insectAttackEffBeetle, tr, insectType); break;
            case EnumDefinition.InsectType.union: EnableAttackEffect(insectAttackEffUnion, tr, insectType); break;
        }
    }

    void EnableAttackEffect(List<ParticleRoate> particles, Transform tr, EnumDefinition.InsectType insectType)
    {
        var effect = GetDisableParticleRoate(particles, insectType);
        var zRot = tr.eulerAngles.z;
        effect.transform.position = tr.position;
        effect.SetParticleRotateDirection(zRot);
        effect.gameObject.SetActive(true);
    }



    ParticleRoate GetDisableParticleRoate(List<ParticleRoate> particles, EnumDefinition.InsectType insectType)
    {

        foreach (var eff in particles)
        {
            if (!eff.gameObject.activeInHierarchy)
            {
                return eff;
            }
        }
        var effect = Instantiate(prefabInsectAttackEff[(int)insectType], trEffects);
        particles.Add(effect);
        return effect;

        //return particles.FirstOrDefault(f => !f.gameObject.activeSelf);
    }

    ParticleSystem GetInsectDisableEff()
    {

        foreach (var eff in insectDisableEffects)
        {
            if (!eff.gameObject.activeInHierarchy)
            {
                return eff;
            }
        }
        var effect = Instantiate(prefabInsectDisableEff, trEffects);
        insectDisableEffects.Add(effect);
        return effect;


        //return insectDisableEffects.FirstOrDefault(f => !f.gameObject.activeSelf);
    }

    // 골드 생성 포인트
    public Transform GetGoldSfxRandomPoint(EnumDefinition.GoldPosType pointType)
    {
        return goldSFX_RandomPoints[(int)pointType];
    }

    public void EnableFloatingText(double damage, bool isCritical, Transform tr, EnumDefinition.InsectType insectType)
    {
        float dmg = (float)Mathf.Floor((float)damage);//소수점 버림
        string strDmg = UtilityMethod.ChangeSymbolNumber(dmg.ToString("#,##0"));//3자리마다 콤마 << 이렇게 해야 Infinite 변환 에서 에러가 안남
        DamageNumber damageNumber = numberPrefab.Spawn(tr.position,  strDmg, 0);//3번째 파라미터는 의미없는 값

        //곤충이 치명타
        if (isCritical)
        {
            damageNumber.SetScale(1f);
            damageNumber.SetColor(Color.magenta);
        }
        else
        {
            //유니온 일반 공격
            if (insectType == EnumDefinition.InsectType.union)
            {
                damageNumber.SetColor(Color.cyan);
                damageNumber.SetScale(0.7f);
            }
            else
            {
                //곤충 일반 공격
                damageNumber.SetColor(Color.white);
                damageNumber.SetScale(0.5f);
            }

        }

    }

    // 플로팅 텍스트 Pool
    void CreateFloatingTextPool()
    {
        for (int i = 0; i < flotingTextPoolCount; i++)
        {
            FloatingText floatingText = Instantiate(prefabFloatingText, trEffects);
            floatingText.gameObject.SetActive(false);
            floatingTextPool.Add(floatingText);
        }
    }

    public FloatingText GetFloatingText()
    {
        for (int i = 0; i < floatingTextPool.Count; i++)
        {
            if (!floatingTextPool[i].gameObject.activeInHierarchy)
            {
                return floatingTextPool[i];
            }
        }

        FloatingText floatingText = Instantiate(prefabFloatingText, trEffects);
        floatingTextPool.Add(floatingText);
        return floatingText;
    }




    /// <summary> 진화전 트랜지션 이펙트 </summary>
    public IEnumerator EffTransitioEvolutionUpgrade(UnityAction transitionEvent)
    {
        yield return null;

        // 트랜지션 인
        var image = UtilityMethod.GetCustomTypeImageById(20);
        var colorAlpha_None = new Color(1, 1, 1, 1);
        var colorAlpha = new Color(1, 1, 1, 0);
        animContTransition.animData = animDataTranIn;
        yield return StartCoroutine(animContTransition.UI_ImageColorAnim(image, colorAlpha, colorAlpha_None));

        transitionEvent.Invoke();
        // UI PANEL 숨김
        GlobalData.instance.uiController.AllDisableMenuPanels();

        yield return new WaitForSeconds(1f);

        // 트랜지션 아웃
        animContTransition.animData = animDataTranOut;
        yield return StartCoroutine(animContTransition.UI_ImageColorAnim(image, colorAlpha_None, colorAlpha));
    }

    public void EnableTransitionBlackScreen()
    {
        var image = UtilityMethod.GetCustomTypeImageById(20);
        image.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator TransitionOut()
    {
        var image = UtilityMethod.GetCustomTypeImageById(20);
        animContTransition.animData = animDataTranOut;
        var colorAlpha_None = new Color(1, 1, 1, 1);
        var colorAlpha = new Color(1, 1, 1, 0);
        yield return StartCoroutine(animContTransition.UI_ImageColorAnim(image, colorAlpha_None, colorAlpha));
    }

    public void EnableTransitionEffBossAttack()
    {
        transitionAnimEffBossAttack.gameObject.SetActive(true);
    }

    public void EnableTransitionEffSkillOnByType(EnumDefinition.SkillType skillType)
    {
        StopCoroutine("EnableTransitionEffSkillOnByType_Cor");
        StartCoroutine(EnableTransitionEffSkillOnByType_Cor(skillType));
    }
    IEnumerator EnableTransitionEffSkillOnByType_Cor(EnumDefinition.SkillType skillType)
    {
        transitionAnimEffSkillOn.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        transitionAnimEffSkillOnImage.sprite = skillTypeToIconMap[skillType];
        transitionAnimEffSkillOn.gameObject.SetActive(true);
    }

    public void EnableTransitionEffDungeonInByType(EnumDefinition.MonsterType monsterType)
    {
        StopCoroutine("EnableTransitionEffEffDungeonInByType_Cor");
        StartCoroutine(EnableTransitionEffEffDungeonInByType_Cor(monsterType));
    }
    IEnumerator EnableTransitionEffEffDungeonInByType_Cor(EnumDefinition.MonsterType monsterType)
    {
        transitionAnimEffDungeonIn.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        imgDungeonKey.sprite = dungeonTypeToIconMap[monsterType];
        imgDungeonKeyShadow.sprite = dungeonTypeToIconMap[monsterType];
        transitionAnimEffDungeonIn.gameObject.SetActive(true);
    }

    public void EnableTransitionEffEvolution()
    {
        transitionAnimEffEvolutionIn.gameObject.SetActive(true);
    }

    public void EnableTransitionEffStageClear()
    {
        transitionAnimEffStageClear.gameObject.SetActive(true);
    }


    public void EnableTransitionEffCastle()
    {
        transitionAnimEffCastleIn.gameObject.SetActive(true);
    }

    public void EnableKingMonsterHitEffect(Transform tr, int hitIndex)
    {
        effectKingMonsterHits[hitIndex].transform.position = tr.position;
        effectKingMonsterHits[hitIndex].SetActive(true);
    }

    public void EnableMonsterDieSkullEffect()
    {
        if (monsterDieEffect.activeInHierarchy == false)
            monsterDieEffect.SetActive(true);
    }

}
