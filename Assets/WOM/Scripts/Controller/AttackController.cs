using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackController : MonoBehaviour
{
    public InsectManager insectManager;
    /// <summary> 공격 가능 상태  </summary>
    bool isAttackableState = false ;
    public float spawnInterval = 0.1f;
    float lastSpawnTime;

    void Start()
    {
        insectManager = GlobalData.instance.insectManager;
    }



    // Update is called once per frame
    void Update()
    {
        if (isAttackableState == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - lastSpawnTime >= spawnInterval)
                {
                    lastSpawnTime = Time.time;

                    var pos = Input.mousePosition;
                    // 포인터 위치가 UI 위에 있는지 판단
                    if (EventSystem.current!= null && EventSystem.current.enabled)
                    {
                        var isPointerOnUI = EventSystem.current.IsPointerOverGameObject();
                        if (isPointerOnUI == false)
                            EnableInsectBullet(pos);
                    }
                }
            }
        }
    }
 
    void EnableInsectBullet(Vector2 enablePos)
    {
        var worldPos = Camera.main.ScreenToWorldPoint(enablePos);
        if (IsHalfPoint(enablePos.y))
            worldPos = Camera.main.ScreenToWorldPoint(GetDownSideRandomPos());

        insectManager.EnableBullet(GetProbabilityInsectType(), worldPos);
    }
    bool IsHalfPoint(float pointY)
    {
        var pos = pointY / Screen.height * 100f;
        return pos > 50f;
    }

    /// <summary> 공격 가능 상태 제어 </summary>
    public void SetAttackableState(bool value)
    {
        isAttackableState = value;
    }
    public bool GetAttackableState()
    {
        return isAttackableState;
    }

    Vector2 GetDownSideRandomPos()
    {
        var randomX = Random.Range(0, Screen.width);
        var randomY = Random.Range(0, Screen.height * 0.5f);
        return (new Vector2(randomX, randomY));
    }


    /* 확률에 따른 곤충 생성 */
    // mentis : 35% , bee : 35% , beelte : 30 %
    EnumDefinition.InsectType GetProbabilityInsectType()
    {
        var randomValue = Random.Range(0, 100);
        {
            if (randomValue >= 0 && randomValue <= 35) return EnumDefinition.InsectType.mentis;
            else if (randomValue >= 36 && randomValue <= 70) return EnumDefinition.InsectType.bee;
            else if (randomValue >= 71 && randomValue <= 100) return EnumDefinition.InsectType.beetle;
        }
        return EnumDefinition.InsectType.none;
    }



}


