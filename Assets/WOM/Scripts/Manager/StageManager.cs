using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageData stageData;
    public SpriteRenderer rdBg;
    public List<Sprite> bgImages = new List<Sprite>();
    public BackgroundAnimController bgAnimController;

    public BackgroundSpriteFileData bgSpriteFileData;

    void Start()
    {

    }

    public void PlayAnimBgScroll()
    {
        StartCoroutine(bgAnimController.ScrollAnim_BG());
    }

    public IEnumerator Init(int stageId)
    {
        yield return null;
        SetStageData(stageId);
        SetBgImage();
    }

    public IEnumerator SetStageById(int stageIdx)
    {
        // 일일 퀘스트 완료 : 스테이지 클리어
        EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.progressStage);

        // set data
        SetStageData(stageIdx, out bool isBgImgChange);

        // 배경 변경
        if (isBgImgChange)
        {
            var nextBgImg = GetCurrentBgImg();
            yield return StartCoroutine(bgAnimController.TransitinBG(nextBgImg));
        }

        yield return null;
    }

    void SetStageData(int stageId)
    {
        var data = GlobalData.instance.dataManager.GetStageDataById(stageId);
        stageData = data.CopyInstance();
    }

    void SetStageData(int stageId, out bool isBgImgChange)
    {
        var data = GlobalData.instance.dataManager.GetStageDataById(stageId);

        // 배경 변경 여부 판단
        isBgImgChange = stageData.bgId != data.bgId;

        stageData = data.CopyInstance();
    }

    public void SetBgImage()
    {
        bgAnimController.SetBgTex_Back(GetCurrentBgImg().texture);
    }

    public void SetDungeonBgImage(int bgId)
    {
        bgAnimController.SetBgTex_Back(GetBgImgById(bgId).texture);
    }


    Sprite GetCurrentBgImg()
    {
        var idx = stageData.bgId;
        return (GetBgImgById(idx));
    }


    Sprite GetBgImgById(int id)
    {
        return bgSpriteFileData.background[id];
    }
}
