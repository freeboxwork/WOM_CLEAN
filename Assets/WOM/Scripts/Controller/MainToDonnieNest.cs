using UnityEngine;
using UnityEngine.UI;



public class MainToDonnieNest : MonoBehaviour
{

    public Button btnEmail;


    void Start()
    {
        AddBtnEvnet();
    }

    void AddBtnEvnet()
    {
        btnEmail.onClick.AddListener(EnableMailWindow);
    }

    public void EnableMailWindow()
    {
        string Emailadd = "donnie127834@gmail.com";
        string Emailname = System.Uri.EscapeDataString("버그 리포트 / 문의사항");

        string body = $"내용을 작성해주세요 : \n\n\n\nDevice Model : {SystemInfo.deviceModel} \nDevice OS : {SystemInfo.operatingSystem} \n\n";
        string Emailbody = System.Uri.EscapeDataString
            (
             body
            );

        string mailtoUrl = string.Format("mailto:{0}?subject={1}&body={2}", Emailadd, Emailname, Emailbody);
        //Application.OpenURL("mailto:" + Emailadd + "?subject=" + Emailname + "&body=" + Emailbody);
        Application.OpenURL(mailtoUrl);
    }

    // [System.Obsolete]
    // private string EscapeURL(string url)
    // {
    //     return WWW.EscapeURL(url).Replace("+", "%20");
    // }
}
