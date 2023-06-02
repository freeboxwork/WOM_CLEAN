using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using TMPro;

namespace ProjectGraphics
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField]Transform textTransform;
        MeshRenderer rend;
        TextMeshPro textMesh;

        [Header("애니메이션 길이")]
        public float duration = 1.0f;
        [Header("목표 Y 위치")]
        public float targetPosY = 0.5f;

        //임시
        //public bool isCritical = false;

        //노말 텍스트 컬러, 크리티컬 텍스트 컬러,
        //크리티컬 글자!
        public Color nomalColor;
        public Color criticalColor;

        private void Awake()
        {
            rend = GetComponentInChildren<MeshRenderer>();
            textMesh = GetComponentInChildren<TextMeshPro>();

            //폰트 형태 변경
            //textMesh.fontStyle = FontStyles.Normal;
        }

        private void OnEnable()
        {
            /*
            if (isCritical)
            {
                textMesh.fontSize = 4;
                textMesh.text = "Critical!\n" + "0000";
                textMesh.color = criticalColor;
                textMesh.fontStyle = FontStyles.Italic | FontStyles.Bold | FontStyles.UpperCase;
            }
            else
            {
                textMesh.fontSize = 3;
                textMesh.text = "0000";
                textMesh.color = nomalColor;
                textMesh.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
            }
            */
            StartAnimation();
        }

        public void SetText(string text, bool crit)
        {
            if (crit)
            {
                textMesh.fontSize = 4;
                textMesh.text = "Critical!\n" + text;
                textMesh.color = criticalColor;
                textMesh.fontStyle = FontStyles.Italic | FontStyles.Bold | FontStyles.UpperCase;
            }
            else
            {
                textMesh.fontSize = 3;
                textMesh.text = text;
                textMesh.color = nomalColor;
                textMesh.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
            }
        }

        private void StartAnimation()
        {
            //위로 올라가는 애니메이션.
            //끝나면 위치 초기화
            textTransform.DOLocalMoveY(targetPosY, duration).SetEase(Ease.OutCirc).OnComplete(() => this.gameObject.SetActive(false));
            
        }

        private void OnDisable()
        {
            textTransform.localPosition = Vector3.zero;
            textMesh.color = nomalColor;
        }
    }
}
