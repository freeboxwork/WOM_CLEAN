using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoodsPoolingConrtoller : MonoBehaviour
{

    // 프리팹 생성될 부모 모드젝트
    public Transform trEffects;

    public int goldPoolCount = 25;
    // 골드 프리팹
    public GoldAnimController prefabGoldAnimCont;

    // 골드 오브젝트 풀
    public List<GoldAnimController> goldAnimConts = new List<GoldAnimController>();
    // 활성화된 골드 오브젝트들
    List<GoldAnimController> enableGoldAnimConts = new List<GoldAnimController>();

    public EnumDefinition.GoodsType goodsType;

    public void Init()
    {
        //풀링 오브젝트 생성
        CreateInstanceGolds();
    }

    void CreateInstanceGolds()
    {
        for (int i = 0; i < goldPoolCount; i++)
        {
            var gold = Instantiate(prefabGoldAnimCont, trEffects);
            gold.goodsType = goodsType;
            goldAnimConts.Add(gold);
            gold.gameObject.SetActive(false);
        }
    }

    public IEnumerator EnableGoldEffects(int enableCount)
    {
        StopAllCoroutines();
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < enableCount; i++)
        {
            var gold = GetDisableGold();
            if (gold != null)
            {
                gold.gameObject.SetActive(true);
                gold.GoldInAnimStart();
                enableGoldAnimConts.Add(gold);
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                Debug.Log(goodsType + " Pool 이 비어 있습니다.");
            }
        }
    }

    public IEnumerator DisableGoldEffects()
    {
        for (int i = 0; i < enableGoldAnimConts.Count; i++)
        {
            var gold = enableGoldAnimConts[i];
            if (gold.gameObject.activeSelf)
            {
                gold.GoldOutAnimStart(() => {
                    // 골드가 UI 쪽으로 이동후 개별 이벤트 실행
                    //Debug.Log("gold goal event!!");
                });

                yield return new WaitForSeconds(0.05f);
            }

        }
        enableGoldAnimConts.Clear();
    }


    GoldAnimController GetDisableGold()
    {
        var obj = goldAnimConts?.FirstOrDefault(f => !f.gameObject.activeSelf);
        if (obj != null) return obj;
        else return null;
        //return goldAnimConts.FirstOrDefault(f => !f.gameObject.activeSelf);
    }


}
