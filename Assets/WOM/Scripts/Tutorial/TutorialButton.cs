using UnityEngine;
using UnityEngine.UI;


public class TutorialButton : MonoBehaviour
{
    public int id;
    public Image image;
    public Button button;
    TutorialManager tutorialManager;


    void Start()
    {
        //GetManager();
        GetButton();

        //     button.onClick.AddListener(() =>
        //    {
        //        EventManager.instance.RunEvent<int>(CallBackEventType.TYPES.OnTutorialBtnClick, id);
        //        Debug.Log("click!!!");
        //    });
    }

    void GetManager()
    {
        if (tutorialManager == null)
            tutorialManager = FindAnyObjectByType<TutorialManager>();
    }

    public void GetButton()
    {
        button = GetComponent<Button>();
        image = button.image;
    }
}
