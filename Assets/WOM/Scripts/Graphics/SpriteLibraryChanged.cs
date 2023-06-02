using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.U2D.Animation;

//실험중


namespace ProjectGraphics
{
    public class SpriteLibraryChanged : MonoBehaviour
    {
        Dictionary<string, SpriteResolver> resolver = new Dictionary<string, SpriteResolver>();

        private void Start()
        {
            SpriteResolver[] re = GetComponentsInChildren<SpriteResolver>();
            foreach (var item in re)
            {
                string name = item.GetCategory();
                resolver.Add(name, item);
            }
        }

        //모든스프라이트 교체
        public void ChangedSpriteAllImage(int number)
        {
            if(number < CountOfSprite())
            {
                foreach (var item in resolver)
                {
                    item.Value.SetCategoryAndLabel(item.Key, number.ToString());
                }
            }
        }

        //지정스프라이트 교체
        public void ChangedSpritePartImage(string key, int num)
        {
            if(resolver.ContainsKey(key) && num < CountOfSprite())
            {
                resolver[key].SetCategoryAndLabel(key, num.ToString());
            }
        }

        //스프라이트 갯수
        public int CountOfSprite()
        {
            SpriteLibrary sptiteAsset = GetComponent<SpriteLibrary>();
            IEnumerable<string> Category = sptiteAsset.spriteLibraryAsset.GetCategoryNames();
            string[] n = Category.ToArray<string>();

            //List<string> n = new List<string>();
            //foreach (var item in Category) n.Add(item);

            IEnumerable<string> label = sptiteAsset.spriteLibraryAsset.GetCategoryLabelNames(n[0]);
            return label.Count<string>();
            /*
            int count = 0;
            foreach (var item in label) count++;
            Debug.Log(count);
            return count;
            */
        }
    }
}