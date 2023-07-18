using System.Collections;
using System.Collections.Generic;
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
    void Update()
    {
        if (!dontTouch && currentTimeIndex != (cutFrames.Length-1))
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
            if(t >= deleyTime)
            {
                t = 0.0f;
                dontTouch = false;
            }
        }
    }

    public void OnMarkableIntroSignal()
    {
        dontTouch = true;
    }


    private void OnDisable()
    {
        currentTimeIndex = 0;
        director.Stop();    
    }
}
