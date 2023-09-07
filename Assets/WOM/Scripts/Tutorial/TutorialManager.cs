using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    public TextAsset tutorialJsonData;

    // ?à¨?Ü†Î¶¨Ïñº ?ç∞?ù¥?Ñ∞
    public TutorialStepDatas tutorialStepData;

    // setId Í∏∞Ï???úºÎ°? ?Çò?àà ?à¨?Ü†Î¶¨Ïñº ?Ñ∏?ä∏ ?ç∞?ù¥?Ñ∞
    public List<TutorialStepSetData> tutorialStepSetDatas = new List<TutorialStepSetData>();

    // ?à¨?Ü†Î¶¨Ïñº Î≤ÑÌäº?ì§
    public List<TutorialButton> tutorialButtons = new List<TutorialButton>();

    // ?òÑ?û¨ ÏßÑÌñâÏ§ëÏù∏ ?à¨?Ü†Î¶¨Ïñº ?Ñ∏?ä∏?ùò ?ïÑ?ù¥?îî
    public int curTutorialSetID = 0;
    // ?òÑ?û¨ ÏßÑÌñâÏ§ëÏù∏ ?à¨?Ü†Î¶¨Ïñº ?Ñ∏?ä∏?ùò ?ä§?Öù ?ïÑ?ù¥?îî
    public int curTutorialStepID = 0;

    public TutorialUiController tutorialUiCont;

    public List<PatternBase> patterns = new List<PatternBase>();

    public bool isTutorial = false;

    public bool isUnionGamblingTutorial = false;

    // ?ã†Í∑? ?ú†??? ?Ñ†Î¨? ?ã´Í∏? Î≤ÑÌäº
    public List<Button> tutoStartBtns = new List<Button>();

    public bool isAdPass = false;

    //?à¨?Ü†Î¶¨Ïñº Í≤åÏûÑ ?ò§Î∏åÏ†ù?ä∏
    public List<TutorialGameObject> tutorialGameObjects = new List<TutorialGameObject>();

    public GameObject tutorialPlayingTextBox;

    public GameObject newUserEventPopupObj;

    void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        foreach (var btn in tutoStartBtns)
        {
            btn.onClick.AddListener(() =>
            {
                TutorialStart();
            });
        }
    }

    public void TutorialStart()
    {
        // max check
        if (IsTutorialAllComplete() == false)
        {
            isTutorial = true;
            EnableTutorialSet();
            tutorialPlayingTextBox.gameObject.SetActive(true);
        }
        else
        {
            isTutorial = false;
            tutorialPlayingTextBox.gameObject.SetActive(false);
            Debug.Log("Î™®Îì† ?à¨?Ü†Î¶¨Ïñº ?ôÑÎ£?");
        }
    }

    public IEnumerator Init()
    {
        // get tutorial set id ( load data )
        curTutorialSetID = GlobalData.instance.saveDataManager.saveDataTotal.saveDataTutorial.tutorialSetId;

        // get json data
        tutorialStepData = JsonUtility.FromJson<TutorialStepDatas>(tutorialJsonData.text);

        yield return new WaitForEndOfFrame();

        SetData();

        //isTutorial = false;

        // max check
        if (IsTutorialAllComplete() == false)
        {
            isTutorial = true;
        }
        else
        {
            isTutorial = false;
        }

        yield return null;
    }

    void SetData()
    {
        // set set data ( setId Í∏∞Ï???úºÎ°? ?Ñ∏?ä∏ Î¶¨Ïä§?ä∏Î•? ÎßåÎì¶ )
        for (int i = 0; i < tutorialStepData.data.Count; i++)
        {
            var data = tutorialStepData.data[i];
            if (!tutorialStepSetDatas.Any(a => a.setId == data.setId))
            {
                tutorialStepSetDatas.Add(new TutorialStepSetData
                {
                    setId = data.setId,
                    steps = new List<TutorialStep> { data }
                });
            }
            else
            {
                tutorialStepSetDatas.First(s => s.setId == data.setId).steps.Add(data);
            }


        }

        foreach (var set in tutorialStepSetDatas)
        {
            if (set.setId < curTutorialSetID)
            {
                set.isSetComplete = true;
            }
        }

    }


    public TutorialStepSetData GetTutorialSetById(int setID)
    {
        var set = tutorialStepSetDatas.FirstOrDefault(f => f.setId == setID);
        if (set == null)
        {
            Debug.LogError($"{setID}?óê ?ï¥?ãπ?ïò?äî ?à¨?Ü†Î¶¨Ïñº ?Ñ∏?ä∏Í∞? ?óÜ?äµ?ãà?ã§.");
        }
        return tutorialStepSetDatas.FirstOrDefault(f => f.setId == setID);
    }



    public void EnableTutorialSet()
    {
        var tutorialSet = GetTutorialSetById(curTutorialSetID);

        if (tutorialSet == null)
        {
            Debug.LogError($"{curTutorialSetID}?óê ?ï¥?ãπ?ïò?äî ?à¨?Ü†Î¶¨Ïñº ?Ñ∏?ä∏Í∞? ?óÜ?äµ?ãà?ã§.");

            // ?à¨?Ü†Î¶¨Ïñº Ï¢ÖÎ£å
            isTutorial = false;
            return;
        }
        var step = tutorialSet.steps[curTutorialStepID];
        EnableTutorialStep(step);
    }


    void EnableTutorialStep(TutorialStep stepData)
    {

        // set id , step id log
        Debug.Log($"Tutorial Set ID : {stepData.setId} , Step ID : {stepData.step}");

        var partern = GetPatternByType(stepData.patternType);
        partern.EventStart(stepData);
    }

    PatternBase GetPatternByType(string type)
    {
        var patternType = (EnumDefinition.PatternType)System.Enum.Parse(typeof(EnumDefinition.PatternType), type);
        return patterns.FirstOrDefault(f => f.patternType == patternType);
    }

    public bool skipSet = false;
    public int skipSetID = 0;

    public void CompleteStep()
    {
        var tutorialSet = GetTutorialSetById(curTutorialSetID);

        // ?òÑ?û¨ ?ä§?Öù ?ôÑÎ£?
        tutorialSet.steps[curTutorialStepID].isStepComplete = true;


        // ?ã§?ùå ?ä§?Öù ?ûà?äîÏß? ?ôï?ù∏
        if (tutorialSet.steps.Any(a => a.step == curTutorialStepID + 1))
        {
            // ?ã§?ùå ?ä§?Öù ?ã§?ñâ
            ++curTutorialStepID;
            var stepData = tutorialSet.steps[curTutorialStepID];
            EnableTutorialStep(stepData);
            // var tutoBtn = GetTutorialButtonById(step.tutorialBtnId);
            // tutorialUiCont.EnableTutorial(step.description, tutoBtn.image, tutoBtn.button);
        }
        else
        {
            // ?à¨?Ü†Î¶¨Ïñº ?Ñ∏?ä∏ ?ôÑÎ£?
            Debug.Log("Tutorial Complete : " + curTutorialSetID);
            tutorialSet.isSetComplete = true;
            tutorialSet.steps[curTutorialStepID].isStepComplete = true;

            if (skipSet)
            {
                curTutorialSetID = skipSetID;
                skipSet = false;
                skipSetID = 0;
            }

            curTutorialSetID++;
            curTutorialStepID = 0;

            tutorialUiCont.DisableTutorial();

            // set save data
            GlobalData.instance.saveDataManager.SaveDataTutorialSetID(curTutorialSetID);

            if (IsTutorialAllComplete() == false)
            {
                EnableTutorialSet();
            }
            else
            {
                isTutorial = false;

                // Í≥®Îìú?îºÍ∑? ?ì±?û•
                GlobalData.instance.goldPigController.EnableGoldPig();
                // ?ÉÅ?†ê Î≤ÑÌäº ?ôú?Ñ±?ôî
                UtilityMethod.GetCustomTypeBtnByID(6).gameObject.SetActive(true);
                // ?à¨?Ü†Î¶¨Ïñº ?ïà?Ç¥ ?Öç?ä§?ä∏ ÎπÑÌôú?Ñ±?ôî
                tutorialPlayingTextBox.gameObject.SetActive(false);

                Debug.Log("?à¨?Ü†Î¶¨Ïñº Ï¢ÖÎ£å");
            }

        }

        //Debug.Log(curTutorialStepID);
    }

    bool IsTutorialAllComplete()
    {
        var lastSetID = tutorialStepData.data.Max(m => m.setId);
        return lastSetID < curTutorialSetID;
    }

    public TutorialButton GetTutorialButtonById(int id)
    {
        var btn = tutorialButtons.FirstOrDefault(f => f.id == id);
        if (btn == null)
        {
            Debug.LogError($"{id}?óê ?ï¥?ãπ?ïò?äî ?à¨?Ü†Î¶¨Ïñº Î≤ÑÌäº?ù¥ ?óÜ?äµ?ãà?ã§.");
        }
        return tutorialButtons.FirstOrDefault(f => f.id == id);
    }

    public void SetUnionGambleingState(bool state)
    {
        isUnionGamblingTutorial = state;
    }


    public TutorialGameObject GetTutorialGameObjectById(int id)
    {
        var obj = tutorialGameObjects.FirstOrDefault(f => f.id == id);
        if (obj == null)
        {
            Debug.LogError($"{id}?óê ?ï¥?ãπ?ïò?äî ?à¨?Ü†Î¶¨Ïñº Í≤åÏûÑ ?ò§Î∏åÏ†ù?ä∏Í∞? ?óÜ?äµ?ãà?ã§.");
        }
        return obj;
    }

}

[System.Serializable]
public class TutorialStepDatas
{
    public List<TutorialStep> data = new List<TutorialStep>();
}

[System.Serializable]
public class TutorialStepSetData
{
    public int setId;
    public bool isSetComplete = false;
    public List<TutorialStep> steps = new List<TutorialStep>();
}