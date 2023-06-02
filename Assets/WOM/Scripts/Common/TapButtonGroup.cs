using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapButtonGroup : MonoBehaviour
{
    public List<TapButton> tapButtons;
    public Color focusColor;

    private void Start()
    {
        SetFirstTap();
    }

    public void EnableTapButton(TapButton tapButton)
    {
        foreach (var button in tapButtons)
        {
            var isSelected = button == tapButton;
            
            if(button.tapFocus != null)
                button.tapFocus.enabled = isSelected;
            
            button.txtName.color = isSelected ? focusColor : Color.white;   
        }
    }

    void SetFirstTap()
    {
        EnableTapButton(tapButtons[0]);
    }


}
