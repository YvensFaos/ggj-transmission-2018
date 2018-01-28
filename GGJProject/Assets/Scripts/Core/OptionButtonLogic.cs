using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[RequireComponent(typeof(Text))]
public class OptionButtonLogic : MonoBehaviour
{
    public Text OptionText;
    public GamePhraseOption gamePhraseOption;

    void Start()
    {
        if (OptionText == null)
        {
            OptionText = GetComponent<Text>();
        }
    }

    void Update()
    {
    }

    public void SetGamePhraseOption(GamePhraseOption gamePhraseOption)
    {
        this.gamePhraseOption = gamePhraseOption;
        OptionText.text = gamePhraseOption.optionPhrase;
    }
}