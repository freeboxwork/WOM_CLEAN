using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PerformanceTest : MonoBehaviour
{
    //public static void RunPerformanceTest()
    //{
    //    int iterations = 1000;
    //    long totalTime = 0L ;

    //    for (int i = 0; i < iterations; i++)
    //    {
    //        string filePath = Application.dataPath + "/test" + i + ".json";

    //        float startTime = Time.realtimeSinceStartup;
    //       // SerializeClassToJson(myClass, filePath);
    //        float endTime = Time.realtimeSinceStartup;

    //        float elapsedTime = endTime - startTime;
    //        totalTime += elapsedTime;
    //    }

    //    float averageTime = totalTime / iterations;
    //    Debug.Log("Average time per iteration: " + averageTime + " seconds.");
    //}

    int iterations = 1000;
    long totalTime = 0L;

    public TextMeshProUGUI txtResult;
    public TextMeshProUGUI txtCount;
    public TMP_InputField tMP_InputField;
    public Button btnTestSingleClass;
    public Button btnTestTotalClass;

    public SaveDataManager saveDataManager;

    // todo : total save data - data set...

    private void Start()
    {
      
        SetTotalSaveData();
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        btnTestSingleClass.onClick.AddListener(() => {

            StartCoroutine(RunPerformanceTestSaveDataSingle_Cor());
        });

        btnTestTotalClass.onClick.AddListener(() => {

            StartCoroutine(RunPerformanceTestSaveDataTotal_Cor());
        });


    }

    void SetTotalSaveData()
    {
        saveDataManager.SetData();
    }

    // todo : traning save data - data set....
    void SetTraningSaveData()
    {

    }
    string GetSaveDataFilePaht(string fileName)
    {
        string path = "";
#if UNITY_EDITOR
        path = Application.dataPath + "/" + fileName;
#elif UNITY_ANDROID
            path = Application.persistentDataPath + "/"+ fileName;
#endif
        return path;
    }


    // 클래스 10개 분량을 한번에 저장 , 1000 회 반복
    public void RunPerformanceTestSaveDataTotal()
    {
        for (int i = 0; i < iterations; i++)
        {
            string filePath =  GetSaveDataFilePaht("testTotalSave.json");
            float startTime = Time.realtimeSinceStartup * 1000f;

            

            //SerializeClassToJson(totalSaveData, filePath);
           

            float endTime = Time.realtimeSinceStartup * 1000f;
            long elapsedTime = (long)(endTime - startTime);
            totalTime += elapsedTime;
        }

        float averageTime = (float)totalTime / iterations;
        Debug.Log("Average time per iteration: " + averageTime.ToString("F2") + " milliseconds.");
    }

    // 클래스 1개 분량을 한번에 저장 , 1000 회 반복
    public void RunPerformanceTestSaveDataElement()
    {
        for (int i = 0; i < iterations; i++)
        {
            string filePath = GetSaveDataFilePaht("testSingleSave.json");
            float startTime = Time.realtimeSinceStartup * 1000f;





            float endTime = Time.realtimeSinceStartup * 1000f;
            long elapsedTime = (long)(endTime - startTime);
            totalTime += elapsedTime;
        }

        float averageTime = (float)totalTime / iterations;
        Debug.Log("Average time per iteration: " + averageTime.ToString("F2") + " milliseconds.");
    }

    IEnumerator RunPerformanceTestSaveDataSingle_Cor()
    {
        totalTime = 0;
        iterations = int.Parse(tMP_InputField.text);
        for (int i = 0; i < iterations; i++)
        {
            txtCount.text = "COUNT : " + i;

            string filePath = GetSaveDataFilePaht("testSingleSave.json");
            //float startTime = Time.realtimeSinceStartup * 1000f;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            SerializeClassToJson(saveDataManager.saveDataTotal.saveDataTranings, filePath);

            //float endTime = Time.realtimeSinceStartup * 1000f;

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            //long elapsedTime = (long)(endTime - startTime);
            totalTime += elapsedTime;

            yield return null;
        }

        float averageTime = (float)totalTime / iterations;
        var result = "Average time per iteration: " + averageTime.ToString("F2") + " milliseconds.";
        txtResult.text = result;
        Debug.Log("Average time per iteration: " + averageTime.ToString("F2") + " milliseconds.");
    }


    IEnumerator RunPerformanceTestSaveDataTotal_Cor()
    {
        totalTime = 0;
        iterations = int.Parse(tMP_InputField.text);
        for (int i = 0; i < iterations; i++)
        {
            txtCount.text = "COUNT : " + i;

            string filePath = GetSaveDataFilePaht("testTotalSave.json");
           // float startTime = Time.realtimeSinceStartup * 1000f;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();


            SerializeClassToJson(saveDataManager.saveDataTotal, filePath);

            //float endTime = Time.realtimeSinceStartup * 1000f;
            //long elapsedTime = (long)(endTime - startTime);

            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            totalTime += elapsedTime;

            yield return null;
        }

        float averageTime = (float)totalTime / iterations;
        var result = "Average time per iteration: " + averageTime.ToString("F2") + " milliseconds.";
        txtResult.text = result;
        Debug.Log("Average time per iteration: " + averageTime.ToString("F2") + " milliseconds.");
    }

    public static void SerializeClassToJson<T>(T obj, string filePath)
    {
        string json = JsonUtility.ToJson(obj);
        File.WriteAllText(filePath, json);
    }


}

