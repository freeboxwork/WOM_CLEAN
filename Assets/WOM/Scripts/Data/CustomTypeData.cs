using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("CUSTOM_TYPE_DATA")]
public class CustomTypeData : MonoBehaviour
{
    public CustomTypeDataComponents components;
    [FormerlySerializedAs("custimType")] public EnumDefinition.CustomDataType customType;

    public int index;

    private void Start()
    {
        GetMyComponents();
    }

    public void GetMyComponents()
    {
        components.animController = UtilityMethod.CustomGetComponetMy<AnimationController>(gameObject);
        components.button         = UtilityMethod.CustomGetComponetMy<Button>(gameObject);
        components.text           = UtilityMethod.CustomGetComponetMy<TextMeshProUGUI>(gameObject);
        components.image          = UtilityMethod.CustomGetComponetMy<Image>(gameObject);
        components.tr             = UtilityMethod.CustomGetComponetMy<Transform>(gameObject);
        components.sr             = UtilityMethod.CustomGetComponetMy<SpriteRenderer>(gameObject);
        components.gm             = gameObject;
    }

}




