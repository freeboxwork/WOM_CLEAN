using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using ProjectGraphics;

public class GlobalPopupController : MonoBehaviour
{

    // GLOBAL MESSAGE POPUP
    public GameObject popupSet;
    public TextMeshProUGUI txtTtile;
    public TextMeshProUGUI txtMessage;
    public Button btnConfirm;

    // ANIMATION GLOBLA POPUP
    public GlobalPopupAnimationController animationController;

    // GLOBAL MESSAGE TWO BUTTON POPUP
    public GlobalMessageTwoBtnPopup twoBtnPopup;

    void Start()
    {
        SetButtonEvent();
    }


    
    

    public void EnableMessageTwoBtnPopup(int messageId, UnityAction apply , UnityAction cancel)
    {
        var messageData = GetMessageById(messageId);
        twoBtnPopup.gameObject.SetActive(true);
        twoBtnPopup.SetTxtMessage(messageData.message_kor);
        twoBtnPopup.SetBtnApplyEvent(apply);
        twoBtnPopup.SetBtnCancelEvent(cancel);
    }

    void SetButtonEvent()
    {
        btnConfirm.onClick.AddListener(() => {
            popupSet.SetActive(false);
        });
    }

    public void EnableGlobalPopupByMessageId(string title = "",int messageId=0)
    {
        var messageData = GetMessageById(messageId);
     
        animationController.gameObject.SetActive(true);
        animationController.OpenThePopup(messageData.title, messageData.message_kor);   
    }


    public void EnableGlobalPopup(string title, string message)
    {
        SetTxtTitle(title);
        SetTxtMessage(message);
        popupSet.SetActive(true);
    }


    public void EnableGlobalPopupEventByMesageID(string title, int messageId, UnityAction evn)
    {
        btnConfirm.onClick.RemoveAllListeners();
        btnConfirm.onClick.AddListener(()=> {
            evn.Invoke();
            ResetBtnEvent();
        });

          var messageData = GetMessageById(messageId);

        SetTxtTitle(messageData.title);
        SetTxtMessage(messageData.message_kor);
        popupSet.SetActive(true);

    }

    public void EnableGlobalPopupOutValueByMesageID(string title, int messageId, out bool value)
    {
        bool confirmValue = false;
        btnConfirm.onClick.RemoveAllListeners();
        btnConfirm.onClick.AddListener(() =>
        {
            confirmValue = true;
            ResetBtnEvent();
        });

        var messageData = GetMessageById(messageId);

        SetTxtTitle(messageData.title);
        SetTxtMessage(messageData.message_kor);
        popupSet.SetActive(true);


        value = confirmValue;
    }

    public IEnumerator EnableGlobalPopupCor(string title, int messageId)
    {
        bool confirmValue = false;
        btnConfirm.onClick.RemoveAllListeners();
        btnConfirm.onClick.AddListener(() => {
            confirmValue = true;
            ResetBtnEvent();
        });

    var messageData = GetMessageById(messageId);

        SetTxtTitle(messageData.title);
        SetTxtMessage(messageData.message_kor);
        popupSet.SetActive(true);

        yield return new WaitUntil(() => confirmValue);
    }

    // remove event
    void ResetBtnEvent()
    {
        btnConfirm.onClick.RemoveAllListeners();
        btnConfirm.onClick.AddListener(() => {
            popupSet.SetActive(false);
        });

        popupSet.SetActive(false);
    }

    GlobalMessageData GetMessageById(int id)
    {
        //TODO: 영문 국문 시세템 언어에 따라 적용
        return GlobalData.instance.dataManager.GetGlobalMessageDataById(id);
    }


    void SetTxtTitle(string value)
    {
        txtTtile.text = value;
    }
    void SetTxtMessage(string value)
    {
        txtMessage.text = value;
    }




}
