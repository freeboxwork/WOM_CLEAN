using UnityEngine;
using static EnumDefinition;
public class SpriteDataManager : MonoBehaviour
{
    public IconSpriteFileData iconSpriteFileData;
    public SpriteFileData spriteFileData;

    public SerializableDictionary<RewardType, Sprite> rewardIconDic = new SerializableDictionary<RewardType, Sprite>();

    public SerializableDictionary<UiIconType, Sprite> uiIconDic = new SerializableDictionary<UiIconType, Sprite>();

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
