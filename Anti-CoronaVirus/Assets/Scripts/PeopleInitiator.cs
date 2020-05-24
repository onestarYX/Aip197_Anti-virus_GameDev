using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleInitiator : MonoBehaviour
{
    public GameObject personPrefab;
    public List<GameObject> people = new List<GameObject>();
    public int pplPerHouse = 3;
    private float personY = 0.5f;
    private float spawnOffset = 3;
    private RoadManager roadManager;
    public GameObject startRoad;
    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        roadManager = GameObject.Find("Roads").GetComponent<RoadManager>();
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3);
        startRoad = findNearestStartRoad(transform.position);
        startPos = CalculateStartPos(transform.position, startRoad);

        List<AgentMove> agentMoveScriptsList = new List<AgentMove>();

        for (int i = 0; i < pplPerHouse; i++)
        {
            float initPosX = Random.Range(transform.position.x - spawnOffset, transform.position.x + spawnOffset);
            float initPosZ = Random.Range(transform.position.z - spawnOffset, transform.position.z + spawnOffset);
            Vector3 initPos = new Vector3(initPosX, 0.5f, initPosZ);
            GameObject newPerson = Instantiate(personPrefab, initPos, personPrefab.transform.rotation);
            people.Add(newPerson);
            AgentMove agentMoveScript = newPerson.GetComponent<AgentMove>();
            agentMoveScriptsList.Add(agentMoveScript);
        }

        for (int i = 0; i < pplPerHouse; i++)
        {
            agentMoveScriptsList[i].Setup(0, 0, transform.position.x, transform.position.z, 5);
        }

        for (int i = 0; i < pplPerHouse; i++)
        {
            yield return new WaitForSeconds(3);
            agentMoveScriptsList[i].SetOnRoad(startRoad, startPos);
        }
    }

    Vector3 CalculateStartPos(Vector3 housePos, GameObject startRoad)
    {
        if (startRoad.transform.rotation.eulerAngles.y == 90)
        {
            if (housePos.x < (startRoad.transform.position.x - startRoad.GetComponent<Collider>().bounds.size.x / 2 + startRoad.GetComponent<Collider>().bounds.size.z / 2))
            {
                Debug.Log("LeftBound");
                float startPosX = startRoad.transform.position.x - startRoad.GetComponent<Collider>().bounds.size.x / 2 + startRoad.GetComponent<Collider>().bounds.size.z / 2;
                Debug.Log(startRoad.GetComponent<Collider>().bounds.size.z / 2);
                Debug.Log(startRoad.GetComponent<Collider>().bounds.size.x / 2);
                return new Vector3(startPosX, personY, startRoad.transform.position.z);
            } else if (housePos.x > (startRoad.transform.position.x + startRoad.GetComponent<Collider>().bounds.size.x / 2 - startRoad.GetComponent<Collider>().bounds.size.z / 2))
            {
                Debug.Log("RightBound");
                float startPosX = startRoad.transform.position.x + startRoad.GetComponent<Collider>().bounds.size.x / 2 - startRoad.GetComponent<Collider>().bounds.size.z / 2;
                Debug.Log(startPosX);
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

    GameObject findNearestStartRoad(Vector3 pos)
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
            //if (road.transform.rotation.eulerAngles.y == 90)
            //{
            //    dist = Mathf.Abs(pos.z - road.transform.position.z);
            //}
            //else
            //{
            //    dist = Mathf.Abs(pos.x - road.transform.position.x);
            //}

            dist = Mathf.Abs(pos.z - road.transform.position.z) + Mathf.Abs(pos.x - road.transform.position.x);

            Debug.Log(road.name);
            Debug.Log(road.transform.rotation.eulerAngles);
            Debug.Log(dist);

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
