using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationModel : MonoBehaviour
{
    // Population
    public static int pplPerHouse = 5;
    // Virus parameters
    public static float normTransP = 0.1f;
    public static float initialInfectedRate = 0.01f;
    public static float fatalityRate = 0.05f;
    public static int normIncubationT = 21600;
    public static int normCureT = 43200;
    // People's behavior
    public static float goForTestP = 0.75f;
    public static int normContectsPerHr = 1;
    // Strategy Settings
    // Put on Mask (Pom)
    public static float normTransPMask = 0.1f;
    public static float obeyPomRate = 0.8f;
    // Stay at home (Sah)
    public static float obeySahRate = 0.75f;
}
