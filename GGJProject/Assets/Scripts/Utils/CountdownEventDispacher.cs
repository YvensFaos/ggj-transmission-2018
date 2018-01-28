using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CountdownEventDispacher : MonoBehaviour {

    [Header("Settings")]
    public float CountdownTimer;
    private float CountdownCounter;

    public delegate void MethodDelegate();
    [Header("Events to Invoke")]
    public MethodDelegate MethodsToInvoke;

    private float _normalizedTimer;
    public float NormalizedTimer {
        get {
            return CountdownCounter / CountdownTimer;
        }
        private set {
            _normalizedTimer = value;
        }
    }

    private void Init()
    {
        CountdownCounter = CountdownTimer;
    }

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {

        CountdownCounter -= Time.deltaTime;

        if(CountdownCounter < 0)
        {
            Destroy(this);
            MethodsToInvoke.Invoke();
        }

	}
}
