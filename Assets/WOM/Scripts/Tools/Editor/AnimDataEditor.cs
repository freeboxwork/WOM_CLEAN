using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimData))]
public class AnimDataEditor : Editor
{
    GUISkin skin;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        AnimData _target = (AnimData)target;
        GUILayout.Space(10);

        //if (GUI.changed)
        //{
        
            
        //}


        if(skin == null)
        {
            var skinPath = $"Assets/WOM/EditorResources/SKIN/EditorGUI_Skin.guiskin";
            skin = (GUISkin)AssetDatabase.LoadAssetAtPath(skinPath, typeof(GUISkin));
        }
            

        var assetPaht = $"Assets/WOM/EditorResources/EasingThumbnails/{_target.animCurveType.ToString()}.png";
        var texture = (Texture)AssetDatabase.LoadAssetAtPath(assetPaht ,typeof(Texture));
        if (texture != null)
        {
            var previewImage = AssetPreview.GetAssetPreview(texture);
        }
        else
        {
            var path = $"Assets/WOM/EditorResources/EasingThumbnails/defualtThumbnail.png";
            texture = (Texture)AssetDatabase.LoadAssetAtPath(path, typeof(Texture));
        }

        GUILayout.BeginVertical("Box");
        GUILayout.Space(10);
        GUILayout.Label("CURVE TYPE PERVIEW", skin.GetStyle("easing"));
        GUILayout.Space(10);
        GUILayout.Label(texture, skin.GetStyle("easing"), GUILayout.Height(150f)) ;
        GUILayout.Space(10);
        GUILayout.EndHorizontal();

        
 

    }

}
