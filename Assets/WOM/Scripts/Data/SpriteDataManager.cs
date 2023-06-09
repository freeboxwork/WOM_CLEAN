using System.Collections.Generic;
using UnityEngine;
using static EnumDefinition;
public class SpriteDataManager : MonoBehaviour
{
    public IconSpriteFileData iconSpriteFileData;

    Dictionary<RewardType, Sprite> rewardIconDic = new Dictionary<RewardType, Sprite>();


    void Start()
    {
        SetRewardIconDic();
    }

    void SetRewardIconDic()
    {
        foreach (var item in System.Enum.GetValues(typeof(RewardType)))
        {
            rewardIconDic.Add((RewardType)item, iconSpriteFileData.GetIconData((int)item));
        }
    }

    public Sprite GetRewardIcon(RewardType type)
    {
        return rewardIconDic[type];
    }
}
