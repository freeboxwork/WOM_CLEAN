using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] PopupButton popupButton = null;
    [SerializeField] Transform parent;
    [SerializeField] GameObject[] slotRewad;

    Queue<GameObject> slotQueue = new Queue<GameObject>();
    WaitForSeconds delay = new WaitForSeconds(0.3f);

    void OnDisable()
    {
        for (int i = 0; i < slotRewad.Length; i++)
        {
            slotRewad[i].SetActive(false);
        }
    }

    public void Init()
    {
        StartCoroutine("Show");
    }

    IEnumerator Show()
    {
        // sfx 팝업 보여질때 효과음
        GlobalData.instance.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.Reward);

        popupButton.gameObject.SetActive(false);

        yield return delay;

        while (slotQueue.Count > 0)
        {
            // sfx 아이템 하나씩 나올때 효과음
            GlobalData.instance.soundManager.PlaySfxInGame(EnumDefinition.SFX_TYPE.CustomPopup_Item);

            slotQueue.Dequeue().SetActive(true);

            //Insert Audio Effect Play Code 

            yield return delay;
        }

        slotQueue.Clear();

        popupButton.gameObject.SetActive(true);
    }

    public void SetTitle(string _title)
    {
        this.titleText.text = _title;
    }

    public void SetButtons(PopupButtonInfo info)
    {
        popupButton.Init(info.text, info.callback, this.gameObject);
    }
    public void SetRewardInfo(List<RewardInfoData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject rif = slotRewad[i];
            slotQueue.Enqueue(rif);
            PopupRewardIcon popupRewardIcon = rif.GetComponent<PopupRewardIcon>();
            popupRewardIcon.SetUI(list[i]);
        }
    }


}
