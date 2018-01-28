using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[RequireComponent(typeof(Text))]
public class OptionButtonLogic : MonoBehaviour
{
    public Text OptionText;
    public GamePhraseOption gamePhraseOption;
    public bool isActive;

    void Start()
    {
        if (OptionText == null)
        {
            OptionText = GetComponent<Text>();
        }
        BlankGamePhraseOption();
    }

    public void SetGamePhraseOption(GamePhraseOption gamePhraseOption)
    {
        this.gamePhraseOption = gamePhraseOption;
        OptionText.text = gamePhraseOption.optionPhrase;
        isActive = true;
    }

    public void BlankGamePhraseOption()
    {
        gamePhraseOption = null;
        OptionText.text = MessageScrambler.NOISE;
        isActive = false;
    }

    public void PickMe()
    {
        if(isActive)
        {
            StaticData.Instance.coreLogic.OptionPicked(gamePhraseOption);
        }
    }
}