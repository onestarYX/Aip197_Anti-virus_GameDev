/***********************************************************************
/*      Copyright Niugnep Software, LLC 2013 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GraphMaster {
  public class Utils {
#if UNITY_EDITOR
    static public void DrawSeparator() {
      GUILayout.Space(12f);

      if (Event.current.type == EventType.Repaint) {
        Texture2D tex = EditorGUIUtility.whiteTexture;
        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = new Color(0f, 0f, 0f, 0.25f);
        GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
        GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
        GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
        GUI.color = Color.white;
      }
    }

    static public void RegisterUndo(string name, params Object[] objects) {
      if (objects != null && objects.Length > 0) {
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
           UnityEditor.Undo.RegisterUndo(objects, name);
#else
        UnityEditor.Undo.RecordObjects(objects, name);
#endif
        foreach (Object obj in objects) {
          if (obj == null) continue;
          EditorUtility.SetDirty(obj);
        }
      }
    }

    static public GameObject SelectedRoot<T>() where T : Component {
      GameObject go = Selection.activeGameObject;

      // Only use active objects
      if (go != null && !GetActive(go)) go = null;

      // Try to find a panel
      T p = (go != null) ? FindInParents<T>(go) : null;

      // No selection? Try to find the root automatically
      if (p == null) {
        T[] panels = FindActive<T>();
        if (panels.Length > 0) go = panels[0].gameObject;
      }
      return go;
    }
#endif

    static public bool GetActive(GameObject go) {
#if UNITY_3_5
        return go && go.active;
#else
      return go && go.activeInHierarchy;
#endif
    }

    static public T FindInParents<T>(GameObject go) where T : Component {
      if (go == null)
        return null;
      object comp = go.GetComponent<T>();

      if (comp == null) {
        Transform t = go.transform.parent;

        while (t != null && comp == null) {
          comp = t.gameObject.GetComponent<T>();
          t = t.parent;
        }
      }
      return (T)comp;
    }

    static public T[] FindActive<T>() where T : Component {
#if UNITY_3_5 || UNITY_4_0
        return GameObject.FindSceneObjectsOfType(typeof(T)) as T[];
#else
      return GameObject.FindObjectsOfType(typeof(T)) as T[];
#endif
    }

    public static bool checkForNan(Vector2 v) {
      if (float.IsNaN(v.x) || float.IsNaN(v.y)) {
        return true;
      }
      return false;
    }

    public static bool checkForNan(Vector3 v) {
      if (float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z)) {
        return true;
      }
      return false;
    }

    public static bool checkForNan(Rect r) {
      if (float.IsNaN(r.xMin) || float.IsNaN(r.yMin) || float.IsNaN(r.xMax) || float.IsNaN(r.yMax)) {
        return true;
      }
      return false;
    }

    public static bool checkForInf(Vector2 v) {
      if (float.IsInfinity(v.x) || float.IsInfinity(v.y)) {
        return true;
      }
      return false;
    }

    public static bool checkForInf(Vector3 v) {
      if (float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z)) {
        return true;
      }
      return false;
    }

    public static bool checkForInf(Rect r) {
      if (float.IsInfinity(r.xMin) || float.IsInfinity(r.yMin) || float.IsInfinity(r.xMax) || float.IsInfinity(r.yMax)) {
        return true;
      }
      return false;
    }

    public static bool checkForNanOrInf(Vector2 v) {
      return checkForNan(v) || checkForInf(v);
    }

    public static bool checkForNanOrInf(Vector3 v) {
      return checkForNan(v) || checkForInf(v);
    }
    public static bool checkForNanOrInf(Rect r) {
      return checkForNan(r) || checkForInf(r);
    }

    public static GameObject AddGameObject(GameObject pParentGo, float zValue, string name, bool setRectTransformToFullStrech = true) {
      GameObject pNewGo = new GameObject(name);
      AddGameObject(pParentGo, pNewGo, zValue, setRectTransformToFullStrech);
      SortChildren(pParentGo);

      return pNewGo;
    }

    public static void SortChildren(GameObject pParentGo) {
      // Sort the children by z value
      List<Transform> children = new List<Transform>();
      for (int i = pParentGo.transform.childCount - 1; i >= 0; i--) {
        Transform child = pParentGo.transform.GetChild(i);
        children.Add(child);
      }
      children.Sort((Transform t1, Transform t2) => { return t2.transform.position.z.CompareTo(t1.transform.position.z); });
      int y = 0;
      foreach (Transform child in children) {
        child.SetSiblingIndex(y++);
      }
    }

    public static GameObject AddGameObject(GameObject pParentGo, GameObject pWillBeChildGo, float zValue, bool setRectTransformToFullStrech = true) {
      pWillBeChildGo.layer = pParentGo.layer;
      pWillBeChildGo.transform.SetParent(pParentGo.transform);
      pWillBeChildGo.transform.localScale = Vector3.one;
      pWillBeChildGo.transform.localEulerAngles = Vector3.zero;
      pWillBeChildGo.transform.localPosition = new Vector3(0, 0, zValue);

      Graph ng = pParentGo.GetComponentInParent<Graph>();
      if (ng == null) {
        ng = pParentGo.GetComponent<Graph>();
      }
      
      RectTransform rt = pWillBeChildGo.AddComponent<RectTransform>();
      if (setRectTransformToFullStrech) {
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
      }
      return pWillBeChildGo;
    }

    public static void AddMesh(GameObject pGo, out MeshRenderer pMeshRenderer, out Mesh pMesh) {
      pMeshRenderer = pGo.AddComponent<MeshRenderer>();
      pMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      pMeshRenderer.receiveShadows = false;

      pMesh = pGo.AddComponent<MeshFilter>().mesh;
    }

    public static CanvasRenderer AddCanvasRenderer(GameObject pGo) {
      CanvasRenderer pCanvasRenderer = pGo.GetComponent<CanvasRenderer>();
      if (pCanvasRenderer != null) {
        return pCanvasRenderer;
      }

      RectTransform pRectTransform = pGo.GetComponent<RectTransform>();
      if (pRectTransform == null) {
        Vector3 pos = pGo.transform.localPosition;
        Quaternion rot = pGo.transform.localRotation;
        Vector3 scale = pGo.transform.localScale;

        pRectTransform = pGo.AddComponent<RectTransform>();
        pRectTransform.localPosition = pos;
        pRectTransform.localRotation = rot;
        pRectTransform.localScale = scale;
      }

      pCanvasRenderer = pGo.GetComponent<CanvasRenderer>();
      if (pCanvasRenderer == null) {
        pCanvasRenderer = pGo.AddComponent<CanvasRenderer>();
      }
      return pCanvasRenderer;
    }
  }
}