using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleInitiator : MonoBehaviour
{
    public GameObject personPrefab;
    public List<GameObject> people = new List<GameObject>();
    List<AgentMove> agentMoveScriptsList = new List<AgentMove>();
    public int pplPerHouse = 3;

    private float personY = 0.5f;
    private float spawnOffset = 3;
    private WorkPlaceManager workPlaceManager;
    private RoadManager roadManager;
    private GameManager gameManager;

    public GameObject startRoad;
    public Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        roadManager = GameObject.Find("Roads").GetComponent<RoadManager>();
        workPlaceManager = GameObject.Find("Work Places").GetComponent<WorkPlaceManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(3);
        startRoad = FindNearestStartRoad(transform.position);
        startPos = CalculateStartPos(transform.position, startRoad);

        for (int i = 0; i < pplPerHouse; i++)
        {
            float initPosX = Random.Range(transform.position.x - spawnOffset, transform.position.x + spawnOffset);
            float initPosZ = Random.Range(transform.position.z - spawnOffset, transform.position.z + spawnOffset);
            Vector3 initPos = new Vector3(initPosX, 0.5f, initPosZ);
            GameObject newPerson = Instantiate(personPrefab, initPos, personPrefab.transform.rotation);
            if (Random.Range(0.0f, 1.0f) < 0.05f)
            {
                newPerson.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            people.Add(newPerson);
            AgentMove agentMoveScript = newPerson.GetComponent<AgentMove>();
            agentMoveScriptsList.Add(agentMoveScript);
        }

        for (int i = 0; i < pplPerHouse; i++)
        {
            int workPlaceIdx = Random.Range(0, workPlaceManager.workPlaces.Count);
            agentMoveScriptsList[i].Setup(gameObject, workPlaceManager.workPlaces[workPlaceIdx], 5);
        }

    }


    Vector3 CalculateStartPos(Vector3 housePos, GameObject startRoad)
    {
        if (startRoad.transform.rotation.eulerAngles.y == 90)
        {
            if (housePos.x < (startRoad.transform.position.x - startRoad.GetComponent<Collider>().bounds.size.x / 2 + startRoad.GetComponent<Collider>().bounds.size.z / 2))
            {
                float startPosX = startRoad.transform.position.x - startRoad.GetComponent<Collider>().bounds.size.x / 2 + startRoad.GetComponent<Collider>().bounds.size.z / 2;
                return new Vector3(startPosX, personY, startRoad.transform.position.z);
            } else if (housePos.x > (startRoad.transform.position.x + startRoad.GetComponent<Collider>().bounds.size.x / 2 - startRoad.GetComponent<Collider>().bounds.size.z / 2))
            {
                float startPosX = startRoad.transform.position.x + startRoad.GetComponent<Collider>().bounds.size.x / 2 - startRoad.GetComponent<Collider>().bounds.size.z / 2;
                return new Vector3(startPosX, personY, startRoad.transform.position.z);
            } else
            {
                return new Vector3(housePos.x, personY, startRoad.transform.position.z);
            }
        } else
        {
            if (housePos.z < (startRoad.transform.position.z - startRoad.GetComponent<Collider>().bounds.size.z / 2 + startRoad.GetComponent<Collider>().bounds.size.x / 2))
            {
                float startPosZ = startRoad.transform.position.z - startRoad.GetComponent<Collider>().bounds.size.z / 2 + startRoad.GetComponent<Collider>().bounds.size.x / 2;
                return new Vector3(startRoad.transform.position.x, personY, startPosZ);
            } else if (housePos.z > (startRoad.transform.position.z + startRoad.GetComponent<Collider>().bounds.size.z / 2 - startRoad.GetComponent<Collider>().bounds.size.x / 2))
            {
                float startPosZ = startRoad.transform.position.z + startRoad.GetComponent<Collider>().bounds.size.z / 2 - startRoad.GetComponent<Collider>().bounds.size.x / 2;
                return new Vector3(startRoad.transform.position.x, personY, startPosZ);
            } else
            {
                return new Vector3(startRoad.transform.position.x, personY, housePos.z);
            }
        }
    }

    GameObject FindNearestStartRoad(Vector3 pos)
    {

        if(roadManager.roads.Count == 0)
        {
            Debug.LogError("error in fetching roads");
        }

        float minDist = 10000;
        float dist = -1;
        GameObject roadToReturn = null;

        foreach (GameObject road in roadManager.roads)
        {
            dist = Mathf.Abs(pos.z - road.transform.position.z) + Mathf.Abs(pos.x - road.transform.position.x);

            //Debug.Log(road.name);
            //Debug.Log(road.transform.rotation.eulerAngles);
            //Debug.Log(dist);

            if (dist < minDist)
            {
                minDist = dist;
                roadToReturn = road;
            }
        }

        if (dist == -1)
        {
            Debug.LogError("error in finding min dist road");
        }
        return roadToReturn;
    }

}
