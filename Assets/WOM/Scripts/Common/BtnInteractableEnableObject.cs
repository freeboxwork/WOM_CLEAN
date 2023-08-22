/// <summary>
/// 특정 버튼의 Interactable 값에 따라 게임 오브젝트 ACTIVE/INACTIVE
/// </summary>
using UnityEngine;
using UnityEngine.UI;


public class BtnInteractableEnableObject : MonoBehaviour
{
    public Button btn;
    public GameObject obj;
    void Update()
    {
        if (btn != null && obj != null)
            obj.SetActive(btn.interactable);
    }
}
