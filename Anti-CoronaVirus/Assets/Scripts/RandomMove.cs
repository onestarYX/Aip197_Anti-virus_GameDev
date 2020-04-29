using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    private int direction;
    public float workplace_x = 0;
    public float workplace_z = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateDirection", 2, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 10.0f;
        // Up-down angle
        // 0: up
        // 1: right
        // 2: down
        // 3: left
        switch(direction)
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
        
    }

    void UpdateDirection()
    {
        int decision = Random.Range(0, 2);

        if (transform.position.x > workplace_x)
        {
            if(transform.position.z > workplace_z)
            {
                if (decision == 0)
                {
                    direction = 2;
                } else
                {
                    direction = 3;
                }
            } else
            {
                if (decision == 0)
                {
                    direction = 0;
                } else
                {
                    direction = 3;
                }
            }
        } else
        {
            if(transform.position.z > workplace_z)
            {
                if (decision == 0)
                {
                    direction = 1;
                } else
                {
                    direction = 2;
                }
            } else
            {
                if (decision == 0)
                {
                    direction = 0;
                } else
                {
                    direction = 1;
                }
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
