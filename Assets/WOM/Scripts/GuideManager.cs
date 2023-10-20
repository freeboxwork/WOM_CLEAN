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
            Debug.LogError($"{id}�� �ش��ϴ� ���̵� ��ư�� �����ϴ�.");
        }
        return tutorialButtons.FirstOrDefault(f => f.id == id);
    }



}
