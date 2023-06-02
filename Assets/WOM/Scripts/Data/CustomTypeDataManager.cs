using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CustomTypeDataManager : MonoBehaviour
{
    

    public List<CustomTypeData> customTypeDatas = new List<CustomTypeData>();

    // TODO : 타입별로 셋팅 진행
    public List<CustomTypeData> customTypeDatas_animCont = new List<CustomTypeData>();
    public List<CustomTypeData> customTypeDatas_uiButton = new List<CustomTypeData>();
    public List<CustomTypeData> customTypeDatas_uiText = new List<CustomTypeData>();
    public List<CustomTypeData> customTypeDatas_uiImage = new List<CustomTypeData>();
    public List<CustomTypeData> customTypeDatas_transform = new List<CustomTypeData>();
    public List<CustomTypeData> customTypeDatas_gameObject = new List<CustomTypeData>();

    void Start()
    {
        
    }


    public CustomTypeData GetCustomTypeData(EnumDefinition.CustomDataType customType, int index)
    {
        var data = customTypeDatas?.FirstOrDefault(f => f.customType == customType && f.index == index);
        if (data != null)
            return data;
        else
            Debug.LogError($"{customType}_{index} custom type data 가 없습니다..");
        return null;
    }

    public List<CustomTypeData> GetCustomTypeDatas(EnumDefinition.CustomDataType dataType)
    {
        switch (dataType)
        {
            case EnumDefinition.CustomDataType.animCont: return customTypeDatas_animCont;
            case EnumDefinition.CustomDataType.button: return customTypeDatas_uiButton;
            case EnumDefinition.CustomDataType.text: return customTypeDatas_uiText;
            case EnumDefinition.CustomDataType.image: return customTypeDatas_uiImage;
            case EnumDefinition.CustomDataType.tr: return customTypeDatas_transform;
            case EnumDefinition.CustomDataType.gm: return customTypeDatas_gameObject;
            default: return new List<CustomTypeData>();
        }
    }

    public Button GetCustomTypeData_Button(int index)
    {
        //Debug.Log(index);
        var data = customTypeDatas_uiButton?.FirstOrDefault(f=> f.index == index);
        if (data != null)
            return data.components.button;
        else
            Debug.LogError($"{EnumDefinition.CustomDataType.button}_{index} custom type data 가 없습니다.");
        return null;
    }

    public TextMeshProUGUI GetCustomTypeData_Text(int index)
    {
        TextMeshProUGUI data = null;
        try
        {
            data = customTypeDatas_uiText.FirstOrDefault(f => f.index == index).components.text;
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
            Debug.LogError($"커스텀 데이터 {EnumDefinition.CustomDataType.text} 타입의 인덱스 넘버 : {index} 가 없습니다.");
        }
        return data;
        
        //if (data != null)
        //    return data;
        //else
        //    Debug.LogError($"{EnumDefinition.CustomDataType.text}_{index} custom type data 가 없습니다.");
        //return null;
    }

    public Image GetCustomTypeData_Image(int index)
    {
        var data = customTypeDatas_uiImage?.FirstOrDefault(f => f.index == index).components.image;
        if (data != null)
            return data;
        else
            Debug.LogError($"{EnumDefinition.CustomDataType.image}_{index} custom type data 가 없습니다.");
        return null;
    }

    public Transform GetCustomTypeData_Transform(int index)
    {
        var data = customTypeDatas_transform?.FirstOrDefault(f => f.index == index).components.tr;
        if (data != null)
            return data;
        else
            Debug.LogError($"{EnumDefinition.CustomDataType.tr}_{index} custom type data 가 없습니다.");
        return null;
    }

    public GameObject GetCustomTypeData_Gameobject(int index)
    {
        var data = customTypeDatas_gameObject?.FirstOrDefault(f => f.index == index).components.gm;
        if (data != null)
            return data;
        else
            Debug.LogError($"{EnumDefinition.CustomDataType.tr}_{index} custom type data 가 없습니다.");
        return null;
    }


    public AnimationController GetCustomTypeData_AnimCont(int index)
    {
        var data = customTypeDatas_animCont?.FirstOrDefault(f => f.index == index).components.animController;
        if (data != null)
            return data;
        else
            Debug.LogError($"{EnumDefinition.CustomDataType.animCont}_{index}  custom type data 가 없습니다.");
        return null;
    }
   
}
