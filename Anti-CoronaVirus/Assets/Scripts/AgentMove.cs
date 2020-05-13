using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum all_states
{
    atHome = 0,
    onRoad = 1,
    inOtherBuilding = 2
}

public class AgentMove : MonoBehaviour
{
    private int direction = 3;
    private float speed;
    public float workplace_x = 0;
    public float workplace_z = 0;
    public float house_x;
    public float house_z;
    private float inHouseRange;
    public all_states status = all_states.atHome;
    public bool onFirstRoad = false;
    public float firstRoadDistToCross;
    public float distTravelled = 0;
    private RoadManager roadManager;
    public GameObject curRoad;

    public void Setup(float workplace_x_p, float workplace_z_p, float house_x_p, float house_z_p, float inHouseRange_p)
    {
        workplace_x = workplace_x_p;
        workplace_z = workplace_z_p;
        house_x = house_x_p;
        house_z = house_z_p;
        inHouseRange = inHouseRange_p;
    }

    // Start is called before the first frame update
    void Start()
    {
        roadManager = GameObject.Find("Roads").GetComponent<RoadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Up-down angle
        // 0: up
        // 1: right
        // 2: down
        // 3: left
        UpdateDirection();
        CheckArrived();
        UpdateSpeed();

        switch (direction)
        {
            case 0:
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                break;
            case 1:
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                break;
            case 2:
                transform.Translate(Vector3.forward * Time.deltaTime * -speed);
                break;
            case 3:
                transform.Translate(Vector3.right * Time.deltaTime * -speed);
                break;
            default:
                break;
        }

        if (status == all_states.onRoad)
        {
            distTravelled += Time.deltaTime * speed;
        }

    }

    public void SetOnRoad(GameObject road, Vector3 startPos)
    {
        transform.position = startPos;
        curRoad = road;
        status = all_states.onRoad;
        onFirstRoad = true;
        distTravelled = 0;
        Debug.Log(curRoad.GetComponent<Collider>().bounds.size.x);
        Debug.Log(curRoad.GetComponent<Collider>().bounds.size.z);
    }

    void UpdateSpeed()
    {
        if (status == all_states.atHome || status == all_states.inOtherBuilding)
        {
            speed = 2.0f;
        } else
        {
            speed = 10.0f;
        }
    }

    void UpdateDirection()
    {
        if (status == all_states.atHome)
        {
            if (transform.position.x > house_x + inHouseRange)
            {
                direction = 3;
            } else if (transform.position.x < house_x - inHouseRange)
            {
                direction = 1;
            } else if (transform.position.z > house_z + inHouseRange)
            {
                direction = 2;
            } else if (transform.position.z < house_z - inHouseRange)
            {
                direction = 0;
            }

            return;
        }

        if (status == all_states.inOtherBuilding)
        {
            if (transform.position.x > -11 + inHouseRange)
            {
                direction = 3;
            }
            else if (transform.position.x < -11 - inHouseRange)
            {
                direction = 1;
            }
            else if (transform.position.z > 0 + inHouseRange)
            {
                direction = 2;
            }
            else if (transform.position.z < 0 - inHouseRange)
            {
                direction = 0;
            }

            return;
        }


        if (onFirstRoad && distTravelled == 0)
        {
            if (curRoad.transform.rotation.y == 90)
            {
                if (transform.position.x > workplace_x)
                {
                    direction = 3;
                    firstRoadDistToCross = transform.position.x - (curRoad.transform.position.x - curRoad.GetComponent<Collider>().bounds.size.z / 2) - curRoad.GetComponent<Collider>().bounds.size.x / 2;
                } else
                {
                    direction = 1;
                    firstRoadDistToCross = (curRoad.transform.position.x + curRoad.GetComponent<Collider>().bounds.size.z / 2) - curRoad.GetComponent<Collider>().bounds.size.x / 2 - transform.position.x;
                }
            } else
            {
                if (transform.position.z > workplace_z)
                {
                    direction = 2;
                    firstRoadDistToCross = transform.position.z - (curRoad.transform.position.z - curRoad.GetComponent<Collider>().bounds.size.z / 2) - curRoad.GetComponent<Collider>().bounds.size.x / 2;
                } else
                {
                    direction = 0;
                    firstRoadDistToCross = (curRoad.transform.position.z + curRoad.GetComponent<Collider>().bounds.size.z / 2) - curRoad.GetComponent<Collider>().bounds.size.x / 2 - transform.position.z;
                }
            }
            return;
        }

        if (onFirstRoad && distTravelled >= firstRoadDistToCross)
        {
            Debug.Log("Condition2");
            curRoad = SwitchRoad();
            onFirstRoad = false;
            distTravelled = 0;
            if (curRoad.transform.position.x < transform.position.x - 2.5)
            {
                direction = 3;
            } else if (curRoad.transform.position.x > transform.position.x + 2.5)
            {
                direction = 1;
            } else if (curRoad.transform.position.z < transform.position.z - 2.5)
            {
                direction = 2;
            } else
            {
                direction = 0;
            }
            return;
        }

        if (!onFirstRoad && distTravelled >= Mathf.Abs(curRoad.GetComponent<Collider>().bounds.size.x - curRoad.GetComponent<Collider>().bounds.size.z))
        {
            Debug.Log("Condition3");
            curRoad = SwitchRoad();
            distTravelled = 0;
            if (curRoad.transform.position.x < transform.position.x - 2.5)
            {
                direction = 3;
            }
            else if (curRoad.transform.position.x > transform.position.x + 2.5)
            {
                direction = 1;
            }
            else if (curRoad.transform.position.z < transform.position.z - 2.5)
            {
                direction = 2;
            }
            else if (curRoad.transform.position.z > transform.position.z + 2.5)
            {
                direction = 0;
            } else
            {
                Debug.LogError("Error in updating direction");
            }
        }

    }

    void CheckArrived()
    {
        if (status == all_states.atHome || status == all_states.inOtherBuilding)
        {
            return;
        }
        if (transform.position.x >= workplace_x - 0.5 && transform.position.x <= workplace_x + 0.5
            && transform.position.z >= workplace_z - 0.5 && transform.position.z <= workplace_z + 0.5)
        {
            status = all_states.inOtherBuilding;
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
    }

    GameObject SwitchRoad()
    {
        List<GameObject> candidateRoads = new List<GameObject>();

        foreach (GameObject road in roadManager.roads)
        {
            if (road == curRoad)
            {
                continue;
            }

            if (road.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds))
            {
                candidateRoads.Add(road);
            }
        }

        Debug.Log("candidate count:");
        Debug.Log(candidateRoads.Count);

        if (candidateRoads.Count == 0 || candidateRoads.Count > 3)
        {
            Debug.LogError("Error in SwitchRoad");
        }

        int upRoadIndex = -1;
        int downRoadIndex = -1;
        int leftRoadIndex = -1;
        int rightRoadIndex = -1;
        for (int i = 0; i < candidateRoads.Count; i++)
        {
            if (candidateRoads[i].transform.position.x < transform.position.x - 2.5)
            {
                leftRoadIndex = i;
            } else if (candidateRoads[i].transform.position.x > transform.position.x + 2.5)
            {
                rightRoadIndex = i;
            } else if (candidateRoads[i].transform.position.z < transform.position.z - 2.5)
            {
                downRoadIndex = i;
            } else if (candidateRoads[i].transform.position.z > transform.position.z + 2.5)
            {
                upRoadIndex = i;
            }
        }

        if (transform.position.x > workplace_x)
        {
            if (leftRoadIndex != -1)
            {
                return candidateRoads[leftRoadIndex];
            }
        } else
        {
            if (rightRoadIndex != -1)
            {
                return candidateRoads[rightRoadIndex];
            }
        }

        if (transform.position.z > workplace_z)
        {
            if (downRoadIndex != -1)
            {
                return candidateRoads[downRoadIndex];
            }
        } else
        {
            if (upRoadIndex != -1)
            {
                return candidateRoads[upRoadIndex];
            }
        }

        Debug.Log("There is only road to select");
        return candidateRoads[0];
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    switch (direction)
    //    {
    //        case 0:
    //            direction = 2;
    //            break;
    //        case 1:
    //            direction = 3;
    //            break;
    //        case 2:
    //            direction = 0;
    //            break;
    //        case 3:
    //            direction = 1;
    //            break;
    //        default:
    //            break;
    //    }

    //    if(other.gameObject.GetComponent<MeshRenderer>().material.color == Color.red)
    //    {
    //        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    //    }
    //}
}
