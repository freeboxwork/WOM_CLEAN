using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{
    public TapButtonGroup tapButtonGroup;
    public TextMeshProUGUI txtName;
    public Image tapFocus;
    public Button btnTap;

    private void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        btnTap.onClick.AddListener(() => {
            tapButtonGroup.EnableTapButton(this);
        });
    }
        
}
