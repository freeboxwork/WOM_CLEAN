using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InsectSpriteData", menuName = "ScriptableObject/InsectSpriteData")]
public class InsectSpriteFileData : ScriptableObject
{
    [Header("beeSprite")] public AnimationSpriteSet[] bee;
    [Header("beetleSprite")] public AnimationSpriteSet[] beetle;
    [Header("mantisSprite")] public AnimationSpriteSet[] mantis;

    public Sprite[] GetInsectFaceSprite(EnumDefinition.InsectType type, int id)
    {
        switch(type)
        {
            case EnumDefinition.InsectType.bee: return bee[id].GetSprites();
            case EnumDefinition.InsectType.beetle: return beetle[id].GetSprites();
            case EnumDefinition.InsectType.mentis: return mantis[id].GetSprites();
        }
        return null;
    }

    [System.Serializable]
    public struct AnimationSpriteSet
    {
        public Sprite[] sprites;
        public Sprite[] GetSprites() => sprites;
    }
}
