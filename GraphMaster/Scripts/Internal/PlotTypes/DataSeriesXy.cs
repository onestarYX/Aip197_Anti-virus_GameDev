/***********************************************************************
/*      Copyright Niugnep Software, LLC 2013 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace GraphMaster {
  /*! \brief Line and Bar plot types.
   *
   *  Use Graph.addDataSeries<DataSeriesXy>(...) to add this plot type
   *  to a graph.
   */
  public class DataSeriesXy : DataSeries {

    protected RectTransform plotAreaRectTransform;

    protected List<Vector2> data = new List<Vector2>();
    protected float plotThickness = 2;
    protected MarkerStyle marerkStyle = MarkerStyle.None;
    protected Style plotStyle = Style.Line;
    protected float markerWeight = 10;
    protected Color markerColor;
    protected List<dataLabelStruct> dataLabels = new List<dataLabelStruct>();

    protected struct dataLabelStruct {
      public float xValue;
      public GameObject gameObj;
      public string text;
    }

    /** The plot's marker color. 
      *  If markers are set this color is used to draw them.  It's default is the plot color
      *  but can be changed here.
      */
    public Color MarkerColor {
      get {
        return markerColor;
      }
      set {
        markerColor = value;
        DrawSeries();
      }
    }

    public enum Style {
      Line,
      Bar
    }

    public enum MarkerStyle {
      None,
      Box,
      Triangle
    }

    /** \brief  The plot's data. 
      * 
      *  This is the data that will be used to draw the graph.
      *  Any values outside of the parent graph's range will be clipped.
      */
    public List<Vector2> Data {
      get { return data; }
      set {
        data = value;
        DrawSeries();
      }
    }

    /** \brief  The plot's thickness. 
      * 
      *  This applies to both line and bar types.
      *  Bar graphs will need a larger value to look good.
      */
    public float PlotThickness {
      get { return plotThickness; }
      set {
        if (plotThickness == value)
          return;

        plotThickness = value;
        DrawSeries();
      }
    }

    /** \brief  The plot's marker style.
      * 
      *  A marker will be drawn in the selected style at every data point.
      */
    public MarkerStyle MarkersStyle {
      get { return marerkStyle; }
      set {
        if (marerkStyle == value)
          return;

        marerkStyle = value;
        DrawSeries();
      }
    }

    /** \brief The plot's style.
      * 
      *  Change between bar and line graphs here.
      */
    public Style PlotStyle {
      get { return plotStyle; }
      set {
        if (plotStyle == value)
          return;

        plotStyle = value;
        DrawSeries();
      }
    }

    public float MarkerWeight {
      get { return markerWeight; }
      set {
        if (markerWeight == value)
          return;

        markerWeight = value;
        DrawSeries();
      }
    }

    public override void setup(Graph pGraph) {
      base.setup(pGraph);
      markerColor = PlotColor;
    }

    /*
    public int addDataLabel(float xValue, string label = null) {
      GameObject pLabelGo = Utils.AddGameObject(dataLabelGo, 0, "Plot Label - " + dataLabels.Count);
      dataLabelStruct str;
      str.xValue = xValue;
      str.gameObj = pLabelGo;
      str.text = label;

      dataLabels.Add(str);
      int pos = dataLabels.Count - 1;
      drawDataLabel(dataLabels[pos]);
      return pos;
    }

    public bool removeDataLabel(int index) {
      if (index >= dataLabels.Count)
        return false;

      Destroy(dataLabels[index].gameObj);
      dataLabels.RemoveAt(index);
      return true;
    }
    */

    /*
    protected bool drawDataLabel(dataLabelStruct labelInfo) {
      if (data == null || data.Count < 1)
        return false;

      float xValue = labelInfo.xValue;
      Vector2 p0 = Vector2.zero;
      Vector2 p1 = Vector2.zero;
      bool dataPointFound = false;
      for (int index = 0; index < data.Count; index++) {
        Vector2 pDataPoint = data[index];
        p1 = pDataPoint;
        if (index == 0)
          p0 = p1;
        if (pDataPoint.x > xValue)
          break;
        dataPointFound = true;
        p0 = pDataPoint;
      }

      if (!dataPointFound)
        return false;

      //float yValue = p0.y + (p1.y - p0.y) + ((xValue - p0.x) / (p1.x - p0.x));

      //mDataLabelCallback(labelInfo.gameObj, new Vector3(xValue, yValue, 0), labelInfo.text);

      return true;
    }
    */
  }
}