using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        // 게임 오브젝트에 있는 Animator 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Animator 컴포넌트에서 현재 재생 중인 애니메이션 상태를 가져옵니다.
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션의 재생이 끝났는지 확인합니다.
        if (currentState.normalizedTime >= 1.0f)
        {
            // 애니메이션이 종료되면 게임 오브젝트를 비활성화합니다.
            gameObject.SetActive(false);
        }
    }
}
