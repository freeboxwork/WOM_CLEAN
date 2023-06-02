using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlData : ScriptableObject
{
    public List<Url_Data> url_Datas = new List<Url_Data>();
}

[System.Serializable]
public class Url_Data
{
    public string name;
    public string url;
}
