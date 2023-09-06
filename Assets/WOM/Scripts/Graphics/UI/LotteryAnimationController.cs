using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGraphics
{
    public class LotteryAnimationController : MonoBehaviour
    {
        public SpriteFileData data;
        [Header("DNA Icon ?΄λ―Έμ??"), SerializeField]
        private Sprite[] dnaIcons;
        [Header("? ??¨λ‘ν°λ¦? ?΄λ―Έμ??")]
        public Color[] effectColor;
        public Sprite[] gradeBackImage;
        public Lottery_Slot[] slots;

        [Header("?€?΅??? ?μ’λ₯λ‘? λΆλ₯ ?κ³? ?λ‘μΈ?€ ??? ?€?΅ λ³?? ? μ§?")]
        private bool isSkip = true;
        private bool isSkipDNA = true;
        public bool isUnion = true;
        public bool isEnd = false;

        [Header("μΊ ν??")]
        public CampPopup campPopup;

        public Toggle[] toggles; // 0 : ?°??? , 1 : ?°μΆ? ?€?΅
        public Toggle toggleEffSkip;
        public Toggle toggleRepeatGame;

        public GameObject skipEffButton;

        [SerializeField] new AudioSource audio;


#if UNITY_EDITOR
        //[SerializeField] int ii;
#endif
        private void OnEnable()
        {
            // foreach (var toggle in toggles) toggle.isOn = false;

            // //μΊ ν???? ? κΈ? ? λ³΄λ?? λΆλ¬???? ? ?©
            // if (isUnion)
            // {
            //     for (int s = 0; s < campPopup.togglesUnion.Length; s++)
            //     {
            //         toggles[s].isOn = campPopup.togglesUnion[s].isOn;
            //     }
            // }
            // else
            // {
            //     for (int s = 0; s < campPopup.togglesDNA.Length; s++)
            //     {
            //         toggles[s].isOn = campPopup.togglesDNA[s].isOn;
            //     }
            // }



        }




        void ToggleReset()
        {
            toggleEffSkip.isOn = false;
            toggleRepeatGame.isOn = false;
        }

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void StartLotteryAnimation(int[] unionIndex)
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
            StartCoroutine(ShowUnionSlotCardOpenProcess(unionIndex));
        }

        // DOCKO λ³?κ²? -> ( μΉ΄λ ?€? ? ??λ―Έμ μ’λ£ ???κΈ°λ?? ??¨ )
        public void StartLotteryAnimation()
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
        }

        public IEnumerator ShowUnionSlotCardOpenProcess(int[] u)
        {
            // if (toggles[0].isOn || toggles[1].isOn) isSkip = true;
            // else isSkip = false;

            skipEffButton.SetActive(true);

            isEnd = false;
            isUnion = true;         //? ??¨?Έμ§? ??μ§?

            yield return new WaitForSeconds(0.02f);

            for (int i = 0; i < u.Length; i++)
            {
                int typeIndex = SetImageFromUnionType(data.GetGradeData(u[i]));

                slots[i].SetSlotImage(effectColor[typeIndex], gradeBackImage[typeIndex], data.GetIconData(u[i]));
                slots[i].gameObject.SetActive(true);
                slots[i].SetActiveAction(typeIndex);

                //?¬κΈ? μΆν ?¬?΄? ???¨.
                audio.Play();

                if (toggleEffSkip.isOn) continue;
                yield return new WaitForSeconds(0.03f);
            }

            isEnd = true;
        }

        //?¬λ‘? ?? ??Έ ?κ³?, λ°? ?΄λ―Έμ?? μ§??°κ³? ??΄μ½? ?΄λ―Έμ??λ§? μ²λ¦¬ ?΄??Έ μ»¬λ¬ ?΅?Ό.
        public IEnumerator ShowDNAIconSlotCardOpenProcess(int[] u)
        {
            // if (toggles[0].isOn || toggles[1].isOn) isSkipDNA = true;
            // else isSkipDNA = false;
            skipEffButton.SetActive(false);

            isEnd = false;
            isUnion = false;            //? ??¨?Έμ§? ??μ§?

            foreach (var slot in slots) slot.gameObject.SetActive(false);
            //StartCoroutine(ShowDNAIconSlotCardOpenProcess(u));

            yield return new WaitForSeconds(0.02f);

            for (int i = 0; i < u.Length; i++)
            {
                //DNA ? ?????΄ μ‘΄μ¬ ??¨.
                slots[i].SetSlotImage(dnaIcons[u[i]]);
                slots[i].gameObject.SetActive(true);
                slots[i].SetActiveAction(0);

                //?¬κΈ? μΆν ?¬?΄? ???¨.
                audio.Play();

                if (toggleEffSkip.isOn) continue;
                yield return new WaitForSeconds(0.03f);
            }

            isEnd = true;
        }

        int SetImageFromUnionType(EnumDefinition.UnionGradeType type)
        {
            switch (type)
            {
                case EnumDefinition.UnionGradeType.high: return 1;
                case EnumDefinition.UnionGradeType.rare: return 2;
                case EnumDefinition.UnionGradeType.hero: return 3;
                case EnumDefinition.UnionGradeType.legend: return 4;
                case EnumDefinition.UnionGradeType.unique: return 5;
                default: return 0;
            }
        }

        public void OnClickSkipButton(Toggle togle)
        {
            if (isUnion) isSkip = togle.isOn;
            else isSkipDNA = togle.isOn;
        }

        public void OnClickSkipUnion(bool on)
        {
            isSkip = on;
        }
        public void OnClickSkipDNA(bool on)
        {
            isSkipDNA = on;
        }

        //λ‘ν°λ¦? ? ?λ©μ΄? μ’λ£
        void OnDisable()
        {
            ToggleReset();
            campPopup.ToggleReset();
            foreach (var slot in slots) slot.gameObject.SetActive(false);
        }
    }
}