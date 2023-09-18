
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumDefinition;

public class InsectManager : MonoBehaviour
{

    public InsectBee insectBee;
    public InsectBeetle insectBeetle;
    public InsectMentis insectMentis;

    public InsectBullet prefabInsectBee;
    public InsectBullet prefabInsectBeetle;
    public InsectBullet prefabInsectMentis;

    // using object pooling...
    public List<InsectBullet> insectBullets_Bee;
    public List<InsectBullet> insectBullets_Beetle;
    public List<InsectBullet> insectBullets_Mentis;

    public Transform tr_insectPool;
    [Header("풀링 곤충의 갯수")]
    public int insectBirthCount = 15;

    public List<InsectBase> insects = new List<InsectBase>();

    public Transform trHalfPoint;

    public bool damageDebug = false;
    public float debugDamage = 20;
    Player player;
    TraningManager traningManager;
    StatManager statManager;
    public InsectSpriteFileData insectSpriteFileData;

    public float dps;

    public Transform insectTargetPoint;

    //UNION 발사체 ( Spwan Timer 활용 )
    public SpriteFileData spriteFileData;
    public InsectBullet prefabUnion;
    public List<InsectBullet> insectBullets_Union = new List<InsectBullet>();
    public int unionBirthCount = 30;
    public Transform tr_UnionPool;
    int disableUnionIndex = 0;

    //INSECT 발사체 ( Spwan Timer 활용 )
    int insectStBirthCount = 20;
    int disableInsectBeeIndex, disableInsectBeetleIndex, disableInsectMentisIndex = 0;
    public List<InsectBullet> st_insectBullets_Bee;
    public List<InsectBullet> st_insectBullets_Beetle;
    public List<InsectBullet> st_insectBullets_Mentis;

    public List<InsectBullet> enableInsects = new List<InsectBullet>();

    int beetleOrderInLayerID = 0;
    int beeOrderInLayerID = 10;
    int mentisOrderInLayerID = 20;

    void Start()
    {
        player = GlobalData.instance.player;
        traningManager = GlobalData.instance.traningManager;
        statManager = GlobalData.instance.statManager;
    }



    public IEnumerator Init(PlayerDataManager playerDataManager)
    {
        // 곤충 생성
        BirthInsectBullets();

        // 곤충 인스턴스 생성
        SetInsectData(EnumDefinition.InsectType.bee, playerDataManager.saveData.beeSaveData.evolutionLastData);
        SetInsectData(EnumDefinition.InsectType.beetle, playerDataManager.saveData.beetleSaveData.evolutionLastData);
        SetInsectData(EnumDefinition.InsectType.mentis, playerDataManager.saveData.mentisSaveData.evolutionLastData);

        // 스폰 유니온 풀링 생성
        CreateUnionPooling(unionBirthCount);

        // 스폰 곤충 풀링 생성


        // add insects list 
        // 순서 : mentis, bee, beetle
        insects.Add(insectMentis);
        insects.Add(insectBee);
        insects.Add(insectBeetle);

        yield return new WaitForEndOfFrame();


        // Set insect face
        SetAllInsectFace(playerDataManager.saveData.evolutionLevel);
    }


    public void SetAllInsectData(StageData stageData)
    {
        // 진화 공식 필요    
    }

    EvolutionData GetInsectEvolDataById(EnumDefinition.InsectType insectType, int idx)
    {
        return GlobalData.instance.dataManager.GetEvolutionDataById(insectType, idx);
    }


    /// <summary> 몬스터 제거시 하프라인 위의 곤충들 제거 </summary>
    public void DisableHalfLineInsects()
    {
        DisableInsects(insectBullets_Bee);
        DisableInsects(insectBullets_Beetle);
        DisableInsects(insectBullets_Mentis);
        DisableInsects(insectBullets_Union);
        DisableInsects(st_insectBullets_Bee);
        DisableInsects(st_insectBullets_Beetle);
        DisableInsects(st_insectBullets_Mentis);
    }

    /// <summary> 활성화된 곤충들 모두 제거 </summary>
    public void DisableAllAvtiveInsects()
    {
        DisableActiveInsects(insectBullets_Bee);
        DisableActiveInsects(insectBullets_Beetle);
        DisableActiveInsects(insectBullets_Mentis);
        DisableActiveInsects(insectBullets_Union);
        DisableActiveInsects(st_insectBullets_Bee);
        DisableActiveInsects(st_insectBullets_Beetle);
        DisableActiveInsects(st_insectBullets_Mentis);
    }

    public void AddEnableInsects(InsectBullet insectBullet)
    {
        enableInsects.Add(insectBullet);
    }

    public void RemoveEnableInsects(InsectBullet insectBullet)
    {
        enableInsects.Remove(insectBullet);
    }


    void DisableActiveInsects(List<InsectBullet> insectBullets)
    {
        insectBullets.Where(w => w.gameObject.activeSelf).ToList().ForEach(f => f.DisableInsect());
    }

    bool IsHalfPointUpSide(InsectBullet insectBullet)
    {
        return insectBullet.transform.position.y > trHalfPoint.position.y;
    }

    public void DisableInsects(List<InsectBullet> insectBullets)
    {
        foreach (var insect in insectBullets)
            if (insect.gameObject.activeSelf)
                if (IsHalfPointUpSide(insect))
                    insect.DisableInsect();
    }



    public InsectBase GetInsect(EnumDefinition.InsectType insectType)
    {
        return insects[(int)insectType];
    }


    // /// <summary> 계산된 곤충 데미지 값 </summary>
    // public double GetInsectDamageOld(EnumDefinition.InsectType insectType, int unionIndex, out bool isCritical)
    // {
    //     isCritical = false;
    //     if (insectType == InsectType.union)
    //     {
    //         var damage = statManager.GetUnionDamage(unionIndex);
    //         //var talentDamage = statManager.GetUnionTalentDamage(unionIndex);
    //         //return damage + talentDamage;
    //         return damage;
    //     }
    //     else
    //     {
    //         var damage = statManager.GetInsectDamage(insectType);

    //         if (HasCriticalDamage(insectType)) // 크리티컬 데미지 터졌을때
    //         {
    //             damage = damage * (2 + (statManager.GetInsectCriticalDamage(insectType) * 0.01f));
    //             isCritical = true;
    //         }

    //         if (damageDebug) return debugDamage;
    //         return damage;
    //     }
    // }

    public float GetInsectDamage(EnumDefinition.InsectType insectType, int unionIndex, out bool isCritical)
    {
        float damage = 0;
        //double talentDamage = 0;
        isCritical = false;

        if (insectType == InsectType.union)
        {
            damage = statManager.GetUnionDamage(unionIndex);
        }
        else
        {
            damage = statManager.GetInsectDamage(insectType);

            //Debug.Log($"기본데미지{damage}");
            if (HasCriticalDamage(insectType)) // 크리티컬 데미지 터졌을때
            {
                damage = damage * (2 + (statManager.GetInsectCriticalDamage(insectType) * 0.01f));
                isCritical = true;
                //Debug.Log($"크리데미지{damage}");

            }
        }

        // 광란 스킬 사용 했을때 ( 모든 유닛 크리티컬 데미지 적용 )
        if (GlobalData.instance.statManager.allUnitCriticalOn)
        {
            damage = damage * (2 + (statManager.GetInsectCriticalDamage(insectType) * 0.01f));
            isCritical = true;
        }


        //damage += talentDamage;
        if (damageDebug) return debugDamage;
        return damage;
    }


    // 크리티컬 데미지를 가지고 있는지? ( 크리티컬 데미지카 터졌는지 )
    bool HasCriticalDamage(InsectType insectType)
    {
        //var percentage = 1+insect.criticalChance;
        //var percentage = 1+insect.criticalChance + player.GetStatValue(SaleStatType.trainingCriticalDamage) + player.GetStatValue(SaleStatType.talentCriticalDamage)+ RewardPolishEvolutionData insectCriticalChance;
        //var percentage = 1+insect.criticalChance + traningManager.GetStatPower(SaleStatType.trainingCriticalDamage) + traningManager.GetStatPower(SaleStatType.talentCriticalDamage);
        var percentage = statManager.GetInsectCriticalChance(insectType);
        var randomValue = Random.Range(0f, 100f);
        return randomValue <= percentage;
    }

    ///talentSpawnSpeed 방치형으로 게임이 전환될 경우 필요한 스텟 현재 사용되지 않으나 테스트 후 필요에 의해 사용될 수 있음
    ///
    ///talentMoveSpeed 현재 구현되어 있는 곤충의 스피드가 Max. Max Speed값 = 500, Min Speed값 = 0, InsectBullet.AttackAnim speed 값이 필요하며 EvolutionData Speed의 값이 대입되고 + Player.GetStatValue(SaleStatType.trainingMoveSpeed) 가 됨

    ///talentGoldBonus 재화를 획득하는 순간 체크 EventController.GainGold -> Player.AddGold 

    //---------- 훈련 업그레이드 끝


    public void SetInsectData(EnumDefinition.InsectType insectType, EvolutionData evolutionData)
    {
        switch (insectType)
        {
            case EnumDefinition.InsectType.mentis:
                insectMentis.insectType = insectType;
                SetInsectEvolutionData(insectMentis, evolutionData);
                break;
            case EnumDefinition.InsectType.bee:
                insectBee.insectType = insectType;
                SetInsectEvolutionData(insectBee, evolutionData);
                break;
            case EnumDefinition.InsectType.beetle:
                insectBeetle.insectType = insectType;
                SetInsectEvolutionData(insectBeetle, evolutionData);
                break;
        }
    }

    void SetInsectEvolutionData(InsectBase insectBase, EvolutionData evolutionData)
    {
        insectBase.name = evolutionData.name;
        insectBase.damage = evolutionData.damage;
        insectBee.damageRate = evolutionData.damageRate;
        insectBee.criticalChance = evolutionData.criticalChance;
        insectBee.criticalDamage = evolutionData.criticalDamage;
        insectBee.speed = evolutionData.speed;
        insectBee.goldBonus = evolutionData.goldBonus;
        insectBee.bossDamage = evolutionData.bossDamage;
    }

    void BirthInsectBullets()
    {
        // 사용자가 터치 하여 나오는 곤충 풀
        InitancingInsects(prefabInsectBee, insectBullets_Bee);
        InitancingInsects(prefabInsectBeetle, insectBullets_Beetle);
        InitancingInsects(prefabInsectMentis, insectBullets_Mentis);

        // 자동으로 생성되는 곤충 풀
        InitancingInsects(prefabInsectBee, st_insectBullets_Bee);
        InitancingInsects(prefabInsectBeetle, st_insectBullets_Beetle);
        InitancingInsects(prefabInsectMentis, st_insectBullets_Mentis);

    }

    // pooling system
    public void EnableBullet(EnumDefinition.InsectType insectType, Vector2 targetPos)
    {
        // var bullets = GetBulletsByInsectType(insectType);
        // var bullet = bullets.FirstOrDefault(f => !f.gameObject.activeSelf);
        // if (bullet != null)
        // {


        //     SetOrderInLayerID(insectType, bullet);
        //     bullet.transform.position = targetPos;
        //     bullet.gameObject.SetActive(true);
        //     AddEnableInsects(bullet);

        //     // SKILL EFFECT
        //     if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.insectDamageUp))
        //         bullet.effectContoller.AuraEffect(true);
        //     if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.unionDamageUp))
        //         bullet.effectContoller.FireEffect(true);
        //     if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.allUnitSpeedUp))
        //         bullet.effectContoller.TrailEffect(true);
        //     if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.glodBonusUp))
        //         bullet.effectContoller.GoldEffect(true);
        //     if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.allUnitCriticalChanceUp))
        //         bullet.effectContoller.ThunderEffect(true);

        // }
        // else
        // {
        //     //TODO 모든 오브젝트 ENABLE 상태일때 새로운 BULLET 추가 

        // }
        var touch = PoolManager.instance.GetTouchRound();
        touch.SetActive(true);
        touch.transform.position = targetPos;
        StartCoroutine(EnableBulletCor(insectType, targetPos));
    }
    IEnumerator EnableBulletCor(EnumDefinition.InsectType insectType, Vector2 targetPos)
    {
        var bullets = GetBulletsByInsectType(insectType);
        var bullet = bullets.FirstOrDefault(f => !f.gameObject.activeSelf);
        if (bullet != null)
        {
            SetOrderInLayerID(insectType, bullet);
            bullet.transform.position = targetPos;
            bullet.gameObject.SetActive(true);
            AddEnableInsects(bullet);

            yield return new WaitForEndOfFrame();

            // SKILL EFFECT
            if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.insectDamageUp))
                bullet.effectContoller.AuraEffect(true);
            // if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.unionDamageUp))
            //     bullet.effectContoller.FireEffect(true);
            if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.allUnitSpeedUp))
                bullet.effectContoller.TrailEffect(true);
            if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.glodBonusUp))
                bullet.effectContoller.GoldEffect(true);
            if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.allUnitCriticalChanceUp))
                bullet.effectContoller.ThunderEffect(true);

        }
        else
        {
            //TODO 모든 오브젝트 ENABLE 상태일때 새로운 BULLET 추가 

        }

        yield return null;
    }

    void SetOrderInLayerID(EnumDefinition.InsectType insectType, InsectBullet bullet)
    {
        switch (insectType)
        {
            case EnumDefinition.InsectType.beetle:
                bullet.spriteRenderer.sortingOrder = beetleOrderInLayerID;
                beetleOrderInLayerID++;
                if (beetleOrderInLayerID > 9)
                {
                    beetleOrderInLayerID = 0;
                }
                break;
            case EnumDefinition.InsectType.bee:
                bullet.spriteRenderer.sortingOrder = beeOrderInLayerID;
                beeOrderInLayerID++;
                if (beeOrderInLayerID > 29)
                {
                    beeOrderInLayerID = 20;
                }
                break;
            case EnumDefinition.InsectType.mentis:
                bullet.spriteRenderer.sortingOrder = mentisOrderInLayerID;
                mentisOrderInLayerID++;
                if (mentisOrderInLayerID > 19)
                {
                    mentisOrderInLayerID = 10;
                }
                break;
        }
    }

    List<InsectBullet> GetBulletsByInsectType(InsectType insectType)
    {
        switch (insectType)
        {
            case InsectType.mentis: return insectBullets_Mentis;
            case InsectType.bee: return insectBullets_Bee;
            case InsectType.beetle: return insectBullets_Beetle;
            default: return null;
        }
    }

    List<InsectBullet> GetST_BulletsByInsectType(InsectType insectType)
    {
        switch (insectType)
        {
            case InsectType.mentis: return st_insectBullets_Mentis;
            case InsectType.bee: return st_insectBullets_Bee;
            case InsectType.beetle: return st_insectBullets_Beetle;
            default: return null;
        }
    }




    void InitancingInsects(InsectBullet prefab, List<InsectBullet> bullets)
    {
        for (int i = 0; i < insectBirthCount; i++)
        {
            var bullet = Instantiate(prefab, tr_insectPool);
            bullets.Add(bullet);
            bullet.gameObject.SetActive(false);
        }
    }

    /// <summary> 곤충 기본 공격력 DPS </summary>
    public float GetInsectsDps()
    {
        //TODO: DPS 계산식 추가 필요함.
        return dps;
    }

    public void SetAllInsectFace(int faceID)
    {
        foreach (InsectType type in System.Enum.GetValues(typeof(InsectType)))
        {
            SetInsectFace(type, faceID);
        }
    }
    public void SetInsectFace(InsectType insectType, int id)
    {
        var insects = GetBulletsByInsectType(insectType);
        if (insects != null)
        {
            var face = insectSpriteFileData.GetInsectFaceSprite(insectType, id);
            foreach (var insect in insects)
            {
                insect.SetInsectFace(face);
            }
        }

        var insects_st = GetST_BulletsByInsectType(insectType);
        if (insects_st != null)
        {
            var face = insectSpriteFileData.GetInsectFaceSprite(insectType, id);
            foreach (var insect in insects_st)
            {
                insect.SetInsectFace(face);
            }
        }

    }
    void CreateUnionPooling(int birthCount)
    {
        for (int i = 0; i < birthCount; i++)
        {
            var union = Instantiate(prefabUnion, tr_UnionPool);
            union.SetInsectType(EnumDefinition.InsectType.union);
            union.gameObject.SetActive(false);
            insectBullets_Union.Add(union);
        }
    }




    public InsectBullet GetDisableUnion()
    {
        var union = insectBullets_Union[disableUnionIndex];
        if (union.gameObject.activeSelf)
        {
            //TODO: Pool 부족시 자동생성 및 추가
            Debug.Log("유니온 풀링 부족 추가 생성 필요함");
            return null;
        }
        else
        {
            disableUnionIndex++;
            if (disableUnionIndex == insectBullets_Union.Count - 1)
                disableUnionIndex = 0;

            //Debug.Log("선택된 유니온 프리팹 인덱스 : " + insectBullets_Union.IndexOf(union));
            return union;
        }
    }

    public InsectBullet GetDisableST_InsectByType(InsectType insectType)
    {
        switch (insectType)
        {
            case InsectType.bee: return GetDisableST_Insect(ref disableInsectBeeIndex, st_insectBullets_Bee);
            case InsectType.beetle: return GetDisableST_Insect(ref disableInsectBeetleIndex, st_insectBullets_Beetle);
            case InsectType.mentis: return GetDisableST_Insect(ref disableInsectMentisIndex, st_insectBullets_Mentis);
            default: return null;
        }
    }

    public InsectBullet GetDisableST_Insect(ref int idCount, List<InsectBullet> insectBullets)
    {
        var insect = insectBullets[idCount];
        if (insect.gameObject.activeSelf)
        {
            Debug.Log("곤충 BEE 풀링 부족 추가 생성 필요함");
            return null;
        }
        else
        {
            idCount++;
            if (idCount == insectBullets.Count - 1)
                idCount = 0;

            return insect;
        }
    }

}



