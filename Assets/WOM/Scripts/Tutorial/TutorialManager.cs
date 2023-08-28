using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TutorialManager : MonoBehaviour
{
    public TextAsset tutorialJsonData;

    // 투토리얼 데이터
    public TutorialStepDatas tutorialStepData;

    // setId 기준으로 나눈 투토리얼 세트 데이터
    public List<TutorialStepSetData> tutorialStepSetDatas = new List<TutorialStepSetData>();

    // 투토리얼 버튼들
    public List<TutorialButton> tutorialButtons = new List<TutorialButton>();

    // 현재 진행중인 투토리얼 세트의 아이디
    public int curTutorialSetID = 0;
    // 현재 진행중인 투토리얼 세트의 스텝 아이디
    public int curTutorialStepID = 0;

    public TutorialUiController tutorialUiCont;

    public List<PatternBase> patterns = new List<PatternBase>();

    public bool isTutorial = false;

    public bool isUnionGamblingTutorial = false;

    // 신규 유저 선물 닫기 버튼
    public List<Button> tutoStartBtns = new List<Button>();

    public bool isAdPass = false;

    //투토리얼 게임 오브젝트
    public List<TutorialGameObject> tutorialGameObjects = new List<TutorialGameObject>();

    public GameObject tutorialPlayingTextBox;

    void Start()
    {
        //StartCoroutine(Init());
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

    public void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     EnableTutorialSet();
        // }
    }

    //public bool disableTutorial = false;

    public void TutorialStart()
    {
        //if (disableTutorial) return;

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
            Debug.Log("모든 투토리얼 완료");
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
        isTutorial = !IsTutorialAllComplete();


        // if (disableTutorial)
        //     isTutorial = false;

        yield return null;
    }

    void SetData()
    {
        // set set data ( setId 기준으로 세트 리스트를 만듦 )
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
            Debug.LogError($"{setID}에 해당하는 투토리얼 세트가 없습니다.");
        }
        return tutorialStepSetDatas.FirstOrDefault(f => f.setId == setID);
    }



    public void EnableTutorialSet()
    {
        var tutorialSet = GetTutorialSetById(curTutorialSetID);

        if (tutorialSet == null)
        {
            Debug.LogError($"{curTutorialSetID}에 해당하는 투토리얼 세트가 없습니다.");

            // 투토리얼 종료
            isTutorial = false;
            return;
        }
        var step = tutorialSet.steps[curTutorialStepID];
        EnableTutorialStep(step);
    }


    void EnableTutorialStep(TutorialStep stepData)
    {
        var partern = GetPatternByType(stepData.patternType);
        partern.EventStart(stepData);
    }

    PatternBase GetPatternByType(string type)
    {
        var patternType = (EnumDefinition.PatternType)System.Enum.Parse(typeof(EnumDefinition.PatternType), type);
        return patterns.FirstOrDefault(f => f.patternType == patternType);
    }

    public void CompleteStep()
    {
        var tutorialSet = GetTutorialSetById(curTutorialSetID);

        // 현재 스텝 완료
        tutorialSet.steps[curTutorialStepID].isStepComplete = true;


        // 다음 스텝 있는지 확인
        if (tutorialSet.steps.Any(a => a.step == curTutorialStepID + 1))
        {
            // 다음 스텝 실행
            ++curTutorialStepID;
            var stepData = tutorialSet.steps[curTutorialStepID];
            EnableTutorialStep(stepData);
            // var tutoBtn = GetTutorialButtonById(step.tutorialBtnId);
            // tutorialUiCont.EnableTutorial(step.description, tutoBtn.image, tutoBtn.button);
        }
        else
        {
            // 투토리얼 세트 완료
            Debug.Log("Tutorial Complete : " + curTutorialSetID);
            tutorialSet.isSetComplete = true;
            tutorialSet.steps[curTutorialStepID].isStepComplete = true;

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

                // 골드피그 등장
                GlobalData.instance.goldPigController.EnableGoldPig();
                // 상점 버튼 활성화
                UtilityMethod.GetCustomTypeBtnByID(6).gameObject.SetActive(true);
                // 투토리얼 안내 텍스트 비활성화
                tutorialPlayingTextBox.gameObject.SetActive(false);

                Debug.Log("투토리얼 종료");
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
            Debug.LogError($"{id}에 해당하는 투토리얼 버튼이 없습니다.");
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
            Debug.LogError($"{id}에 해당하는 투토리얼 게임 오브젝트가 없습니다.");
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