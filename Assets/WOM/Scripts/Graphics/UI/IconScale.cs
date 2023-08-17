using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IconScale : MonoBehaviour
{

    public RectTransform rectTransform;
    [Header("x 아이콘 크기 배수"), Range(0.0f, 2)]
    public float multiscale = 1;
    [Range(0, 3)]
    public float timimg = 1;
    private float p = 0;

    public AnimationCurve actionCurve;

    private bool isDelay = true;
    [Header("액션 대기")]
    public float delayTime = 1.0f;
    private float dt = 0.0f;

    private void Start()
    {
        isDelay = true;
    }

    void FixedUpdate()
    {
        dt += Time.deltaTime;
        if (dt >= delayTime) isDelay = false;

        if (isDelay) return;

        p += Time.deltaTime;
        float comparePoint = Mathf.Clamp01(p * timimg);
        float value = actionCurve.Evaluate(comparePoint) * multiscale;
        value++;
        rectTransform.localScale = Vector3.one * value;

        //reset
        if(comparePoint >= 1)
        {
            isDelay = true;
            p = dt = 0;
        }
    }
}
