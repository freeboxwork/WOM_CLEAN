using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] SpriteLibraryChanged spritesController;
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
        public PartNumbesrs partNum;
        private Dictionary<string, int> parts = new Dictionary<string, int>();

        void Start()
        {
            save = GetComponent<SavePartsList>();

            anim.Play("Idle");
            InitializedPartDictionary();
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

        public void ChangedPartDictionaryValue()
        {
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
    }
}