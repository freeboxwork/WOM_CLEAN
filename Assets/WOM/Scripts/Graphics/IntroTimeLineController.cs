using UnityEngine;
using UnityEngine.Playables;


public class IntroTimeLineController : MonoBehaviour
{
    public PlayableDirector director;
    public double[] cutFrames;
    public int currentTimeIndex = 0;

    [Range(0.0f, 2.0f)]
    public float deleyTime = 0.8f;
    private bool dontTouch = false;

    private void OnEnable()
    {
        director.initialTime = cutFrames[currentTimeIndex = 0];
        director.Play();
    }

    float t = 0.0f;
    bool endTime = false;

    public IntroManager introManager;

    bool bEndIntro = false;


    void Update()
    {
        if (!bEndIntro)
        {
            if (!dontTouch && currentTimeIndex != (cutFrames.Length - 1))
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.P))
                {
                    double currentTime = director.time;
                    for (int i = 0; i < cutFrames.Length; i++)
                    {
                        if (currentTime <= cutFrames[i])
                        {
                            currentTimeIndex = i;
                            break;
                        }
                    }

                    director.time = cutFrames[currentTimeIndex];
                    dontTouch = true;
                }
            }
            else
            {
                t += Time.deltaTime;
                if (t >= deleyTime)
                {
                    t = 0.0f;
                    dontTouch = false;
                }
            }

            if (director.time > 18 && !endTime)
            {
                bEndIntro = true;
                endTime = true;
                introManager.ShowMainCanvas(true);
                StartCoroutine(introManager.SceneLoad());
            }
        }



    }

    public void OnMarkableIntroSignal()
    {
        currentTimeIndex = 5;
        dontTouch = true;
    }


    private void OnDisable()
    {
        currentTimeIndex = 0;
        director.Stop();
    }



    public void SkipIntro()
    {
        bEndIntro = true;
        director.time = cutFrames[cutFrames.Length - 1];
        director.Evaluate();
    }
}
