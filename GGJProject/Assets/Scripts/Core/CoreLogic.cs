using UnityEngine;
using UnityEngine.UI;

public class CoreLogic : MonoBehaviour
{
    public Text MessageText;
    public OptionButtonLogic[] optionButtons;
    public CallButtonLogic[] callButtons;
        
    public MessageScrambler messageScrambler;

    private void Awake()
    {
        messageScrambler = new MessageScrambler();
    }

    private void Start()
    {
        StaticData.Instance.coreLogic = this;
    }

    private void Update()
    {

    }

    public void CallPhone(GameMessageNode messageNode)
    {
        foreach(CallButtonLogic callButton in callButtons)
        {
            if(callButton.CurrentCallState == CallState.WAITING)
            {
                callButton.ChangeCallState(CallState.CALLING, messageNode);
            }
        }
    }

    public void ActivateMessage(GameMessageNode messageNode)
    {
        MessageText.text = messageScrambler.Scramble(messageNode);
        messageNode.reveleadWords++;

        optionButtons[0].SetGamePhraseOption(messageNode.options[0]);
        optionButtons[1].SetGamePhraseOption(messageNode.options[1]);
        optionButtons[2].SetGamePhraseOption(messageNode.options[2]);
    }

    private void OnDestroy()
    {
        StaticData.Instance.coreLogic = null;
    }
}