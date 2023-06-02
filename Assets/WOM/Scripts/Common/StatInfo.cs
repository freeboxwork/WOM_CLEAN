using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class StatInfo : MonoBehaviour
{

    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtValue;
    public Button btnReload;
    
    UnityAction<float> setValue;


    void Start()
    {
        setValue = (value) => setValue(value);
    }

    void SetBtnEvnet()
    {
        btnReload.onClick.AddListener(() => {
            
        });
    }

    void SetTxtTitle(string value)
    {
        txtTitle.text = value;  
    }
    void SetTxtValue(float value)
    {
        txtValue.text = value.ToString();   
    }
}
