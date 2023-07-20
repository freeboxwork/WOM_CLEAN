using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LabBuildSlot : MonoBehaviour
{
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtValue;
    public TextMeshProUGUI txtPrice;
    public Button btnLevelUp;

    public EnumDefinition.GoodsType goodsType;

    void Start()
    {
        SetButtonEvent();
    }

    void SetButtonEvent()
    {
        btnLevelUp.onClick.AddListener(() =>
        {
            GlobalData.instance.labBuildingManager.LevelUpLabBuild(goodsType);
        });
    }

    public void SetUI(LabBuildIngameData data, bool isMax)
    {
        txtLevel.text = "Lv" + data.level.ToString();
        txtValue.text = data.value.ToString() + "%";
        txtPrice.text = isMax ? "MAX" : data.price.ToString();
        btnLevelUp.interactable = !isMax;
    }

}
