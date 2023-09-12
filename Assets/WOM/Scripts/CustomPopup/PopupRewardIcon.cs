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
        icon.sprite = reward.icon;
        if(!reward.type.Equals(EnumDefinition.RewardType.gem) && !reward.type.Equals(EnumDefinition.RewardType.union))
        {
             amount.text = UtilityMethod.ChangeSymbolNumber(reward.amount.ToString());
             return;
        }

        if(reward.type.Equals(EnumDefinition.RewardType.union))
        {
                    amount.text = GetGradeName((int)reward.amount);
return;

        }

        amount.text = reward.amount.ToString();

    }


    string GetGradeName(int index)
    {
        //index가 40~47사이라면 유니크, 32~39사이라면 전설, 24~31이라면 영웅, 16~23사이라면 희귀, 8~15사이라면 고급, 0~7사이라면 일반 텍스트를 반환한다.
        if (index >= 40 && index <= 47)
            return "<color=#D73232>유니크</color>";
        else if (index >= 32 && index <= 39)
            return "<color=#FF7E28>전설</color>";
        else if (index >= 24 && index <= 31)
            return "<color=#FF0E8C>영웅</color>";
        else if (index >= 16 && index <= 23)
            return "<color=#00CBFF>희귀</color>";
        else if (index >= 8 && index <= 15)
            return "<color=#46D732>고급</color>";
        else
            return "<color=#B7B7B7>일반</color>";

    }





}
