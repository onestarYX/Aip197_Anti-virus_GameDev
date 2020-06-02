using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContact : MonoBehaviour
{
    public float infectionP = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<MeshRenderer>().material.color == Color.red)
        {
            if (Random.Range(0.0f, 1.0f) < infectionP)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
