using UnityEngine;

public class IconScale : MonoBehaviour
{
    public float powValue = 1;
    public RectTransform rectTransform;
    [Header("x 아이콘 크기 배수"), Range(0.5f, 2)]
    public float multiscale = 1;
    [Range(0, 3)]
    public float timimg = 1;
    private float pingpong = 0;
    // Update is called once per frame
    void Update()
    {
        var time = Mathf.Pow(Time.time, powValue);
        pingpong = (Mathf.PingPong((Time.time * timimg), 1) + 1) * multiscale;
        rectTransform.localScale = new Vector3(pingpong, pingpong, pingpong);
    }
}
