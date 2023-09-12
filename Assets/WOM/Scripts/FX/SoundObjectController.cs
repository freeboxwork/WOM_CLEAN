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
        // AudioClip의 길이를 기다렸다가 게임 오브젝트를 비활성화
        yield return new WaitForSeconds(audioSource.clip.length);

        gameObject.SetActive(false);
    }
}
