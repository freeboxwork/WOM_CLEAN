using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimData : MonoBehaviour
{
    public string dataName;
    public EnumDefinition.AnimCurveType animCurveType;
    public float animDuration = 0.5f;
    [HideInInspector] public float value;
    [HideInInspector] public float animTime;
    [HideInInspector] public float animValue;
    [HideInInspector] public float animStartTime;

    public void ResetAnimData()
    {
        value = 0;
        animTime = 0f;
        animValue = 0f;
        animStartTime = Time.time;
    }

}
