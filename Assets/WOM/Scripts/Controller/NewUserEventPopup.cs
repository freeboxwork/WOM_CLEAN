using System.Collections.Generic;
using UnityEngine;

public class NewUserEventPopup : MonoBehaviour
{
    public GameObject popupSet;
    public List<NewUserSlot> newUserSlots = new List<NewUserSlot>();

    void Start()
    {

    }

    public void SetSlotUI()
    {
        var datas = GlobalData.instance.dataManager.newUserDatas.data;

        for (int i = 0; i < datas.Count; i++)
        {
            var data = datas[i].ClonInstance();
            var slot = newUserSlots[i];
            slot.SetUI(data);
        }
    }
}
