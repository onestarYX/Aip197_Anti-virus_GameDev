using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    private int timePassed;
    public int minute;
    public int hour;
    private int day;
    private string minStr;
    private string hourStr;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        minute = 0;
        hour = 6;
        minStr = "00";
        hourStr = "06";
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += 2;
        if(timePassed % 60 == 0)
        {
            minute++;
            if(minute % 60 == 0)
            {
                minute = 0;
                hour++;
                if (hour % 24 == 0)
                {
                    hour = 0;
                    day++;
                }
            }
            
            if(minute < 10)
            {
                minStr = "0" + minute;
            }
            else
            {
                minStr = minute.ToString();
            }

            if(hour < 10)
            {
                hourStr = "0" + hour;
            }
            else
            {
                hourStr = hour.ToString();
            }
        }

        dayText.text = "Day " + day;
        timeText.text = "Time: " + hourStr + ":" + minStr;

    }
}
