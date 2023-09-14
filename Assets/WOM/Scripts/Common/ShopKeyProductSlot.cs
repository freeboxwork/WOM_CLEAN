using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopKeyProductSlot : MonoBehaviour
{
    public TextMeshProUGUI txtKeyCount;
    public Button btnBuy;
    public EnumDefinition.GoodsType goodsType;
    const string loadDataKey = "_shopKeyProduct";
    // 구매 가능한 키 수
    int leftKeyCount;

    int maxKeyCount = 1;

    public long price;

    void Start()
    {
        SetButtonEvent();
    }

    void SetButtonEvent()
    {
        btnBuy.onClick.AddListener(BuyKey);
    }

    public void LoadData()
    {
        var keyName = loadDataKey + goodsType.ToString();
        if (PlayerPrefs.HasKey(keyName))
        {
            leftKeyCount = PlayerPrefs.GetInt(keyName);
            if (leftKeyCount <= 0)
            {
                leftKeyCount = 0;
                btnBuy.interactable = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt(keyName, maxKeyCount);
            leftKeyCount = maxKeyCount;
        }
        UpdateUI();
    }

    public void ResetKeyCount()
    {
        var keyName = loadDataKey + goodsType.ToString();
        PlayerPrefs.SetInt(keyName, maxKeyCount);
        UpdateUI();
    }

    void BuyKey()
    {
        if (IsValidGemCount() && leftKeyCount > 0)
        {
            // pay gem
            GlobalData.instance.player.PayGem(price);
            // add key
            GlobalData.instance.player.AddDungeonKey(goodsType, 1);
            // left key count
            leftKeyCount--;
            // save
            var keyName = loadDataKey + goodsType.ToString();
            PlayerPrefs.SetInt(keyName, leftKeyCount);

            if (leftKeyCount == 0)
            {
                btnBuy.interactable = false;
            }
        }
        UpdateUI();
    }

    bool IsValidGemCount()
    {
        var value = GlobalData.instance.player.gem >= price;
        if (value == false)
        {
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 3);
        }

        return value;
    }

    void UpdateUI()
    {
        var txtVelue = $"일일 구매 가능 횟수 (<color=#40ff80>{leftKeyCount}/{maxKeyCount})</color>";
        txtKeyCount.text = txtVelue;
    }

}
