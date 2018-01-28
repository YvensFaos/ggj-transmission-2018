using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoreLogic : MonoBehaviour
{
    public Text MessageText;

    public MessageBarController messageBarController;
    public PowerBarLogic powerBarLogic;

    public OptionButtonLogic[] optionButtons;
    public CallButtonLogic[] callButtons;
        
    public MessageScrambler messageScrambler;
    public GameMessageNode currentGameMessageNode;
    public CallButtonLogic currentButton;
    public EffectManager effectManager;

    public GameObject chargeScreen;
    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    public bool gameEnded;

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
        gameEnded = false;
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

        currentGameMessageNode = messageNode;

        optionButtons[0].SetGamePhraseOption(messageNode.options[0]);
        optionButtons[1].SetGamePhraseOption(messageNode.options[1]);
        optionButtons[2].SetGamePhraseOption(messageNode.options[2]);

        powerBarLogic.LoseEnergy(5);
    }

    public void RepeatMessage()
    {
        if(currentGameMessageNode.repeat >= currentGameMessageNode.reveleadWords)
        {
            ++currentGameMessageNode.reveleadWords;
            MessageText.text = messageScrambler.Scramble(currentGameMessageNode);
            powerBarLogic.LoseEnergy(10);
        }
        else
        {
            TimedOut();
            powerBarLogic.LoseEnergy(15);
        }
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

        powerBarLogic.LoseEnergy(5);
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

    public void RechargeGenerator()
    {
        chargeScreen.SetActive(true);
        StartCoroutine("RechargeCoroutine");
    }

    private IEnumerator RechargeCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        powerBarLogic.Recharge(Random.Range(30, 51));
        chargeScreen.SetActive(false);
    }

    public void GameOver()
    {
        if(!gameEnded)
        {
            gameOverScreen.SetActive(true);
            gameEnded = true;
        }
    }

    public void Victory()
    {
        if (!gameEnded)
        {
            victoryScreen.SetActive(true);
            gameEnded = true;
        }
    }

    public void ExitToMainMenu()
    {
        StaticData.Instance.CallLoadingScene("MainMenu");
    }

    private void OnDestroy()
    {
        StaticData.Instance.coreLogic = null;
    }
}