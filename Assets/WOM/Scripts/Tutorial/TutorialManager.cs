using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    public TextAsset tutorialJsonData;

    // ?¬? λ¦¬μΌ ?°?΄?°
    public TutorialStepDatas tutorialStepData;

    // setId κΈ°μ???Όλ‘? ?? ?¬? λ¦¬μΌ ?Έ?Έ ?°?΄?°
    public List<TutorialStepSetData> tutorialStepSetDatas = new List<TutorialStepSetData>();

    // ?¬? λ¦¬μΌ λ²νΌ?€
    public List<TutorialButton> tutorialButtons = new List<TutorialButton>();

    // ??¬ μ§νμ€μΈ ?¬? λ¦¬μΌ ?Έ?Έ? ??΄?
    public int curTutorialSetID = 0;
    // ??¬ μ§νμ€μΈ ?¬? λ¦¬μΌ ?Έ?Έ? ?€? ??΄?
    public int curTutorialStepID = 0;

    public TutorialUiController tutorialUiCont;

    public List<PatternBase> patterns = new List<PatternBase>();

    public bool isTutorial = false;

    public bool isUnionGamblingTutorial = false;

    // ? κ·? ? ??? ? λ¬? ?«κΈ? λ²νΌ
    public List<Button> tutoStartBtns = new List<Button>();

    public bool isAdPass = false;

    //?¬? λ¦¬μΌ κ²μ ?€λΈμ ?Έ
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
            Debug.Log("λͺ¨λ  ?¬? λ¦¬μΌ ?λ£?");
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
        // set set data ( setId κΈ°μ???Όλ‘? ?Έ?Έ λ¦¬μ€?Έλ₯? λ§λ¦ )
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
            Debug.LogError($"{setID}? ?΄?Ή?? ?¬? λ¦¬μΌ ?Έ?Έκ°? ??΅??€.");
        }
        return tutorialStepSetDatas.FirstOrDefault(f => f.setId == setID);
    }



    public void EnableTutorialSet()
    {
        var tutorialSet = GetTutorialSetById(curTutorialSetID);

        if (tutorialSet == null)
        {
            Debug.LogError($"{curTutorialSetID}? ?΄?Ή?? ?¬? λ¦¬μΌ ?Έ?Έκ°? ??΅??€.");

            // ?¬? λ¦¬μΌ μ’λ£
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

        // ??¬ ?€? ?λ£?
        tutorialSet.steps[curTutorialStepID].isStepComplete = true;


        // ?€? ?€? ??μ§? ??Έ
        if (tutorialSet.steps.Any(a => a.step == curTutorialStepID + 1))
        {
            // ?€? ?€? ?€?
            ++curTutorialStepID;
            var stepData = tutorialSet.steps[curTutorialStepID];
            EnableTutorialStep(stepData);
            // var tutoBtn = GetTutorialButtonById(step.tutorialBtnId);
            // tutorialUiCont.EnableTutorial(step.description, tutoBtn.image, tutoBtn.button);
        }
        else
        {
            // ?¬? λ¦¬μΌ ?Έ?Έ ?λ£?
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

                // κ³¨λ?Όκ·? ?±?₯
                GlobalData.instance.goldPigController.EnableGoldPig();
                // ??  λ²νΌ ??±?
                UtilityMethod.GetCustomTypeBtnByID(6).gameObject.SetActive(true);
                // ?¬? λ¦¬μΌ ??΄ ??€?Έ λΉν?±?
                tutorialPlayingTextBox.gameObject.SetActive(false);

                Debug.Log("?¬? λ¦¬μΌ μ’λ£");
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
            Debug.LogError($"{id}? ?΄?Ή?? ?¬? λ¦¬μΌ λ²νΌ?΄ ??΅??€.");
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
            Debug.LogError($"{id}? ?΄?Ή?? ?¬? λ¦¬μΌ κ²μ ?€λΈμ ?Έκ°? ??΅??€.");
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