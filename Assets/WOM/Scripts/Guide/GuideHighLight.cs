using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GuideHighLight : MonoBehaviour
{
   
    Image image;
Sequence sequence; 
    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
           DOTween.Init();
    }
    void OnEnable()
    {

        // DOTween을 사용하여 alpha 값을 반복적으로 변경
        sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0.5f, 0.7f));
        sequence.Append(image.DOFade(0.2f, 0.7f));
        sequence.SetLoops(-1); // 무한 반복

    }
    void OnDisable()
    {
        sequence.Kill();
    }

}
