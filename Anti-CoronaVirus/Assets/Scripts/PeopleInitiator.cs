using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleInitiator : MonoBehaviour
{
    public GameObject personPrefab;
    public List<GameObject> people = new List<GameObject>();
    public int pplPerHouse = 1;
    private RoadManager roadManager;

    // Start is called before the first frame update
    void Start()
    {
        float spawnOffset = 3;

        for (int i = 0; i < pplPerHouse; i++)
        {
            float initPosX = Random.Range(transform.position.x - spawnOffset, transform.position.x + spawnOffset);
            float initPosZ = Random.Range(transform.position.z - spawnOffset, transform.position.z + spawnOffset);
            Vector3 initPos = new Vector3(initPosX, 0.5f, initPosZ);
            GameObject newPerson = Instantiate(personPrefab, initPos, personPrefab.transform.rotation);
            people.Add(newPerson);
        }

        // For test purposes
        roadManager = GameObject.Find("Roads").GetComponent<RoadManager>();
        GameObject startRoad = null;
        AgentMove someAgentMove = people[0].GetComponent<AgentMove>();
        someAgentMove.Setup(0, 0, transform.position.x, transform.position.z, 5);
        StartCoroutine(waiter(startRoad, someAgentMove));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waiter(GameObject startRoad, AgentMove someAgentMove)
    {
        yield return new WaitForSeconds(10);
        startRoad = findNearestStartRoad(transform.position);
        someAgentMove.SetOnRoad(startRoad, startRoad.transform.position + new Vector3(0, 0, 45));
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
