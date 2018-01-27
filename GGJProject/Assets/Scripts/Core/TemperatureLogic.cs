using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TemperatureLogic : MonoBehaviour {

    public Text TemperatureText;

    void Start () {
        if (TemperatureText == null)
        {
            TemperatureText = GetComponent<Text>();
        }
    }

    public void ControlWeather(float newTemperature)
    {
        TemperatureText.text = newTemperature.ToString() + "ºC";

        //TODO logic that controls the weathery effects
    }
}
