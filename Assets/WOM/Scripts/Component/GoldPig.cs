using System.Collections;
using UnityEngine;

public class GoldPig : MonoBehaviour
{

    public Transform pig;
    float speed;
    public float[] speedRange;
    public float[] yRangeRange;


    float GetRandomRange(float range)
    {
        return Random.Range(0, range);
    }

    void OnEnable()
    {
        StartCoroutine(PigWaveAnim());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator PigWaveAnim()
    {
        var randomRangeY = Random.Range(yRangeRange[0], yRangeRange[1]);
        speed = Random.Range(speedRange[0], speedRange[1]);
        var pigY = pig.position.y;
        while (true)
        {
            var yPos = 0 + randomRangeY * Mathf.Sin(Time.time * speed);
            var targetPos = new Vector3(pig.position.x, yPos, pig.position.z);
            pig.position = targetPos;
            yield return null;
        }

    }



}
