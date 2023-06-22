using System.Collections.Generic;
using UnityEngine;

public class NewUserEventPopup : MonoBehaviour
{
    public GameObject popupSet;
    public List<NewUserSlot> newUserSlots = new List<NewUserSlot>();

    void Start()
    {

    }

    public void EnablePopup(bool value)
    {
        popupSet.SetActive(value);
    }

}
