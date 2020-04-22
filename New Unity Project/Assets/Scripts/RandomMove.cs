using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateDirection", 2, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 10.0f;
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
        direction = Random.Range(0, 4);
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
