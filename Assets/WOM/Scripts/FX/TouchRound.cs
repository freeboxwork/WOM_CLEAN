using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TouchRound : MonoBehaviour
{
    Vector3  originScale;
    void Awake()
    {
        originScale = transform.localScale;
    }
    // Start is called before the first frame update
     void OnEnable()
    {
       transform.DOScale(originScale * 1.5f, 0.2f).OnKill(() =>transform.localScale = originScale).OnComplete(() => gameObject.SetActive(false));
    }

    void OnDisable()
    {
        transform.localScale = originScale;
    }
}
