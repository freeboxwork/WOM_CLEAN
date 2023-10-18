using System.Collections;
using UnityEngine;
using ProjectGraphics;

/// <summary>
/// 몬스터 향해 이동 하는 발사체 ( 공격 )
/// </summary>
[RequireComponent(typeof(InsectSpriteAnimation))]
public class InsectBullet : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public EnumDefinition.InsectType insectType;
    public AnimData animData;
    //public ParticleSystem effDisable;
    public InsectSpriteAnimation spriteAnim;

    float minSpeed = 20f;
    float maxSpeed = 450f;
    float speed = 1f;

    float currentSpeed;
    Vector3 lookDir;

    /* WIGGLE */
    //float wiggleRotationAmount = 0.02f;     // 회전 움직임의 크기
    //float wigglePositionAmount = 0.02f;   // 위치 움직임의 크기
    //float wiggleDuration = 0.3f;         // 한 번 움직이는 데 걸리는 시간
    private Quaternion originalRotation;        // 원래의 회전값
    private Vector3 originalPosition;           // 원래의 위치값


    void GetDefaultWiggleValue()
    {
        originalRotation = transform.localRotation;  // 원래의 회전값 저장
        originalPosition = transform.localPosition;  // 원래의 위치값 저장
    }

    // UNION
    public UnionInGameData inGameData;

    public InsectEffectContoller effectContoller;

    public void SetInsectType(EnumDefinition.InsectType insectType)
    {
        this.insectType = insectType;
    }


    private void OnEnable()
    {
        // 공격 가능 상태에서만 애니메이션 진행
        if (GlobalData.instance.attackController.GetAttackableState() == true)
        {
            StartCoroutine(AttackMove());
            // EFFECT ON OFF
            // 사기의 외침
            // var unionDamageUp = GlobalData.instance.skillManager.GetSkillBtnByType(EnumDefinition.SkillType.unionDamageUp);
            // effectContoller.FireEffect(unionDamageUp.skillAddValue);

            // // 광란
            // var allUnitCriticalUp = GlobalData.instance.skillManager.GetSkillBtnByType(EnumDefinition.SkillType.allUnitCriticalChanceUp);
            // effectContoller.ThunderEffect(allUnitCriticalUp.skillAddValue);
        }

        //StartCoroutine(AttackAnim());
        else
            gameObject.SetActive(false);

    }

    void OnDisable()
    {
        GlobalData.instance.insectManager.RemoveEnableInsects(this);
    }

    float GetSpeed()
    {
        if (insectType == EnumDefinition.InsectType.union)
        {
            return GlobalData.instance.statManager.GetUnionMoveSpeed(inGameData.unionIndex);
        }
        else
        {

            return GlobalData.instance.statManager.GetInsectMoveSpeed(insectType);
            // return GlobalData.instance.insectManager.GetInsect(insectType).speed;
        }
    }

    public IEnumerator AttackAnim()
    {
        animData.ResetAnimData();
        var animPoints = GetAnimPoints();
        speed = GetSpeed();
        while (animData.animValue < 0.99f)
        {
            // TODO : DATA 의 SPEED 적용
            animData.animTime = ((Time.time - animData.animStartTime) / (animData.animDuration * speed));
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);

            // 직선이동
            transform.position = Vector3.Lerp(animPoints.startPoint, animPoints.targetPoint, animData.animValue);

            // 회전
            lookDir = (animPoints.targetPoint - transform.position);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f));
            yield return null;

        }
        gameObject.SetActive(false);
    }

    public IEnumerator AttackMove()
    {
        var targetPoint = GetRandomMonsterPoint();
        speed = GetSpeed();
        currentSpeed = speed;

        var startLegnth = (transform.position - targetPoint).sqrMagnitude;
        //Debug.Log("legnth : " + legnth);

        while (!IsGoalTargetPoint(targetPoint))
        {

            var currentLength = (transform.position - targetPoint).sqrMagnitude;

            var resultLength = UtilityMethod.RemapValue(currentLength, startLegnth, 28f, 0f, 1f);
            //Debug.Log("currentLength " + currentLength);
            //Debug.Log("resultLength : " + resultLength);

            // 이동 방향
            var direction = transform.position - targetPoint;

            // 방향 벡터 정규화 ( 이동거리간 일정한 속도를 위해 )
            direction.Normalize();

            // 직선 이동
            transform.position = GetMovePosition(direction, currentSpeed, resultLength);

            // 회전
            lookDir = (targetPoint - transform.position);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f));
            yield return null;
        }

        gameObject.SetActive(false);
    }


    public AnimationCurve animCurve;

    Vector3 GetMovePosition(Vector3 direction, float speed, float curveTime)
    {
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        float resultSpeed = UtilityMethod.RemapValue(speed, minSpeed, maxSpeed, 0.7f, 1.5f);

        var animCurveValue = animCurve.Evaluate(curveTime);

        if (GlobalData.instance.eventController.IsInsectMovementStop)
        {
            resultSpeed = 0.1f;
        }
        return transform.position - (direction * Time.deltaTime * resultSpeed) * animCurveValue;

    }

    bool IsGoalTargetPoint(Vector3 targetPoint)
    {
        var offset = transform.position - targetPoint;
        int length = (int)offset.sqrMagnitude;
        return length <= 0;
    }




    (Vector3 startPoint, Vector3 targetPoint) GetAnimPoints()
    {
        (Vector3, Vector3) points;
        points.Item1 = transform.position;
        points.Item2 = GetRandomMonsterPoint();
        return points;
    }
    Vector3 GetRandomMonsterPoint()
    {
        //var monPos = GlobalData.instance.player.currentMonster.transform.position;
        var monPos = GlobalData.instance.insectManager.insectTargetPoint.position;
        var x = Random.Range(monPos.x - 1, monPos.x + 1);
        return new Vector3(x, monPos.y, monPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GlobalData.instance.attackController.GetAttackableState() == true)
        {
            var tag = collision.tag;

            //if (collision.transform.CompareTag("monster"))
            if (tag.Contains("monster"))
            {
                // Attack Effect Enable
                GlobalData.instance.effectManager.EnableAttackEffectByInsectType(insectType, this.transform);

                // SFX PLAY
                GlobalData.instance.soundManager.PlayMonsterHitSound();
                CallBackEventType.TYPES hitEventType = tag == "monster" ? CallBackEventType.TYPES.OnMonsterHit : CallBackEventType.TYPES.OnDungeonMonsterHit;

                int parameter = insectType == EnumDefinition.InsectType.union ? inGameData.unionIndex : 0;

                // monster hit event!
                EventManager.instance.RunEvent(hitEventType, insectType, parameter, this.transform);

                // 소멸
                gameObject.SetActive(false);



                //// Attack Effect Enable
                //GlobalData.instance.effectManager.EnableAttackEffectByInsectType(insectType, this.transform);

                //// SFX PLAY
                //GlobalData.instance.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.MONSTER_HIT);

                //CallBackEventType.TYPES hitEventType;

                //if (tag == "monster")
                //{
                //    hitEventType = CallBackEventType.TYPES.OnMonsterHit;
                //}
                //else //if(tag == "monsterDungeon")
                //{
                //    hitEventType = CallBackEventType.TYPES.OnDungeonMonsterHit;
                //}

                //// monster hit event!
                //if (insectType == EnumDefinition.InsectType.union)
                //    EventManager.instance.RunEvent(hitEventType, insectType, inGameData.unionIndex, this.transform);
                //else
                //    EventManager.instance.RunEvent(hitEventType, insectType, 0, this.transform);


                //// 소멸
                //gameObject.SetActive(false);
            }
        }
    }

    // 어떠한 이유로 곤충 갑자기 사라져야 할 때
    public void DisableInsect()
    {
        // 파티클 이펙트 추가
        //GlobalData.instance.effectManager.EnableInsectDisableEffect(this.transform);
        gameObject.SetActive(false);
    }

    public void SetInsectFace(Sprite[] sprites)
    {
        spriteAnim.SetSprite(sprites);
    }

    public void DieInsect()
    {
        // 파티클 이펙트 추가
        GlobalData.instance.effectManager.EnableInsectDisableEffect(this.transform);
        gameObject.SetActive(false);
    }
}


