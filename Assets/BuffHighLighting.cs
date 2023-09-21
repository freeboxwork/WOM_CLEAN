using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BuffHighLighting : MonoBehaviour
{
    Image image;
    bool increasing = true;
    float fillSpeed = 0.5f;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        image.fillAmount = 0f;

        StartCoroutine(StartHighLighting());
    }

    IEnumerator StartHighLighting()
    {
        while (true)
        {

            if (increasing)
            {
                image.fillAmount += Time.deltaTime * fillSpeed;

                if (image.fillAmount >= 1f)
                {
                    increasing = false;
                    image.fillClockwise = false;
                }
            }
            else
            {
                image.fillAmount -= Time.deltaTime * fillSpeed;

                if (image.fillAmount <= 0f)
                {
                    increasing = true;
                    image.fillClockwise = true;
                }
            }


            
            yield return null;
        }
    }

}
