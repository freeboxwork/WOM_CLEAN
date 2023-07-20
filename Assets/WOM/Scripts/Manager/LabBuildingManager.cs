using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabBuildingManager : MonoBehaviour
{
    public List<LabBuildIngameData> labBuildIngameDataList = new List<LabBuildIngameData>();


    void Start()
    {

    }


    public IEnumerator Init()
    {
        // set data ( load data )
        var data = GlobalData.instance.saveDataManager.saveDataTotal.saveDataLabBuildingData.labBuildIngameDatas;
        for (int i = 0; i < data.Count; i++)
        {
            var buildData = data[i].CloneInstance();
            labBuildIngameDataList.Add(buildData);
        }
        yield return null;
    }




}
