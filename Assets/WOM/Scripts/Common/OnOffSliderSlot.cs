using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnOffSliderSlot : MonoBehaviour
{

    public bool isOn;
    public TextMeshProUGUI txtOnOff;
    public Slider slider;

    public Image imgSliderBg;
    public Image imgSliderToggle;

    public Sprite sliderBgOn;
    public Sprite sliderBgOff;
    public Sprite sliderToggleOn;
    public Sprite sliderToggleOff;

    void Start()
    {

    }

    public void Init(bool value)
    {
        SetSliderValue(value);
    }

    public void SetSliderValue(bool value)
    {
        isOn = value;
        var sliderValue = value ? 1 : 0;
        var sliderBg = value ? sliderBgOn : sliderBgOff;
        var sliderToggle = value ? sliderToggleOn : sliderToggleOff;
        var txtValue = value ? "ON" : "OFF";

        txtOnOff.text = txtValue;
        imgSliderBg.sprite = sliderBg;
        imgSliderToggle.sprite = sliderToggle;
        slider.value = sliderValue;
    }

}
