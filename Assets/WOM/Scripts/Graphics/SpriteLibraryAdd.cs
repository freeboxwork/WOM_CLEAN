using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;


namespace ProjectGraphics
{
    public class CategoryName
    {
        public static string ChangeNameIn(string n)
        {
            string changedName = "";

            switch (n)
            {
                case "Tail": changedName = "tail"; break;
                case "Hand": changedName = "hand"; break;
                case "Finger": changedName = "finger"; break;
                case "ForeArm": changedName = "foreArm"; break;
                case "UpperArm": changedName = "upperArm"; break;
                case "Head": changedName = "head"; break;
                case "Body": changedName = "body"; break;
                case "0_Leg": changedName = "leg_0"; break;
                case "1_Leg": changedName = "leg_1"; break;
                case "2_Leg": changedName = "leg_2"; break;
            }

            return changedName;
        }

        public static string ChangeNameOut(string n)
        {
            string changedName = "";

            switch (n)
            {
                case "tail": changedName = "Tail"; break;
                case "hand": changedName = "Hand"; break;
                case "finger": changedName = "Finger"; break;
                case "foreArm": changedName = "ForeArm"; break;
                case "upperArm": changedName = "UpperArm"; break;
                case "head": changedName = "Head"; break;
                case "body": changedName = "Body"; break;
                case "leg_0": changedName = "0_Leg"; break;
                case "leg_1": changedName = "1_Leg"; break;
                case "leg_2": changedName = "2_Leg"; break;
            }

            return changedName;
        }
    }


    public class SpriteLibraryAdd : MonoBehaviour
    {
        [SerializeField]
        SpriteLibraryAsset baseLibrary;

        [SerializeField]
        SpriteLibraryAsset[] libraryAssets;

        public void GenerateLibrary()
        {
            //Informations
            Dictionary<string, List<Sprite>> lib = new Dictionary<string, List<Sprite>>();
            IEnumerable<string> c_Names = baseLibrary.GetCategoryNames();

            foreach (var item in c_Names)
            {
                string key = item;

                List<Sprite> s = new List<Sprite>();

                for (int i = 0; i < libraryAssets.Length; i++)
                {
                    //IEnumerable<string> l_Name = libraryAssets[i].GetCategoryLabelNames(key);
                    IEnumerable<string> l_Name = 
                        libraryAssets[i].GetCategoryLabelNames(CategoryName.ChangeNameOut(key));

                    foreach (var v in l_Name)
                    {
                        //s.Add(libraryAssets[i].GetSprite(item, v));
                        s.Add(libraryAssets[i].
                            GetSprite(CategoryName.ChangeNameOut(item), v));
                    }
                }

                lib.Add(key, s);
            }

            
            //리스트 순서 바꾸기.
            foreach (var item in lib)
            {
                item.Value.Reverse();
            }
            

            foreach (var item in lib)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    int count = (item.Value.Count - i) - 1;
                    baseLibrary.AddCategoryLabel(item.Value[i], item.Key, count.ToString());
                }
            }
            
        }

        public void ClearLibrary()
        {
            Dictionary<string, List<string>> lib = new Dictionary<string, List<string>>();
            IEnumerable<string> c = baseLibrary.GetCategoryNames();

            foreach (var item in c)
            {
                IEnumerable<string> s = baseLibrary.GetCategoryLabelNames(item);
                List<string> label = new List<string>();

                foreach (var v in s)
                {
                    label.Add(v);
                }

                lib.Add(item, label);
            }

            foreach (var item in lib)
            {
                foreach (var s in item.Value)
                {
                    baseLibrary.RemoveCategoryLabel(item.Key, s, false);
                }
            }
        }

    }
}