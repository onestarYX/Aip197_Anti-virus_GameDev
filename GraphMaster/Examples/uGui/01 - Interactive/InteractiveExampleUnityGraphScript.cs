/***********************************************************************
/*      Copyright Niugnep Software, LLC 2014 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// This MonoBehavior expects to be attached to the same game object
// that a UINgraph is attached to.  This object is called "graph" if
// you created it with the NGraph Graph Creation Wizard.
namespace GraphMaster {
  public class InteractiveExampleUnityGraphScript : MonoBehaviour {
    public Slider Plot1ThicknessSlider;
    public Slider Plot1MarkerWeightSlider;
    public Slider Plot1RevealTimeSlider;
    public Toggle Plot1Enabled;
    public Button Plot1Style;
    public Button Plot1MarkerStyle;

    public Slider Plot2MarkerWeightSlider;
    public Slider Plot2ThicknessSlider;
    public Slider Plot2RevealTimeSlider;
    public Toggle Plot2Enabled;
    public Button Plot2Style;
    public Button Plot2MarkerStyle;

    public Slider AxisThicknessSlider;
    public Toggle AxisLabelsXEnabled;
    public Toggle AxisLabelsYEnabled;

    UGuiGraph graph;
    UGuiDataSeriesXy series1;
    UGuiDataSeriesXy series2;

    List<Vector2> data1 = new List<Vector2>();
    List<Vector2> data2 = new List<Vector2>();

    void Awake() {
      // Do not try to create run-time objects unless we are in play mode
      if (!Application.isPlaying) {
        return;
      }

      // Capture the graph
      captureGraph();

      // Setup the graph
      graph.setRanges(0, 500, 0, 1000);

      // Create the data we want to plot
      data1.Add(new Vector2(-50, -100));
      data1.Add(new Vector2(0, 0));
      data1.Add(new Vector2(50, 200));
      data1.Add(new Vector2(100, 200));
      data1.Add(new Vector2(150, 350));
      data1.Add(new Vector2(200, 400));
      data1.Add(new Vector2(250, 690));
      data1.Add(new Vector2(300, 800));
      data1.Add(new Vector2(350, 620));
      data1.Add(new Vector2(400, 800));
      data1.Add(new Vector2(450, 860));
      data1.Add(new Vector2(500, 1000));
      data1.Add(new Vector2(550, 1120));

      // Add a X/Y Plot the the graph and capture the plot and color it blue
      createPlot1();

      // Create a different set of data for the second plot
      data2.Add(new Vector2(-50, -100));
      data2.Add(new Vector2(0, 250));
      data2.Add(new Vector2(50, 75));
      data2.Add(new Vector2(100, 322));
      data2.Add(new Vector2(150, 200));
      data2.Add(new Vector2(200, 360));
      data2.Add(new Vector2(250, 210));
      data2.Add(new Vector2(300, 55));
      data2.Add(new Vector2(350, 175));
      data2.Add(new Vector2(400, 322));
      data2.Add(new Vector2(450, 260));
      data2.Add(new Vector2(500, 200));
      data2.Add(new Vector2(550, -50));

      // Add a second X/Y Plot the the graph and capture the plot and color it red
      createPlot2();
    }

    // Update is called once per frame
    void Update() {
    }

    void captureGraph() {
      graph = gameObject.GetComponent<UGuiGraph>();
    }

    public void createPlot1() {
      captureGraph();
      series1 = graph.addDataSeries<UGuiDataSeriesXy>("1", Color.blue);
      series1.PlotThickness = Plot1ThicknessSlider.value;
      series1.MarkerWeight = Plot1MarkerWeightSlider.value;
      series1.Reveal = Plot1RevealTimeSlider.value;
      Plot1PlotStlyeChange();
      Plot1MarkerStlyeChange();

      // Apply our data to the plot.
      series1.Data = data1;
    }

    public void createPlot2() {
      captureGraph();
      series2 = graph.addDataSeries<UGuiDataSeriesXy>("2", Color.red);
      series2.PlotThickness = Plot1ThicknessSlider.value;
      series2.MarkerWeight = Plot1MarkerWeightSlider.value;
      series2.Reveal = Plot2RevealTimeSlider.value;
      Plot2PlotStlyeChange();
      Plot2MarkerStlyeChange();

      // Apply our data to the plot.
      series2.Data = data2;
    }

    public void ShowPlot1() {
      // Do not try to create run-time objects unless we are in play mode
      if (!Application.isPlaying) {
        return;
      }

      captureGraph();
      if (series1 != null) {
        graph.removeDataSeries(series1);
        series1 = null;
      } else {
        createPlot1();
      }
    }
    public void Plot1ThicknessChange() {
      if (series1 == null)
        return;
      series1.PlotThickness = Plot1ThicknessSlider.value;
    }
    public void Plot1MarkerWeightChange() {
      if (series1 == null)
        return;
      series1.MarkerWeight = Plot1MarkerWeightSlider.value;
      graph.recalculateAndDraw();
    }
    public void Plot1RevealTimeChange() {
      if (series1 == null)
        return;
      series1.Reveal = Plot1RevealTimeSlider.value;
    }

    public void Plot1PlotStlyeChange() {
      if (series1 == null)
        return;
      Text pText = Plot1Style.GetComponentInChildren<Text>();
      switch (pText.text) {
        case "Line": pText.text = "Bar"; break;
        case "Bar": pText.text = "Line"; break;
      }

      switch (pText.text) {
        case "Line": series1.PlotStyle = DataSeriesXy.Style.Line; series1.RevealDirection = DataSeries.RevealDirectionType.LeftToRight; break;
        case "Bar": series1.PlotStyle = DataSeriesXy.Style.Bar; series1.RevealDirection = DataSeries.RevealDirectionType.BottomToTop; break;
      }
      Plot1ThicknessChange();
    }

    public void Plot1MarkerStlyeChange() {
      if (series1 == null)
        return;

      Text pText = Plot1MarkerStyle.GetComponentInChildren<Text>();
      switch (pText.text) {
        case "None": pText.text = "Box"; break;
        case "Box": pText.text = "Triangle"; break;
        case "Triangle": pText.text = "None"; break;
      }

      switch (pText.text) {
        case "None": series1.MarkersStyle = DataSeriesXy.MarkerStyle.None; break;
        case "Box": series1.MarkersStyle = DataSeriesXy.MarkerStyle.Box; break;
        case "Triangle": series1.MarkersStyle = DataSeriesXy.MarkerStyle.Triangle; break;
      }

      captureGraph();
      graph.recalculateAndDraw();
    }

    public void ShowPlot2() {
      // Do not try to create run-time objects unless we are in play mode
      if (!Application.isPlaying) {
        return;
      }

      captureGraph();
      if (series2 != null) {
        graph.removeDataSeries(series2);
        series2 = null;
      } else {
        createPlot2();
      }
    }
    public void Plot2ThicknessChange() {
      if (series2 == null)
        return;
      series2.PlotThickness = Plot2ThicknessSlider.value;
    }
    public void Plot2MarkerWeightChange() {
      if (series2 == null)
        return;
      series2.MarkerWeight = Plot2MarkerWeightSlider.value;
    }
    public void Plot2RevealTimeChange() {
      if (series2 == null)
        return;
      series2.Reveal = Plot2RevealTimeSlider.value;
    }

    public void Plot2PlotStlyeChange() {
      if (series2 == null)
        return;
      Text pText = Plot2Style.GetComponentInChildren<Text>();
      switch (pText.text) {
        case "Line": pText.text = "Bar"; break;
        case "Bar": pText.text = "Line"; break;
      }

      switch (pText.text) {
        case "Line": series2.PlotStyle = DataSeriesXy.Style.Line; series2.RevealDirection = DataSeries.RevealDirectionType.LeftToRight; break;
        case "Bar": series2.PlotStyle = DataSeriesXy.Style.Bar; series2.RevealDirection = DataSeries.RevealDirectionType.BottomToTop; break;
      }
      Plot2ThicknessChange();
    }

    public void Plot2MarkerStlyeChange() {
      if (series2 == null)
        return;

      Text pText = Plot2MarkerStyle.GetComponentInChildren<Text>();
      switch (pText.text) {
        case "None": pText.text = "Box"; break;
        case "Box": pText.text = "Triangle"; break;
        case "Triangle": pText.text = "None"; break;
      }

      switch (pText.text) {
        case "None": series2.MarkersStyle = DataSeriesXy.MarkerStyle.None; break;
        case "Box": series2.MarkersStyle = DataSeriesXy.MarkerStyle.Box; break;
        case "Triangle": series2.MarkersStyle = DataSeriesXy.MarkerStyle.Triangle; break;
      }

      captureGraph();
      graph.recalculateAndDraw();
    }

    public void ShowXLables() {
      if (graph == null)
        return;

      captureGraph();
      graph.DrawXLabel = AxisLabelsXEnabled.isOn;
      graph.recalculateAndDraw();
    }
    public void ShowYLables() {
      if (graph == null)
        return;

      captureGraph();
      graph.DrawYLabel = AxisLabelsYEnabled.isOn;
      graph.recalculateAndDraw();
    }

    public void AxisThicknessChange() {
      if (graph == null)
        return;

      captureGraph();
      graph.AxesThickness = AxisThicknessSlider.value;
      graph.recalculateAndDraw();
    }

    public void XAxisStyleChange() {
      if (graph == null)
        return;

      captureGraph();
      /*
      switch(UIPopupList.current.value)
      {
      case "Even": mGraph.XTickStyle = NGraph.TickStyle.EvenSpace; break;
      case "Even - Low": mGraph.XTickStyle = NGraph.TickStyle.EvenSpaceLow; break;
      case "Even - High": mGraph.XTickStyle = NGraph.TickStyle.EvenSpaceHigh; break;
      case "Even - High And Low": mGraph.XTickStyle = NGraph.TickStyle.EvenSpaceLowAndHigh; break;
      }
      */
      graph.recalculateAndDraw();
    }
    public void YAxisStyleChange() {
      if (graph == null)
        return;

      captureGraph();
      /*
      switch(UIPopupList.current.value)
      {
      case "Even": mGraph.YTickStyle = NGraph.TickStyle.EvenSpace; break;
      case "Even - Low": mGraph.YTickStyle = NGraph.TickStyle.EvenSpaceLow; break;
      case "Even - High": mGraph.YTickStyle = NGraph.TickStyle.EvenSpaceHigh; break;
      case "Even - High And Low": mGraph.YTickStyle = NGraph.TickStyle.EvenSpaceLowAndHigh; break;
      }
      */
      graph.recalculateAndDraw();
    }
  }
}