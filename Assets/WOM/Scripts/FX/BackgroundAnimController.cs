using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimController : MonoBehaviour
{

    public float y_offsetRange = 0.2f;
    float cur_y_offset = 0f;

    public SpriteRenderer bg_rd;
    Material bgMat;
    
    // 보스 몬스터 제거시 배경 전환
    public AnimData animData_transition;

    // 일반,골드 몬스터 제거시 일정한 간격으로 이동
    public AnimData animData_BgScrollAnim;


    void Start()
    {
        bgMat = bg_rd.material;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        StartCoroutine(ScrollAnim_BG());
    //    }

    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        var nextBg = GlobalData.instance.stageManager.bgImages[1];
    //        StartCoroutine(TransitinBG(nextBg));
    //    }
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        var nextBg = GlobalData.instance.stageManager.bgImages[2];
    //        StartCoroutine(TransitinBG(nextBg));
    //    }
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        var nextBg = GlobalData.instance.stageManager.bgImages[3];
    //        StartCoroutine(TransitinBG(nextBg));
    //    }
    //}

    


    public void SetBgTex_Back(Texture2D tex)
    {
        bgMat.SetTexture("_texBack", tex);
    }

    public void SetBgTex_Front(Texture2D tex)
    {
        bgMat.SetTexture("_texFront", tex);
    }

    public void SetScrollSpeed(float value)
    {
        bgMat.SetFloat("_speed", value);
    }

    public void SetOffsetY(float value)
    {
        bgMat.SetFloat("_offsetY", value);
    }

    public float GetOffsetY()
    {
        return bgMat.GetFloat("_offsetY");
    }

    void ResetBG_Mat()
    {
        bgMat.SetFloat("_transition", 0);
    }


    public IEnumerator ScrollAnim_BG()
    {
        var offsetValue = GetOffsetY();
        animData_BgScrollAnim.ResetAnimData();
        // 0~1 in
        while (animData_BgScrollAnim.animValue < 0.999f)
        {
            animData_BgScrollAnim.animTime = (Time.time - animData_BgScrollAnim.animStartTime) / animData_BgScrollAnim.animDuration;
            animData_BgScrollAnim.animValue = EaseValues.instance.GetAnimCurve(animData_BgScrollAnim.animCurveType, animData_BgScrollAnim.animTime);
            float value = Mathf.Lerp(0f, y_offsetRange, animData_BgScrollAnim.animValue);
            cur_y_offset = offsetValue + value;
            SetOffsetY(cur_y_offset);
            yield return null;
        }
    }


    public IEnumerator TransitinBG(Sprite nextBg)
    {
        // SET TEXTURE - FRONT ( next bg )
        SetBgTex_Front(nextBg.texture);

        animData_transition.ResetAnimData();
        while (animData_transition.animValue < 0.999f)
        {
            animData_transition.animTime = ((Time.time - animData_transition.animStartTime) / animData_transition.animDuration);
            animData_transition.animValue = EaseValues.instance.GetAnimCurve(animData_transition.animCurveType, animData_transition.animTime);
            bgMat.SetFloat("_transition", animData_transition.animValue);
            yield return null;
        }

        // SET TEXTURE - BACK ( CURRENT BG (nextBg) )
        SetBgTex_Back(nextBg.texture);

        // RESET TRANSITION VALUE ( 0 )
        bgMat.SetFloat("_transition", 0f);
    }

}
