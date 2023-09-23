using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Firebase;
public class FirebaseManager : MonoBehaviour
{

    FirebaseApp app;
    bool isInitComplete = false;

    public IEnumerator Init()
    {
        yield return null;

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



    public void LogEvent(string eventName, params Parameter[] parameters)
    {
        if(isInitComplete)
        FirebaseAnalytics.LogEvent(eventName,parameters);
    }



}
