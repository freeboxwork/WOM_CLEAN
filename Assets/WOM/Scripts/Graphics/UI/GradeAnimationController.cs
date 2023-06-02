using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EnumDefinition;

namespace ProjectGraphics
{
    public class GradeAnimationController : MonoBehaviour
    {
        [System.Serializable]
        public struct ImageResources
        {
            public string name;
            public Sprite icon;
            public Color backGroundColor;
            public Color glareColor;
            public Color backShinyColor;
        }

        private Animator anim;
        private AudioSource audioSource;

        public Image backgroundColor;
        public Image glareColor;
        public Image shinyColor;
        public Image iconUI;
        public TextMeshProUGUI gradeText;

        public List<GameObject> enableObjects;

        public Button btnClose;

        // 추가 

        [Header("현재 시작하는 등급")]
        public int gradeIndex = 0;
        [SerializeField] ImageResources[] imageResources;

        void Awake()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

        }


        private void Start()
        {
            btnClose = GetComponent<Button>();
            SetBtnEvent();
        }

        void SetBtnEvent()
        {

            btnClose.onClick.AddListener(() =>
            {
                // 진화 메뉴 등장
                GlobalData.instance.uiController.EnableMenuPanel(MenuPanelType.evolution);
                // 일반 몬스터 등장
                // GlobalData.instance.eventController.NormalMonsterIn();

                GlobalData.instance.eventController.evalGradeEffectShow = false;

                gameObject.SetActive(false);

            });

        }
        private void OnEnable()
        {
            //시작시 gradeIndex 값을 정의 해주세요.
            int startIndex = 0;
            if (gradeIndex <= 0 || gradeIndex >= imageResources.Length) startIndex = 0;
            else startIndex = gradeIndex - 1;
            SetImageResources(startIndex);

            //animation Start
            anim.SetTrigger("Action");
        }



        public void SetImageResources(int num)
        {
            if (num >= imageResources.Length) return;

            gradeText.text = imageResources[num].name;
            backgroundColor.color = imageResources[num].backGroundColor;
            glareColor.color = imageResources[num].glareColor;
            shinyColor.color = imageResources[num].backShinyColor;
            iconUI.sprite = imageResources[num].icon;
        }

        public void AnimEventChangeGradeAction()
        {
            SetImageResources(gradeIndex);
        }

        public void EnableEffects(bool value)
        {
            foreach (var element in enableObjects)
                element.SetActive(value);
        }

    }
}


