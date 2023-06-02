using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectSpriteAnimation : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] sprites;

    [Header("초당 프레임")]
    public float samplelate = 12;
    private float frameTime = 0;
    private float t = 0;

    private int currentIndex = 0;

    public delegate void EffectAnimation(int index);
    public EffectAnimation EffectEvent;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        t = 0;
        currentIndex = 0;   
    }

    private void Update()
    {
        //활성화 되지 않았을때 업데이트 돌리지 말것.
        if(gameObject.activeSelf == false) return;

        //초당 몇장이 그려질지 정의해줌
        frameTime = 1 / samplelate;

        //프레임 확인하기
        t += Time.deltaTime;
        if(t >= frameTime)
        {
            rend.sprite = sprites[currentIndex];

            if(EffectEvent != null) EffectEvent.Invoke(currentIndex);

            t = 0;
            currentIndex = (currentIndex >= sprites.Length - 1) ? 0 : currentIndex + 1;
        }
    }

    public void SetSprite(Sprite[] s)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = s[i];
        }
    }
}
