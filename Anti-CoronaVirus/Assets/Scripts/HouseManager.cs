using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public List<GameObject> houses = new List<GameObject>();
    public int population;
    public int infected;
    public int detected;
    public int recovered;
    public int death;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            houses.Add(child.gameObject);
            Debug.Log("Adding House");
        }
        population = CalPopulation();
        infected = 0;
        detected = 0;
        recovered = 0;
        death = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CalPopulation()
    {
        int population = 0;
        foreach(GameObject house in houses)
        {
            PeopleInitiator peopleInitScript = house.GetComponent<PeopleInitiator>();
            population += peopleInitScript.pplPerHouse;
        }
        return population;
    }

    public int GetPopulation()
    {
        return population;
    }

    public int GetInfected()
    {
        return infected;
    }

    public int GetDetected()
    {
        return detected;
    }

    public int GetRecovered()
    {
        return recovered;
    }

    public int GetToll()
    {
        return death;
    }



    public void IncreInfected()
    {
        infected++;
    }

    public void DecreInfected()
    {
        infected--;
    }

    public void IncreDetected()
    {
        detected++;
    }

    public void DecreDetected()
    {
        detected--;
    }

    public void IncreRecovered()
    {
        recovered++;
    }

    public void IncreToll()
    {
        death++;
    }
}
