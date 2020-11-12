using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xAxisMove = Input.GetAxis("Horizontal");
        float zAxisMove = Input.GetAxis("Vertical");
        float scale = 4;
        float yAxisMove = Input.mouseScrollDelta.y * scale;
        transform.Translate(new Vector3(xAxisMove, zAxisMove, yAxisMove));
    }
}
