using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace ProjectGraphics
{
    public class ClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("온오프 오브젝트")]
        public GameObject slotBack;
        public Image slotImage;
        [Header("AdditiveColor")]
        public Gradient gradientColor;
        public AnimationCurve curve;
        [SerializeField, Range(0.5f, 2.0f)] float sizeFilter = 0.5f;
        [Header("ActionText")]
        public TextMeshProUGUI[] actionTexts;
        private float[] baseFontSizes;
        public float duration = 1.0f;
        bool isLongClick = false;
        bool isButtonClick = false;
        public TraningSlot trainingSlot;

        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();

        private void Awake()
        {
            baseFontSizes = new float[actionTexts.Length];
            for (int i = 0; i < actionTexts.Length; i++)
            {
                baseFontSizes[i] = actionTexts[i].fontSize;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(isButtonClick || !trainingSlot.GetBuyButtonInteracTable()) return;
            isButtonClick = true;
            isLongClick = false;
            StopCoroutine("ClickEffectProcess");
            StartCoroutine("ClickEffectProcess");
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isButtonClick = false;
            isLongClick = false;
        }
        public void SetButtonPressType(bool bLong)
        {
            isLongClick = bLong;
        }

        public void ResetClickEffect()
        {
            StopCoroutine("ClickEffectProcess");

            slotBack.SetActive(false);
        }

        IEnumerator ClickEffectProcess()
        {
            if (!trainingSlot.GetBuyButtonInteracTable())
            {
                slotBack.SetActive(false);
                yield break;
            }

            slotBack.SetActive(true);

            yield return null;
            float t = 0;
            float clampValue = 0;

            while (clampValue <= 1)
            {
                t += Time.deltaTime;
                clampValue = t / duration;

                slotImage.color = gradientColor.Evaluate(clampValue);
                float upsize = (curve.Evaluate(clampValue) * sizeFilter) + 1;

                for (int i = 0; i < actionTexts.Length; i++)
                {
                    actionTexts[i].fontSize = baseFontSizes[i] * upsize;
                }

 


                yield return new WaitUntil(() => !isLongClick);

                yield return waitFrame;
            }


            yield return null;

            //Return
            for (int i = 0; i < actionTexts.Length; i++)
            {
                actionTexts[i].fontSize = baseFontSizes[i];
            }



            slotBack.SetActive(false);
        }
    }
}