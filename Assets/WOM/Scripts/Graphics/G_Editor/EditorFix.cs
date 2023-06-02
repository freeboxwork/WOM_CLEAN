using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FIxPSDImporter
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    public static void ResetPSDImporterFoldout()
    {
        UnityEditor.EditorPrefs.DeleteKey("PSDImporterEditor.m_PlatformSettingsFoldout");
    }
#endif
}
