using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleInitiator : MonoBehaviour
{
    public GameObject personPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int numOfPeople = 300;
        float xRange = 45;
        float zRange = 45;
        for (int i = 0; i < numOfPeople; i++)
        {
            float initPosX = Random.Range(-xRange, xRange);
            float initPosZ = Random.Range(-zRange, zRange);
            Vector3 initPos = new Vector3(initPosX, 0.5f, initPosZ);
            Instantiate(personPrefab, initPos, personPrefab.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
