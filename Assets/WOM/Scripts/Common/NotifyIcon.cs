using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NotifyIcon : MonoBehaviour
{
    public EnumDefinition.NotifyType notifyType;
    public List<Button> buttonList = new List<Button>();
    public List<GameObject> gameObjectList = new List<GameObject>();
    public List<Image> imageList = new List<Image>();
    public Image iconNotify;
    public GameObject textNotify;

    public List<NotifyIcon> notifyIconList = new List<NotifyIcon>();

    public bool isActive = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (notifyType)
        {
            case EnumDefinition.NotifyType.Button:
                NotifyCheck_Buttons();
                break;
            case EnumDefinition.NotifyType.GameObject:
                NotifyCheck_GameObjects();
                break;
            case EnumDefinition.NotifyType.Image:
                NotifyCheck_Images();
                break;
            case EnumDefinition.NotifyType.Notify:
                NotifyCheckNotify();
                break;
            default:
                break;
        }
        iconNotify.enabled = isActive;
        textNotify.SetActive(isActive);
    }


    void NotifyCheck_Buttons()
    {
        isActive = buttonList.Any(x => x.interactable == true);
    }
    void NotifyCheck_GameObjects()
    {
        isActive = gameObjectList.Any(x => x.activeSelf == true);
    }
    void NotifyCheck_Images()
    {
        isActive = imageList.Any(x => x.enabled == true);
    }
    void NotifyCheckNotify()
    {
        isActive = notifyIconList.Any(x => x.isActive == true);
    }

}
