using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopKeyProductSlot : MonoBehaviour
{
    public TextMeshProUGUI txtKeyCount;
    public Button btnBuy;
    public EnumDefinition.RewardType rewardType;
    const string loadDataKey = "_shopKeyProduct";
    // 구매 가능한 키 수
    int leftKeyCount;

    int maxKeyCount = 2;

    public long price;

    void Start()
    {

    }

    public void LoadData()
    {
        var keyName = loadDataKey + rewardType.ToString();
        if (PlayerPrefs.HasKey(keyName))
        {
            leftKeyCount = PlayerPrefs.GetInt(keyName);
        }
        else
        {
            PlayerPrefs.SetInt(keyName, maxKeyCount);
            leftKeyCount = maxKeyCount;
        }
    }

    void ResetKeyCount()
    {
        var keyName = loadDataKey + rewardType.ToString();
        PlayerPrefs.SetInt(keyName, maxKeyCount);
    }

    void BuyKey()
    {

    }

    bool IsValidGemCount()
    {
        return GlobalData.instance.player.gem >= price;
    }

}
