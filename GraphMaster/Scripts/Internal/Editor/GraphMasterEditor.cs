/******************************//*****************************************
/*      Copyright Niugnep Software, LLC 2013 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace GraphMaster {
  public class GraphMasterEditor : Editor {

    protected virtual void postAxis() {

    }

    public override void OnInspectorGUI() {
      GUIStyle titlestyle = new GUIStyle(GUI.skin.label);
      titlestyle.alignment = TextAnchor.MiddleCenter;
      titlestyle.fontSize = 12;
      titlestyle.fontStyle = FontStyle.Bold;

      bool b = false;
      float f = 0.0f;
      Vector2 vec2;
      Vector4 vec4;
      int i = 0;
      Graph pGraph = (Graph)target;

      Utils.DrawSeparator();
      // Colors
      EditorGUILayout.LabelField("Graph", titlestyle);
      Color c = EditorGUILayout.ColorField("Plot Background Color", pGraph.PlotBackgroundColor);
      if (c != pGraph.PlotBackgroundColor)
        UndoableAction<Graph>(gr => gr.PlotBackgroundColor = c);
      c = EditorGUILayout.ColorField("Margin Background Color", pGraph.MarginBackgroundColor);
      if (c != pGraph.MarginBackgroundColor)
        UndoableAction<Graph>(gr => gr.MarginBackgroundColor = c);

      vec4 = EditorGUILayout.Vector4Field("Margins", pGraph.Margin);
      vec4.w = Mathf.Max(0, vec4.w);
      vec4.x = Mathf.Max(0, vec4.x);
      vec4.y = Mathf.Max(0, vec4.y);
      vec4.z = Mathf.Max(0, vec4.z);
      if (vec4 != pGraph.Margin)
        UndoableAction<Graph>(gr => gr.Margin = vec4);

      EditorGUILayout.LabelField("Range");
      EditorGUILayout.BeginHorizontal();
      f = EditorGUILayout.FloatField("  X min", pGraph.RangeX.x);
      if (f != pGraph.RangeX.x)
        UndoableAction<Graph>(gr => pGraph.setRangeX(f, pGraph.RangeX.y));

      f = EditorGUILayout.FloatField("X max", pGraph.RangeX.y);
      if (f != pGraph.RangeX.y)
        UndoableAction<Graph>(gr => pGraph.setRangeX(pGraph.RangeX.x, f));
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      f = EditorGUILayout.FloatField("  Y Min", pGraph.RangeY.x);
      if (f != pGraph.RangeY.x)
        UndoableAction<Graph>(gr => pGraph.setRangeY(f, pGraph.RangeY.y));

      f = EditorGUILayout.FloatField("Y Max", pGraph.RangeY.y);
      if (f != pGraph.RangeY.y)
        UndoableAction<Graph>(gr => pGraph.setRangeY(pGraph.RangeY.x, f));
      EditorGUILayout.EndHorizontal();

      Utils.DrawSeparator();

      // Axis
      EditorGUILayout.LabelField("Axis", titlestyle);
      
      Graph.TickStyle tickStyle;
      f = Mathf.Max(0, EditorGUILayout.FloatField("Axis Thickness", pGraph.AxesThickness));
      if (f != pGraph.AxesThickness)
        UndoableAction<Graph>(gr => gr.AxesThickness = f);
      vec2 = EditorGUILayout.Vector2Field("Axis Draw At", pGraph.AxesDrawAt);
      if (vec2 != pGraph.AxesDrawAt)
        UndoableAction<Graph>(gr => gr.AxesDrawAt = vec2);
      c = EditorGUILayout.ColorField("Axis Line Color", pGraph.AxisColor);
      if (c != pGraph.AxisColor)
        UndoableAction<Graph>(gr => gr.AxisColor = c);
      c = EditorGUILayout.ColorField("Axis Label Color", pGraph.AxisLabelColor);
      if (c != pGraph.AxisLabelColor)
        UndoableAction<Graph>(gr => gr.AxisLabelColor = c);
      tickStyle = (Graph.TickStyle)EditorGUILayout.EnumPopup("X Axis Tick Style", pGraph.XTickStyle);
      if (tickStyle != pGraph.XTickStyle)
        UndoableAction<Graph>(gr => gr.XTickStyle = tickStyle);
      tickStyle = (Graph.TickStyle)EditorGUILayout.EnumPopup("Y Axis Tick Style", pGraph.YTickStyle);
      if (tickStyle != pGraph.YTickStyle)
        UndoableAction<Graph>(gr => gr.YTickStyle = tickStyle);

      EditorGUILayout.LabelField("Tick Count");
      EditorGUILayout.BeginHorizontal();
      i = Mathf.Max(0, EditorGUILayout.IntField("  X Axis", pGraph.XNumberOfTicks));
      if (i != pGraph.XNumberOfTicks)
        UndoableAction<Graph>(gr => gr.XNumberOfTicks = i);
      i = Mathf.Max(0, EditorGUILayout.IntField("  Y Axis", pGraph.YNumberOfTicks));
      if (i != pGraph.YNumberOfTicks)
        UndoableAction<Graph>(gr => gr.YNumberOfTicks = i);
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.LabelField("Draw Labels At Ticks");
      EditorGUILayout.BeginHorizontal();
      b = EditorGUILayout.Toggle("  X Axis", pGraph.DrawXLabel);
      if (b != pGraph.DrawXLabel)
        UndoableAction<Graph>(gr => gr.DrawXLabel = b);
      b = EditorGUILayout.Toggle("  Y Axis", pGraph.DrawYLabel);
      if (b != pGraph.DrawYLabel)
        UndoableAction<Graph>(gr => gr.DrawYLabel = b);
      EditorGUILayout.EndHorizontal();

      postAxis();

      Utils.DrawSeparator();

      // Grid
      EditorGUILayout.LabelField("Grid", titlestyle);

      vec2 = EditorGUILayout.Vector2Field("Major Grid Separation (0 for no grid)", pGraph.GridLinesSeparationMajor);
      vec2.x = Mathf.Max(0, vec2.x);
      vec2.y = Mathf.Max(0, vec2.y);
      if (vec2 != pGraph.GridLinesSeparationMajor)
        UndoableAction<Graph>(gr => gr.GridLinesSeparationMajor = vec2);

      f = Mathf.Max(0, EditorGUILayout.FloatField("Major Grid Thickness", pGraph.GridLinesThicknesMajor));
      if (f != pGraph.GridLinesThicknesMajor)
        UndoableAction<Graph>(gr => gr.GridLinesThicknesMajor = f);

      c = EditorGUILayout.ColorField("Major Grid Color", pGraph.GridLinesColorMajor);
      if (c != pGraph.GridLinesColorMajor)
        UndoableAction<Graph>(gr => gr.GridLinesColorMajor = c);
      /*
      vec2 = EditorGUILayout.Vector2Field("Minor Grid Separation (0 for no grid)", pGraph.GridLinesSeparationMinor);
      if (vec2 != pGraph.GridLinesSeparationMinor)
         UndoableAction<Graph>( gr => gr.GridLinesSeparationMinor = vec2 );
      c = EditorGUILayout.ColorField("Minor Grid Color", pGraph.GridLinesColorMinor);
      if (c != pGraph.GridLinesColorMinor)
         UndoableAction<Graph>( gr => gr.GridLinesColorMinor = c );
      f = EditorGUILayout.FloatField("Minor Grid Thickness", pGraph.GridLinesThicknesMinor);
      if (f != pGraph.GridLinesThicknesMinor)
         UndoableAction<Graph>( gr => gr.GridLinesThicknesMinor = f );
      */
    }

    protected void UndoableAction<T>(System.Action<T> action) where T : Graph {
      T pGraph = (T)target;

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
        Undo.RegisterUndo(pGraph, "Inspector");
#else
      Undo.RecordObject(pGraph, "Inspector");
#endif

      action(pGraph);
      EditorUtility.SetDirty(pGraph.gameObject);
      pGraph.recalculateAndDraw();
    }
  }
}