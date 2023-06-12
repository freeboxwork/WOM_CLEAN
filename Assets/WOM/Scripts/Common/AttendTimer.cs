using UnityEngine;

public class AttendTimer : MonoBehaviour
{
    public string attendTestDateValue = "2023-06-12";
    const string LAST_ATTENDANCE_DATE_KEY = "last_attendance_date";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool HasLastAttendanceDate()
    {
        return PlayerPrefs.HasKey(LAST_ATTENDANCE_DATE_KEY);
    }
}
