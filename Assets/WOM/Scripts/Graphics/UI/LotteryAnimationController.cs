using System.Collections;
using UnityEngine;

namespace ProjectGraphics
{
    public class LotteryAnimationController : MonoBehaviour
    {
        public SpriteFileData data;
        [Header("DNA Icon 이미지"), SerializeField]
        private Sprite[] dnaIcons;

        public Color[] effectColor;
        public Sprite[] gradeBackImage;
        public Lottery_Slot[] slots;

        private bool isSkip = false;

#if UNITY_EDITOR
        //[SerializeField] int ii;
#endif

        private void Awake()
        {
            // slots = GetComponentsInChildren<Lottery_Slot>();
            // foreach(var slot in slots) slot.gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        void Start()
        {
            /*
            int[] uIndex = new int[ii];
            for (int i = 0; i < uIndex.Length; i++)
            {
                uIndex[i] = Random.Range(0, data.GetDataSize());
            }
            StartCoroutine(ShowUnionSlotCardOpenProcess(uIndex));
            */
        }
#endif

        public void StartLotteryAnimation(int[] unionIndex)
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
            StartCoroutine(ShowUnionSlotCardOpenProcess(unionIndex));
        }

        // DOCKO 변경 -> ( 카드 오픈 애니에미션 종료 대기를 위함 )
        public void StartLotteryAnimation()
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
        }


        public IEnumerator ShowUnionSlotCardOpenProcess(int[] u)
        {
            yield return new WaitForSeconds(0.05f);

            for (int i = 0; i < u.Length; i++)
            {
                int typeIndex = SetImageFromUnionType(data.GetGradeData(u[i]));

                if (isSkip == false)
                {
                    if (typeIndex == 2)
                    {
                        yield return new WaitForSeconds(0.7F);
                    }
                }

                slots[i].SetSlotImage(effectColor[typeIndex], gradeBackImage[typeIndex], data.GetIconData(u[i]));
                slots[i].gameObject.SetActive(true);

                //여기 출현 사운드 필요함.

                if (isSkip == false)
                {
                    for (int j = 0; j < i; j++)
                    {
                        slots[j].SetShakeAction();
                    }
                    yield return new WaitForSeconds(0.05f);
                    //if (typeIndex == 2)
                    //{
                    //    yield return new WaitForSeconds(1F);
                    //}
                    //else
                    //{
                    //    yield return new WaitForSeconds(0.1f);
                    //}
                }
            }

            isSkip = false;
        }

        public void StartDNASlotAnimation(int[] u)
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
            StartCoroutine(ShowDNAIconSlotCardOpenProcess(u));
        }

        //슬롯 형태 확인 하고, 백 이미지 지우고 아이콘 이미지만 처리 이펙트 컬러 통일.
        IEnumerator ShowDNAIconSlotCardOpenProcess(int[] u)
        {
            yield return new WaitForSeconds(0.05f);

            for (int i = 0; i < u.Length; i++)
            {
                //타입 없음
                //GradeBackImage = DNA Icon
                int typeIndex = 0;

                slots[i].SetSlotImage(effectColor[typeIndex], dnaIcons[u[i]]);
                slots[i].gameObject.SetActive(true);

                //여기 출현 사운드 필요함.

                for (int j = 0; j < i; j++)
                {
                    slots[j].SetShakeAction();
                }

                if (isSkip) continue;
                yield return new WaitForSeconds(0.05f);
            }
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

        public void OnClickSkipButton()
        {
            isSkip = true;
        }

        private void OnDisable()
        {
            foreach (var slot in slots) slot.gameObject.SetActive(false);
        }
    }
}