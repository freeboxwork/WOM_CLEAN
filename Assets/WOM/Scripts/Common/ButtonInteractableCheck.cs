using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 재화에 따른 버튼 Interactable 체크
/// </summary>
public class ButtonInteractableCheck : MonoBehaviour
{

    public EnumDefinition.RewardType type;

    public int enableCount;
    public Button btn;


    void Start()
    {
        if (TryGetComponent(out Button button))
        {
            btn = button;
        }
    }

    void OnEnable()
    {

    }


    void InteractableCheck()
    {

    }



}
