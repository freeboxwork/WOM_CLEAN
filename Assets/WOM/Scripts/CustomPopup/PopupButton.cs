using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonString = null;
    [SerializeField] private TextMeshProUGUI closeCountText = null;

    private List<Action> callbackEvent = null;

    private GameObject target = null;

    [SerializeField] Button btn;

    bool isReward;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void Init(string _text, List<Action> _callback, GameObject _target)
    {

        this.buttonString.text = _text;
        this.callbackEvent = _callback;
        this.target = _target;
        btn.onClick.AddListener(OnButton);
    }

    void OnEnable()
    {
        isReward = false;
        StopCoroutine("AutoClose");
        StartCoroutine("AutoClose");
    }

    IEnumerator AutoClose()
    {
        float time = 5f;

        while (time > 0)
        {
            time -= Time.deltaTime;
            var t = Math.Floor(time);
            if (t < 0)
            {
                t = 0;
            }
            closeCountText.text = string.Format("{0}s", t);
            yield return null;
        }

        OnButton();
    }

    public void OnButton()
    {
        if(isReward == true)
        {
            return;
        }

        isReward = true;
        
        StopCoroutine("AutoClose");

        foreach (var action in this.callbackEvent)
        {
            action();
        }
    }
}
public class PopupButtonInfo
{
    public string text = null;
    public List<Action> callback = null;

    public PopupButtonInfo(string _text, List<Action> _callback)
    {
        this.text = _text;
        if (_callback != null)
        {
            this.callback = _callback;
        }
    }
}