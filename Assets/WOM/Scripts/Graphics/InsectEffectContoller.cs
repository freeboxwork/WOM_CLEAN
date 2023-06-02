using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace ProjectGraphics
{
    public class InsectEffectContoller : MonoBehaviour
    {
        [SerializeField] SpriteRenderer fire;
        [SerializeField] SpriteRenderer thunder;
        [SerializeField] TrailRenderer trail;

        [SerializeField] InsectSpriteAnimation anim;
        [SerializeField] Sprite[] fireSprites;
        [SerializeField] Sprite[] thunderSprites;


        private void Awake()
        {
            fire.enabled = thunder.enabled = trail.enabled = false;    
        }

        #region »≠ø∞ ¿Ã∆Â∆Æ
        public void FireEffect(bool on)
        {
            if (on) anim.EffectEvent += FireEffectAnimation;
            else anim.EffectEvent -= FireEffectAnimation;
            fire.enabled = on;
        }

        public void FireEffectAnimation(int i)
        {
            fire.sprite = fireSprites[i];
        }

        #endregion

        #region π¯∞≥ ¿Ã∆Â∆Æ
        public void ThunderEffect(bool on)
        {
            if(on) anim.EffectEvent += ThunderEffectAnimation;
            else anim.EffectEvent-= ThunderEffectAnimation; 
            thunder.enabled = on;
        }

        public void ThunderEffectAnimation(int i)
        {
            thunder.sprite = thunderSprites[i];
        }
        #endregion

        public void TrailEffect(bool on)
        {
            if (on) trail.enabled = true;
            else
            {
                trail.Clear();
                trail.enabled = false;
            }
        }

        bool test = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                test = !test;
                FireEffect(test);
                ThunderEffect(test);
            }
        }
    }
}