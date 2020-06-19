using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPlaceManager : MonoBehaviour
{
    public List<GameObject> workPlaces = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            workPlaces.Add(child.gameObject);
            Debug.Log("Adding WorkPlace");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
