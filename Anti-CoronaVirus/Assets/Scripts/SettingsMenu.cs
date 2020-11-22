using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public TextMeshProUGUI pplPerHouseText;
    public TextMeshProUGUI totalPopulationText;
    public Slider pplPerHouseSlider;

    public TMP_InputField transPInputField;
    public TMP_InputField initInfectedRateField;
    public TMP_InputField fatalityRateField;
    public TMP_InputField incubationTField;
    public TMP_InputField cureTField;

    public Slider normContactsPerHrSlider;
    public TextMeshProUGUI normContactsPerHrText;
    public TMP_InputField goForTestPField;

    public TMP_InputField transPMaskInputField;
    public TMP_InputField obeyPomField;
    public TMP_InputField obeySahField;

    private void Start()
    {
        pplPerHouseSlider.value = SimulationModel.pplPerHouse;
        pplPerHouseText.text = SimulationModel.pplPerHouse.ToString();
        totalPopulationText.text = "Total Population: " + (SimulationModel.pplPerHouse * 43).ToString();  // TODO: This is definitely subject to change!

        transPInputField.text = SimulationModel.normTransP.ToString();
        initInfectedRateField.text = SimulationModel.initialInfectedRate.ToString();
        fatalityRateField.text = SimulationModel.fatalityRate.ToString();
        incubationTField.text = (SimulationModel.normIncubationT / 3600).ToString();
        cureTField.text = (SimulationModel.normCureT / 3600).ToString();

        normContactsPerHrSlider.value = SimulationModel.normContectsPerHr;
        normContactsPerHrText.text = SimulationModel.normContectsPerHr.ToString();
        goForTestPField.text = SimulationModel.goForTestP.ToString();

        transPMaskInputField.text = SimulationModel.normTransPMask.ToString();
        obeyPomField.text = SimulationModel.obeyPomRate.ToString();
        obeySahField.text = SimulationModel.obeySahRate.ToString();
    }


    public void setPplPerHouse(float value)
    {
        SimulationModel.pplPerHouse = (int)value;
        pplPerHouseText.text = value.ToString();
        totalPopulationText.text = "Total Population: " + (value * 43).ToString();  // TODO: This is definitely subject to change!
    }

    public void setNormTransP()
    {
        float newValue = float.Parse(transPInputField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            transPInputField.text = newValue.ToString();
        }
        if(newValue > 1)
        {
            newValue = 1;
            transPInputField.text = newValue.ToString();
        }
        SimulationModel.normTransP = newValue;
    }

    public void setInitInfectedRate()
    {
        float newValue = float.Parse(initInfectedRateField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            initInfectedRateField.text = newValue.ToString();
        }
        if (newValue > 1)
        {
            newValue = 1;
            initInfectedRateField.text = newValue.ToString();
        }
        SimulationModel.initialInfectedRate = newValue;
    }

    public void setFatalityRate()
    {
        float newValue = float.Parse(fatalityRateField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            fatalityRateField.text = newValue.ToString();
        }
        if (newValue > 1)
        {
            newValue = 1;
            fatalityRateField.text = newValue.ToString();
        }
        SimulationModel.fatalityRate = newValue;
    }

    public void setIncubationT()
    {
        float newValue = float.Parse(incubationTField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            incubationTField.text = newValue.ToString();
        }
        SimulationModel.normIncubationT = (int)newValue * 3600;
    }

    public void setCureT()
    {
        float newValue = float.Parse(cureTField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            cureTField.text = newValue.ToString();
        }
        SimulationModel.normCureT = (int)newValue * 3600;
    }

    public void setNormContactsPerHr(float value)
    {
        Debug.Log(value);
        SimulationModel.normContectsPerHr = (int)value;
        normContactsPerHrText.text = value.ToString();
    }

    public void setGoForTestP()
    {
        float newValue = float.Parse(goForTestPField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            goForTestPField.text = newValue.ToString();
        }
        if (newValue > 1)
        {
            newValue = 1;
            goForTestPField.text = newValue.ToString();
        }
        SimulationModel.goForTestP = newValue;
    }

    public void setTransPMaks()
    {
        float newValue = float.Parse(transPMaskInputField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            transPMaskInputField.text = newValue.ToString();
        }
        if (newValue > 1)
        {
            newValue = 1;
            transPMaskInputField.text = newValue.ToString();
        }
        SimulationModel.normTransPMask = newValue;
    }

    public void setObeyPomRate()
    {
        float newValue = float.Parse(obeyPomField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            obeyPomField.text = newValue.ToString();
        }
        if (newValue > 1)
        {
            newValue = 1;
            obeyPomField.text = newValue.ToString();
        }
        SimulationModel.obeyPomRate = newValue;
    }

    public void setObeySahRate()
    {
        float newValue = float.Parse(obeySahField.text);
        if (newValue <= 0)
        {
            newValue = 0;
            obeySahField.text = newValue.ToString();
        }
        if (newValue > 1)
        {
            newValue = 1;
            obeySahField.text = newValue.ToString();
        }
        SimulationModel.obeySahRate = newValue;
    }
}
