using UnityEngine;
using UnityEngine.Events;

public class EventRunner : MonoBehaviour
{

    public UnityEvent enableEvent;
    public UnityEvent disableEvent;

    void OnEnable()
    {
        EnableEvent();
    }

    void OnDisable()
    {
        DisableEvent();
    }

    public void EnableEvent()
    {
        enableEvent?.Invoke();
    }
    public void DisableEvent()
    {
        disableEvent?.Invoke();
    }


}
