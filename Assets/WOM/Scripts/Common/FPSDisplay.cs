using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    private float currentFPS;
    private float updateInterval = 0.5f;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeLeft; // Left time for current interval

    void Start()
    {
        timeLeft = updateInterval;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeLeft <= 0.0)
        {
            // display two fractional digits (f2 format)
            currentFPS = (accum / frames);
            timeLeft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 5 * 12;
        Rect rect = new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50);
        GUI.Label(rect, "FPS: " + currentFPS.ToString("f2"), style);
    }
}
