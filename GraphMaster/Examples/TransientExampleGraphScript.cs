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
  public class TransientExampleGraphScript : MonoBehaviour {
    public bool TakeScreenShots = false;
    Graph mGraph;
    DataSeriesXyLiveTransient mSeries1;

    void Awake() {
      mGraph = gameObject.GetComponent<Graph>();
      if (mGraph == null) {
        Debug.LogWarning("GraphMaster.Graph component not found.  Aborting.");
        return;
      }
      mGraph.setRanges(-10, 0, -8, 8);

      List<Vector2> data = new List<Vector2>();

      mSeries1 = mGraph.addDataSeries<DataSeriesXyLiveTransient>("Transient", Color.green);
      mSeries1.Data = data;
      mSeries1.UpdateRate = 0.005f;
    }

    float mLastShot = 0;
    private int screenshotCount = 0;
    public void Update() {
      if (mGraph == null)
        return;

      mSeries1.UpdateValue = Mathf.Sin(Time.time) * 7;

      if (!TakeScreenShots)
        return;

      mLastShot += Time.deltaTime;
      if (mLastShot > 0.5f) {
        mLastShot = 0;
        string screenshotFilename;
        do {
          screenshotCount++;
          screenshotFilename = "transient_" + screenshotCount + ".png";
        } while (System.IO.File.Exists(screenshotFilename));

        ScreenCapture.CaptureScreenshot(screenshotFilename);
      }
    }
  }
}