using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
public class GuideManager : MonoBehaviour
{
    
    public List<GuideButton> guideButtons = new List<GuideButton>();
    int nunber = 0;

    public IEnumerator Init()
    {

        for(int i = 0; i < guideButtons.Count; i++)
        {
            var unLockData = GlobalData.instance.dataManager.GetUnLockContentDataByType(i);
            guideButtons[i].ShowGuideIndexText(unLockData.guideMessage);

        }

        yield return null;
    }



    public void ClearGuide(int index)
    {
       GetGuideButtonById(index).ClearGuide();

    }

    public GuideButton GetGuideButtonById(int id)
    {
        var btn = guideButtons.FirstOrDefault(f => f.id == id);
        if (btn == null)
        {
            Debug.LogError($"{id}에 해당하는 가이드 버튼이 없습니다.");
        }
        return guideButtons.FirstOrDefault(f => f.id == id);
    }








}
