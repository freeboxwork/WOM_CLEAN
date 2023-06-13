using UnityEngine;
using UnityEditor;

public class PlayerPrefsViewer : EditorWindow
{
    [MenuItem("GM_TOOLS/PlayerPrefs Viewer")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsViewer>("PlayerPrefs Viewer");
    }

    void OnGUI()
    {
        GUILayout.Label("PlayerPrefs Viewer", EditorStyles.boldLabel);

        // 모든 PlayerPrefs 키를 가져온다.
        string[] keys = PlayerPrefs.GetString("UnityEditor.PlayerPrefsWindow.plist", "").Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 모든 PlayerPrefs 데이터를 출력한다.
        foreach (string key in keys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(key, GUILayout.Width(200));
            GUILayout.Label(PlayerPrefs.GetString(key), GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }
    }
}