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
        [SerializeField] Image dot;
        [SerializeField] Image ring;
        [SerializeField] Image grade;
        [SerializeField] Image insect;
        [SerializeField] Animator anim;

        static readonly int[] TriggerHash =
            { Animator.StringToHash("Normal"),
            Animator.StringToHash("High"),
            Animator.StringToHash("Rare"),
            Animator.StringToHash("Hero"),
            Animator.StringToHash("Legend"),
            Animator.StringToHash("Unique") };

        //Union
        public void SetSlotImage(Color glow, Sprite grade, Sprite insect)
        {
            this.glow.color = glow;
            this.shine.color = glow;
            this.star.color = glow;
            this.dot.color = glow;
            this.ring.color = glow;
            //this.star.color = new Color(glow.r, glow.g, glow.b, 1.0f);
            this.grade.sprite = grade;
            this.insect.enabled = true;
            this.insect.sprite = insect;
        }

        //DNA (Not used glow Color)
        public void SetSlotImage(Sprite grade)
        {
            //this.glow.color = glow;
            //this.shine.color = glow;
            this.grade.sprite = grade;
            this.insect.enabled = false;
            this.insect.sprite = null;
        }

        public void SetActiveAction(int gradeNumbers)
        {
            //그레이드에맞는 애니메이션을 따로 만들거나, 
            //혹은 표현볍을 달리하거나.
            anim.SetTrigger(TriggerHash[gradeNumbers]);
        }

        private void OnDisable()
        {
            // �ִϸ����� ���� ���� �ؼ� ����Ǿ����� ���� ���� ����.
            //anim.keepAnimatorStateOnDisable = false;
        }
    }
}