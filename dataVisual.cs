using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataVisual: MonoBehaviour
{
    GraphMaster.UGuiGraph graph;
    GraphMaster.UGuiDataSeriesXy seriesInfected;
    GraphMaster.UGuiDataSeriesXy seriesDetected;
    GraphMaster.UGuiDataSeriesXy seriesRecovered;
    GraphMaster.UGuiDataSeriesXy seriesDeath;
    List<Vector2> infected = new List<Vector2>();
    List<Vector2> detected = new List<Vector2>();
    List<Vector2> recovered = new List<Vector2>();
    List<Vector2> death = new List<Vector2>();

    public void Awake(){
        this.graph = gameObject.GetComponent<GraphMaster.UGuiGraph>();

        this.graph.setRanges(0,30,0,50000);

        infected.Add(new Vector2(0,0));
        detected.Add(new Vector2(0,0));
        recovered.Add(new Vector2(0,0));
        death.Add(new Vector2(0,0));

        this.seriesInfected = this.graph.addDataSeries<GraphMaster.UGuiDataSeriesXy>("infected",Color.yellow);
        this.seriesDetected = this.graph.addDataSeries<GraphMaster.UGuiDataSeriesXy>("detected",Color.red);
        this.seriesRecovered = this.graph.addDataSeries<GraphMaster.UGuiDataSeriesXy>("recovered",Color.blue);
        this.seriesDeath = this.graph.addDataSeries<GraphMaster.UGuiDataSeriesXy>("death",Color.black);
        this.seriesInfected.Data = infected;
        this.seriesDetected.Data = detected;
        this.seriesRecovered.Data = recovered;
        this.seriesDeath.Data = death;
    }

    public void addInfection(int a, int b){
        infected.Add(new Vector2(a,b));
    }

    public void addDetected(int a, int b){
        detected.Add(new Vector2(a,b));
    }

    public void addRecovered(int a, int b){
        recovered.Add(new Vector2(a,b));
    }

    public void addDeath(int a, int b){
        death.Add(new Vector2(a,b));
    }

    public void presentGraph(){
        this.seriesInfected.Data = infected;
        this.seriesDetected.Data = detected;
        this.seriesRecovered.Data = recovered;
        this.seriesDeath.Data = death;
    }
}
