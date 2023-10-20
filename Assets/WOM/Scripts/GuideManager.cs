using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GuideManager : MonoBehaviour
{
    
    public List<GuideButton> tutorialButtons = new List<GuideButton>();



    public GuideButton GetGuideButtonById(int id)
    {
        var btn = tutorialButtons.FirstOrDefault(f => f.id == id);
        if (btn == null)
        {
            Debug.LogError($"{id}에 해당하는 가이드 버튼이 없습니다.");
        }
        return tutorialButtons.FirstOrDefault(f => f.id == id);
    }



}
