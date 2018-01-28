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
    public GameMessageNode messageNode;

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
	
    public void ChangeCallState(CallState newCallState, GameMessageNode messageNode = null)
    {
        CurrentCallState = newCallState;
        if(messageNode != null)
        {
            this.messageNode = messageNode;
        }

        switch (CurrentCallState)
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
                if(StaticData.Instance.coreLogic.currentButton != null && StaticData.Instance.coreLogic.currentButton != this)
                {
                    StaticData.Instance.coreLogic.currentButton.ChangeCallState(CallState.WAITING);
                    //TODO add logic that activates the bad answers for this call.
                }
                StaticData.Instance.coreLogic.ActivateMessage(this, messageNode);
                ChangeCallState(CallState.ANSWERING);
                break;
            default:
                //TODO play a buzz sound or something
                break;
        }
    }
}
