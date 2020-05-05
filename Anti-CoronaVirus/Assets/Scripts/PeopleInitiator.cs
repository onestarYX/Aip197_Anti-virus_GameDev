using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleInitiator : MonoBehaviour
{
    public GameObject personPrefab;
    private List<Vector3> housesPositions = new List<Vector3>();
    public List<GameObject> people = new List<GameObject>();
    public int pplPerHouse = 10;
    private RoadManager roadManager;

    // Start is called before the first frame update
    void Start()
    {
        housesPositions.Add(new Vector3(60, 0.5f, 60));
        //housesPositions.Add(new Vector3(60, 0.5f, 0));
        //housesPositions.Add(new Vector3(60, 0.5f, -60));
        //housesPositions.Add(new Vector3(0, 0.5f, -60));
        //housesPositions.Add(new Vector3(-60, 0.5f, -60));
        //housesPositions.Add(new Vector3(-60, 0.5f, 0));
        //housesPositions.Add(new Vector3(-60, 0.5f, 60));
        //housesPositions.Add(new Vector3(0, 0.5f, 60));
        float spawnOffset = 5;

        foreach (Vector3 housePos in housesPositions)
        {
            for (int i = 0; i < pplPerHouse; i++)
            {
                float initPosX = Random.Range(housePos.x - spawnOffset, housePos.x + spawnOffset);
                float initPosZ = Random.Range(housePos.z - spawnOffset, housePos.z + spawnOffset);
                Vector3 initPos = new Vector3(initPosX, 0.5f, initPosZ);
                GameObject newPerson = Instantiate(personPrefab, initPos, personPrefab.transform.rotation);
                people.Add(newPerson);
            }
        }

        // For test purposes
        Vector3 curHousePos = housesPositions[0];
        roadManager = GameObject.Find("Roads").GetComponent<RoadManager>();
        GameObject startRoad = findNearestStartRoad(curHousePos);
        AgentMove someAgentMove = people[0].GetComponent<AgentMove>();
        someAgentMove.SetMoveSignal(startRoad, startRoad.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject findNearestStartRoad(Vector3 pos)
    {
        if(roadManager.roads.Count == 0)
        {
            Debug.LogError("error in fetching roads");
        }

        return roadManager.roads[0];
    }
}
