using UnityEngine;
using static EnumDefinition;
public class SpriteDataManager : MonoBehaviour
{
    public IconSpriteFileData iconSpriteFileData;
    public SpriteFileData spriteFileData;

    public SerializableDictionary<RewardType, Sprite> rewardIconDic = new SerializableDictionary<RewardType, Sprite>();

    public SerializableDictionary<UiIconType, Sprite> uiIconDic = new SerializableDictionary<UiIconType, Sprite>();


    void Start()
    {
        // SetRewardIconDic();
    }

    // void SetRewardIconDic()
    // {
    //     foreach (var item in System.Enum.GetValues(typeof(RewardType)))
    //     {
    //         rewardIconDic.Add((RewardType)item, iconSpriteFileData.GetIconData((int)item));
    //         // Debug.Log(rewardIconDic[(RewardType)item].name);
    //     }
    // }

    public Sprite GetRewardIcon(RewardType type)
    {
        return rewardIconDic[type];

    }

    public Sprite GetUIIcon(string type)
    {
        return uiIconDic[(UiIconType)System.Enum.Parse(typeof(UiIconType), type)];
    }

    public Sprite GetRewardIcon(RewardType rewardType, int value)
    {
        if (rewardType == EnumDefinition.RewardType.union)
        {
            return spriteFileData.GetIconData(value);
        }
        else
        {
            return rewardIconDic[rewardType];
        }

    }

    public Sprite GetUnionSprite(int id)
    {
        return spriteFileData.GetIconData(id);
    }
}
