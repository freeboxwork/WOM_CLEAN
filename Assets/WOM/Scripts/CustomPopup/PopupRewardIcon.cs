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
        //index�� 40~47���̶�� ����ũ, 32~39���̶�� ����, 24~31�̶�� ����, 16~23���̶�� ���, 8~15���̶�� ���, 0~7���̶�� �Ϲ� �ؽ�Ʈ�� ��ȯ�Ѵ�.
        if (index >= 40 && index <= 47)
            return "<color=#D73232>����ũ</color>";
        else if (index >= 32 && index <= 39)
            return "<color=#FF7E28>����</color>";
        else if (index >= 24 && index <= 31)
            return "<color=#FF0E8C>����</color>";
        else if (index >= 16 && index <= 23)
            return "<color=#00CBFF>���</color>";
        else if (index >= 8 && index <= 15)
            return "<color=#46D732>���</color>";
        else
            return "<color=#B7B7B7>�Ϲ�</color>";

    }





}
