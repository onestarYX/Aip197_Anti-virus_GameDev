using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public List<GameObject> houses = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            houses.Add(child.gameObject);
            Debug.Log("Adding road");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
