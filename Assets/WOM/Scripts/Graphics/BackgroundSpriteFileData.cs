using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BackgroundSpriteFileData", menuName = "ScriptableObject/BackgroundSpriteFileData")]
public class BackgroundSpriteFileData : ScriptableObject
{
    [Header("Background")] public Sprite[] background;
    
    public Sprite GetBackgroundData(int id)
    {
        return background[id];
    }






}
