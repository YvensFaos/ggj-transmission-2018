using UnityEngine;
using UnityEngine.UI;

public enum CallState
{
    CALLING, ANSWERING, WAITING
}

[System.Serializable]
[RequireComponent(typeof(Image))]
public class CallButtonLogic : MonoBehaviour {

    public CallState CurrentCallState;

    public Color AnsweringColor;
    public Color CallingColor;
    public Color WaitingColor;

    public Image CallImage;

    void Start () {
        if (CallImage == null)
        {
            CallImage = GetComponent<Image>();
        }

        ChangeCallState(CallState.WAITING);
    }
	
    public void ChangeCallState(CallState newCallState)
    {
        CurrentCallState = newCallState;

        switch(CurrentCallState)
        {
            case CallState.ANSWERING:
                CallImage.CrossFadeColor(AnsweringColor, 1.0f, false, true);
                break;
            case CallState.CALLING:
                CallImage.CrossFadeColor(CallingColor, 1.0f, false, true);
                break;
            case CallState.WAITING:
                CallImage.CrossFadeColor(WaitingColor, 1.0f, false, true);
                break;
        }
    }

    public void ClickCallButton()
    {
        switch (CurrentCallState)
        {
            case CallState.CALLING:
                break;
            default:
                //TODO play a buzz sound or something
                break;
        }
    }
}
