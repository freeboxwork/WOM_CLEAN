using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupRewardIcon : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amount;

    public void SetUI(RewardInfoData reward)
    {
        Debug.Log(reward.icon);
        icon.sprite = reward.icon;
        amount.text = UtilityMethod.ChangeSymbolNumber(reward.amount);
    }
}
