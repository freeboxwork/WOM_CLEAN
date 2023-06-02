using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGraphics
{
    public class FollowShadow : MonoBehaviour
    {
        private Transform parentTr;
        private Vector3 distPos;

        [Header("그림자 거리"), Range(0.1f, 1.0f)]
        public float distance = 0.5f;

        private void Start()
        {
            parentTr = transform.parent;
        }

        private void LateUpdate()
        {
            distPos = new Vector3(parentTr.position.x, parentTr.position.y - distance);
            transform.position = distPos;
        }
    }
}