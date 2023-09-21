using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGraphics
{
    public class LotteryAnimationController : MonoBehaviour
    {
        public SpriteFileData data;
        private Sprite[] dnaIcons;
        public Color[] effectColor;
        public Sprite[] gradeBackImage;
        public Lottery_Slot[] slots;

        private bool isSkip = true;
        private bool isSkipDNA = true;
        public bool isUnion = true;
        public bool isEnd = false;
        public CampPopup campPopup;
        [SerializeField] new AudioSource audio;

        void Awake()
        {
            audio = GetComponent<AudioSource>();

        }

            public void StartLotteryAnimation(int[] unionIndex)
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
            StartCoroutine(ShowUnionSlotCardOpenProcess(unionIndex));
        }

        public void StartLotteryAnimation()
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
        }

        public IEnumerator ShowUnionSlotCardOpenProcess(int[] u)
        {

            isEnd = false;
            isUnion = true;         


            for (int i = 0; i < u.Length; i++)
            {
                int typeIndex = SetImageFromUnionType(data.GetGradeData(u[i]));

                slots[i].SetSlotImage(effectColor[typeIndex], gradeBackImage[typeIndex], data.GetIconData(u[i]));
                slots[i].gameObject.SetActive(true);
                slots[i].SetActiveAction(typeIndex);

                
                if (typeIndex > 3)
                GlobalData.instance.soundManager.PlaySfxUI(EnumDefinition.SFX_TYPE.Unique);

                audio.Play();

                if (campPopup.GetIsOnToggleSkipUnionIsOn()) continue;
                yield return new WaitForSeconds(0.02f);
            }

            isEnd = true;
        }

        public IEnumerator ShowDNAIconSlotCardOpenProcess(int[] u)
        {

            isEnd = false;
            isUnion = false;            

            foreach (var slot in slots) slot.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.02f);

            for (int i = 0; i < u.Length; i++)
            {
                slots[i].SetSlotImage(dnaIcons[u[i]]);
                slots[i].gameObject.SetActive(true);
                slots[i].SetActiveAction(0);


                //????¢¬? ?????? ?????¢¥??? ????????¡§.
                audio.Play();

                if (campPopup.GetIsOnToggleSkipUnionIsOn()) continue;
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


        void OnDisable()
        {
            campPopup.ToggleReset();
            foreach (var slot in slots) slot.gameObject.SetActive(false);
        }
    }
}