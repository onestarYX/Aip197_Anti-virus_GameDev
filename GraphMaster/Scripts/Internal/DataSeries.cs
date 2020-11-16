/***********************************************************************
/*      Copyright Niugnep Software, LLC 2013 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GraphMaster {
  /*! \brief Base NGraph Data Series (Plot) class.
   *
   *  This class is used by the different GUI endpoints.  Most of the 
   *  logic for setting up and drawing a data series is handled here.
   */
  public abstract class DataSeries : MonoBehaviour {
    protected Color color;
    protected Graph graph;
    protected GameObject plotGo;
    protected GameObject markersGo;
    protected GameObject dataLabelGo;
    protected float revealTime = 0.0f;
    protected float elapsedRevealTime = 0.0f;
    private float revealPercent = 0.0f;
    protected bool finalizedReveal = false;

    protected RectTransform parentRectTransform;
    protected RectTransform rectTransform;
    protected RectTransform plotRectTransform;
    protected RectTransform markersRectTransform;
    protected RectTransform dataLabelRectTransform;

    protected RectMask2D rectMask;


    public enum RevealDirectionType {
      LeftToRight,
      BottomToTop
    }
    private RevealDirectionType revealDirection = RevealDirectionType.LeftToRight;
    public RevealDirectionType RevealDirection {
      set {
        revealDirection = value;
      }
      get {
        return revealDirection;
      }
    }

    public float Reveal {
      set {
        finalizedReveal = false;
        elapsedRevealTime = 0.0f;
        revealTime = value;
      }
    }

    protected virtual void Start() {
      rectTransform = GetComponent<RectTransform>();
      if(rectTransform == null) {
        rectTransform = gameObject.AddComponent<RectTransform>();
      }

      parentRectTransform = transform.parent.GetComponent<RectTransform>();
      graph = GetComponentInParent<Graph>();
      if (graph) {
        graph.addDataSeries(this);
      }

      rectMask = GetComponent<RectMask2D>();
      if(rectMask == null) {
        rectMask = gameObject.AddComponent<RectMask2D>();
      }

      if (plotGo == null) {
        plotGo = Utils.AddGameObject(gameObject, 0, "Plot", false);
      }
      plotRectTransform = plotGo.GetComponent<RectTransform>();

      if (markersGo == null) {
        markersGo = Utils.AddGameObject(gameObject, 0, "Markers", false);
      }
      markersRectTransform = markersGo.GetComponent<RectTransform>();

      if (dataLabelGo == null) {
        dataLabelGo = Utils.AddGameObject(gameObject, 0, "Data Labels", false);
      }
      dataLabelRectTransform = dataLabelGo.GetComponent<RectTransform>();

      resizeRects();
    }

    public void resizeRects() {
      resizeRect(rectTransform, true);
      resizeRect(plotRectTransform, false);
      resizeRect(markersRectTransform, false);
      resizeRect(dataLabelRectTransform, false);
    }

    private void resizeRect(RectTransform rt, bool obayPercent) {
      if(rt == null) {
        return;
      }
      rt.pivot = Vector2.zero;
      rt.anchorMin = Vector2.zero;
      rt.offsetMin = Vector2.zero;
      rt.anchorMax = Vector2.zero;
      rt.offsetMax = Vector2.zero;
      
      if (parentRectTransform != null) {
        if (obayPercent && !float.IsNaN(revealPercent)) {
          switch (revealDirection) {
            case RevealDirectionType.LeftToRight:
              rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentRectTransform.rect.height);
              rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentRectTransform.rect.width * revealPercent);
              break;
            case RevealDirectionType.BottomToTop:
              rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentRectTransform.rect.height * revealPercent);
              rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentRectTransform.rect.width);
              break;
          }
        } else {
          rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentRectTransform.rect.height);
          rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentRectTransform.rect.width);
        }
      }
    }

    /** \brief  The plot's color. 
      * 
      *  Used to reference or change the plot color.
      */
    public Color PlotColor {
      get { return color; }
      set { color = value; redraw(); }
    }

    public virtual void Update() {
      elapsedRevealTime += Time.unscaledDeltaTime;
      if (elapsedRevealTime >= revealTime) {
        if (finalizedReveal)
          return;
        finalizedReveal = true;
        resizeRect(rectTransform, false);
      }

      revealPercent = elapsedRevealTime / revealTime;
      if (revealPercent > 1)
        revealPercent = 1;
      
      resizeRect(rectTransform, true);
    }

    private void redraw() {
      DrawSeries();
    }

    protected Vector4 mClipping = Vector4.zero;
    public virtual void DrawSeries() {
    }

    public virtual void setup(Graph pGraph) {
      graph = pGraph;
    }

    protected void emitSetupError() {
      Debug.LogError("Data Series has not been added to Graph object.  Call \"addDataSeries\" on the graph you wish to add this Data Series to.");
    }
  }
}