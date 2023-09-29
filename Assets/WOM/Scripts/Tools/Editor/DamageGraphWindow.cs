using UnityEngine;
using UnityEditor;

public class DamageCalculator
{
    public static float CalculateDamage(int level, float a, float b)
    {
        return a * Mathf.Exp(b * level);
    }
}

public class DamageGraphWindow : EditorWindow
{
    private int maxLevel = 20;  // 초기값
    private float[] damages;
    private float a = 1f;  // 초기값
    private float b = 0.03f;  // 초기값
    private Vector2 scrollPosition;

    float aValueMin = 0.1f;
    float aValueMax = 5f;

    float bValueMin = 0.01f;
    float bValueMax = 0.1f;

    [MenuItem("GM_TOOLS/Damage Graph")]
    public static void ShowWindow()
    {
        GetWindow<DamageGraphWindow>("Damage Graph");
    }

    void OnEnable()
    {
        UpdateDamages();
    }

    private void UpdateDamages()
    {
        damages = new float[maxLevel + 1];
        for (int i = 0; i <= maxLevel; i++)
        {
            damages[i] = DamageCalculator.CalculateDamage(i, a, b);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Damage Curve Parameters", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        aValueMin = EditorGUILayout.FloatField("a Min", aValueMin);
        aValueMax = EditorGUILayout.FloatField("a Max", aValueMax);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        bValueMin = EditorGUILayout.FloatField("b Min", bValueMin);
        bValueMax = EditorGUILayout.FloatField("b Max", bValueMax);
        GUILayout.EndHorizontal();


        a = EditorGUILayout.Slider("a Value", a, aValueMin, aValueMax);
        b = EditorGUILayout.Slider("b Value", b, bValueMin, bValueMax);
        maxLevel = EditorGUILayout.IntSlider("Max Level", maxLevel, 0, 200);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateDamages();
        }

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Damage Curve", EditorStyles.boldLabel);

        Rect graphRect = GUILayoutUtility.GetRect(0, 400, 0, this.position.height / 3);
        DrawGrid(graphRect);
        DrawGraph(graphRect);

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Damage Values", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(this.position.height / 2));
        for (int i = 0; i <= maxLevel; i++)
        {
            EditorGUILayout.LabelField($"Level {i}: {damages[i]:0}");
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawGrid(Rect rect)
    {
        Handles.DrawSolidRectangleWithOutline(rect, new Color(0.18f, 0.18f, 0.18f), Color.black);

        // Draw vertical lines
        for (int i = 1; i < 10; i++)
        {
            float x = rect.x + i * rect.width / 10;
            Handles.DrawLine(new Vector3(x, rect.y), new Vector3(x, rect.yMax));
        }

        // Draw horizontal lines
        for (int i = 1; i < 10; i++)
        {
            float y = rect.y + i * rect.height / 10;
            Handles.DrawLine(new Vector3(rect.x, y), new Vector3(rect.xMax, y));
        }
    }

    private void DrawGraph(Rect rect)
    {
        float maxDamage = damages[maxLevel];

        for (int i = 1; i <= maxLevel; i++)
        {
            float xPrev = rect.x + (i - 1) * rect.width / maxLevel;
            float yPrev = rect.yMax - (damages[i - 1] / maxDamage) * rect.height;

            float xCurrent = rect.x + i * rect.width / maxLevel;
            float yCurrent = rect.yMax - (damages[i] / maxDamage) * rect.height;

            Handles.color = Color.green;
            Handles.DrawLine(new Vector3(xPrev, yPrev), new Vector3(xCurrent, yCurrent));
        }
    }
}
