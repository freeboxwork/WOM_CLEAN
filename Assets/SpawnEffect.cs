using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        // ���� ������Ʈ�� �ִ� Animator ������Ʈ�� �����ɴϴ�.
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Animator ������Ʈ���� ���� ��� ���� �ִϸ��̼� ���¸� �����ɴϴ�.
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // �ִϸ��̼��� ����� �������� Ȯ���մϴ�.
        if (currentState.normalizedTime >= 1.0f)
        {
            // �ִϸ��̼��� ����Ǹ� ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            gameObject.SetActive(false);
        }
    }
}
