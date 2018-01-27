using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[RequireComponent(typeof(Text))]
public class OptionButtonLogic : MonoBehaviour
{
    public string OptionMessage;
    public Text OptionText;

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
}