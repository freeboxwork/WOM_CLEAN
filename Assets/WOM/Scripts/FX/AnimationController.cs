using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class AnimationController : MonoBehaviour
{
    public AnimData animData;
    public Transform m_transform;
    public int animSystem_ID;
    public bool isAnimPlay = false;

    void Start()
    {
        if (animData == null)
            animData = UtilityMethod.CustomGetComponet<AnimData>();
    }

    public bool GetAnimPlayState()
    {
        return isAnimPlay;
    }

    // ANIM POSITION
    public IEnumerator AnimPosition(Vector3 startPos, Vector3 enaPos)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            //Debug.Log(animData.animTime);
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            m_transform.localPosition = Vector3.Lerp(startPos, enaPos, animData.animValue);
            yield return null;
        }
        isAnimPlay = false;
    }

    // TODO: ANIM ROTATION

    // TODO: ANIM SCALE

    // MATERIAL PROPERTY ANIMATION
    public IEnumerator MaterialAnim(Material mat, string property)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            //Debug.Log(animData.animTime);
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            mat.SetFloat(property, animData.animValue);
            yield return null;
        }
        isAnimPlay = false;
    }

    public IEnumerator MaterialAnimMinMax(Material mat, string property, (float,float)minMax)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            //Debug.Log(animData.animTime);
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            var value = Mathf.Lerp(minMax.Item1,minMax.Item2,animData.animValue);   
            mat.SetFloat(property, value);
            yield return null;
        }
        isAnimPlay = false;
    }


    /* ColorChange */
    public IEnumerator MaterialAnimColor(Material mat, string property, (Color, Color) colors)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            //Debug.Log(animData.animTime);
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            var value = Color.Lerp(colors.Item1, colors.Item2, animData.animValue);

            mat.SetColor(property, value);
            yield return null;
        }
        isAnimPlay = false;
    }


    // MATERIAL PROPERTY ANIMATION CallBack Event
    public IEnumerator MaterialAnim(Material mat, string property, UnityAction callBackEvent)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            //Debug.Log(animData.animTime);
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            mat.SetFloat(property, animData.animValue);
            yield return null;
        }
        isAnimPlay = false;
        
        //yield return new WaitForEndOfFrame();
        callBackEvent.Invoke();
    }

    // UI IMAGE CLOLOR ANIM - CALLBACK COMPLETE EVENT
    public IEnumerator UI_ImageColorAnim(Image image, Color start, Color end,UnityAction callBackEvent = null)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            image.color = Color.Lerp(start, end, animData.animValue);
            yield return null;
        }
        isAnimPlay = false;
        if(callBackEvent != null)   
            callBackEvent.Invoke();
    }


    // UI IMAGE FILL AMOUNT
    public IEnumerator UI_ImageFillAmountAnim(Image image, float start,float end,  UnityAction callBackEvent = null)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            image.fillAmount = Mathf.Lerp(start, end, animData.animValue);
            yield return null;
        }
        isAnimPlay = false;
        if (callBackEvent != null)
            callBackEvent.Invoke();
    }


    // UI TEXT ANIMATION
    public IEnumerator UI_TextAnim(TextMeshProUGUI text, float start, float end, UnityAction callBackEvent = null)
    {
        isAnimPlay = true;
        animData.ResetAnimData();
        while (animData.animTime < 0.999f)
        {
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
            var textValue = Mathf.Lerp(start, end, animData.animValue);
            text.text = string.Format("{0:0.00} ", textValue) +"s";
            yield return null;
        }
        isAnimPlay = false;
        if (callBackEvent != null)
            callBackEvent.Invoke();
    }

}
