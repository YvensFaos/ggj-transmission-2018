using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScramblerTester : MonoBehaviour {

    public string message;
    public string result;

    MessageScrambler scrambler;

    private void Awake()
    {
        scrambler = new MessageScrambler();
        scrambler.DefDict(new Dictionary<string, string>()
        {
            { "test", "rewrwe" },
            { "skin", "fsfsd" },
            { "pain", "fvxvx" },
            { "heart", "opiiop" }
        });
    }

    public void Test()
    {
        result =
            //scrambler.ScrambleRandomly(message, 2);
            //scrambler.ScrambleFromDict(message);
            //scrambler.ScrambleNumbered(message);
            scrambler.ScrambleDelimited(message);
    } // Test

    private void Start()
    {
        result = scrambler.ScrambleRandomly(message, 2);
    }

    void Update()
    {
        //result = scrambler.ScrambleFromDict(message);
    }
}
