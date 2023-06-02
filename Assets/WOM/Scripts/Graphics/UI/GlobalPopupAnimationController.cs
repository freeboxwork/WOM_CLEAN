using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGraphics
{
    public class GlobalPopupAnimationController : MonoBehaviour
    {
        [Header("등장 속도")]
        public float openDuration = 0.5f;
        [Header("올라가는 속도")]
        public float climbDuration = 2f;
        [Header("사라지는 속도")]
        public float fadeOutDuration = 1f;
        public Vector3 climeTargetPosition;
        public Vector3 startPosition;

        public MessageSlot[] slots;
        [SerializeField] int currentIndex;

        bool allSlotHide = true; //예외처리용

        void Start()
        {
            currentIndex = 0;
        }

        public void OpenThePopup(string title, string message)
        {
            //내 자신이 안켜져 있을땐 실행 안됨.
            if (gameObject.activeSelf == false) return;

            //처음열었을때 0번 인덱스 호출하고 인덱스 입력
            //현재 인덱스가 열려 있을때 다음 인덱스 호출.
            //만약 현재 인덱스가 마지막 인덱스 일때 다시 0번인덱스 호출
            //현재 인덱스는 올라가는 애니메이션 진행.

            if (slots[currentIndex].gameObject.activeSelf == true) currentIndex++;

            slots[currentIndex].gameObject.SetActive(true);
            slots[currentIndex].OpenTheMessageAnimation(title, message);

            int previousIndex = 0;

            if (currentIndex == 0)
            {
                if (slots[slots.Length - 1].gameObject.activeSelf == true)
                {
                    slots[slots.Length - 1].ClimbTheMessageAnimation();
                }
                return;
            }
            else if (currentIndex == slots.Length - 1)
            {
                
                previousIndex = currentIndex - 1;
                currentIndex = 0;
            }
            else
            {
                previousIndex = currentIndex - 1;
            }

            slots[previousIndex].ClimbTheMessageAnimation();
        }

        private void OnDisable()
        {
            currentIndex = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}
