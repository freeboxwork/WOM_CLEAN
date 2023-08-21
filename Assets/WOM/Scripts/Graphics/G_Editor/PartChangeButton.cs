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
        string minValue;
        string maxValue;
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

            if (GUILayout.Button("List Up Monster Data : 이미지 캡쳐합니다."))
            {
                change.SaveListMonsterPartNumbers();
            }

            if (GUILayout.Button("Save CSV Data : 데이터를 저장합니다."))
            {
                change.SaveCSVFileMonsterParts();
            }



            if (GUILayout.Button("무작위 이미지 조합 생성"))
            {
                change.ChangedRandomPart(int.Parse(minValue), int.Parse(maxValue));
            }


            minValue = EditorGUILayout.TextField("Min: ", minValue);

            maxValue = EditorGUILayout.TextField("Max", maxValue);


            if (GUILayout.Button("SET RAND"))
            {
                SetRand(change);
            }





            /*
            if (GUILayout.Button("CaptureImage On Playable"))
            {
                change.CaptureImage();
            }
            */
        }

        void SetRand(PartsChangeTest change)
        {
            // head , body
            int head = GetRand(0, 9);
            int body = GetRand(0, 9);
            // tail 
            int tail = GetRand(0, 9);
            // finger
            int finger = GetRand(0, 9);
            // foreArm
            int foreArm = GetRand(0, 9);
            // upperArm
            int upperArm = GetRand(0, 9);
            // leg
            int leg = GetRand(0, 9);

            // head , body
            change.parts["head"] = head;
            change.parts["body"] = body;
            // tail
            change.parts["tail"] = tail;
            // finger
            change.parts["finger"] = finger;
            // foreArm
            change.parts["foreArm"] = foreArm;
            // upperArm
            change.parts["upperArm"] = upperArm;
            // leg
            change.parts["leg_0"] = leg;
            change.parts["leg_1"] = leg;
            change.parts["leg_2"] = leg;

            foreach (var item in change.parts)
            {
                change.spritesController.ChangedSpritePartImage(item.Key, item.Value);
            }


        }

        int GetRand(int min, int max)
        {
            return Random.Range(min, max);
        }
    }
#endif
}