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
  /*! \brief Base NGraph class.
   *
   *  This class is used by the different GUI endpoints.  Most of the 
   *  logic for setting up and drawing a graph is handled here.
   */
  [ExecuteInEditMode]
  public abstract class Graph : MonoBehaviour {
    public const float LEVEL_STEP = -0.1f;
    public const int BACKGROUND_LEVEL = 0;
    public const int AXES_LEVEL = 1;
    public const int PLOT_START_LEVEL = 4;
    public float AxesThickness = 4.0f;

    [SerializeField]
    private Vector2 axesDrawAt;
    public Vector2 AxesDrawAt {
      get { return axesDrawAt; }
      set { axesDrawAt = value; }
    }

    [SerializeField]
    private Vector4 margin = new Vector4(60, 40, 60, 40);
    public Vector4 Margin {
      get { return margin; }
      set { margin = value; recalculateAndDraw(); }
    }
    [SerializeField]
    Dictionary<string, DataSeries> dataSeriesByName = new Dictionary<string, DataSeries>();

    [SerializeField]
    private Color axisColor = Color.white;
    public Color AxisColor {
      get { return axisColor; }
      set { axisColor = value; axisColorChanged(); }
    }
    protected abstract void axisColorChanged();

    [SerializeField]
    private Color axisLabelColor = Color.white;
    public Color AxisLabelColor {
      get { return axisLabelColor; }
      set { axisLabelColor = value; axisLabelColorChanged(); }
    }
    protected abstract void axisLabelColorChanged();

    [SerializeField]
    private Color marginBackgroundColor = new Color(1, 1, 1, 0.4f);
    public Color MarginBackgroundColor {
      get { return marginBackgroundColor; }
      set { marginBackgroundColor = value; marginBackgroundColorChanged(); }
    }
    protected abstract void marginBackgroundColorChanged();

    public bool DrawXLabel = true;
    public bool DrawYLabel = true;
    public TickStyle XTickStyle = TickStyle.EvenSpaceLowAndHigh;
    public int XNumberOfTicks = 5;
    public TickStyle YTickStyle = TickStyle.EvenSpaceLowAndHigh;
    public int YNumberOfTicks = 5;

    [SerializeField]
    private Color plotBackgroundColor = new Color(1, 1, 1, 0.4f);
    public Color PlotBackgroundColor {
      get { return plotBackgroundColor; }
      set { plotBackgroundColor = value; plotBackgroundColorChanged(); }
    }
    protected abstract void plotBackgroundColorChanged();

    [SerializeField]
    protected GameObject meshesContainer;
    [SerializeField]
    protected GameObject dataLabelTopContainer;
    [SerializeField]
    protected GameObject plotAreaBackgroundGo;
    [SerializeField]
    protected RectTransform plotAreaBackgroundRectTransform;
    [SerializeField]
    protected GameObject axesLabelContainerGo;
    [SerializeField]
    protected GameObject axesGo;
    [SerializeField]
    protected RectTransform axesRectTransform;
    [SerializeField]
    protected GameObject xAxesGo;
    [SerializeField]
    protected GameObject yAxesGo;
    [SerializeField]
    protected GameObject marginBackgroundGo;
    [SerializeField]
    protected GameObject gridContainer;
    [SerializeField]
    protected GameObject plotsContainer;
    [SerializeField]
    protected RectTransform rectTransform;

    public delegate void DataSeriesDataLabelCallback(GameObject pLabelGo, Vector3 pPosition, string text);
    protected abstract void AddAxisLabel(Axis axis, int index, Vector3 pPosition, float val);

    protected GameObject DataLabelTopContainer {
      get {
        if (dataLabelTopContainer == null) {
          dataLabelTopContainer = Utils.AddGameObject(gameObject, -1, "Data Label Containers");
          newDataSeriesDataLabelTopContainerGameObject();
        }
        return dataLabelTopContainer;
      }
    }

    /** \brief  Axs labels. 
      * 
      *  Used to reference the axis being worked with.
      */
    public enum Axis {
      X,
      Y
    }

    /** \brief  Tick style for axis.
      */
    public enum TickStyle {
      EvenSpace,              /* Evenly space the tick marks. */
      EvenSpaceHigh,          /* Evenly space the tick marks, including the max value on the axis. */
      EvenSpaceLow,           /* Evenly space the tick marks, including the min value on the axis. */
      EvenSpaceLowAndHigh     /* Evenly space the tick marks, including both the max and min values on the axis. */
    }

    [SerializeField]
    Vector2 rangeX;
    /** \brief  The range of the X axis.  (x = min, y = max).
      */
    public Vector2 RangeX {
      get { return rangeX; }
      protected set { rangeX = value; }
    }

    [SerializeField]
    Vector2 rangeY;
    /** \brief The range of the Y axis.  (x = min, y = max).
      */
    public Vector2 RangeY {
      get { return rangeY; }
      protected set { rangeY = value; }
    }

    /** \brief The area in which plots are allowed to draw.
      * The effective plot area after padding has been accounted for.
      */
    public Rect PlotArea {
      get { return plotAreaBackgroundRectTransform.rect; }
    }

    public float GridLinesThicknesMajor;
    public Vector2 GridLinesSeparationMajor;
    [SerializeField]
    private Color gridLinesColorMajor;
    public Color GridLinesColorMajor {
      get {
        return gridLinesColorMajor;
      }
      set {
        gridLinesColorMajor = value;
        DrawGrid();
      }
    }

    public float GridLinesThicknesMinor;
    public Vector2 GridLinesSeparationMinor;
    public Color GridLinesColorMinor;

    protected virtual Rect adjustPlotArea(Rect pRect) {
      return pRect;
    }

    [ExecuteInEditMode]
    void OnValidate() {
      XNumberOfTicks = Mathf.Max(0, XNumberOfTicks);
      YNumberOfTicks = Mathf.Max(0, YNumberOfTicks);
    }

    public virtual void Awake() {
      rectTransform = gameObject.GetComponent<RectTransform>();
      if (!rectTransform)
        rectTransform = gameObject.AddComponent<RectTransform>();

      setupGraphHierarchy();
      recalculateAndDraw();
    }

    public virtual void Start() {
      rectTransform = gameObject.GetComponent<RectTransform>();
      if (!rectTransform)
        rectTransform = gameObject.AddComponent<RectTransform>();

      setupGraphDefaults();
      recalculateAndDraw();
    }

    void OnDestroy() {
      if (!Application.isPlaying) {
        if (meshesContainer != null) {
          DestroyImmediate(meshesContainer);
        }
        if (dataLabelTopContainer != null) {
          DestroyImmediate(dataLabelTopContainer);
        }
      }
    }
    
    protected void OnRectTransformDimensionsChange() {
      recalculateAndDraw();
    }

    public void recalculateAndDraw() {
      DrawPlotBackground();
      DrawMarginBackground();
      DrawGrid();
      DrawAxes();
      DrawAxisTicks();
      foreach (KeyValuePair<string, DataSeries> pair in dataSeriesByName) {
        pair.Value.resizeRects();
      }
    }

    public virtual void setupGraphHierarchy() {
      if(meshesContainer == null) {
        meshesContainer = Utils.AddGameObject(gameObject, 0, "Meshes Container", true);
      }
      if(dataLabelTopContainer == null) {
        dataLabelTopContainer = Utils.AddGameObject(gameObject, 0, "Data Labels Container", true);
      }
      
      if (marginBackgroundGo == null) {
        marginBackgroundGo = Utils.AddGameObject(meshesContainer, 0, "Margin Area Background", true);
      }
      if (plotAreaBackgroundGo == null) {
        plotAreaBackgroundGo = Utils.AddGameObject(meshesContainer, 0, "Plot Area Background", true);
        plotAreaBackgroundRectTransform = plotAreaBackgroundGo.GetComponent<RectTransform>();
      }

      if (gridContainer == null) {
        gridContainer = Utils.AddGameObject(plotAreaBackgroundGo, LEVEL_STEP * 2, "Grid");
      }
      if (axesGo == null) {
        axesGo = Utils.AddGameObject(plotAreaBackgroundGo, 0, "Axes Lines And Ticks", true);
        axesRectTransform = axesGo.GetComponent<RectTransform>();
      }
      if (plotsContainer == null) {
        plotsContainer = Utils.AddGameObject(plotAreaBackgroundGo, LEVEL_STEP * 2, "Plots");
      }

      if (xAxesGo == null) {
        xAxesGo = Utils.AddGameObject(axesGo, 0, "X Axis", true);
      }
      if (yAxesGo == null) {
        yAxesGo = Utils.AddGameObject(axesGo, 0, "Y Axis", true);
      }
      if (axesLabelContainerGo == null) {
        axesLabelContainerGo = Utils.AddGameObject(axesGo, 0, "Axes Labels", true);
      }


    }

    public void setupGraphDefaults() {
      if (transform.parent != null) {
        gameObject.layer = transform.parent.gameObject.layer;
      }
    }

    /** \brief  Set the X and Y axis ranges.
     * 
     * \param xMin The X axis min value.
     * \param xMax The X axis max value.
     * \param yMin The Y axis min value.
     * \param yMax The Y axis max value.
      */
    public void setRanges(float xMin, float xMax, float yMin, float yMax) {
      RangeX = new Vector2(xMin, xMax);
      RangeY = new Vector2(yMin, yMax);
      recalculateAndDraw();
    }

    /** \brief  Set the just the X axis range.
     * 
     * \param xMin The X axis min value.
     * \param xMax The X axis max value.
     */
    public void setRangeX(float xMin, float xMax) {
      RangeX = new Vector2(xMin, xMax);
      recalculateAndDraw();
    }

    /** \brief  Set the just the Y axis range.
     * 
     * \param yMin The Y axis min value.
     * \param yMax The Y axis max value.
     */
    public void setRangeY(float yMin, float yMax) {
      RangeY = new Vector2(yMin, yMax);
      recalculateAndDraw();
    }

    /** \brief  Add a data series type to the graph.
     * 
     *  T must be a child of NGraphDataSeries.
     * \param name Name of this plot.
     * \param pPlotColor Default color for this plot.
     * \param pMaterial Optional default material override for this plot.  Pass null for NGraph default.
     */
    public T addDataSeries<T>(string name, Color plotColor) where T : DataSeries {

      if(dataSeriesByName.ContainsKey(name)) {
        Debug.LogError("Graph already contains DataSeries: " + name);
        return null;
      }

      GameObject pGameObject = Utils.AddGameObject(plotsContainer, PLOT_START_LEVEL * LEVEL_STEP, name);
      T dataSeries = pGameObject.AddComponent<T>();
      dataSeries.PlotColor = plotColor;

      dataSeriesByName.Add(name, dataSeries);

      dataSeries.setup(this);

      return dataSeries;
    }

    public void addDataSeries(DataSeries dataSeries) {
      if (dataSeriesByName.ContainsValue(dataSeries)) {
        return;
      }

      if (dataSeriesByName.ContainsKey((dataSeries.gameObject.name))) {
        Debug.LogError("Graph already contains DataSeries: " + dataSeries.gameObject.name);
      }

      dataSeriesByName.Add(dataSeries.gameObject.name, dataSeries);

      dataSeries.setup(this);
    }

    /** \brief  Use to clear graph of all plots.  Should also be called with true before calling Destroy on graph.
     * 
     * \param destroyAllContainers set to true if you are about to Destroy the graph.
     */
    public virtual void breakdown(bool destroyAllContainers) {
      foreach (KeyValuePair<string, DataSeries> pair in dataSeriesByName) {
        Destroy(pair.Value.gameObject);
      }
      dataSeriesByName.Clear();
    }
    
    protected virtual void newDataSeriesDataLabelTopContainerGameObject() {
    }

    public bool removeDataSeries(DataSeries dataSeries) {
      Destroy(dataSeries.gameObject);
      string keyToRemove = null;
      foreach(KeyValuePair<string, DataSeries> pair in dataSeriesByName) {
        if(pair.Value == dataSeries) {
          keyToRemove = pair.Key;
          break;
        }
      }
      if(keyToRemove == null) {
        return false;
      }
      return dataSeriesByName.Remove(keyToRemove);
    }

    public bool removeDataSeries(string key) {
      if (!dataSeriesByName.ContainsKey(key)) {
        return false;
      }
      DataSeries dataSeries = dataSeriesByName[key];
      dataSeriesByName.Remove(key);

      if(dataSeries != null) {
        Destroy(dataSeries.gameObject);
      }

      return true;
    }

    protected abstract void _drawPlotBackground(List<UIVertex> pVertexList);
    public virtual void DrawPlotBackground() {
      plotAreaBackgroundRectTransform.offsetMin = new Vector2(margin.x, margin.w);
      plotAreaBackgroundRectTransform.offsetMax = new Vector2(-margin.z, -margin.y);
      List<UIVertex> pList = new List<UIVertex>(4);

      /* 
       * 1a       2b
       * 
       * 
       *    
       * 0c       3d
       * 
       */
      Rect pPlotArea = plotAreaBackgroundRectTransform.rect;
      Vector3 a = new Vector3(pPlotArea.xMin, pPlotArea.yMax, 0);
      Vector3 b = new Vector3(pPlotArea.xMax, pPlotArea.yMax, 0);
      Vector3 c = new Vector3(pPlotArea.xMin, pPlotArea.yMin, 0);
      Vector3 d = new Vector3(pPlotArea.xMax, pPlotArea.yMin, 0);

      UIVertex pUIVertex = new UIVertex();

      pUIVertex.position = c;
      pUIVertex.uv0 = new Vector2(1, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = a;
      pUIVertex.uv0 = new Vector2(0, 0);
      pList.Add(pUIVertex);

      pUIVertex.position = b;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);


      pUIVertex.position = d;
      pUIVertex.uv0 = new Vector2(1, 0);
      pList.Add(pUIVertex);
      
      this._drawPlotBackground(pList);
    }

    protected virtual void addedAxesLabelContainer() {
      // Nothing here.  Used in child classes as simple callback.
    }

    protected abstract void _drawAxisTick(Axis axis, int index, Vector2 location);
    protected abstract void adjustLabelCount(Axis axis, int num);
    protected virtual void DrawAxisTicks() {
      int numTicks = XNumberOfTicks;
      if (XTickStyle == TickStyle.EvenSpaceLowAndHigh)
        numTicks--;
      float step = (RangeX.y - RangeX.x) / numTicks;
      if (XTickStyle == TickStyle.EvenSpaceLowAndHigh)
        numTicks++;
      adjustLabelCount(Axis.X, DrawXLabel ? numTicks : 0);
      for (int i = 0; i < numTicks; i++) {
        float val = step * (i + 1);
        if (XTickStyle == TickStyle.EvenSpace)
          val -= step / 2;
        else if (XTickStyle == TickStyle.EvenSpaceLow || XTickStyle == TickStyle.EvenSpaceLowAndHigh)
          val -= step;
        Vector2 pPoint = new Vector2(val + RangeX.x, AxesDrawAt.y);
        pPoint = adjustPoint(pPoint);
        
        if (DrawXLabel) {
          Vector3 pPosition = new Vector3(pPoint.x, pPoint.y - 4, 0);
          AddAxisLabel(Axis.X, i, pPosition, val + RangeX.x);
        }
        this._drawAxisTick(Axis.X, i, pPoint);
      }

      numTicks = YNumberOfTicks;
      if (YTickStyle == TickStyle.EvenSpaceLowAndHigh)
        numTicks--;
      step = (RangeY.y - RangeY.x) / numTicks;
      if (YTickStyle == TickStyle.EvenSpaceLowAndHigh)
        numTicks++;
      adjustLabelCount(Axis.Y, DrawYLabel ? numTicks : 0);
      for (int i = 0; i < numTicks; i++) {
        float val = step * (i + 1);
        if (YTickStyle == TickStyle.EvenSpace)
          val -= step / 2;
        else if (YTickStyle == TickStyle.EvenSpaceLow || YTickStyle == TickStyle.EvenSpaceLowAndHigh)
          val -= step;
        Vector2 pPoint = new Vector2(AxesDrawAt.x, val + RangeY.x);
        pPoint = adjustPoint(pPoint);
        
        if (DrawYLabel) {
          Vector3 pPosition = new Vector3(pPoint.x - 6, pPoint.y, 0);
          AddAxisLabel(Axis.Y, i, pPosition, val + RangeY.x);
        }

        this._drawAxisTick(Axis.Y, i, pPoint);
      }
    }

    protected abstract void _drawAxes(Rect xAxis, Rect yAxis);
    protected void DrawAxes() {
      //axesRectTransform.offsetMin = new Vector2(Margin.x, Margin.w);
      //axesRectTransform.offsetMax = new Vector2(-Margin.z, -Margin.y);
      if (RangeX == Vector2.zero)
        return;

      if (RangeY == Vector2.zero)
        return;

      axesDrawAt.x = Mathf.Clamp(AxesDrawAt.x, RangeX.x, RangeX.y);
      axesDrawAt.y = Mathf.Clamp(AxesDrawAt.y, RangeY.x, RangeY.y);

      Vector2 pTop = adjustPoint(new Vector2(AxesDrawAt.x, RangeY.y));
      Vector2 pBottom = adjustPoint(new Vector2(AxesDrawAt.x, RangeY.x));
      Vector2 pLeft = adjustPoint(new Vector2(RangeX.x, AxesDrawAt.y));
      Vector2 pRight = adjustPoint(new Vector2(RangeX.y, AxesDrawAt.y));

      Rect x = new Rect(pLeft.x, AxesThickness / 2 + pLeft.y, pRight.x - pLeft.x, -AxesThickness);
      Rect y = new Rect(pTop.x - AxesThickness / 2, pTop.y, AxesThickness, -(pTop.y - pBottom.y));

      this._drawAxes(x, y);
    }

    protected abstract void _drawMarginBackground(List<UIVertex> pVertexList);
    protected void DrawMarginBackground() {
      /* 
       * a  b&m       n&i j
       *    p          o 
       *               
       *              
       *               
       *               l  k
       *     e            f
       * c  d&g           h
       * 
       */
      Rect pRect = rectTransform.rect;
      Vector3 a = new Vector3(pRect.xMin, pRect.yMax, 0);
      Vector3 b = new Vector3(pRect.xMin + margin.x, pRect.yMax, 0);
      Vector3 c = new Vector3(pRect.xMin, pRect.yMin, 0);
      Vector3 d = new Vector3(pRect.xMin + margin.x, pRect.yMin, 0);

      Vector3 e = new Vector3(pRect.xMin + margin.x, pRect.yMin + margin.w, 0);
      Vector3 f = new Vector3(pRect.xMax, pRect.yMin + margin.w, 0);
      Vector3 g = new Vector3(pRect.xMin + margin.x, pRect.yMin, 0);
      Vector3 h = new Vector3(pRect.xMax, pRect.yMin, 0);

      Vector3 i = new Vector3(pRect.xMax - margin.z, pRect.yMax, 0);
      Vector3 j = new Vector3(pRect.xMax, pRect.yMax, 0);
      Vector3 k = new Vector3(pRect.xMax, pRect.yMin + margin.w, 0);
      Vector3 l = new Vector3(pRect.xMax - margin.z, pRect.yMin + margin.w, 0);

      Vector3 m = new Vector3(pRect.xMin + margin.x, pRect.yMax, 0);
      Vector3 n = new Vector3(pRect.xMax - margin.z, pRect.yMax, 0);
      Vector3 o = new Vector3(pRect.xMax - margin.z, pRect.yMax - margin.y, 0);
      Vector3 p = new Vector3(pRect.xMin + margin.x, pRect.yMax - margin.y, 0);

      List<UIVertex> pList = new List<UIVertex>(6);
      UIVertex pUIVertex = new UIVertex();


      pUIVertex.position = a;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = b;
      pUIVertex.uv0 = new Vector2(0.5f, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = d;
      pUIVertex.uv0 = new Vector2(0.5f, 0.5f);
      pList.Add(pUIVertex);

      pUIVertex.position = c;
      pUIVertex.uv0 = new Vector2(0, 0);
      pList.Add(pUIVertex);


      pUIVertex.position = e;
      pUIVertex.uv0 = new Vector2(1, 0.5f);
      pList.Add(pUIVertex);

      pUIVertex.position = f;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = h;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = g;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);


      pUIVertex.position = i;
      pUIVertex.uv0 = new Vector2(1, 0.5f);
      pList.Add(pUIVertex);

      pUIVertex.position = j;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = k;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = l;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);


      pUIVertex.position = m;
      pUIVertex.uv0 = new Vector2(1, 0.5f);
      pList.Add(pUIVertex);

      pUIVertex.position = n;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = o;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);

      pUIVertex.position = p;
      pUIVertex.uv0 = new Vector2(0, 1);
      pList.Add(pUIVertex);
      
      this._drawMarginBackground(pList);
    }

    protected abstract void _clearMajorGridLines();
    protected abstract void _addMajorGridLine(Axis axis, int index, float r);
    protected abstract void _majorGridLinesDone();
    protected void DrawGrid() {
      _clearMajorGridLines();

      int index = 0;
      if (GridLinesSeparationMajor.x > 0) {
        for (float r = RangeX.x; r <= RangeX.y; r += GridLinesSeparationMajor.x) {
          this._addMajorGridLine(Axis.X, index++, r);
        }
      }

      index = 0;
      if (GridLinesSeparationMajor.y > 0) {
        for (float r = RangeY.x; r <= RangeY.y; r += GridLinesSeparationMajor.y) {
          this._addMajorGridLine(Axis.Y, index++, r);
        }
      }

      _majorGridLinesDone();
    }

    public float adjustPointX(float f, bool offsetFromBottomLeft = false) {
      float xPercent = (f - RangeX.x) / (RangeX.y - RangeX.x);
      
      float newX = plotAreaBackgroundRectTransform.rect.xMin + (plotAreaBackgroundRectTransform.rect.width * xPercent);
      if (offsetFromBottomLeft) {
        newX += plotAreaBackgroundRectTransform.rect.width / 2;
      }
      return newX;
    }

    public float adjustPointY(float f, bool offsetFromBottomLeft = false) {
      float yPercent = (f - RangeY.x) / (RangeY.y - RangeY.x);
      
      float newY = plotAreaBackgroundRectTransform.rect.yMin + (plotAreaBackgroundRectTransform.rect.height * yPercent);
      if (offsetFromBottomLeft) {
        newY += plotAreaBackgroundRectTransform.rect.height / 2;
      }

      return newY;
    }

    public Vector2 adjustPoint(Vector2 pDataPoint, bool offsetFromBottomLeft = false) {
      pDataPoint.x = adjustPointX(pDataPoint.x, offsetFromBottomLeft);
      pDataPoint.y = adjustPointY(pDataPoint.y, offsetFromBottomLeft);
      return pDataPoint;
    }
  }
}