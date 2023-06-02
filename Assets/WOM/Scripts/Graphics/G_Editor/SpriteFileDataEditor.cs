using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SpriteFileData.SpriteDataInfo))]
public class SpriteDataInfoDraw : PropertyDrawer
{
    SpriteFileData.SpriteDataInfo data;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            EditorGUIUtility.labelWidth = 50;

            position.height = EditorGUIUtility.singleLineHeight;
            var halfWidth = position.width * 0.5f;

            var d = new Rect(position.x, position.y, 10, 80);
            EditorGUI.DrawRect(d, GetPropertyColor(property));

            //맨 윗줄
            //라벨 (정보표시)
            string indexNumber = property.displayName;
            indexNumber = Regex.Replace(indexNumber, @"\D", "");
            string spriteName = "";
            if (property.FindPropertyRelative("icon").objectReferenceValue != null)
            {
                spriteName = property.FindPropertyRelative("icon").objectReferenceValue.name;
            }

            EditorGUI.LabelField(new Rect(position.x + 10, position.y, position.width * 0.5f, position.height),
                                "No : " + indexNumber + " , " + spriteName);
            
            //Enum
            var enumRect = new Rect(position.x + 160, position.y, halfWidth - 20, position.height);
            var enumPropoerty = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(enumRect, enumPropoerty, GUIContent.none);

            /*
            //라벨
            EditorGUI.LabelField(new Rect(position.x + 180, position.y, position.width * 0.2f, position.height),
                                "Dis");
            //설명
            var discriptionRect =
                new Rect(position.x + 220, position.y, position.width * 0.3f, position.height);
            var discriptionProperty = property.FindPropertyRelative("discription");
            EditorGUI.PropertyField(discriptionRect, discriptionProperty, GUIContent.none);
            */

            //두번쨰 줄

            //아이콘 이미지 위치조정
            var iconRect = new Rect(position.x + 10, position.y + (position.height + 2), 50, 50);
            var iconProperty = property.FindPropertyRelative("icon");
            iconProperty.objectReferenceValue =
                EditorGUI.ObjectField(iconRect, iconProperty.objectReferenceValue, typeof(Sprite), false);
            EditorGUI.LabelField(new Rect(position.x + 10, position.y + 50, 50, 50), "icon");

            /*
            //스프라이트 이미지 위치조정 --> 배열로 변경됨.
            var spriteRect = new Rect(position.x + 75, position.y, 64, 64);
            var spriteProperty = property.FindPropertyRelative("sprites");
            spriteProperty.objectReferenceValue =
                EditorGUI.ObjectField(spriteRect, spriteProperty.objectReferenceValue, typeof(Sprite), false);
            EditorGUI.LabelField(new Rect(position.x + 75, position.y + 40, 64, 64), "sprites");
            */

            //스프라이트 이미지 배열처리
            int sideValue = 45;
            var spriteRect = new Rect(position.x + 70, position.y + (position.height + 2), 40, 40);
            var spritePropety = property.FindPropertyRelative("sprites");
            EditorGUI.PropertyField(spriteRect, spritePropety, GUIContent.none);
            int arraySize = spritePropety.arraySize;
            for (int i = 0; i < arraySize; i++)
            {
                var r = new Rect(position.x + 60 + ((i+1) * sideValue), position.y + (position.height + 2), 45, 45);
                var pr = spritePropety.GetArrayElementAtIndex(i);
                pr.objectReferenceValue = 
                    EditorGUI.ObjectField(r, pr.objectReferenceValue, typeof(Sprite), false);
            }
            EditorGUI.LabelField(new Rect(position.x + 105, position.y + 50, 50, 50), "sprites");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //return base.GetPropertyHeight(property, label);
        //float height = EditorGUI.GetPropertyHeight(property, label, true);
        return 80;
    }

    public Color GetPropertyColor(SerializedProperty property)
    {
        var valueIndex = property.FindPropertyRelative("type").enumValueIndex;
        switch (valueIndex)
        {
            case 0: return new Color(0.8f, 0.8f, 0.8f);
            case 1: return new Color(0.8f, 0.95f, 0.75f);
            case 2: return new Color(1.0f, 0.98f, 0.55f);
            case 3: return new Color(0.8f, 0.5f, 0.6f);
            case 4: return new Color(1.0f, 0.6f, 0.0f);
            case 5: return Color.red;
        }
        return Color.white;
    }
}
#endif
