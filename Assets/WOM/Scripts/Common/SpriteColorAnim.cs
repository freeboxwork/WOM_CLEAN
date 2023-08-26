using UnityEngine;
using System.Collections.Generic;


public class SpriteColorAnim : MonoBehaviour
{

    public Color colorNormal;
    public Color colorChange;
    public List<Color> colorList; // stage 별 컬러 값?

    public SpriteRenderer sr;
    public AnimationController animController;

    public LightController lightController;

    void Start()
    {
        lightController = GameObject.Find("Lights").GetComponent<LightController>();
    }

    /// <summary>
    /// 컬러 변경 애니메이션 ( boss )
    /// </summary>
    public void ColorChangeAnim()
    {
        //StartCoroutine(animController.SpriteRendereColorAnim(sr, colorNormal, colorChange));
        lightController.SetLightStateSetup(LightController.LightState.BOSS);
    }

    // 원래 컬러로 변경
    public void ColorNormalAnim()
    {
        //StartCoroutine(animController.SpriteRendereColorAnim(sr, colorChange, colorNormal));
        lightController.SetLightStateSetup(LightController.LightState.NORMAL);
    }

}
