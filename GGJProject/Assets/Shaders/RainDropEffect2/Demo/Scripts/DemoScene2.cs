using UnityEngine;
using System.Collections;

public class DemoScene2 : MonoBehaviour {

	[SerializeField]
	RainCameraController frozenRain;

	[SerializeField]
	AudioSource windAudio;

	[SerializeField]
	float frozenValue = 0f;

	enum PlayMode {
		None = 0,
		Blood = 1,
		SplashIn = 2,
		SplashOut = 3,
		Frozen = 4,
	};

	PlayMode playMode = 0;
    float rainAlpha = 1f;


    private void Awake () 
	{
        // For mobile optimization, we should reduce the resolution on iOS & Android
#if UNITY_IOS || UNITY_ANDROID
		SetResolution (512);
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Application.targetFrameRate = 60;
#endif
    }


    private void SetResolution (int resolution)
	{
		float screenRate = Mathf.Min (1f, (float)resolution/Screen.height);
		int width = (int)(Screen.width * screenRate);
		int height = (int)(Screen.height * screenRate);
		Screen.SetResolution (width, height, true, 15);
	}


    private void StopAll () 
	{
		frozenRain.StopImmidiate();
		windAudio.Stop ();
	}


    private void Start () 
	{
		frozenRain.Play ();
		windAudio.Play ();
		frozenRain.Alpha = frozenValue;
		windAudio.volume = frozenValue;
		playMode = PlayMode.Frozen;
	}

	private void Update ()
	{
		frozenRain.Alpha = frozenValue;
		windAudio.volume = frozenValue;
	}


    private bool GuiButton (string buttonName) 
	{
		return GUILayout.Button (buttonName, GUILayout.Height (40), GUILayout.Width (150));
	}
}
