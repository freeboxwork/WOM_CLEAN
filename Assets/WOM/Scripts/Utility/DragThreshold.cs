using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragThreshold : MonoBehaviour
{
    private const float inchToCm = 2.54f;
    private EventSystem eventSystem = null;

    //��ġ�� �������� ���� 0.1��Ƽ���� ������ �з��� ���� ���¸� �����ϰڴٴ� �ǹ�
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

