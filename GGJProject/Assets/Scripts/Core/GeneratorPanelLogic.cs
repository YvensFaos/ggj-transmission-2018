using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorPanelLogic : MonoBehaviour {

    [Header("Settings")]
    public float TimeToShowMessage;
    private CountdownEventDispacher Countdown;

	// Use this for initialization
	void Start () {
        //OpenGeneratorPanel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenGeneratorPanel()
    {
        Countdown = gameObject.AddComponent<CountdownEventDispacher>();
        Countdown.CountdownTimer = TimeToShowMessage;
        Countdown.MethodsToInvoke = CountdownCallback;
    }

    public void CountdownCallback()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OpenGeneratorPanel();
    }
}
