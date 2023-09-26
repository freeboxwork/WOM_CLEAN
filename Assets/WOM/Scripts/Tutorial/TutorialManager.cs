using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextAsset tutorialJsonData;

    // ���丮�� ������
    public TutorialStepDatas tutorialStepData;

    // setId �������� ���� ���丮�� ��Ʈ ������
    public List<TutorialStepSetData> tutorialStepSetDatas = new List<TutorialStepSetData>();

    // ���丮�� ��ư��
    public List<TutorialButton> tutorialButtons = new List<TutorialButton>();

    // ���� �������� ���丮�� ��Ʈ�� ���̵�
    public int curTutorialSetID = 0;
    // ���� �������� ���丮�� ��Ʈ�� ���� ���̵�
    public int curTutorialStepID = 0;

    public TutorialUiController tutorialUiCont;

    public List<PatternBase> patterns = new List<PatternBase>();

    public bool isEndTutorial = false;

    public bool isUnionGamblingTutorial = false;

    public bool isAdPass = false;

    //���丮�� ���� ������Ʈ
    public List<TutorialGameObject> tutorialGameObjects = new List<TutorialGameObject>();

    public GameObject tutorialPlayingTextBox;

    public GameObject newUserEventPopupObj;

    TutorialStep currentTutorialStep;

    public GameObject evolutionInfoBox;
    public GameObject bossToggleBox;

    public void TutorialStart()
    {
        // max check
        if (IsTutorialAllComplete() == false)
        {
            isEndTutorial = false;
            EnableTutorialSet();
            //tutorialPlayingTextBox.gameObject.SetActive(true);

        }
        else
        {
            isEndTutorial = true;
            // ��ȭ ���� ���� �ڽ� Ȱ��ȭ
            evolutionInfoBox.SetActive(true);
            bossToggleBox.SetActive(true);
            //tutorialPlayingTextBox.gameObject.SetActive(false);
            //Debug.Log("��� ���丮�� �Ϸ�");
        }
    }
    public int GetTutorialSetId()
    {
        return curTutorialSetID;
    }

    public IEnumerator Init()
    {

        // get json data
        tutorialStepData = JsonUtility.FromJson<TutorialStepDatas>(tutorialJsonData.text);
        yield return new WaitForEndOfFrame();
        SetData();
        yield return new WaitForEndOfFrame();

        // get tutorial set id ( load data )
        curTutorialSetID = GlobalData.instance.saveDataManager.saveDataTotal.saveDataTutorial.tutorialSetId;
        if (curTutorialSetID > 0)
        {
            var max = tutorialStepSetDatas.Last().setId;
            if (curTutorialSetID > max)
            {
                Debug.Log("Ʃ�丮�� ����");
            }
            else
            {
                ++curTutorialSetID;
                GlobalData.instance.saveDataManager.SaveDataTutorialSetID(curTutorialSetID);
            }
        }

        //isTutorial = false;

        // max check
        if (IsTutorialAllComplete() == false)
        {
            isEndTutorial = false;
        }
        else
        {
            isEndTutorial = true;
        }

        yield return null;
    }

    void SetData()
    {
        // set set data ( setId �������� ��Ʈ ����Ʈ�� ���� )
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
            Debug.LogError($"{setID}�� �ش��ϴ� ���丮�� ��Ʈ�� �����ϴ�.");
        }
        return tutorialStepSetDatas.FirstOrDefault(f => f.setId == setID);
    }



    public void EnableTutorialSet()
    {
        var tutorialSet = GetTutorialSetById(curTutorialSetID);

        if (tutorialSet == null)
        {

            Debug.LogError($"{curTutorialSetID}�� �ش��ϴ� ���丮�� ��Ʈ�� �����ϴ�.");


            isEndTutorial = true;
            return;
        }
        var step = tutorialSet.steps[curTutorialStepID];
        currentTutorialStep = step;
        EnableTutorialStep();
    }

    public TextMeshProUGUI tutorialProgressText;
    void EnableTutorialStep()
    {
        var lastSetID = tutorialStepData.data.Max(m => m.setId);
        tutorialPlayingTextBox.gameObject.SetActive(true);

        tutorialProgressText.text = $"Ʃ�丮�� ������... {currentTutorialStep.setId + 1}/{lastSetID + 1}";
        // set id , step id log
        Debug.Log($"Tutorial Set ID : {currentTutorialStep.setId} , Step ID : {currentTutorialStep.step}");

        var partern = GetPatternByType(currentTutorialStep.patternType);
        partern.EventStart(currentTutorialStep);
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

        // ���� ���� �Ϸ�
        tutorialSet.steps[curTutorialStepID].isStepComplete = true;


        // ���� ���� �ִ��� Ȯ��
        if (tutorialSet.steps.Any(a => a.step == curTutorialStepID + 1))
        {
            // ���� ���� ����
            ++curTutorialStepID;
            var stepData = tutorialSet.steps[curTutorialStepID];
            currentTutorialStep = stepData;
            EnableTutorialStep();
            // var tutoBtn = GetTutorialButtonById(step.tutorialBtnId);
            // tutorialUiCont.EnableTutorial(step.description, tutoBtn.image, tutoBtn.button);
        }
        else
        {
            // ���丮�� ��Ʈ �Ϸ�
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
            tutorialPlayingTextBox.gameObject.SetActive(false);

            // set save data
            GlobalData.instance.saveDataManager.SaveDataTutorialSetID(curTutorialSetID);

            if (IsTutorialAllComplete() == false)
            {
                EnableTutorialSet();
            }
            else
            {
                isEndTutorial = true;

                // ����Ǳ� ����
                GlobalData.instance.goldPigController.EnableGoldPig();
                // ���� ��ư Ȱ��ȭ
                UtilityMethod.GetCustomTypeBtnByID(6).gameObject.SetActive(true);
                // ���丮�� �ȳ� �ؽ�Ʈ ��Ȱ��ȭ
                tutorialPlayingTextBox.gameObject.SetActive(false);

                // ��ȭ ���� ���� �ڽ� Ȱ��ȭ
                evolutionInfoBox.SetActive(true);
                bossToggleBox.SetActive(true);

                if (GlobalData.instance.insectSpwanManager.isSpwanTimerStart == false)
                    GlobalData.instance.insectSpwanManager.AllTimerStart();

                Debug.Log("���丮�� ����");
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
            Debug.LogError($"{id}�� �ش��ϴ� ���丮�� ��ư�� �����ϴ�.");
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
            Debug.LogError($"{id}�� �ش��ϴ� ���丮�� ���� ������Ʈ�� �����ϴ�.");
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