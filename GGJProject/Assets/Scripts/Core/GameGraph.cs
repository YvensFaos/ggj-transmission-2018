using System.Collections.Generic;
using System.IO;

using UnityEngine;

public enum GameOptionResult
{
    POSITIVE, NEUTER, NEGATIVE
}

[System.Serializable]
public class GamePhraseOption
{
    public string optionPhrase;
    public GameOptionResult optionResult;

    public GamePhraseOption(string optionPhrase, GameOptionResult optionResult)
    {
        this.optionPhrase = optionPhrase;
        this.optionResult = optionResult;
    }
}

[System.Serializable]
public class GameNode
{
    public string phrase;
    public string scrambledPhrase;
    public GamePhraseOption[] options;
    public GameNode[] nodes;

    public GameNode(string phrase, GamePhraseOption[] options)
    {
        this.phrase = phrase;
        this.options = new GamePhraseOption[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            this.options[i] = options[i];
        }

        //TODO call scramble note
    }
}

public class GameGraph : MonoBehaviour
{
    public GameNode rootNode;
    public static string gameJsonFile = "gameTestJson.json";

    private void Awake()
    {
        InitGameGraph();
    }

    private void InitGameGraph()
    {
        string path = Path.Combine(Application.dataPath, gameJsonFile);
        StreamReader reader = new StreamReader(path);
        string jsonString = reader.ReadToEnd();
        JSONObject mainObject = new JSONObject(jsonString);

        Stack<JSONObject> stackJson = new Stack<JSONObject>();
        stackJson.Push(mainObject);
        JSONObject pointer;
        while (stackJson.Count > 0)
        {
            pointer = stackJson.Pop();
            //TODO read json graph
        }
    }
}