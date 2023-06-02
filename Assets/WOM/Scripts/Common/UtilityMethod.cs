using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;


#if UNITY_EDITOR
#endif

public static class UtilityMethod
{
    static readonly string[] symbol = new string[] { "", "K", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", };

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

    public static void SetTxtsCustomTypeByIDs(int[] ids, float[] values)
    {
        for (int i = 0; i < ids.Length; i++)
            SetTxtCustomTypeByID(ids[i], values[i].ToString());
    }


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

    public static void SetBtnsInteractableEnable(List<int> ids, bool value)
    {
        foreach (var id in ids)
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
        return 10 + (10 * GetUnLockCount());
    }

    static int GetUnLockCount()
    {
        var slotDatas = GlobalData.instance.evolutionManager.evolutionSlots;
        int unlock = 0;
        foreach (var slot in slotDatas)
            if (slot.isUnlock && slot.statOpend) unlock++;
        return unlock;
    }

    ///<summary> 골드 및 뼛조각 숫자 Text를 심볼로 변경 </summary>
    public static string ChangeSymbolNumber(float number)
    {


        string zero = "0";

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Max";
        }

        //  ??? ??? ?????
        string significant = (number < 0) ? "-" : string.Empty;

        //  ?????? ????
        string showNumber = string.Empty;

        //  ???? ?????
        string unityString = string.Empty;

        //  ?????? ???? ????? ???? ?????? ???? ????????? ?????? ?? ???
        string[] partsSplit = number.ToString("E").Split('+');

        //  ????
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ???? (????? ???)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  ???? ????? ?ε???
        int quotient = exponent / 3;

        //  ???????? ?????? ????? ??꿡 ???(10?? ????????? ???)
        int remainder = exponent % 3;

        //  1A ????? ??? ???
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate(number).ToString();
        }
        else
        {
            //  10?? ????????? ????? ????? ??????? ????? ???.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  ??? ??°????????? ??????.
            //showNumber = temp.ToString("F").Replace(".0", "");
            showNumber = temp.ToString("F");
        }

        unityString = symbol[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
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
