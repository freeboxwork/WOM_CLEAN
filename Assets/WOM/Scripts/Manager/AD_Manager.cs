using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class AD_Manager : MonoBehaviour
{
    public List<BuffADSlot> buffADSlots = new List<BuffADSlot>();
    void Start()
    {

    }

    IEnumerator Init()
    {
        yield return null;
    }


    void SetBuffAdSlotData()
    {
        // foreach (var slot in buffADSlots)
        // {
        //   var data = GlobalData.instance.saveDataManager.GetSaveDataBuffAD_LeftDataByType(slot.buffADType);

        // }
    }

    BuffADSlot GetBuffAdSlotByType(EnumDefinition.BuffADType buffADType)
    {
        return buffADSlots.Where(x => x.buffADType == buffADType).FirstOrDefault();
    }
}
