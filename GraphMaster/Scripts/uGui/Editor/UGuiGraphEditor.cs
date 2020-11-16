/***********************************************************************
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
  [CustomEditor(typeof(UGuiGraph))]
  public class UGuiGraphEditor : GraphMasterEditor {
    protected override void postAxis() {
      UGuiGraph pGraph = (UGuiGraph)target;

      GUILayout.BeginHorizontal();
      Font fnt = (Font)EditorGUILayout.ObjectField(pGraph.AxisLabelDynamicFont, typeof(Font), false, GUILayout.Width(140f));
      if (fnt != pGraph.AxisLabelDynamicFont)
        UndoableAction<UGuiGraph>(gr => gr.AxisLabelDynamicFont = fnt);

      GUILayout.Label("font used by the tick labels");
      GUILayout.EndHorizontal();
    }
  }
}