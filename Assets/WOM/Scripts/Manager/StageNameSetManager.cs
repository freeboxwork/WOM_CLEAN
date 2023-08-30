using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class StageNameSetManager : MonoBehaviour
{
    public TextMeshProUGUI txtStageNameNormal;
    public TextMeshProUGUI txtStageNameEvolution;
    public TextMeshProUGUI txtStageNameDungeon;
    private float scaleFactor = 1.3f;
    Dictionary<EnumDefinition.StageNameType, TextMeshProUGUI> dicStageName = new Dictionary<EnumDefinition.StageNameType, TextMeshProUGUI>();

    void Start()
    {
        SetDicStageName();
    }

    void SetDicStageName()
    {
        dicStageName.Add(EnumDefinition.StageNameType.normal, txtStageNameNormal);
        dicStageName.Add(EnumDefinition.StageNameType.evolution, txtStageNameEvolution);
        dicStageName.Add(EnumDefinition.StageNameType.dungeon, txtStageNameDungeon);
    }
    public void SetTxtStageName(EnumDefinition.StageNameType stageNameType, string stageName)
    {
        dicStageName[stageNameType].text = stageName;

        // �ش� �������� �̸� �ؽ�Ʈ�� �������� 1.2��� Ȯ���� ���� ���� ũ��� �ǵ�����
        dicStageName[stageNameType].transform.DOScale(Vector3.one * scaleFactor, 0.3f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                //���� ũ��� �ǵ�����
                dicStageName[stageNameType].transform.DOScale(Vector3.one, 0.1f)
                    .SetEase(Ease.OutQuad);
            });
    }
    public void EnableStageName(EnumDefinition.StageNameType stageNameType)
    {
        foreach (var item in dicStageName)
            item.Value.gameObject.SetActive(item.Key == stageNameType);
    }




}
