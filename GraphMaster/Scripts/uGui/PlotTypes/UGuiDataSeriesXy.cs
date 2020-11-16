using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster {
  public class UGuiDataSeriesXy : DataSeriesXy {

    protected GraphMeshUGiu plotMesh;
    protected GraphMeshUGiu markersMesh;

    protected override void Start() {
      base.Start();

      plotMesh = GraphMeshUGiu.AddToGameObject(plotGo, PlotColor);
      markersMesh = GraphMeshUGiu.AddToGameObject(markersGo, PlotColor);
      DrawSeries();
    }

    public override void DrawSeries() {
      if (plotMesh == null || graph == null || data == null)
        return;

      base.DrawSeries();
      plotMesh.clearVertices();
      markersMesh.clearVertices();

      Vector2 zero = graph.adjustPoint(Vector2.zero, true);

      for (int i = 0; i < data.Count; ++i) {
        Vector2 pDataPoint = graph.adjustPoint(data[i], true);

        if (MarkersStyle != MarkerStyle.None)
          drawMarker(pDataPoint, i);

        if (plotStyle == Style.Line && i != 0) {
          Vector2 pPrevDataPoint = graph.adjustPoint(data[i - 1], true);
          plotMesh.addLineSegment(pPrevDataPoint, pDataPoint, PlotThickness);
        } else if (plotStyle == Style.Bar) {
          plotMesh.addRect(new Rect(pDataPoint.x - PlotThickness / 2, zero.y, PlotThickness, pDataPoint.y - zero.y));
        }
      }

      plotMesh.SetVerticesDirty();
    }

    protected void drawMarker(Vector2 pDataPoint, int dataPointIndex) {
      switch (MarkersStyle) {
        case MarkerStyle.Box:
          markersMesh.addRect(new Rect(new Vector2(pDataPoint.x - markerWeight / 2, pDataPoint.y - markerWeight / 2), new Vector2(markerWeight, markerWeight)));
          break;
        case MarkerStyle.Triangle:
          List<UIVertex> vertices = new List<UIVertex>(4);

          UIVertex vert = new UIVertex();
          vert.position = new Vector3(pDataPoint.x, pDataPoint.y + markerWeight / 2, 0);
          vertices.Add(vert);

          vert = new UIVertex();
          vert.position = new Vector3(markerWeight / 2 + pDataPoint.x, -markerWeight / 2 + pDataPoint.y, 0);
          vertices.Add(vert);

          vert = new UIVertex();
          vert.position = new Vector3(-markerWeight / 2 + pDataPoint.x, -markerWeight / 2 + pDataPoint.y, 0);
          vertices.Add(vert);

          markersMesh.addTriangleVertices(vertices);
          break;
      }
    }
  }
}
