using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

namespace ProjectGraphics
{
    public class Lottery_Slot : MonoBehaviour
    {
        [SerializeField] Image glow;
        [SerializeField] Image shine;
        [SerializeField] Image star;
        [SerializeField] Image grade;
        [SerializeField] Image insect;
        [SerializeField] Animator anim;

        public void SetSlotImage(Color glow, Sprite grade, Sprite insect)
        {
            this.glow.color = this.shine.color = glow;
            //this.star.color = new Color(glow.r, glow.g, glow.b, 1.0f);
            this.grade.sprite = grade;
            this.insect.enabled = true;
            this.insect.sprite = insect;
        }

        public void SetSlotImage(Color glow, Sprite grade)
        {
            this.glow.color = this.shine.color = glow;
            this.grade.sprite = grade;
            this.insect.enabled = false;
            this.insect.sprite = null;
        }

        public void SetShakeAction()
        {
            anim.SetTrigger("Shake");
        }

        private void OnDisable()
        {
            // �ִϸ����� ���� ���� �ؼ� ����Ǿ����� ���� ���� ����.
            anim.keepAnimatorStateOnDisable = false;
        }
    }
}