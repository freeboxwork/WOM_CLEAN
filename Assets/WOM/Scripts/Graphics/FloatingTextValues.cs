using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace ProjectGraphics
{
    public class FloatingTextValues : MonoBehaviour
    {
        public Gradient boneColor;
        public Gradient goldColor;
        public Gradient jewelColor;
        public TextMeshProUGUI textMesh;

        public enum ValueType
        {
            Bone, Gold, jewel
        }
        ValueType valueType = ValueType.Bone;

        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();

        public void SetText(string text, ValueType type)
        {
            textMesh.text = text;
            valueType = type;
            StartCoroutine(TextActionProcess());
        }

        IEnumerator TextActionProcess()
        {
            yield return null;

            float t = 0;

            while (t <= 1)
            {
                t += Time.deltaTime;

                switch (valueType)
                {
                    case ValueType.Bone:
                        textMesh.color = boneColor.Evaluate(t);
                        break;
                    case ValueType.Gold:
                        textMesh.color = goldColor.Evaluate(t);
                        break;
                    case ValueType.jewel:
                        textMesh.color = jewelColor.Evaluate(t);
                        break;
                }
                yield return waitFrame;
            }

            gameObject.SetActive(false);
        }
    }
}