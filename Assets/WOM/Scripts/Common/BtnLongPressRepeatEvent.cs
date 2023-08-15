using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;

public class BtnLongPressRepeatEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float buttonPressStartTime;
    private float longPressTime = 0.1f; // 길게 누르는 시간을 1초로 설정
    private bool isLongPressing;

    public UnityEvent longPressingEvent;



    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressStartTime = Time.time;
        isLongPressing = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressStartTime = 0; // 초기화
        isLongPressing = false; // 초기화
    }

    void Update()
    {
        if (isLongPressing) return;

        if (Time.time - buttonPressStartTime > longPressTime && buttonPressStartTime > 0)
        {
            //Debug.Log(" 버튼을 길게 눌렀습니다.");
            isLongPressing = true;
            StartCoroutine(LongPress());
        }
    }

    IEnumerator LongPress()
    {
        while (isLongPressing)
        {
            longPressingEvent.Invoke();
            Debug.Log("LongPressing 연속 이벤트 실행!");
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

}
