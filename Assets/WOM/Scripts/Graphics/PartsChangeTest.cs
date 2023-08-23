using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;


namespace ProjectGraphics
{
    [System.Serializable]
    public struct PartNumbesrs
    {
        [Range(0, 32)]
        public int tail, hand, finger, upperArm, foreArm, head, body, leg0, leg1, leg2;
    }

    [RequireComponent(typeof(SavePartsList))]
    public class PartsChangeTest : MonoBehaviour
    {
        [SerializeField] public SpriteLibraryChanged spritesController;
        [SerializeField] Animator anim;
        [SerializeField] Camera captureCam;
        SavePartsList save;



        private int num = 0;
        private int maxNum = 5;

        [Header("Captures")]
        public string capturePath;
        private string path;
        public string fileBase;
        public int upScale = 4;
        public bool BackAlpha = true;

        [Header("Parts Change"), Tooltip("0~4, 5~9, 10~14, 15~19, 20~24 = 동일한 모습에 색만 다른 일반형 , 25~29 = 메카닉 , 30,31,32 = 레이드보스")]
        public PartNumbesrs partNum; //이거 변경?
        public Dictionary<string, int> parts = new Dictionary<string, int>();

        // random make skin data
        public TextAsset monFaceRandomDataTextAsset;
        public MonFaceRandomDatas monFaceRandomDatas;

        public MonSkinTotalData monSkinTotalDatas = new MonSkinTotalData();

        public void MakeSkinData()
        {
            StartCoroutine(MakeSkinDataSet());
        }

        IEnumerator MakeSkinDataSet()
        {
            var data = monFaceRandomDatas.data;

            // get random value
            foreach (var d in data)
            {
                MonSkinDatas monSkinDatas = new MonSkinDatas();

                for (int i = 0; i < d.totalCount; i++)
                {
                    // 중복이면 다시 뽑기
                    var randData = GetRandomNumbers(d.min, d.max, 8);
                    while (monSkinDatas.randomIdxList.Contains(randData))
                    {
                        randData = GetRandomNumbers(d.min, d.max, 8);
                        yield return null;
                    }
                    monSkinDatas.randomIdxList.Add(randData);
                }

                monSkinDatas.SetMonSkinData();
                monSkinTotalDatas.data.Add(monSkinDatas);
            }


            // save json
            string json = JsonUtility.ToJson(monSkinTotalDatas);
            File.WriteAllText(Application.dataPath + "/CaptureImage/MonSkinDatas.json", json);

            yield return new WaitForEndOfFrame();

            // save picture

            // for (int i = 0; i < monSkinTotalDatas.data.Count; i++)
            // {

            //     var skinData = monSkinTotalDatas.data[i].randomIdxList;


            //     for (int j = 0; j < skinData.Count; j++)
            //     {

            //         // change skin
            //         ChangeSkin(skinData[j]);

            //         // capture
            //         yield return StartCoroutine(CaptterMonImage("CaptureImage/level_" + (i + 1), "MonSkin_", j));
            //     }

            //     yield return new WaitForEndOfFrame();
            // }


        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(MakeScreenShotAll());
            }
        }

        IEnumerator MakeScreenShotAll()
        {

            for (int i = 0; i < 33; i++)
            {
                List<int> values = new List<int>() { i, i, i, i, i, i, i, i };
                ChangeSkin(values);
                yield return StartCoroutine(CaptterMonImage("CaptureImage/All", "All_", i));
            }

        }




        void ChangeSkin(List<int> values)
        {
            parts["tail"] = partNum.tail = values[0];
            parts["hand"] = partNum.hand = values[1];
            parts["finger"] = partNum.finger = values[2];
            parts["foreArm"] = partNum.foreArm = values[3];
            parts["upperArm"] = partNum.upperArm = values[4];
            parts["head"] = partNum.head = values[5];
            parts["body"] = partNum.body = values[6];
            parts["leg_0"] = partNum.leg0 = values[7];
            parts["leg_1"] = partNum.leg1 = values[7];
            parts["leg_2"] = partNum.leg2 = values[7];

            foreach (var item in parts)
            {
                spritesController.ChangedSpritePartImage(item.Key, item.Value);
            }
        }

        List<int> GetRandomNumbers(int min, int max, int count)
        {
            List<int> randomNumbers = new List<int>();
            for (int i = 0; i < count; i++)
            {
                randomNumbers.Add(UnityEngine.Random.Range(min, max));
            }
            return randomNumbers;
        }




        void Start()
        {
            save = GetComponent<SavePartsList>();

            anim.Play("Idle");
            InitializedPartDictionary();
            SetmonFaceRandomData();
        }

        void SetmonFaceRandomData()
        {
            monFaceRandomDatas = JsonUtility.FromJson<MonFaceRandomDatas>(monFaceRandomDataTextAsset.text);
        }

        void InitializedPartDictionary()
        {
            parts.Add("tail", partNum.tail);
            parts.Add("hand", partNum.hand);
            parts.Add("finger", partNum.finger);
            parts.Add("foreArm", partNum.foreArm);
            parts.Add("upperArm", partNum.upperArm);
            parts.Add("head", partNum.head);
            parts.Add("body", partNum.body);
            parts.Add("leg_0", partNum.leg0);
            parts.Add("leg_1", partNum.leg1);
            parts.Add("leg_2", partNum.leg2);
        }



        public void ChangedRandomPart(int min, int max)
        {


            //값 들어가는거.
            parts["tail"] = partNum.tail = UnityEngine.Random.Range(min, max);
            parts["hand"] = partNum.hand = UnityEngine.Random.Range(min, max);
            parts["finger"] = partNum.finger = UnityEngine.Random.Range(min, max);
            parts["foreArm"] = partNum.foreArm = UnityEngine.Random.Range(min, max);
            parts["upperArm"] = partNum.upperArm = UnityEngine.Random.Range(min, max);
            parts["head"] = partNum.head = UnityEngine.Random.Range(min, max);
            parts["body"] = partNum.body = UnityEngine.Random.Range(min, max);
            parts["leg_0"] = partNum.leg0 = UnityEngine.Random.Range(min, max);
            parts["leg_1"] = partNum.leg1 = UnityEngine.Random.Range(min, max);
            parts["leg_2"] = partNum.leg2 = UnityEngine.Random.Range(min, max);

            //부위 변환
            foreach (var item in parts)
            {
                spritesController.ChangedSpritePartImage(item.Key, item.Value);
            }

            ChangePartNumberDatas();
        }


        public void ChangedPartDictionaryValue()
        {
            //값 들어가는거.
            parts["tail"] = partNum.tail;
            parts["hand"] = partNum.hand;
            parts["finger"] = partNum.finger;
            parts["foreArm"] = partNum.foreArm;
            parts["upperArm"] = partNum.upperArm;
            parts["head"] = partNum.head;
            parts["body"] = partNum.body;
            parts["leg_0"] = partNum.leg0;
            parts["leg_1"] = partNum.leg1;
            parts["leg_2"] = partNum.leg2;

            //부위 변환
            foreach (var item in parts)
            {
                spritesController.ChangedSpritePartImage(item.Key, item.Value);
            }

            ChangePartNumberDatas();
        }

        void ChangePartNumberDatas()
        {
            partNum.tail = parts["tail"];
            partNum.hand = parts["hand"];
            partNum.finger = parts["finger"];
            partNum.foreArm = parts["foreArm"];
            partNum.upperArm = parts["upperArm"];
            partNum.head = parts["head"];
            partNum.body = parts["body"];
            partNum.leg0 = parts["leg_0"];
            partNum.leg1 = parts["leg_1"];
            partNum.leg2 = parts["leg_2"];
        }

        #region CSV 파일 저장 관련
        public void SaveListMonsterPartNumbers()
        {
            save.AddPartList(partNum);
        }

        public void SaveCSVFileMonsterParts()
        {
            save.SaveCSV();
        }
        #endregion

        public void InitializedSpriteImage()
        {
            num = 0;

            maxNum = spritesController.CountOfSprite();
            save.LoadCSV();

            spritesController.ChangedSpriteAllImage(0);
        }

        public void UpdateSpriteImage()
        {
            num = (num < maxNum - 1) ? num + 1 : 0;
            spritesController.ChangedSpriteAllImage(num);
        }

        public void CaptureImage(int index)
        {
            //데이터 패스 설정
            path = Application.dataPath + capturePath;
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists) Directory.CreateDirectory(path);

            string file = string.Format("{0}_{1:D3}.png", fileBase, index);
            string name = path + file;
            Debug.Log(name);

            //스크린샷 프로세스
            int width = captureCam.pixelWidth * upScale;
            int height = captureCam.pixelHeight * upScale;

            RenderTexture rt = new RenderTexture(width, height, 32);

            Texture2D screeShot = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
            CameraClearFlags clearFlag = captureCam.clearFlags;

            if (BackAlpha)
            {
                captureCam.clearFlags = CameraClearFlags.SolidColor;
                captureCam.backgroundColor = new Color(0, 0, 0, 0);
            }

            //렌더링
            captureCam.targetTexture = rt;

            captureCam.Render();
            RenderTexture.active = rt;
            screeShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screeShot.Apply();

            captureCam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);
            captureCam.clearFlags = clearFlag;

            //저장
            byte[] imageByte = screeShot.EncodeToPNG();
            File.WriteAllBytes(name, imageByte);
        }

        IEnumerator CaptterMonImage(string filePaht, string fileName, int index)
        {
            //데이터 패스 설정
            path = Application.dataPath + "/" + filePaht + "/";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists) Directory.CreateDirectory(path);

            string file = $"{fileName}_{index}";
            string name = path + file + ".png";
            Debug.Log(name);

            //스크린샷 프로세스
            int width = captureCam.pixelWidth * upScale;
            int height = captureCam.pixelHeight * upScale;

            RenderTexture rt = new RenderTexture(width, height, 32);

            Texture2D screeShot = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
            CameraClearFlags clearFlag = captureCam.clearFlags;

            if (BackAlpha)
            {
                captureCam.clearFlags = CameraClearFlags.SolidColor;
                captureCam.backgroundColor = new Color(0, 0, 0, 0);
            }

            //렌더링
            captureCam.targetTexture = rt;

            captureCam.Render();
            RenderTexture.active = rt;
            screeShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screeShot.Apply();

            captureCam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(rt);
            captureCam.clearFlags = clearFlag;

            //저장
            byte[] imageByte = screeShot.EncodeToPNG();
            File.WriteAllBytes(name, imageByte);
            yield return null;
        }
    }




}

[System.Serializable]
public class MonSkinTotalData
{
    public List<MonSkinDatas> data = new List<MonSkinDatas>();
}

[System.Serializable]
public class MonSkinDatas
{
    public List<List<int>> randomIdxList = new List<List<int>>();
    public List<MonSkinData> monSkinData = new List<MonSkinData>();

    public void SetMonSkinData()
    {
        int i = 0;
        foreach (var item in randomIdxList)
        {

            MonSkinData data = new MonSkinData();
            data.id = i;
            data.tail = item[0];
            data.hand = item[1];
            data.finger = item[2];
            data.foreArm = item[3];
            data.upperArm = item[4];
            data.head = item[5];
            data.body = item[6];
            data.leg0 = item[7];
            data.leg1 = item[7];
            data.leg2 = item[7];
            monSkinData.Add(data);
            i++;
        }
    }
}

[System.Serializable]
public class MonSkinData
{
    /*
    public string tailKey = "tail";
    public string handKey = "hand";
    public string fingerKey = "finger";
    public string foreArmKey = "foreArm";
    public string upperArmKey = "upperArm";
    public string headKey = "head";
    public string bodyKey = "body";
    public string leg0Key = "leg_0";
    public string leg1Key = "leg_1";
    public string leg2Key = "leg_2";
    */

    public int id;

    public int tail;
    public int hand;
    public int finger;
    public int foreArm;
    public int upperArm;
    public int head;
    public int body;
    public int leg0;
    public int leg1;
    public int leg2;

}

[System.Serializable]
public class MonFaceRandomDatas
{
    public List<MonFaceRandomData> data = new List<MonFaceRandomData>();
}

[System.Serializable]
public class MonFaceRandomData
{
    public int id;
    public int level;
    public int min;
    public int max;
    public int totalCount;

}