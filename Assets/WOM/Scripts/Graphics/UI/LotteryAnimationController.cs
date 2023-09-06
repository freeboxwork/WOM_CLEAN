using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGraphics
{
    public class LotteryAnimationController : MonoBehaviour
    {
        public SpriteFileData data;
        [Header("DNA Icon ?´ë¯¸ì??"), SerializeField]
        private Sprite[] dnaIcons;
        [Header("?œ ?‹ˆ?˜¨ë¡œí„°ë¦? ?´ë¯¸ì??")]
        public Color[] effectColor;
        public Sprite[] gradeBackImage;
        public Lottery_Slot[] slots;

        [Header("?Š¤?‚µ??? ?‘ì¢…ë¥˜ë¡? ë¶„ë¥˜ ?•˜ê³? ?”„ë¡œì„¸?Š¤ ??‚˜?„ ?Š¤?‚µ ë³??ˆ˜ ?œ ì§?")]
        private bool isSkip = true;
        private bool isSkipDNA = true;
        public bool isUnion = true;
        public bool isEnd = false;

        [Header("ìº í”„?Œ?—…")]
        public CampPopup campPopup;

        public Toggle[] toggles; // 0 : ?—°?†?†Œ?™˜ , 1 : ?—°ì¶? ?Š¤?‚µ
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

            // //ìº í”„?Œ?—…?—?„œ ?† ê¸? ? •ë³´ë?? ë¶ˆëŸ¬????„œ ? ?š©
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

        // DOCKO ë³?ê²? -> ( ì¹´ë“œ ?˜¤?”ˆ ?• ?‹ˆ?—ë¯¸ì…˜ ì¢…ë£Œ ???ê¸°ë?? ?œ„?•¨ )
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
            isUnion = true;         //?œ ?‹ˆ?˜¨?¸ì§? ?•„?‹Œì§?

            yield return new WaitForSeconds(0.02f);

            for (int i = 0; i < u.Length; i++)
            {
                int typeIndex = SetImageFromUnionType(data.GetGradeData(u[i]));

                slots[i].SetSlotImage(effectColor[typeIndex], gradeBackImage[typeIndex], data.GetIconData(u[i]));
                slots[i].gameObject.SetActive(true);
                slots[i].SetActiveAction(typeIndex);

                //?—¬ê¸? ì¶œí˜„ ?‚¬?š´?“œ ?•„?š”?•¨.
                audio.Play();

                if (toggleEffSkip.isOn) continue;
                yield return new WaitForSeconds(0.03f);
            }

            isEnd = true;
        }

        //?Š¬ë¡? ?˜•?ƒœ ?™•?¸ ?•˜ê³?, ë°? ?´ë¯¸ì?? ì§??š°ê³? ?•„?´ì½? ?´ë¯¸ì??ë§? ì²˜ë¦¬ ?´?™?Š¸ ì»¬ëŸ¬ ?†µ?¼.
        public IEnumerator ShowDNAIconSlotCardOpenProcess(int[] u)
        {
            // if (toggles[0].isOn || toggles[1].isOn) isSkipDNA = true;
            // else isSkipDNA = false;
            skipEffButton.SetActive(false);

            isEnd = false;
            isUnion = false;            //?œ ?‹ˆ?˜¨?¸ì§? ?•„?‹Œì§?

            foreach (var slot in slots) slot.gameObject.SetActive(false);
            //StartCoroutine(ShowDNAIconSlotCardOpenProcess(u));

            yield return new WaitForSeconds(0.02f);

            for (int i = 0; i < u.Length; i++)
            {
                //DNA ?Š” ????…?´ ì¡´ì¬ ?•ˆ?•¨.
                slots[i].SetSlotImage(dnaIcons[u[i]]);
                slots[i].gameObject.SetActive(true);
                slots[i].SetActiveAction(0);

                //?—¬ê¸? ì¶œí˜„ ?‚¬?š´?“œ ?•„?š”?•¨.
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

        //ë¡œí„°ë¦? ?• ?‹ˆë©”ì´?…˜ ì¢…ë£Œ
        void OnDisable()
        {
            ToggleReset();
            campPopup.ToggleReset();
            foreach (var slot in slots) slot.gameObject.SetActive(false);
        }
    }
}