using UnityEngine;
using static EnumDefinition;
public class SpriteDataManager : MonoBehaviour
{
    public IconSpriteFileData iconSpriteFileData;

    public SerializableDictionary<RewardType, Sprite> rewardIconDic = new SerializableDictionary<RewardType, Sprite>();


    void Start()
    {
        SetRewardIconDic();
    }

    void SetRewardIconDic()
    {
        foreach (var item in System.Enum.GetValues(typeof(RewardType)))
        {
            rewardIconDic.Add((RewardType)item, iconSpriteFileData.GetIconData((int)item));
            Debug.Log(rewardIconDic[(RewardType)item].name);
        }
    }

    public Sprite GetRewardIcon(RewardType type)
    {
        return rewardIconDic[type];
    }
}
