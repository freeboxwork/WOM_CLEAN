using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GuideInfoUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI guideName;
    [SerializeField] private TextMeshProUGUI guideDescription;
    [SerializeField] private TextMeshProUGUI guideRewardValue;
    [SerializeField] private TextMeshProUGUI guideProgressValue;

    [SerializeField] private Image guideRewardImage;
    [SerializeField] private Button guideButton;
    [SerializeField] private GameObject completedPanel;

    void Awake()
    {
        guideButton.onClick.AddListener(OnGuideButtonClicked);
    }

    public void SetTextGuideName(string name)
    {
        guideName.text = name;
    }
    public void SetTextGuideDescription(string description)
    {
        guideDescription.text = description;
    }
    public void SetTextGuideProgressValue(int value)
    {
        guideProgressValue.text = value.ToString();
    }
    public void SetImageGuideReward(Sprite image)
    {
        guideRewardImage.sprite = image;
    }
    public void SetTextGuideRewardValue(int value)
    {
        guideRewardValue.text = value.ToString();
    }

    public void SetCompletedPanel(bool value)
    {
        completedPanel.SetActive(value);

    }
    
    private void OnGuideButtonClicked()
    {
        
    }


}
