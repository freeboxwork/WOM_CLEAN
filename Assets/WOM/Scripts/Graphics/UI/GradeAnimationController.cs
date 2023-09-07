using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EnumDefinition;
using SRF;

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

             public List<GameObject> enableObjects;

        public Button btnClose;

        // 추가 

        [Header("현재 시작하는 등급")]
        public int gradeIndex = 0;
        [SerializeField] ImageResources[] imageResources;
        [Header("등급별 파티클 추가, 0번은 별도로 플레이됨")]
        [SerializeField] GameObject[] particleObjects;

        [SerializeField] Image[] beforeInsectSprite;
        [SerializeField] Image[] afterInsectSprite;

        [SerializeField] Sprite[] insectSprite1;
        [SerializeField] Sprite[] insectSprite2;
        [SerializeField] Sprite[] insectSprite3;
        [SerializeField] Sprite[] insectSprite4;
        [SerializeField] Sprite[] insectSprite5;

        Dictionary<int, Sprite[]> insectSpriteDic = new Dictionary<int, Sprite[]>();

        public AudioSource drum;
        public AudioSource explotion;

        void Awake()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }


        private void Start()
        {
            SetBtnEvent();

            insectSpriteDic.Add(0,insectSprite1);
            insectSpriteDic.Add(1,insectSprite2);
            insectSpriteDic.Add(2,insectSprite3);
            insectSpriteDic.Add(3,insectSprite4);
            insectSpriteDic.Add(4,insectSprite5);

        }

        void SetBtnEvent()
        {
            btnClose.onClick.AddListener(() =>
            {
                // 진화 메뉴 등장
                //GlobalData.instance.uiController.EnableMenuPanel(MenuPanelType.evolution);
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

            iconUI.sprite = imageResources[startIndex].icon;

            //SetImageResources(startIndex);

            foreach (var obj in particleObjects) { obj.SetActive(false); }

            //animation Start
            anim.SetTrigger("Action");
        }

        public void SetImageResources(int num)
        {
            if (num >= imageResources.Length) return;

            backgroundColor.color = imageResources[num].backGroundColor;
            glareColor.color = imageResources[num].glareColor;
            shinyColor.color = imageResources[num].backShinyColor;
            iconUI.sprite = imageResources[num].icon;

        }

        void EnableInsectIcon()
        {
            for (int i = 0; i < gradeIndex + 1; i++)
            {
                if (i == 0) continue;
                particleObjects[i].SetActive(true);
            }
        }

        public void PlayDrum()
        {
            drum.Play();
        }
        public void PlayExPlotion()
        {
            explotion.Play();
        }
        public void AnimEventChangeGradeAction()
        {
           // beforeGradeIcon.sprite = imageResources[gradeIndex - 1].icon;
            //afterGradeIcon.sprite = imageResources[gradeIndex].icon;

             for (int i = 0; i < beforeInsectSprite.Length; i++)
             {
                beforeInsectSprite[i].sprite = insectSpriteDic[gradeIndex - 1][i];
                afterInsectSprite[i].sprite = insectSpriteDic[gradeIndex][i];
             }

            //SetImageResources(gradeIndex);
            iconUI.sprite = imageResources[gradeIndex].icon;

            //파티클 숨김
            particleObjects[0].SetActive(false);
        }

        public void EnableEffects(bool value)
        {
            foreach (var element in enableObjects)
                element.SetActive(value);
        }

        public void EnableParticle()
        {
            for (int i = 0; i < gradeIndex + 1; i++)
            {
                if (i == 0) continue;
                particleObjects[i].SetActive(true);
            }
        }

        public void EnableStarParticle()
        {
            particleObjects[0].SetActive(true);
        }
    }
}


