using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : MonoBehaviour
{
    private RoadManager roadManager;
    public GameObject nearestRoad;
    public Vector3 addressOnRoad;
    private float personY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        roadManager = GameObject.Find("Roads").GetComponent<RoadManager>();
        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {

    }


    Vector3 CalculateAddress(Vector3 pos, GameObject road)
    {
        if (road.transform.rotation.eulerAngles.y == 90)
        {
            if (pos.x < (road.transform.position.x - road.GetComponent<Collider>().bounds.size.x / 2 + road.GetComponent<Collider>().bounds.size.z / 2))
            {
                float startPosX = road.transform.position.x - road.GetComponent<Collider>().bounds.size.x / 2 + road.GetComponent<Collider>().bounds.size.z / 2;
                Debug.Log(road.GetComponent<Collider>().bounds.size.z / 2);
                Debug.Log(road.GetComponent<Collider>().bounds.size.x / 2);
                return new Vector3(startPosX, personY, road.transform.position.z);
            }
            else if (pos.x > (road.transform.position.x + road.GetComponent<Collider>().bounds.size.x / 2 - road.GetComponent<Collider>().bounds.size.z / 2))
            {
                float startPosX = road.transform.position.x + road.GetComponent<Collider>().bounds.size.x / 2 - road.GetComponent<Collider>().bounds.size.z / 2;
                Debug.Log(startPosX);
                return new Vector3(startPosX, personY, road.transform.position.z);
            }
            else
            {
                return new Vector3(pos.x, personY, road.transform.position.z);
            }
        }
        else
        {
            if (pos.z < (road.transform.position.z - road.GetComponent<Collider>().bounds.size.z / 2 + road.GetComponent<Collider>().bounds.size.x / 2))
            {
                float startPosZ = road.transform.position.z - road.GetComponent<Collider>().bounds.size.z / 2 + road.GetComponent<Collider>().bounds.size.x / 2;
                return new Vector3(road.transform.position.x, personY, startPosZ);
            }
            else if (pos.z > (road.transform.position.z + road.GetComponent<Collider>().bounds.size.z / 2 - road.GetComponent<Collider>().bounds.size.x / 2))
            {
                float startPosZ = road.transform.position.z + road.GetComponent<Collider>().bounds.size.z / 2 - road.GetComponent<Collider>().bounds.size.x / 2;
                return new Vector3(road.transform.position.x, personY, startPosZ);
            }
            else
            {
                return new Vector3(road.transform.position.x, personY, pos.z);
            }
        }
    }

    GameObject FindNearestRoad(Vector3 pos)
    {
        if (roadManager.roads.Count == 0)
        {
            Debug.LogError("error in fetching roads");
        }

        float minDist = 10000;
        float dist = -1;
        GameObject roadToReturn = null;

        foreach (GameObject road in roadManager.roads)
        {
            dist = Mathf.Abs(pos.z - road.transform.position.z) + Mathf.Abs(pos.x - road.transform.position.x);

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

    IEnumerator Init()
    {
        yield return new WaitForSeconds(2);
        nearestRoad = FindNearestRoad(transform.position);
        addressOnRoad = CalculateAddress(transform.position, nearestRoad);
    }
}
