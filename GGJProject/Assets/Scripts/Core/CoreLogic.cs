using UnityEngine;
using UnityEngine.UI;

public class CoreLogic : MonoBehaviour
{
    public Text MessageText;
    public MessageBarController messageBarController;
    public OptionButtonLogic[] optionButtons;
    public CallButtonLogic[] callButtons;
        
    public MessageScrambler messageScrambler;
    public GameMessageNode currentGameMessageNode;
    public CallButtonLogic currentButton;

    public float PositiveScore;
    public float NegativeScore;
    private static float DefaultScoreValue = 10.0f;

    private void Awake()
    {
        messageScrambler = new MessageScrambler();
    }

    private void Start()
    {
        StaticData.Instance.coreLogic = this;
        ResetMessageText();
    }

    public void CallPhone(GameMessageNode messageNode)
    {
        foreach(CallButtonLogic callButton in callButtons)
        {
            if(callButton.CurrentCallState == CallState.WAITING)
            {
                callButton.ChangeCallState(CallState.CALLING, messageNode);
                break;
            }
        }
    }

    public void ActivateMessage(CallButtonLogic callButton, GameMessageNode messageNode)
    {
        currentButton = callButton;
        MessageText.text = messageScrambler.Scramble(messageNode);

        messageBarController.Restart();
        messageBarController.isActive = true;

        messageNode.reveleadWords++;
        currentGameMessageNode = messageNode;

        optionButtons[0].SetGamePhraseOption(messageNode.options[0]);
        optionButtons[1].SetGamePhraseOption(messageNode.options[1]);
        optionButtons[2].SetGamePhraseOption(messageNode.options[2]);
    }

    public void OptionPicked(GamePhraseOption gamePhraseOption)
    {
        ScoreOption(gamePhraseOption);
        StaticData.Instance.gameGraph.timeline.CallNextEvent(currentGameMessageNode, gamePhraseOption);
        messageBarController.StopBar();
        currentButton.ChangeCallState(CallState.WAITING);
        currentButton = null;

        ResetMessageText();
        optionButtons[0].BlankGamePhraseOption();
        optionButtons[1].BlankGamePhraseOption();
        optionButtons[2].BlankGamePhraseOption();
    }

    public void TimedOut()
    {
        int randomOption = Random.Range(0, 3);
        OptionPicked(optionButtons[randomOption].gamePhraseOption);
    }

    public void ScoreOption(GamePhraseOption gamePhraseOption)
    {
        if(gamePhraseOption.effect == 0)
        {
            PositiveScore += DefaultScoreValue;
        }
        else
        {
            NegativeScore += DefaultScoreValue;
        }
    }

    private void ResetMessageText()
    {
        MessageText.text = MessageScrambler.NOISE;
    }

    private void OnDestroy()
    {
        StaticData.Instance.coreLogic = null;
    }
}