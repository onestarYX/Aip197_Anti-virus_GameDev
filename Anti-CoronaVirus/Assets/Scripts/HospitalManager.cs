using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalManager : MonoBehaviour
{
    public List<GameObject> hospitals = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            hospitals.Add(child.gameObject);
            Debug.Log("Adding Hospitals");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
