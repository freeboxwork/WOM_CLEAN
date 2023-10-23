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

        // DOTween�� ����Ͽ� alpha ���� �ݺ������� ����
        sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0.5f, 0.7f));
        sequence.Append(image.DOFade(0.2f, 0.7f));
        sequence.SetLoops(-1); // ���� �ݺ�

    }
    void OnDisable()
    {
        sequence.Kill();
    }

}
