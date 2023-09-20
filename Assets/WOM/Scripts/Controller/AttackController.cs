using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour
{
    public InsectManager insectManager;
    /// <summary> 공격 가능 상태  </summary>
    public bool isAttackableState = false;
    float lastSpawnTime;

    //RaycastHit2D hit;

    public Transform testInsectEnablePos;
    bool insectAutoEnable = false;


   void Start()
    {
        insectManager = GlobalData.instance.insectManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttackableState == true)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current != null && EventSystem.current.enabled)
                {
                    if (EventSystem.current.IsPointerOverGameObject() == false) // 포인터 UI 위에 있지 않을때만 실행
                    {
                        // 메뉴 패널이 열려있으면 닫는다.
                        if (GlobalData.instance.uiController.CheckOpenMenuPanel() == true)
                        {
                            GlobalData.instance.uiController.CloseAllMenuPanel();
                            return;
                        }

                        // gold pig 
                        //hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward);

                        if (hit)
                        {
                            if (hit.collider.tag == "goldPig")
                            {
                                // gold pig event
                                //Debug.Log("get gold pig !!!!");
                                EventManager.instance.RunEvent(CallBackEventType.TYPES.OnGoldPigEvent);
                                // 일일 퀘스트 완료 : 골드피그
                                EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.takeGoldPig);
                                return;
                            }
                        }

                        if (Time.time - lastSpawnTime >= StaticDefine.TOUCH_INTERVAL)
                        {
                            lastSpawnTime = Time.time;

                            var pos = Input.mousePosition;

                            // ?????? ???? ???? ????
                            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTutoInsectCreate);
                            // ???? ????
                            EnableInsectBullet(pos);
                            GlobalData.instance.soundManager.PlayAttackSound();
                        }
                    }


                }
            }
#endif          
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current != null && EventSystem.current.enabled)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false) // 포인터 UI 위에 있지 않을때만 실행
                    {
                        // 메뉴 패널이 열려있으면 닫는다.
                        if(GlobalData.instance.uiController.CheckOpenMenuPanel() == true) 
                        {
                            GlobalData.instance.uiController.CloseAllMenuPanel();
                            return;
                        } 
                       
                        // gold pig 
                        //hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward);

                        if (hit)
                        {
                            if (hit.collider.tag == "goldPig")
                            {
                                // gold pig event
                                //Debug.Log("get gold pig !!!!");
                                EventManager.instance.RunEvent(CallBackEventType.TYPES.OnGoldPigEvent);
                                // 일일 퀘스트 완료 : 골드피그
                                EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.takeGoldPig);
                                return;
                            }
                        }

                        if (Time.time - lastSpawnTime >= StaticDefine.TOUCH_INTERVAL)
                        {
                            lastSpawnTime = Time.time;

                            var pos = Input.mousePosition;

                            // ?????? ???? ???? ????
                            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTutoInsectCreate);
                            // ???? ????
                            EnableInsectBullet(pos);
                            GlobalData.instance.soundManager.PlayAttackSound();
                        }
                    }


                }
            }
#endif            

        }
    }
    // ?????? ????? UI ???? ????? ???
    // private bool IsPointerOverUIObject()
    // {
    //     if (EventSystem.current == null || Input.mousePosition == null)
    //     {
    //         return true;
    //     }

    //     PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //     eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //     List<RaycastResult> results = new List<RaycastResult>();
    //     EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //     return results.Count > 0;
    // }


    /// <summary>
    ///  ?????? ???? ???? ????
    /// </summary>
    public void TestInsectAotoEnable(float insectAutoEnableTime)
    {
        insectAutoEnable = true;
        StartCoroutine(AutoInsectEnable(insectAutoEnableTime));
    }

    public void StopTestInsectAotoEnable()
    {
        insectAutoEnable = false;
    }

    public IEnumerator AutoInsectEnable(float insectAutoEnableTime)
    {
        while (insectAutoEnable)
        {
            yield return new WaitForSeconds(insectAutoEnableTime);
            insectManager.EnableBullet(GetProbabilityInsectType(), InsectAutoEnablePos());
        }


    }

    Vector3 InsectAutoEnablePos()
    {
        var pos = testInsectEnablePos.position;
        var x = pos.x + Random.Range(-1.5f, 1.5f);
        return new Vector3(x, pos.y, pos.z);
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
        return pos > 20f;
    }

    /// <summary> ???? ???? ???? ???? </summary>
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
        var randomY = Random.Range(Screen.height * 0.2f, Screen.height * 0.3f);
        return (new Vector2(randomX, randomY));
    }


    /* ????? ???? ???? ???? */
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



