using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private HouseManager houseManager;
    //private StreamWriter writePtr;
    //private FileStream StreamPtr;
    //1
    //public dataVisual dataVisualizer = new dataVisual();

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
    private string line = "Population,Infected,Detected,Recovered,Death Toll,Time Elapsed,Infected Rate,Recovered Rate,Death Rate\n";

    private int infected_rate = 0;
    private int detected_rate = 0;
    private int death_rate = 0;
    private int recover_rate = 0;
    private int population = 0;
    private int infected = 0;
    private int detected = 0;
    private int recovered = 0;
    private int death = 0;
    private int previousPopulation = 0;
    private int previousInfected = 0;
    private int previousDetected = 0;
    private int previousRecovered = 0;
    private int previousDeath = 0;
    private string path = @"/Users/mittyhainan/Desktop/Anti-CoronaVirus/Assets/Scripts/temp.csv";

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        minute = 0;
        day = 0;
        hour = 6;
        minStr = "00";
        hourStr = "06";

        houseManager = GameObject.Find("Residence").GetComponent<HouseManager>();
        //writePtr = new StreamWriter(path);
        //StreamPtr = new FileStream(path,FileMode.Open,FileAccess.ReadWrite, FileShare.ReadWrite);
        //dataVisualizer.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.gameIsPaused)
        {
            return;
        }

        timePassed += 50;
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

        /*
        dataVisualizer.addInfection(infected,GetCurTime());
        dataVisualizer.addInfecti(detected,GetCurTime());
        dataVisualizer.addInfection(recovered,GetCurTime());
        dataVisualizer.addInfection(death,GetCurTime());
        */

        if(previousPopulation != population || previousInfected != infected || previousDetected != detected || previousRecovered != recovered || previousDeath != death){
            if(previousInfected != infected){
                infected_rate = GetCurTime();
                //dataVisualizer.addInfection(infected,infected_rate);
            }
            if(previousRecovered != recovered){
                recover_rate = GetCurTime();
                //dataVisualizer.addRecovered(recovered,recover_rate);
            }
            if(previousDetected != detected){
                detected_rate = GetCurTime();
                //dataVisualizer.addRecovered(detected,detected_rate);
            }
            if(previousDeath != death){
                death_rate = GetCurTime();
                //dataVisualizer.addRecovered(death,death_rate);
            }
            
            using(StreamWriter writePtr = new StreamWriter(path)){
                line = line + string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", population,
                    infected,detected,recovered,death,GetCurTime(),infected_rate,recover_rate,death_rate);
                writePtr.Write(line);
            }
            previousPopulation = population;
            previousInfected = infected;
            previousDetected = detected;
            previousRecovered = recovered;
            previousDeath = death;
            //dataVisualizer.presentGraph();
        }
    }

    public int GetCurTime()
    {
        return timePassed;
    }
}
