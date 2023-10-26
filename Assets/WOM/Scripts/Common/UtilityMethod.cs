using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;
using Cargold.Infinite;
using TMPro;


#if UNITY_EDITOR
#endif

public static class UtilityMethod
{

    public static void EnableUIEventSystem(bool value)
    {
        GlobalData.instance.eventSystem.enabled = value;
    }
    public static T CustomGetComponet<T>() where T : UnityEngine.Object
    {
        var returnValue = GameObject.FindObjectOfType<T>();
        if (returnValue == null) Debug.Log($"씬에 존재하지 않는 타입 입니다.");
        return returnValue;
    }

    public static T CustomGetComponetMy<T>(GameObject gameObject) where T : UnityEngine.Object
    {
        if (gameObject.TryGetComponent(out T value))
        {
            return value;
        }
        else
        {
            //Debug.LogError($"{gameObject.name} 게임 오브젝트에 컴포넌트 타입이 없습니다.");
            return null;
        }
    }

    public static float GetRandomRangeValue_X(Vector2 point, float rv)
    {
        return Random.Range(point.x - rv, point.x + rv);
    }
    public static float GetRandomRangeValue_Y(Vector2 point, float rv)
    {
        return Random.Range(point.y - rv, point.y + rv);
    }

    public static Vector2 GetTargetRandomRangeValue_V2(Vector2 x_min, Vector2 x_max, Vector2 y_min, Vector2 y_max)
    {
        var x = Random.Range(x_min.x, x_max.x);
        var y = Random.Range(y_min.y, y_max.y);
        return new Vector2(x, y);
    }


    public static void SetTxtCustomTypeByID(int id, string value)
    {
        var txtObj = GlobalData.instance.customTypeDataManager.GetCustomTypeData_Text(id);
        if (txtObj != null)
        {
            txtObj.text = value;
        }
        else
        {
            Debug.LogError($"Custom Type Object - Text - {id} 오브젝트가 없습니다.");
        }
    }

    // public static void SetTxtsCustomTypeByIDs(int[] ids, float[] values)
    // {
    //     for (int i = 0; i < ids.Length; i++)
    //         SetTxtCustomTypeByID(ids[i], values[i].ToString());
    // }


    public static void SetImageSpriteCustomTypeByID(int id, Sprite sprite)
    {
        GetCustomTypeImageById(id).sprite = sprite;
    }


    public static void SetBtnEventCustomTypeByID(int id, UnityAction action)
    {
        var btn = GlobalData.instance.customTypeDataManager.GetCustomTypeData_Button(id);
        if (btn != null)
        {
            btn.onClick.AddListener(action);
        }
        else
        {
            Debug.LogError($"Custom Type Object - Button - {id} 오브젝트가 없습니다.");
        }
    }

    public static void SetBtnInteractableEnable(int id, bool value)
    {
        var btn = GlobalData.instance.customTypeDataManager.GetCustomTypeData_Button(id);
        if (btn != null)
        {
            btn.interactable = value;
        }
        else
        {
            Debug.LogError($"Custom Type Object - Button - {id} 오브젝트가 없습니다.");
        }
    }

    // public static void SetBtnsInteractableEnable(List<int> ids, bool value)
    // {
    //     foreach (var id in ids)
    //     {
    //         var btn = GlobalData.instance.customTypeDataManager.GetCustomTypeData_Button(id);
    //         if (btn != null)
    //         {
    //             btn.interactable = value;
    //         }
    //         else
    //         {
    //             Debug.LogError($"Custom Type Object - Button - {id} 오브젝트가 없습니다.");
    //         }
    //     }
    // }


    public static TextMeshProUGUI GetTxtCustomTypeByID(int id)
    {
        return GlobalData.instance.customTypeDataManager.GetCustomTypeData_Text(id);
    }

    public static Button GetCustomTypeBtnByID(int id)
    {
        return GlobalData.instance.customTypeDataManager.GetCustomTypeData_Button(id);
    }

    public static Image GetCustomTypeImageById(int id)
    {
        return GlobalData.instance.customTypeDataManager.GetCustomTypeData_Image(id);
    }

    public static GameObject GetCustomTypeGMById(int id)
    {
        return GlobalData.instance.customTypeDataManager.GetCustomTypeData_Gameobject(id);
    }

    public static Transform GetCustomTypeTrById(int id)
    {
        return GlobalData.instance.customTypeDataManager.GetCustomTypeData_Transform(id);
    }

    /// <summary> 가중치 랜덤 뽑기 </summary>
    public static float GetWeightRandomValue(float[] probs)
    {
        float total = 0;
        foreach (float elem in probs)
        {
            total += elem;
        }
        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

    ///<summary> 진화 주사위 사용 개수 </summary>
    public static int GetEvolutionDiceUsingCount()
    {
        //return 10 + (10 * GetUnLockCount());
        return 10 * GetUnLockCount();
    }

    static int GetUnLockCount()
    {
        var slotDatas = GlobalData.instance.evolutionManager.evolutionSlots;
        int unlock = 0;
        foreach (var slot in slotDatas)
            if (slot.isUnlock && slot.statOpend) unlock++;
        return unlock;
    }

    private const string FillZeroFormat = "D{0}";
    public static string ToString_Fill_Func(this int _value, int _fillZero)
    {
        string _fillZeroFormat = string.Format(FillZeroFormat, _fillZero);
        return _value.ToString(_fillZeroFormat);
    }
    
    public static string ChangeSymbolNumber(float number)
    {
        var n =  Mathf.Round(number);
        var s = n.ToString("#,##0");
        Infinite value = s.ToInfinite();
        //return value.ToStringLong ();//10억이후 Symbol
        return value.ToString();
    }


    public static EnumDefinition.RewardType GetRewardTypeByTypeName(string typeName)
    {
        foreach (EnumDefinition.RewardType type in System.Enum.GetValues(typeof(EnumDefinition.RewardType)))
            if (type.ToString() == typeName)
                return type;
        return EnumDefinition.RewardType.none;
    }


    public static string FormatDoubleToOneDecimal(float value)
    {
        //return string.Format("{0:F1}", value);
        return string.Format("{0:0.##}", value);
    }

    // public static long ConvertDoubleToLong(double number)
    // {
    //     return (long)System.Math.Round(number);
    // }

    /// <summary>
    /// 값을 한 범위에서 다른 범위로 재매핑합니다.
    /// </summary>
    /// <param name="value">재매핑할 값입니다.</param>
    /// <param name="from1">현재 범위의 최소값입니다.</param>
    /// <param name="to1">현재 범위의 최대값입니다.</param>
    /// <param name="from2">대상 범위의 최소값입니다.</param>
    /// <param name="to2">대상 범위의 최대값입니다.</param>
    public static float RemapValue(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


#if UNITY_EDITOR

    public static void AssignFieldsWithSameName<T, U>(T target, U source)
    {
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            FieldInfo sourceField = typeof(U).GetField(field.Name, BindingFlags.Public | BindingFlags.Instance);

            if (sourceField != null && sourceField.FieldType == field.FieldType)
            {
                object sourceValue = sourceField.GetValue(source);
                field.SetValue(target, sourceValue);
            }
        }
    }



#endif

}
