/***********************************************************************
/*      Copyright Niugnep Software, LLC 2014 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GraphMaster {
  /*! \brief Class used to draw in the native Unity GUI enviornment.
   *
   *  It is best to use the wizard to create this class.
   *  The wizard will add a Game Object with this component
   *  already installed and setup with defaults.
   * 
   */
  [ExecuteInEditMode]
  public class UGuiGraph : Graph {
    [SerializeField]
    public int fontSize = 16;
    [SerializeField]
    public Font AxisLabelDynamicFont;

    public delegate void NguiAxisLabelOverrideCallback(Graph.Axis asix, Text pLabel, float val);
    public NguiAxisLabelOverrideCallback AxisLabelCallback {
      get;
      set;
    }

    public delegate void NguiDataLabelOverrideCallback(Text pLabel, Vector3 pValue, string text);
    public NguiDataLabelOverrideCallback DataLabelCallback {
      get;
      set;
    }

    [SerializeField]
    protected GraphMeshUGiu marginBackground;
    [SerializeField]
    protected GraphMeshUGiu plotBackground;
    [SerializeField]
    protected GraphMeshUGiu gridBackground;
    [SerializeField]
    protected GraphMeshUGiu xAxis;
    [SerializeField]
    protected GraphMeshUGiu yAxis;

    [SerializeField]
    List<Text> xLabels = new List<Text>();
    [SerializeField]
    List<Text> yLabels = new List<Text>();

    public override void setupGraphHierarchy() {
      base.setupGraphHierarchy();

      marginBackground = GraphMeshUGiu.AddToGameObject(marginBackgroundGo, MarginBackgroundColor);
      plotBackground = GraphMeshUGiu.AddToGameObject(plotAreaBackgroundGo, PlotBackgroundColor);
      gridBackground = GraphMeshUGiu.AddToGameObject(gridContainer, GridLinesColorMajor);
      xAxis = GraphMeshUGiu.AddToGameObject(xAxesGo, AxisColor);
      yAxis = GraphMeshUGiu.AddToGameObject(yAxesGo, AxisColor);
    }

    public override void breakdown(bool destroyAllContainers) {
      base.breakdown(destroyAllContainers);
    }

    protected override void marginBackgroundColorChanged() {
      marginBackground.color = MarginBackgroundColor;
    }
    protected override void plotBackgroundColorChanged() {
      plotBackground.color = PlotBackgroundColor;
    }
    protected override void axisColorChanged() {
      xAxis.color = AxisColor;
      yAxis.color = AxisColor;
    }
    protected override void axisLabelColorChanged() {
      foreach(Text t in xLabels) {
        t.color = AxisLabelColor;
      }
      foreach (Text t in yLabels) {
        t.color = AxisLabelColor;
      }
    }

    protected override void _drawPlotBackground(List<UIVertex> pVertexList) {
      plotBackground.clearVertices();
      plotBackground.addRect(plotAreaBackgroundRectTransform.rect);
      plotBackground.SetVerticesDirty();
    }

    protected override void _drawMarginBackground(List<UIVertex> pVertexList) {
      marginBackground.setRectVertices(pVertexList);
    }

    protected override void _drawAxisTick(Axis axis, int index, Vector2 point) {
      if (axis == Axis.X) {
        xAxis.addRect(new Rect(-AxesThickness / 2 + point.x, AxesThickness / 2 + 4 + point.y, AxesThickness, -AxesThickness - 8));
        xAxis.SetVerticesDirty();
      } else if (axis == Axis.Y) {
        yAxis.addRect(new Rect((-AxesThickness / 2) - 4 + point.x, AxesThickness / 2 + point.y, AxesThickness + 8, -AxesThickness));
        yAxis.SetVerticesDirty();
      }
    }
    
    protected override void _clearMajorGridLines() {
      gridBackground.clearVertices();
      gridBackground.color = GridLinesColorMajor;
      gridBackground.SetAllDirty();
    }
    protected override void _addMajorGridLine(Axis axis, int index, float r) {
      if (axis == Axis.X) {
        gridBackground.addRect(new Rect(adjustPointX(r) - (GridLinesThicknesMajor / 2f), adjustPointY(RangeY.y), GridLinesThicknesMajor, adjustPointY(RangeY.x) - adjustPointY(RangeY.y)));
      } else if (axis == Axis.Y) {
        gridBackground.addRect(new Rect(adjustPointX(RangeX.x), adjustPointY(r) + (GridLinesThicknesMajor / 2f), adjustPointX(RangeX.y) - adjustPointX(RangeX.x), -GridLinesThicknesMajor));
      }
    }

    protected override void _majorGridLinesDone() {
      gridBackground.SetVerticesDirty();
      gridBackground.SetAllDirty();
    }

    protected override void DrawAxisTicks() {
      base.DrawAxisTicks();
    }
    
    protected override void _drawAxes(Rect xAxis, Rect yAxis) {
      this.xAxis.clearVertices();
      this.xAxis.addRect(xAxis);
      this.xAxis.SetVerticesDirty();

      this.yAxis.clearVertices();
      this.yAxis.addRect(yAxis);
      this.yAxis.SetVerticesDirty();
    }

    protected override void adjustLabelCount(Graph.Axis axis, int num) {
      List<Text> labels = null;
      switch (axis) {
        case Axis.X:
          labels = xLabels;
          break;
        case Axis.Y:
          labels = yLabels;
          break;
      }

      if (labels == null) {
        return;
      }

      while(labels.Count > num) {
        Text label = labels[labels.Count - 1];
        labels.RemoveAt(labels.Count - 1);
#if UNITY_EDITOR
        if (Application.isEditor && !EditorApplication.isPlaying) {
          DestroyImmediate(label.gameObject);
        } else {
          Destroy(label.gameObject);
        }
#else
        Destroy(label.gameObject);
#endif
      }

      while (labels.Count < num) {
        GameObject go = Utils.AddGameObject(axesLabelContainerGo.gameObject, 0, "Tick Label " + axis.ToString() + ": " + (labels.Count - 1), false);
        Text label = go.AddComponent<Text>();
        labels.Add(label);
      }
    }
    protected override void AddAxisLabel(Graph.Axis axis, int index, Vector3 pPosition, float val) {
      List<Text> labels = null;
      switch (axis) {
        case Axis.X:
          labels = xLabels;
          break;
        case Axis.Y:
          labels = yLabels;
          break;
      }

      if (labels == null || Utils.checkForNanOrInf(pPosition)) {
        return;
      }

      Text pLabel = labels[index];
      RectTransform pRectTransform = pLabel.GetComponent<RectTransform>();

      pLabel.font = AxisLabelDynamicFont;
      pLabel.fontSize = fontSize;
      pLabel.color = AxisLabelColor;
      pLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
      pLabel.verticalOverflow = VerticalWrapMode.Overflow;

      if (axis == Graph.Axis.X) {
        pPosition.y -= 10;
        pLabel.alignment = TextAnchor.MiddleCenter;
      } else if (axis == Graph.Axis.Y) {
        pRectTransform.sizeDelta = new Vector2(100, 100);
        pPosition.x -= 55;
        pLabel.alignment = TextAnchor.MiddleRight;
      }
      pLabel.text = val.ToString();

      pLabel.transform.localPosition = pPosition;

      if (AxisLabelCallback != null) {
        AxisLabelCallback(axis, pLabel, val);
      }
    }

#if UNITY_EDITOR

    /// <summary>
    /// Draw a visible pink outline for the clipped area.
    /// </summary>

    void OnDrawGizmos() {
      GameObject go = UnityEditor.Selection.activeGameObject;
      bool selected = (go != null) && (go.GetComponent<Graph>() == this);

      Transform t = transform;

      if (t != null) {
        Gizmos.matrix = t.localToWorldMatrix;

        if (selected)
          Gizmos.color = new Color(0f, 0.5f, 0.5f);
        else
          Gizmos.color = new Color(0.2f, 0.0f, 0.3f);

        Gizmos.DrawWireCube(rectTransform.rect.center, rectTransform.rect.size);
      }
    }
#endif
  }
}