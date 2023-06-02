using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

using System.Linq;

namespace ProjectGraphics
{
    [RequireComponent(typeof(PartsChangeTest))]
    public class SavePartsList : MonoBehaviour
    {
        PartsChangeTest partsChange;

        [SerializeField]List<PartNumbesrs> partList = new List<PartNumbesrs>();
        public string csvFile;
        string path;

        void Start()
        {
            partsChange = GetComponent<PartsChangeTest>();
            path = Application.dataPath + partsChange.capturePath;
        }

        public void AddPartList(PartNumbesrs num)
        {
            if (partList == null) return;

            bool samePart = false;

            foreach (var item in partList)
            {
                samePart =
                (item.tail == num.tail) && (item.head == num.head) &&
                (item.hand == num.hand) && (item.finger == num.finger) &&
                (item.foreArm == num.foreArm) && (item.upperArm == num.upperArm) &&
                (item.body == num.body) && (item.leg0 == num.leg0) &&
                (item.leg1 == num.leg1) && (item.leg2 == num.leg2);
            }

            if (samePart == false)
            {
                partList.Add(num);
                //캡춰 이미지.
                int count = (partList.Count == 0) ? 33 : partList.Count + 33;
                partsChange.CaptureImage(count);
            }
            else Debug.Log("리스트에 같은 몬스터가 존재합니다.!");
        }

        public void SaveCSV()
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists) Directory.CreateDirectory(path);

            //List to CSV (확인안해봄.. 나중에 천천히)
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("ID,Tail,Hand,Finger,ForeArm,UpperArm,Head,Body,0_Leg,1_Leg,2_Leg");

            for (int i = 0; i < partList.Count; i++)
            {
                int[] app = { partList[i].tail,     partList[i].hand, 
                              partList[i].finger,   partList[i].foreArm, 
                              partList[i].upperArm, partList[i].head,
                              partList[i].body,     partList[i].leg0,
                              partList[i].leg1,     partList[i].leg2        };

                string appendLine = (i + 33).ToString() + ",";

                for (int a = 0; a < app.Length; a++)
                {
                    appendLine += app[a].ToString();
                    if (a != app.Length - 1) appendLine += ",";
                }
                sb.AppendLine(appendLine);
            }

            Debug.Log(sb.ToString());
            File.WriteAllText(path + csvFile, sb.ToString());
        }

        public void LoadCSV()
        {
            string filePath = path + csvFile;

            //파일이 있는지 없는지 확인
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists) return;

            //리스트 목록 클리어
            partList.Clear();

            //파일이 있을경우 리스트 갱신
            //파일 읽어오기.
            string[] lines = File.ReadAllLines(filePath);
            IEnumerable<IEnumerable<int>> multipleQuery =
                from line in lines
                let element = line.Skip(1)
                let scores = line.Split(',')
                select (from str in scores select Convert.ToInt32(str));

            var result = multipleQuery.ToList();

            for(int r = 1; r < result.Count; r++)
            {
                var line = result[r].ToList();

                if (line != null)
                {
                    //순서가 변경될지 모름
                    PartNumbesrs p = new PartNumbesrs();
                    p.tail = line[1];
                    p.hand = line[2];
                    p.finger = line[3];
                    p.foreArm = line[4];
                    p.upperArm = line[5];
                    p.head = line[6];
                    p.body = line[7];
                    p.leg0 = line[8];
                    p.leg1 = line[9];
                    p.leg2 = line[10];
                    partList.Add(p);
                }
            }
        }
    }
}
