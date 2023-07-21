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

    BuffADSlot GetBuffAdSlotByType(EnumDefinition.BuffADType buffADType)
    {
        return buffADSlots.Where(x => x.buffADType == buffADType).FirstOrDefault();
    }
}
