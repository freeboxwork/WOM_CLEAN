using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tools_OpenWebPage : EditorWindow
{
    UrlData urlData;

    float labelWidth = 160f;
    bool settingGui = false;
    Vector2 scrollView;

    [MenuItem("GM_TOOLS/OpenWebPage")]
    public static void ShowWindwo()
    {
        var window = GetWindow<Tools_OpenWebPage>();
        window.Show();
    }




    private void OnGUI()
    {
        if (!settingGui)
        {
            EditorCustomGUI.GUI_Title("버튼을 클릭하면 해당 웹페이지를 열어 줍니다.");

            scrollView = GUILayout.BeginScrollView(scrollView);
            foreach (var data in urlData.url_Datas)
            {
                EditorCustomGUI.GUI_Button(data.name, () => { OpenWebPage(data.url); });
            }
            GUILayout.EndScrollView();
            GUI.color = Color.cyan;
            EditorCustomGUI.GUI_Button("SETTING URL", () => { settingGui = true; });
            GUI.color = Color.white;
        }
        else
        {
            Setting_URL();
        }
        
    }

    void Setting_URL()
    {
        EditorCustomGUI.GUI_Title("URL 을 등록 및 편집 합니다.");
        for (int i = 0; i < urlData.url_Datas.Count; i++)
        {
            try
            {
                var data = urlData.url_Datas[i];
                GUILayout.BeginHorizontal();
                EditorCustomGUI.GUI_TextFiled(50, "NAME : ", ref data.name);
                EditorCustomGUI.GUI_TextFiled(50, "URL : ", ref data.url);
                EditorCustomGUI.GUI_Button("-", () => { urlData.url_Datas.RemoveAt(i); });
                GUILayout.EndHorizontal();
            }
            catch { }
        }
        EditorCustomGUI.GUI_Button("ADD URL", () => { AddData(); });
        GUI.color = Color.cyan;
        EditorCustomGUI.GUI_Button("SETTING COMPLETE", () => { urlData.SetDirty(); settingGui = false; });
        GUI.color = Color.white;
    }


    void AddData()
    {
        Url_Data data = new Url_Data();
        data.name = "이름을 입력 해주세요";
        data.url = "URL 주소를 입력 해주세요";
        urlData.url_Datas.Add(data);
    }
    void OnEnable()
    {
        LoadData();
    }
    void OpenWebPage(string url)
    {
        Application.OpenURL(url);
    }

    
    void LoadData()
    {
        urlData = (UrlData)AssetDatabase.LoadAssetAtPath("Assets/urlDatas.asset", typeof(UrlData));
        if (urlData == null)
        {
            CreateData();
        }
    }
    
    void CreateData()
    {
        urlData = ScriptableObject.CreateInstance<UrlData>();
        AssetDatabase.CreateAsset(urlData, "Assets/urlDatas.asset");
    }


}
