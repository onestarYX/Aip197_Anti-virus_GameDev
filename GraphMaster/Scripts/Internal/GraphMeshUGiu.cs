using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GraphMaster {
  public class GraphMeshUGiu : Image {

    public static GraphMeshUGiu Factory(Transform parent, string name, Color color, Sprite sprite = null, Material material = null) {
      GameObject go = new GameObject(name);
      go.transform.SetParent(parent);

      RectTransform rt = go.AddComponent<RectTransform>();
      rt.anchorMin = new Vector2(0, 0);
      rt.anchorMax = new Vector2(1, 1);
      rt.pivot = new Vector2(0.5f, 0.5f);
      rt.offsetMin = Vector2.zero;
      rt.offsetMax = Vector2.zero;

      return AddToGameObject(go, color, sprite, material);
    }

    public static GraphMeshUGiu AddToGameObject(GameObject go, Color color, Sprite sprite = null, Material material = null) {
      GraphMeshUGiu gm = go.GetComponent<GraphMeshUGiu>();
      if (gm != null) {
        return gm;
      }
      gm = go.AddComponent<GraphMeshUGiu>();
      Image img = go.GetComponent<Image>();
      img.color = color;
      img.sprite = sprite;
      img.material = material;
      return gm;
    }

    [SerializeField]
    List<UIVertex> vertexListRects = new List<UIVertex>();

    [SerializeField]
    List<UIVertex> vertexListTriangles = new List<UIVertex>();

    public void setRectVertices(List<UIVertex> vertexList) {
      //Debug.Log("SetVertices " + gameObject.name + " " + vertexList.Count);
      vertexListRects = vertexList;
      SetVerticesDirty();
    }

    public void clearVertices() {
      vertexListRects.Clear();
      vertexListTriangles.Clear();
      SetVerticesDirty();
    }

    public void addRectVertices(List<UIVertex> vertexList) {
      Debug.Assert(vertexList.Count % 4 == 0, "vertexList count must be a multiple of 4 when calling GraphMeshUGiu.addRectVerticies.");
      vertexListRects.AddRange(vertexList);
    }

    public void addTriangleVertices(List<UIVertex> vertexList) {
      Debug.Assert(vertexList.Count % 3 == 0, "vertexList count must be a multiple of 3 when calling GraphMeshUGiu.addTriangleVertices.");
      vertexListTriangles.AddRange(vertexList);
    }

    public void addRect(Rect rect) {
      if (Utils.checkForNanOrInf(rect)) {
        return;
      }
      UIVertex vert = new UIVertex();
      vert.position = new Vector3(rect.xMin, rect.yMax, 0);
      vert.uv0 = new Vector2(1, 1);
      vertexListRects.Add(vert);

      vert = new UIVertex();
      vert.position = new Vector3(rect.xMax, rect.yMax, 0);
      vert.uv0 = new Vector2(0, 0);
      vertexListRects.Add(vert);

      vert = new UIVertex();
      vert.position = new Vector3(rect.xMax, rect.yMin, 0);
      vert.uv0 = new Vector2(1, 0);
      vertexListRects.Add(vert);

      vert = new UIVertex();
      vert.position = new Vector3(rect.xMin, rect.yMin, 0);
      vert.uv0 = new Vector2(0, 1);
      vertexListRects.Add(vert);
    }

    public void addLineSegment(Vector2 pPrevDataPoint, Vector2 pDataPoint, float thickness) {
      Vector2 line = pPrevDataPoint - pDataPoint;
      Vector2 normal = new Vector2(-line.y, line.x).normalized;

      Vector2 a = pPrevDataPoint - (thickness / 2 * normal);
      Vector2 b = pPrevDataPoint + (thickness / 2 * normal);
      Vector2 c = pDataPoint - (thickness / 2 * normal);
      Vector2 d = pDataPoint + (thickness / 2 * normal);

      if (Utils.checkForNanOrInf(a) || Utils.checkForNanOrInf(b) || Utils.checkForNanOrInf(c) || Utils.checkForNanOrInf(d)) {
        return;
      }

      // Create rectangle
      UIVertex vert = new UIVertex();
      vert.position = new Vector3(a.x, a.y, 0);
      vert.uv0 = new Vector2(1, 1);
      vertexListRects.Add(vert);

      vert = new UIVertex();
      vert.position = new Vector3(b.x, b.y, 0);
      vert.uv0 = new Vector2(0, 0);
      vertexListRects.Add(vert);

      vert = new UIVertex();
      vert.position = new Vector3(d.x, d.y, 0);
      vert.uv0 = new Vector2(1, 0);
      vertexListRects.Add(vert);

      vert = new UIVertex();
      vert.position = new Vector3(c.x, c.y, 0);
      vert.uv0 = new Vector2(0, 1);
      vertexListRects.Add(vert);
    }

    protected override void OnPopulateMesh(VertexHelper vh) {
      //Debug.Log("OnPopulateMesh " + gameObject.name + " " + vertexList.Count);
      vh.Clear();

      // Add rectangles
      if (vertexListRects != null) {
        int idx = 0;
        foreach (UIVertex vert in vertexListRects) {
          UIVertex v = vert;
          v.color = color;
          vh.AddVert(v);

          ++idx;
          if (idx != 0 && idx % 4 == 0) {
            vh.AddTriangle(idx - 1, idx - 4, idx - 3);
            vh.AddTriangle(idx - 3, idx - 2, idx - 1);
          }
        }
      }

      // Add triangles
      if (vertexListTriangles != null) {
        int idx = 0;
        foreach (UIVertex vert in vertexListTriangles) {
          UIVertex v = vert;
          v.color = color;
          vh.AddVert(v);

          ++idx;
          if (idx != 0 && idx % 3 == 0) {
            vh.AddTriangle(idx - 3, idx - 2, idx - 1);
          }
        }
      }

    }
  }
}