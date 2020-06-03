using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum health_status
{
    healthy = 0,
    infected = 1,
    detected = 2,
    recovered = 3,
    dead = 4
}

public class Health : MonoBehaviour
{
    private HouseManager houseManager;

    public float infectionP = 0.1f;
    public health_status status = health_status.healthy;
    public Material red;
    public Material green;
    public Material orange;
    public Material blue;

    // Start is called before the first frame update
    void Start()
    {
        houseManager = GameObject.Find("Residence").GetComponent<HouseManager>();

        if (Random.Range(0.0f, 1.0f) < 0.05f)
        {
            status = health_status.infected;
            GetComponent<MeshRenderer>().material = orange;
            houseManager.IncreInfected();
        }
        else
        {
            status = health_status.healthy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Person"))
        {
            return;
        }

        if (status != health_status.healthy)
        {
            return;
        }

        health_status other_status = other.gameObject.GetComponent<Health>().status;
        if (other_status == health_status.infected || other_status == health_status.detected)
        {
            if (Random.Range(0.0f, 1.0f) < infectionP)
            {
                status = health_status.infected;
                gameObject.GetComponent<MeshRenderer>().material = orange;
                houseManager.IncreInfected();
            }
        }
    }
}
