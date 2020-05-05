using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public List<GameObject> roads = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach(Transform child in transform)
        {
            roads.Add(child.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
