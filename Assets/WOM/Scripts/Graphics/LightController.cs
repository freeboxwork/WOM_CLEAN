using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LightController : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D areaLight;
    public Light2D spotLight;

    //기본라이트 세팅
    private Color baseLightColor;
    private Color baseAraeColor;
    private float baseLightIntensity;
    private float baseAreaIntensity;
    
    [Header("보스전 라이트 세팅")]
    public Color cautionLightColor;
    public Color cautionAraeColor;
    [Range(0.0f, 2.0f), SerializeField]
    private float cautionLightIntensity;
    [Range(0.0f, 2.0f), SerializeField]
    private float cautionAreaIntensity;

    //스팟라이트 세팅
    [Header("스팟라이트"), Range(0.0f, 2.0f), SerializeField]
    private float spotLightIntensity;

    public float changeDuration = 1.0f;
    
    public enum LightState
    {
        NORMAL, BOSS
    }

    private void Awake()
    {
        baseLightColor = globalLight.color;
        baseLightIntensity = globalLight.intensity;
        baseAraeColor = areaLight.color;
        baseAreaIntensity = areaLight.intensity;

        spotLight.intensity = 0;
        spotLight.enabled = false;
    }

    public void SetLightStateSetup(LightState state)
    {
        switch (state)
        {
            case LightState.NORMAL:
                DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, baseLightIntensity, changeDuration);
                DOTween.To(() => globalLight.color, x => globalLight.color = x, baseLightColor, changeDuration);
                DOTween.To(() => areaLight.intensity, x => areaLight.intensity = x, baseAreaIntensity, changeDuration);
                DOTween.To(() => areaLight.color, x => areaLight.color = x, baseAraeColor, changeDuration);
                DOTween.To(() => spotLight.intensity, x => spotLight.intensity = x, 0, changeDuration).OnComplete(() => spotLight.enabled = false);
                /*
                globalLight.color = baseLightColor;
                globalLight.intensity = baseLightIntensity;
                areaLight.color = baseAraeColor;
                areaLight.intensity = baseAreaIntensity;
                */
                break;
            case LightState.BOSS:
                DOTween.To(() => globalLight.color, x => globalLight.color = x, cautionLightColor, changeDuration);
                DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, cautionLightIntensity, changeDuration);
                DOTween.To(() => areaLight.color, x => areaLight.color = x, cautionAraeColor, changeDuration);
                DOTween.To(() => areaLight.intensity, x => areaLight.intensity = x, cautionAreaIntensity, changeDuration);
                spotLight.enabled = true;
                DOTween.To(() => spotLight.intensity, x => spotLight.intensity = x, spotLightIntensity, changeDuration);
                /*
                globalLight.color = cautionLightColor;
                globalLight.intensity = cautionLightIntensity;
                areaLight.color = cautionAraeColor;
                areaLight.intensity = cautionAreaIntensity;
                */
                break;
        }
    }
}
