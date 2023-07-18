using UnityEngine;


namespace ProjectGraphics
{
    public class InsectEffectContoller : MonoBehaviour
    {
        private SpriteRenderer rend;
        [SerializeField] SpriteRenderer fire;
        [SerializeField] SpriteRenderer thunder;
        [SerializeField] SpriteRenderer auraEffect; //«‘º∫
        [SerializeField] SpriteRenderer goldEffect; //∞ÒµÂ
        [SerializeField] TrailRenderer trail;

        [SerializeField] InsectSpriteAnimation anim;
        [SerializeField] Sprite[] fireSprites;
        [SerializeField] Sprite[] thunderSprites;
        [SerializeField] Sprite[] auraSprites;
        [SerializeField] Sprite[] goldSprites;

        private void Awake()
        {
            fire = transform.Find("Fire").GetComponent<SpriteRenderer>();
            thunder = transform.Find("Thunder").GetComponent <SpriteRenderer>();
            auraEffect = transform.Find("Aura").GetComponent<SpriteRenderer>();
            goldEffect = transform.Find("Gold").GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            fire.enabled = thunder.enabled = trail.enabled = false;
            goldEffect.enabled = auraEffect.enabled = false;
            rend = GetComponent<SpriteRenderer>();
        }

        void OnDisable()
        {
            FireEffect(false);
            ThunderEffect(false);
            TrailEffect(false);
            AuraEffect(false);
            GoldEffect(false);
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
            if (on) anim.EffectEvent += ThunderEffectAnimation;
            else anim.EffectEvent -= ThunderEffectAnimation;
            thunder.enabled = on;
        }

        public void ThunderEffectAnimation(int i)
        {
            thunder.sprite = thunderSprites[i];
        }
        #endregion

        #region º”µµ¿Ã∆Â∆Æ(≤ø∏Æ)
        public void TrailEffect(bool on)
        {
            if (on) trail.enabled = true;
            else
            {
                trail.Clear();
                trail.enabled = false;
            }
        }
        #endregion

        #region «‘º∫¿Ã∆Â∆Æ(æ∆øÏ∂Û)
        public void AuraEffect(bool on)
        {

            Debug.Log("AURA EFFECT");
            if (on) anim.EffectEvent += AuraEffectAnimation;
            else anim.EffectEvent -= AuraEffectAnimation;
            auraEffect.enabled = on;
        }

        public void AuraEffectAnimation(int i)
        {
            auraEffect.sprite = auraSprites[i];
        }
        #endregion

        #region ∞ÒµÂ¿Ã∆Â∆Æ(±›∫˚ ø‹∞˚º±)
        public void GoldEffect(bool on)
        {
            if (on) anim.EffectEvent += GoldEffectAnimation;
            else anim.EffectEvent -= GoldEffectAnimation;
            goldEffect.enabled = on;
        }

        public void GoldEffectAnimation(int i)
        {
            goldEffect.sprite = goldSprites[i];
        }
        #endregion

#if UNITY_EDITOR
        public bool test = false;
        private void Update()
        {

            //FireEffect(test);
            //ThunderEffect(test);
            //GoldEffect(test);
            //AuraEffect(test);

        }
#endif
    }
}