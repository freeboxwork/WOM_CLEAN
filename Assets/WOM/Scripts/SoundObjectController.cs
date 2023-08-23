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
        StartCoroutine(DeactivateAfterSoundEnds());
    }

    private IEnumerator DeactivateAfterSoundEnds()
    {
        // AudioClip�� ���̸� ��ٷȴٰ� ���� ������Ʈ�� ��Ȱ��ȭ
        yield return new WaitForSeconds(audioSource.clip.length);

        gameObject.SetActive(false);
    }
}
