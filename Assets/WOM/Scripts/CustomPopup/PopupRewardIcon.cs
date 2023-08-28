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
        if(!reward.type.Equals(EnumDefinition.RewardType.gem))
        {
             amount.text = UtilityMethod.ChangeSymbolNumber(reward.amount.ToString());
             return;
        }
             amount.text = reward.amount.ToString();

    }
}
