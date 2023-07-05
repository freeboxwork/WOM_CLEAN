using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour
{
    public InsectManager insectManager;
    /// <summary> ���� ���� ����  </summary>
    bool isAttackableState = false;
    public float spawnInterval = 0.1f;
    float lastSpawnTime;

    RaycastHit2D hit;

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
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - lastSpawnTime >= spawnInterval)
                {
                    lastSpawnTime = Time.time;

                    var pos = Input.mousePosition;
                    // ������ ��ġ�� UI ���� �ִ��� �Ǵ�
                    if (EventSystem.current != null && EventSystem.current.enabled)
                    {
                        var isPointerOnUI = EventSystem.current.IsPointerOverGameObject();
                        if (isPointerOnUI == false)
                            EnableInsectBullet(pos);
                    }
                }

                if (!IsPointerOverUIObject()) // ������ UI ���� ���� �������� ����
                {
                    // gold pig 
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("goldPig"))
                        {
                            // gold pig event
                            Debug.Log("get gold pig !!!!");
                            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnGoldPigEvent);
                            // ���� ����Ʈ �Ϸ� : ����Ǳ�
                            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.takeGoldPig);
                        }
                    }
                }

            }



        }
    }

    // ������ ��ġ�� UI ���� �ִ��� �Ǵ�
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    /// <summary>
    ///  �׽��͸� ���� ���� ����
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
        return pos > 50f;
    }

    /// <summary> ���� ���� ���� ���� </summary>
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


    /* Ȯ���� ���� ���� ���� */
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


