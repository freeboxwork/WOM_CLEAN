using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObjectController : MonoBehaviour
{
private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        audioSource.Play(); // AudioSource�� ����մϴ�.
        Invoke("DeactivateGameObject", audioSource.clip.length);
    }

    private void DeactivateGameObject()
    {
        // ����� �Ϸ�Ǹ� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
        gameObject.SetActive(false); // ���� ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
    }
}
