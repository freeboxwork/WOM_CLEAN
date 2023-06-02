using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace ProjectGraphics
{
#if UNITY_EDITOR
    [CustomEditor(typeof(PartsChangeTest))]
    public class PartChangeButton : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PartsChangeTest change = (PartsChangeTest)target;

            if (GUILayout.Button("Init : 누르고 시작하세요."))
            {
                change.InitializedSpriteImage();
            }

            if (GUILayout.Button("Change Sprite : 0번부터 32번까지"))
            {
                change.UpdateSpriteImage();
            }

            if (GUILayout.Button("Change Sprite Parts : 이미지를 변경합니다."))
            {
                change.ChangedPartDictionaryValue();
            }

            if(GUILayout.Button("List Up Monster Data : 이미지 캡쳐합니다."))
            {
                change.SaveListMonsterPartNumbers();
            }

            if(GUILayout.Button("Save CSV Data : 데이터를 저장합니다."))
            {
                change.SaveCSVFileMonsterParts();
            }

            /*
            if (GUILayout.Button("CaptureImage On Playable"))
            {
                change.CaptureImage();
            }
            */
        }
    }
#endif
}