using UnityEngine;
[CreateAssetMenu(fileName = "IconSpriteFileData", menuName = "ScriptableObject/IconSpriteFileData")]
public class IconSpriteFileData : ScriptableObject
{
    [Header("Icons")] public Sprite[] rewardIcons;

    public Sprite GetIconData(int id)
    {
        return rewardIcons[id];
    }

}
