using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBtnSlot : MonoBehaviour
{

    public Button btnMain;
    public Sprite sprSelect;
    public Sprite sprUnSelect;
    public Image imgSelect;
    public EnumDefinition.MenuPanelType menuPanelType;

    void Start()
    {
        
    }


    public void Select(bool selectValue)
    {
        imgSelect.sprite = selectValue ? sprSelect : sprUnSelect;
    }
    
}
