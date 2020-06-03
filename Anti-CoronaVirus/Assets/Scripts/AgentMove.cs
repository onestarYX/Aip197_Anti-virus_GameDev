/* This file controls how the ball(agent) will move in the map. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* These are all the possible states that an agent can have. Their state can be
   only be one of them. */
public enum all_states
{
    atHome = 0,
    onRoad = 1,
    atWorkPlace = 2,
    atHospital = 3
}

public class AgentMove : MonoBehaviour
{
    public int direction = 3;
    private float speed;
    // Agent's workplace position.
    public GameObject workPlace;
    public float workplace_x;
    public float workplace_z;
    public GameObject workplace_startRoad;
    // Agent's house's position.
    public GameObject house;
    public float house_x;
    public float house_z;
    public GameObject house_startRoad;

    public int gotoWork_min;
    public int gotoWork_hour;
    public int goHome_min;
    public int goHome_hour;

    public float start_x;
    public float start_z;
    public float dest_x;
    public float dest_z;

    // The distance/range that an agent can move away from the center of their houses. Whenever the agent
    // moves out of the range, we will invert its direction.
    private float inHouseRange;
    // The status of the agent, which is initialized to be atHome.
    public all_states status = all_states.atHome;
    // The bool flag indicating whether or not the agent is on the first road of its trip.
    public bool onFirstRoad = false;
    // The max distance the agent can travel on its first road of its trip.
    public float firstRoadDistToCross;
    // The distance that the agent has travelled on the current road. Will reset to 0 whenever the agent
    // moves to a new road.
    public float distTravelled = 0;
    // The reference to the RoadManager class.
    private RoadManager roadManager;
    // The reference to the GameManager class.
    private GameManager gameManager;
    // The road that the agent is currently moving on.
    public GameObject curRoad;
    private Health healthScript;

    // Start is called before the first frame update
    void Start()
    {
        // Assign the RoadManager object after the game starts.
        roadManager = GameObject.Find("Roads").GetComponent<RoadManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        healthScript = GetComponent<Health>();
        gotoWork_min = Random.Range(0, 60);
        gotoWork_hour = 7;
        goHome_min = Random.Range(0, 60);
        goHome_hour = 18;
    }

    // Update is called once per frame
    void Update()
    {
        if (status == all_states.atHospital)
        {
            return;
        }

        // Up-down angle
        // 0: up
        // 1: right
        // 2: down
        // 3: left

        if (gameManager.minute == gotoWork_min && gameManager.hour == gotoWork_hour && status == all_states.atHome)
        {
            SetOnRoad(house_startRoad, house_x, house_z, workplace_x, workplace_z);
        }

        if (gameManager.minute == goHome_min && gameManager.hour == goHome_hour && status == all_states.atWorkPlace)
        {
            SetOnRoad(workplace_startRoad, workplace_x, workplace_z, house_x, house_z);
        }

        UpdateDirection();
        CheckArrived();
        UpdateSpeed();

        switch (direction)
        {
            case 0:
                transform.Translate(Vector3.forward * speed);
                break;
            case 1:
                transform.Translate(Vector3.right * speed);
                break;
            case 2:
                transform.Translate(Vector3.forward * -speed);
                break;
            case 3:
                transform.Translate(Vector3.right * -speed);
                break;
            default:
                break;
        }

        // Keep the distance that the agent has travelled on the current road.
        if (status == all_states.onRoad)
        {
            distTravelled += speed;
        }

    }


    /* This is the method that is used to initialize the workplace and house coordinates by the agent's house object. */
    public void Setup(GameObject house_p, GameObject workPlace_p, float inHouseRange_p)
    {
        house = house_p;
        PeopleInitiator peopleInitiatorScript = house.GetComponent<PeopleInitiator>();
        house_startRoad = peopleInitiatorScript.startRoad;
        house_x = peopleInitiatorScript.startPos.x;
        house_z = peopleInitiatorScript.startPos.z;

        workPlace = workPlace_p;
        WorkPlace workPlaceScript = workPlace.GetComponent<WorkPlace>();
        workplace_startRoad = workPlaceScript.nearestRoad;
        workplace_x = workPlaceScript.addressOnRoad.x;
        workplace_z = workPlaceScript.addressOnRoad.z;

        inHouseRange = inHouseRange_p;
    }

    /* This is method to change the agent's status from atHome to onRoad. Called by the agent's house object.
     * Param: 
     *      road -- the first road that the agent will travel on
     *      startPos -- the position at the first road that the agent will start at.
     */
    public void SetOnRoad(GameObject road, float start_x_p, float start_z_p, float dest_x_p, float dest_z_p)
    {
        start_x = start_x_p;
        start_z = start_z_p;
        dest_x = dest_x_p;
        dest_z = dest_z_p;
        Vector3 startPos = new Vector3(start_x, transform.position.y, start_z);
        transform.position = startPos;
        curRoad = road;
        status = all_states.onRoad;
        onFirstRoad = true;
        distTravelled = 0;
    }


    /* This is the method to update the agent's speed. The speed will change according to the agent's status.
     * This method will be called in every frame. */
    void UpdateSpeed()
    {
        if (status == all_states.atHome || status == all_states.atWorkPlace)
        {
            speed = 0.04f;
        } else if (status == all_states.atHospital)
        {
            speed = 0;
        } else
        {
            speed = 0.2f;
        }
    }

    /* This is the method which updates the agent's direction. This method will be called in every frame.
     * When do we need to change the agent's direction? Overall there are two scenarios:
     *      1. Agent is atHome or atWorkPlace: In this case we just invert the agent's direction whenever
     *         the agent is out of range.
     *      2. Agent is on the trip to some place:
     *         Then there are three cases that we need to change its direction, or set its direction:
     *              a) Agent is on it first road of its trip. Since each road only has two directions, the
     *                 agent needs to decide which direction to move according to its position and its
     *                 destination's position.
     *              b) Agent is at the intersection of the first road and second road of its trip. The agent
     *                 needs to know which road to switch to, thus which direction to go.
     *              c) Agent is at the intersection of any two roads it encounter later in its trip.
     *                 Note: Why do we seperate case b) from this case? The reason is that on the first road, since
     *                 the agent doesn't necessarily start at one end of the road, instead it can start at any position of
     *                 the road, the distance we need to check whether or not the agent has reached a crossing
     *                 is not necessarily the whole length of the road. On the other hand, at any later crossing, the agent
     *                 knows that the distance it will travel until the next time it changes its direction (i.e. reach another
     *                 crossing) is exactly the length of the current road.
     */
    void UpdateDirection()
    {
        if (status == all_states.atHospital)
        {
            return;
        }

        // If the agent is at home, we just check whether or not the agent is out of range. If it is, invert its current direction.
        if (status == all_states.atHome)
        {
            if (transform.position.x > house.transform.position.x + inHouseRange)
            {
                direction = 3;
            } else if (transform.position.x < house.transform.position.x - inHouseRange)
            {
                direction = 1;
            } else if (transform.position.z > house.transform.position.z + inHouseRange)
            {
                direction = 2;
            } else if (transform.position.z < house.transform.position.z - inHouseRange)
            {
                direction = 0;
            }

            return;
        }

        // If the agent is in some other building, we just check whether or not the agent is out of range. If it is, invert its current direction.
        if (status == all_states.atWorkPlace)
        {
            if (transform.position.x > workPlace.transform.position.x + inHouseRange)
            {
                direction = 3;
            }
            else if (transform.position.x < workPlace.transform.position.x - inHouseRange)
            {
                direction = 1;
            }
            else if (transform.position.z > workPlace.transform.position.z + inHouseRange)
            {
                direction = 2;
            }
            else if (transform.position.z < workPlace.transform.position.z - inHouseRange)
            {
                direction = 0;
            }

            return;
        }

        // Set the direction when the agent start its trip on the first road of its trip.
        if (onFirstRoad && distTravelled == 0)
        {
            // For horizontal road.
            if (curRoad.transform.rotation.eulerAngles.y == 90)
            {
                if (transform.position.x > dest_x)
                {
                    direction = 3;
                    // Calculate the max distance the agent can travel on its first road of the trip.
                    firstRoadDistToCross = transform.position.x - (curRoad.transform.position.x - curRoad.GetComponent<Collider>().bounds.size.x / 2) - curRoad.GetComponent<Collider>().bounds.size.z / 2;
                } else
                {
                    direction = 1;
                    firstRoadDistToCross = (curRoad.transform.position.x + curRoad.GetComponent<Collider>().bounds.size.x / 2) - curRoad.GetComponent<Collider>().bounds.size.z / 2 - transform.position.x;
                }
            // For vertical road.
            } else
            {
                if (transform.position.z > dest_z)
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

        // Change the direction and the road the agent is binding when the agent reaches THE CROSSING OF THE FIRST
        // ROAD AND THE SECOND ROAD. We should use firstRoadDistToCross we calculated earlier to check whether or
        // not the agent has reached this first crossing.
        if (onFirstRoad && distTravelled >= firstRoadDistToCross)
        {
            curRoad = SwitchRoad();
            onFirstRoad = false;
            // Reset distTravelled to 0 since the agent is switching to a new road.
            distTravelled = 0;
            float roadLen = Mathf.Abs(curRoad.GetComponent<Collider>().bounds.size.x - curRoad.GetComponent<Collider>().bounds.size.z);
            if (curRoad.transform.position.x < transform.position.x - 2.5)
            {
                direction = 3;
                transform.position = new Vector3(curRoad.transform.position.x + roadLen / 2, transform.position.y, curRoad.transform.position.z);
            } else if (curRoad.transform.position.x > transform.position.x + 2.5)
            {
                direction = 1;
                transform.position = new Vector3(curRoad.transform.position.x - roadLen / 2, transform.position.y, curRoad.transform.position.z);
            } else if (curRoad.transform.position.z < transform.position.z - 2.5)
            {
                direction = 2;
                transform.position = new Vector3(curRoad.transform.position.x, transform.position.y, curRoad.transform.position.z + roadLen / 2);
            } else
            {
                direction = 0;
                transform.position = new Vector3(curRoad.transform.position.x, transform.position.y, curRoad.transform.position.z - roadLen / 2);
            }
            return;
        }

        // Change the direction and the road the agent is binding when the agent reaches ANY LATER CROSSINGS.
        // Now the max distance we are checking against distTravelled should be the length of the road.
        if (!onFirstRoad && distTravelled >= Mathf.Abs(curRoad.GetComponent<Collider>().bounds.size.x - curRoad.GetComponent<Collider>().bounds.size.z))
        {
            curRoad = SwitchRoad();
            distTravelled = 0;
            float roadLen = Mathf.Abs(curRoad.GetComponent<Collider>().bounds.size.x - curRoad.GetComponent<Collider>().bounds.size.z);
            if (curRoad.transform.position.x < transform.position.x - 2.5)
            {
                direction = 3;
                transform.position = new Vector3(curRoad.transform.position.x + roadLen / 2, transform.position.y, curRoad.transform.position.z);
            }
            else if (curRoad.transform.position.x > transform.position.x + 2.5)
            {
                direction = 1;
                transform.position = new Vector3(curRoad.transform.position.x - roadLen / 2, transform.position.y, curRoad.transform.position.z);
            }
            else if (curRoad.transform.position.z < transform.position.z - 2.5)
            {
                direction = 2;
                transform.position = new Vector3(curRoad.transform.position.x, transform.position.y, curRoad.transform.position.z + roadLen / 2);
            }
            else if (curRoad.transform.position.z > transform.position.z + 2.5)
            {
                direction = 0;
                transform.position = new Vector3(curRoad.transform.position.x, transform.position.y, curRoad.transform.position.z - roadLen / 2);
            } else
            {
                Debug.LogError("Error in updating direction");
            }
        }

    }

    /* This is the method which checks whether or not an agent has arrived its destination. This method is called
     * in every frame. */
    void CheckArrived()
    {
        if (status == all_states.atHome || status == all_states.atWorkPlace)
        {
            return;
        }
        if (transform.position.x >= dest_x - 1.5 && transform.position.x <= dest_x + 1.5
            && transform.position.z >= dest_z - 1.5 && transform.position.z <= dest_z + 1.5)
        {
            if (dest_x == workplace_x)
            {
                status = all_states.atWorkPlace;
                transform.position = new Vector3(workPlace.transform.position.x, transform.position.y, workPlace.transform.position.z);
            } else if (dest_x == house_x)
            {
                status = all_states.atHome;
                transform.position = new Vector3(house.transform.position.x, transform.position.y, house.transform.position.z);
            } else
            {
                status = all_states.atHospital;
                float spawnX = Random.Range(-20, 20);
                float spawnZ = Random.Range(-20, 20);
                transform.position = new Vector3(healthScript.hospitalToGo.transform.position.x + spawnX, transform.position.y, healthScript.hospitalToGo.transform.position.z + spawnZ);
                healthScript.SetDetected();
            }

        }
    }

    /* This method is called whenever an agent reaches a crossing and needs to switch to another road
     * to continue their trip. The method basically checks what are the candidate roads to switch, i.e.
     * which roads on the map are connected to the current crossing. Then the agent determines which
     * road to switch based on their current location and their destination. */
    GameObject SwitchRoad()
    {
        // The list of candidate roads
        List<GameObject> candidateRoads = new List<GameObject>();

        // Loop through all the roads on the map to determine which roads are connected to the current road.
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

        //Debug.Log("candidate count:");
        //Debug.Log(candidateRoads.Count);

        if (candidateRoads.Count == 0 || candidateRoads.Count > 3)
        {
            Debug.LogError("Error in SwitchRoad");
        }

        // Determine which road in the candidate roads list is the left, or the right,
        // or the up, or the down road with respect to the current road.
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

        int goodRoadIdx1 = -1;
        int goodRoadIdx2 = -1;
        if (transform.position.x > dest_x)
        {
            if (leftRoadIndex != -1)
            {
                goodRoadIdx1 = leftRoadIndex;
            }
        } else
        {
            if (rightRoadIndex != -1)
            {
                goodRoadIdx1 = rightRoadIndex;
            }
        }

        if (transform.position.z > dest_z)
        {
            if (downRoadIndex != -1)
            {
                goodRoadIdx2 = downRoadIndex;
            }
        } else
        {
            if (upRoadIndex != -1)
            {
                goodRoadIdx2 = upRoadIndex;
            }
        }

        if(goodRoadIdx1 == -1 && goodRoadIdx2 == -1)
        {
            //Debug.Log("There is only one road to select");
            return candidateRoads[0];
        } else if(goodRoadIdx1 != -1 && goodRoadIdx2 == -1)
        {
            return candidateRoads[goodRoadIdx1];
        } else if(goodRoadIdx1 == -1 && goodRoadIdx2 != -1)
        {
            return candidateRoads[goodRoadIdx2];
        } else
        {
            if(Random.Range(0.0f, 1.0f) < 0.5f)
            {
                return candidateRoads[goodRoadIdx1];
            } else
            {
                return candidateRoads[goodRoadIdx2];  
            }
        }


    }

}
