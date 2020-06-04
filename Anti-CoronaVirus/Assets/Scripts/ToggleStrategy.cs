using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleStrategy : MonoBehaviour
{
    private Toggle toggle;
    private Strategy strategy;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate
        {
            SetStrategy();
        });

        strategy = GameObject.Find("Game Manager").GetComponent<Strategy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetStrategy()
    {
        if(gameObject.name == "Stay At Home Toggle")
        {
            if (toggle.isOn)
            {
                strategy.SetStayHome();
            }
            else
            {
                strategy.UndoStayHome();
            }
        }
        else
        {
            if (toggle.isOn)
            {
                strategy.SetPutOnMask();
            }
            else
            {
                strategy.UndoPutOnMask();
            }
        }
    }
}
