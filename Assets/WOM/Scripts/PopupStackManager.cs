using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupStackManager : MonoBehaviour
{
    public static PopupStackManager instance;

    private Stack<PopupBase> popupStack;


    private void Awake()
    {
        instance = this;
    }

    public void PushPopUp(PopupBase popup)
    {
        popupStack.Push(popup);
    }


    IEnumerator PopUpStackControl()
    {
        while (true)
        {
            if (popupStack.Count > 0)
            {
                PopupBase popup = popupStack.Pop();

                if (popup != null)
                {
                    popup.ShowPopUp();
                    yield return new WaitUntil(() => popup.IsRewarded);
                }
            }

            yield return null;
        }
    }

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {

            GlobalData.instance.globalPopupController.EnableMessageTwoBtnPopup(18, QuitPopupApply, QuitPopupCancel);

        }
    }


    void QuitPopupCancel()
    {
        // ÆË¾÷ ´Ý±â
        Debug.Log("°ÔÀÓÁ¾·á ÆË¾÷ ´Ý±â");
    }

    void QuitPopupApply()
    {
        StartCoroutine(GlobalData.instance.saveDataManager.SaveDataToFileCoroutine());
    }
}
