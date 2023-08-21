using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class WomSystemMnager : MonoBehaviour
{

    public GameObject powerSavingPopup;
    public Button btnPowerSavingOff;


    void Start()
    {
        AddBtnEvents();
    }

    void AddBtnEvents()
    {
        btnPowerSavingOff.onClick.AddListener(() =>
        {
            powerSavingPopup.SetActive(false);
            PowerSavingMone(EnumDefinition.PowerSavingMode.off);
        });
    }

    public void PowerSavingModeOn()
    {
        powerSavingPopup.SetActive(true);
        PowerSavingMone(EnumDefinition.PowerSavingMode.on);
    }

    public void PowerSavingMone(EnumDefinition.PowerSavingMode mode)
    {
        var powerSavingValue = mode == EnumDefinition.PowerSavingMode.on ? 3 : 1;
        OnDemandRendering.renderFrameInterval = powerSavingValue;

    }




}
