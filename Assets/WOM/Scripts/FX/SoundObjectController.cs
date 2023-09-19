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
        audioSource.Play(); // AudioSource를 재생합니다.
        Invoke("DeactivateGameObject", audioSource.clip.length);
    }

    private void DeactivateGameObject()
    {
        // 재생이 완료되면 호출되는 이벤트 핸들러
        gameObject.SetActive(false); // 현재 게임 오브젝트를 비활성화합니다.
    }
}
