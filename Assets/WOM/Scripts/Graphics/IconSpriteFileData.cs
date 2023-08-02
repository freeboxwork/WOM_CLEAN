using UnityEngine;
[CreateAssetMenu(fileName = "IconSpriteFileData", menuName = "ScriptableObject/IconSpriteFileData")]
public class IconSpriteFileData : ScriptableObject
{
    [Header("Icons")] public Sprite[] rewardIcons;
    [Header("DungeonBox")] public Sprite[] boxImage;

    public Sprite GetIconData(int id)
    {
        return rewardIcons[id];
    }
    public Sprite GetBoxIcon(EnumDefinition.MonsterType monsterType)
    {
        return boxImage[(int)monsterType];
    }


}
