﻿using UnityEngine;
using UnityEngine.UI;

public class CoreLogic : MonoBehaviour
{
    public Text MessageText;
    public OptionButtonLogic[] optionButtons;

    private void Start()
    {
        StaticData.Instance.coreLogic = this;
    }

    private void Update()
    {

    }

    public void CallPhone(GameMessageNode messageNode)
    {

    }

    public void ActivateMessage(GameMessageNode messageNode)
    {
        MessageText.text = messageNode.message;
        optionButtons[0].SetGamePhraseOption(messageNode.options[0]);
        optionButtons[1].SetGamePhraseOption(messageNode.options[1]);
        optionButtons[2].SetGamePhraseOption(messageNode.options[2]);
    }

    private void OnDestroy()
    {
        StaticData.Instance.coreLogic = null;
    }
}