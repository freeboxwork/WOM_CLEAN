using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Firebase;
public class FirebaseManager : MonoBehaviour
{

    FirebaseApp app;
    public bool isInitComplete = false;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if(task.Result == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                isInitComplete = true;
            }
        });
    }


    public void LogEvent(string eventName,string paramName, long value)
    {
        if(isInitComplete)
        FirebaseAnalytics.LogEvent(eventName,paramName,value);
    }






}
