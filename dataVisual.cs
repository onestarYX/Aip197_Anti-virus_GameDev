using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataVisual: MonoBehaviour
{
    GraphMaster.UGuiGraph graph;
    GraphMaster.UGuiDataSeriesXy series1;

    void Awake(){
        graph = gameObject.GetComponent<GraphMaster.UGuiGraph>();

        graph.setRanges(0,60,0,3000);

        List<Vector2> data = new List<Vector2>();
        data.Add(new Vector2(0,0));
        data.Add(new Vector2(10,100));
        data.Add(new Vector2(20,350));
        data.Add(new Vector2(40,600));
        data.Add(new Vector2(50,2900));



        series1 = graph.addDataSeries<GraphMaster.UGuiDataSeriesXy>("try",Color.red);
    }
}
