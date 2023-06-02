using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace ProjectGraphics
{
    //�ܼ��ϰ� �����ϰ�
    public class MessageSlot : MonoBehaviour
    {
        //[SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] Image backImage;
        [SerializeField] RectTransform rect;
        [SerializeField] GlobalPopupAnimationController popupAnimationController;

        Color titleTextColor;
        Color messageTextColor;
        Color backImageColor;

        private void Awake()
        {
            //�غ�
            //titleTextColor = titleText.color;
            messageTextColor = messageText.color;
            backImageColor = backImage.color;
        }

        private void OnEnable()
        {
            InitializedSlot();
        }

        //�ʱ�ȭ
        private void InitializedSlot()
        {
            transform.localScale = Vector3.zero;
            rect.anchoredPosition = popupAnimationController.startPosition;

            //titleText.color = titleTextColor;
            //titleText.text = "Title";

            messageText.color = messageTextColor;
            messageText.text = "Message";

            backImage.color = backImageColor;
        }

        //����
        public void OpenTheMessageAnimation(string title, string message)
        {
            //titleText.text = title;
            messageText.text = message;
            transform.DOScale(Vector3.one, popupAnimationController.openDuration).SetEase(Ease.OutBack).OnComplete(FadeOut);
        }

        //�и��� ����
        public void ClimbTheMessageAnimation()
        {
            //titleText.DOFade(0.0f, popupAnimationController.climbDuration).SetEase(Ease.InQuad);
            //FadeOut();
            rect.DOAnchorPos(popupAnimationController.climeTargetPosition, popupAnimationController.climbDuration).SetEase(Ease.OutQuart)
                .OnComplete(OnCompletedAnimation);
        }

        void FadeOut()
        {
            messageText.DOFade(0.0f, popupAnimationController.fadeOutDuration).SetEase(Ease.InQuad);
            backImage.DOFade(0.0f, popupAnimationController.fadeOutDuration).SetEase(Ease.InQuad).OnComplete(OnCompletedAnimation);;
        }

        private void OnCompletedAnimation()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            InitializedSlot();
        }
    }
}