using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class outputData : MonoBehaviour
{
    private HouseManager housemanager = new HouseManager();
    private List<string[]> rowData = new List<string[]>();
    public int infected_output;
    public int recovered_output;
    public int detected_output;
    public int death_output;
    // Start is called before the first frame update
    void Start()
    {
        //write();
    }
    void write(){
        string[] temp = new string[4];
        temp[0] = "Infected Number";
        temp[1] = "Recovered Number";
        temp[2] = "Detected Number";
        temp[3] = "Death Number";
        rowData.Add(temp);

        while(true){
            infected_output = housemanager.GetInfected();
            recovered_output = housemanager.GetRecovered();
            detected_output = housemanager.GetDetected();
            death_output = housemanager.GetToll();
            string[] rowDataIteration = new string[4];
            rowDataIteration[0] = infected_output.ToString();
            rowDataIteration[1] = recovered_output.ToString();
            rowDataIteration[2] = detected_output.ToString();
            rowDataIteration[3] = death_output.ToString();
            rowData.Add(rowDataIteration);
        }

        using(var w = new StreamWriter("temp.csv")){
            foreach(string[] stringarr in rowData){
                string first = stringarr[0];
                string second = stringarr[1];
                string third = stringarr[2];
                string fourth = stringarr[3];
                string line = string.Format("{0},{1},{2},{3}\n", first, second, third, fourth);
                w.WriteLine(line);
                w.Flush();
            }


        }
       
        //foreach(string[] str in rowData){
        //    string temp = str[0] + "," +str[1] + "," + str[2] + "," + str[3] + "\n";
        //    File.AppendAllText(FileName, temp);
        // }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
