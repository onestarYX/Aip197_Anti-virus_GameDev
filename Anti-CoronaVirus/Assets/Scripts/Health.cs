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
    private GameManager gameManager;
    private HospitalManager hospitalManager;
    private AgentMove agentMoveScript;
    private Strategy strategy;

    public health_status status = health_status.healthy;
    public Material red;
    public Material green;
    public Material orange;
    public Material blue;

    public float infectionP = 0.1f;
    public float goForTestP = 0.75f;
    public float fatalityRate = 0.05f;

    private int infectedTimeStamp = -1;
    public int incubationT;
    private int detectedTimeStamp = -1;
    public int sympOnsetTime;
    public GameObject hospitalToGo;
    private Hospital hospitalScript;

    // Start is called before the first frame update
    void Start()
    {
        houseManager = GameObject.Find("Residence").GetComponent<HouseManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        strategy = GameObject.Find("Game Manager").GetComponent<Strategy>();
        hospitalManager = GameObject.Find("Hospitals").GetComponent<HospitalManager>();
        agentMoveScript = GetComponent<AgentMove>();

        int randHospitalIdx = Random.Range(0, hospitalManager.hospitals.Count);
        hospitalToGo = hospitalManager.hospitals[randHospitalIdx];
        hospitalScript = hospitalToGo.GetComponent<Hospital>();

        if (Random.Range(0.0f, 1.0f) < 0.05f)
        {
            status = health_status.infected;
            GetComponent<MeshRenderer>().material = orange;
            infectedTimeStamp = 0;
            houseManager.IncreInfected();
        }
        else
        {
            status = health_status.healthy;
        }

        sympOnsetTime = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(status == health_status.infected)
        {
            int curTime = gameManager.GetCurTime();
            incubationT = curTime - infectedTimeStamp;
            if (incubationT == 20000)
            {
                Debug.Log("Going to hospital");
                if(Random.Range(0.0f, 1.0f) < goForTestP)
                {
                    if(agentMoveScript.status == all_states.atHome)
                    {
                        agentMoveScript.SetOnRoad(agentMoveScript.house_startRoad, agentMoveScript.house_x, agentMoveScript.house_z, hospitalScript.addressOnRoad.x, hospitalScript.addressOnRoad.z);
                    } else if(agentMoveScript.status == all_states.atWorkPlace)
                    {
                        agentMoveScript.SetOnRoad(agentMoveScript.workplace_startRoad, agentMoveScript.workplace_x, agentMoveScript.workplace_z, hospitalScript.addressOnRoad.x, hospitalScript.addressOnRoad.z);
                    } else
                    {
                        agentMoveScript.SetOnRoad(agentMoveScript.curRoad, transform.position.x, transform.position.z, hospitalScript.addressOnRoad.x, hospitalScript.addressOnRoad.z);
                    }
                }
            }
        }

        if(status == health_status.detected)
        {
            int curTime = gameManager.GetCurTime();
            int treatmentT = curTime - detectedTimeStamp;
            if (treatmentT == 40000)
            {
                if(Random.Range(0.0f, 1.0f) < fatalityRate)
                {
                    status = health_status.dead;
                    houseManager.IncreToll();
                    Destroy(gameObject);
                } else
                {
                    status = health_status.recovered;
                    gameObject.GetComponent<MeshRenderer>().material = blue;
                    houseManager.IncreRecovered();
                    agentMoveScript.SetOnRoad(hospitalScript.nearestRoad, hospitalScript.addressOnRoad.x, hospitalScript.addressOnRoad.z, agentMoveScript.house_x, agentMoveScript.house_z);
                }
            }
        }

        if (strategy.HasPutOnMask())
        {
            infectionP = 0.05f;
        }
        else
        {
            infectionP = 0.1f;
        }
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
            if (Random.Range(0.0f, 1.0f) < infectionP )
            {
                status = health_status.infected;
                gameObject.GetComponent<MeshRenderer>().material = orange;
                infectedTimeStamp = gameManager.GetCurTime();
                houseManager.IncreInfected();
            }
        }
    }

    public void SetDetected()
    {
        status = health_status.detected;
        gameObject.GetComponent<MeshRenderer>().material = red;
        detectedTimeStamp = gameManager.GetCurTime();
        houseManager.IncreDetected();
    }
}
