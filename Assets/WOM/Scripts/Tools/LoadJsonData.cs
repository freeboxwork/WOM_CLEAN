using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class LoadJsonData : MonoBehaviour
{
    private string apiKey = "AIzaSyDkkW_QvGbEWwhPzrPfaHix9Kk89uIk1Qs"; // Google API Key
    private string sheetId = "101MDn8hJlg89ZEdgD3JqRDIVx9IaUi8MtTY1mhsE-90"; // Google Sheet ID
    private string sheetName = "GLOBAL POPUP MESSAGE"; // Sheet name

    void Start()
    {
        StartCoroutine(FetchGoogleSheetData());
    }

    IEnumerator FetchGoogleSheetData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{sheetId}/values/{sheetName}?key={apiKey}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching data from Google Sheets: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            //Debug.Log("Google Sheets data: " + jsonResponse);

            // Process JSON data

            var jsonData = jsonResponse.Replace("values", "data");

            //GlobalMessageDatas globalMessageDatas = JsonUtility.FromJson<GlobalMessageDatas>(jsonData);
            //Debug.Log(" data : " + globalMessageDatas.data.Count);


            /*
            JObject jsonData = JObject.Parse(jsonResponse);
            JArray values = (JArray)jsonData["values"];

            if (values != null && values.Count > 1)
            {
                int lastRowIndex = values.Count - 1;
                int lastColumnIndex = values[1].Count() - 1;

                for (int row = 1; row <= lastRowIndex; row++)
                {
                    for (int col = 0; col <= lastColumnIndex; col++)
                    {
                        Debug.Log($"Data[{row},{col}]: {values[row][col]}");
                    }
                }
            }
            else
            {
                Debug.LogWarning("No data found in the sheet.");
            }
            */
        }
    }
}
