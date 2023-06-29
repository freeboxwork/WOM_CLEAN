using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupBuilder
{
    private GameObject poolObj;
    private Transform target = null;

    // Build???? ?????? ?????? ?????? ???? ????????
    private string title = null;

    private PopupButtonInfo buttonInfo = null;

    public List<RewardInfoData> rewards;

    // ????????? ??????? ????????? ????????.
    public PopupBuilder(Transform _target)
    {
        this.target = _target;
        rewards = new List<RewardInfoData>();
    }

    public void Build(PoolManager poolManager)
    {
        // ?????????? ?????????? ?????? ????????
        // MonoBehaviour?? ????? ???? Instantiate?? ?????,??????????? ???? GameObject?? static?????? ???
        poolObj = poolManager.GetPool(PoolUIType.Popup);
        //GameObject popupObject = GameObject.Instantiate(Resources.Load("Popup/" + "CustomPopup", typeof(GameObject))) as GameObject;
        //popupObject.transform.SetParent(this.target, false);
        CustomPopup customPopup = poolObj.GetComponent<CustomPopup>();


        // ???????
        customPopup.SetTitle(this.title);
        customPopup.SetRewardInfo(this.rewards);
        this.buttonInfo.callback.Add(() => poolManager.ReturnPool(poolObj, PoolUIType.Popup));
        customPopup.SetButtons(this.buttonInfo);

        customPopup.Init();
    }



    public void SetTitle(string _title)
    {
        // ???????? ????
        this.title = _title;
    }

    public void SetButton(string _text, List<Action> _callback = null)
    {
        // ??????? ????
        buttonInfo = new PopupButtonInfo(_text, _callback);
    }

    public void SetRewardInfo(EnumDefinition.RewardType type, float amount, Sprite sp)
    {
        rewards.Add(new RewardInfoData(type, amount, sp));
    }



}
