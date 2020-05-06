using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum all_states
{
    atHome = 0,
    onRoad = 1,
    inOtherBuilding = 2
}

public class AgentMove : MonoBehaviour
{
    private int direction;
    private float speed;
    public float workplace_x = 0;
    public float workplace_z = 0;
    public float house_x;
    public float house_z;
    private float inHouseRange;
    private bool onRoad = false;
    private bool onFirstRoad = false;
    private float distTravelled = 0;
    private RoadManager roadManager;
    private GameObject curRoad;

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

        if (onRoad)
        {
            distTravelled += Time.deltaTime * speed;
        }


        //if (shouldMove)
        //{
        //    if (curRoad.transform.rotation.y == 90)
        //    {
        //        transform.Translate(Vector3.right * Time.deltaTime * speed);
        //    } else
        //    {
        //        transform.Translate(Vector3.forward * Time.deltaTime * -speed);
        //    }

        //}
    }

    public void SetOnRoad(GameObject road, Vector3 startPos)
    {
        transform.position = startPos;
        curRoad = road;
        onRoad = true;
        onFirstRoad = true;
        distTravelled = 0;
    }

    void UpdateSpeed()
    {
        if (onRoad)
        {
            speed = 2.0f;
        } else
        {
            speed = 10.0f;
        }
    }

    void UpdateDirection()
    {
        if (!onRoad)
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


        if (onFirstRoad)
        {

            if (transform.position.x > workplace_x)
            {
                direction = 3;
            } else
            {
                direction = 1;
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        switch (direction)
        {
            case 0:
                direction = 2;
                break;
            case 1:
                direction = 3;
                break;
            case 2:
                direction = 0;
                break;
            case 3:
                direction = 1;
                break;
            default:
                break;
        }

        if(other.gameObject.GetComponent<MeshRenderer>().material.color == Color.red)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
