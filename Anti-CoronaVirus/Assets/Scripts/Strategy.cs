using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strategy : MonoBehaviour
{
    public bool stayHome = false;
    public bool putOnMask = false;
    public float obeySahRate = SimulationModel.obeySahRate;
    public float obeyPomRate = SimulationModel.obeyPomRate;

    // Start is called before the first frame update
    void Start()
    {
        SimulationModelInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SimulationModelInit()
    {
        obeyPomRate = SimulationModel.obeyPomRate;
        obeySahRate = SimulationModel.obeySahRate;
    }

    public void SetStayHome()
    {
        stayHome = true;
    }

    public void UndoStayHome()
    {
        stayHome = false;
    }

    public void SetPutOnMask()
    {
        putOnMask = true;
    }

    public void UndoPutOnMask()
    {
        putOnMask = false;
    }

    public bool ShouldStayAtHome()
    {
        if(!stayHome)
        {
            return false;
        }
        else
        {
            if(Random.Range(0f, 1f) < obeySahRate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool HasPutOnMask()
    {
        if(!putOnMask)
        {
            return false;
        }
        else
        {
            if(Random.Range(0f, 1f) < obeyPomRate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
