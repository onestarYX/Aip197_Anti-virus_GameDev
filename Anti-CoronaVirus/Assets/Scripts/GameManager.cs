using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private HouseManager houseManager;

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI infectedText;
    public TextMeshProUGUI detectedText;
    public TextMeshProUGUI recoveredText;
    public TextMeshProUGUI deathText;

    private int timePassed;
    public int minute;
    public int hour;
    private int day;
    private string minStr;
    private string hourStr;

    private int population;
    private int infected;
    private int detected;
    private int recovered;
    private int death;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        minute = 0;
        hour = 6;
        minStr = "00";
        hourStr = "06";

        houseManager = GameObject.Find("Residence").GetComponent<HouseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.gameIsPaused)
        {
            return;
        }

        timePassed += 5;
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

        population = houseManager.GetPopulation();
        infected = houseManager.GetInfected();
        detected = houseManager.GetDetected();
        recovered = houseManager.GetRecovered();
        death = houseManager.GetToll();

        populationText.text = "Population: " + population;
        infectedText.text = "Infected: " + infected;
        detectedText.text = "Detected: " + detected;
        recoveredText.text = "Recovered: " + recovered;
        deathText.text = "Deaths: " + death;
    }

    public int GetCurTime()
    {
        return timePassed;
    }
}
