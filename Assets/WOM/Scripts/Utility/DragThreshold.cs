using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragThreshold : MonoBehaviour
{
    private const float inchToCm = 2.54f;
    private EventSystem eventSystem = null;

    //터치한 지점으로 부터 0.1센티미터 정도는 밀려도 눌린 상태를 유지하겠다는 의미
    private readonly float dragThresholdCM = 0.02f;

    void Start()
    {
        if (eventSystem == null)
        {
            eventSystem = GetComponent<EventSystem>();
        }

        SetDragThreshold();
    }

    private void SetDragThreshold()
    {
        if (eventSystem != null)
        {
            eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
        }
    }
}

