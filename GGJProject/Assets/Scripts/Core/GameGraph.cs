using System.Collections.Generic;
using System.IO;

using UnityEngine;

[System.Serializable]
public class GamePhraseOption
{
    public string optionPhrase;
    public int effect;
    public int path;

    public GamePhraseOption(string optionPhrase, int effect, int path)
    {
        this.optionPhrase = optionPhrase;
        this.effect = effect;
        this.path = path;
    }
}

[System.Serializable]
public class GameNode
{
    public string phrase;
    public string scrambledPhrase;
    public GamePhraseOption[] options;
    public List<GameNode> nodes;

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

/// <summary>
///
/// </summary>
[System.Serializable]
public class GameMessageNode
{
    public int index;
    public int order;
    public int hpath;
    public string message;
    public GamePhraseOption[] options;
    public List<GameMessageNode> nodes;

    public GameMessageNode(int index, int order, int hpath, string message)
    {
        this.index = index;
        this.order = order;
        this.hpath = hpath;
        this.message = message;
        options = new GamePhraseOption[3];
        nodes = new List<GameMessageNode>();

        Debug.Log("Created node: " + index + " " + order + " " + hpath + " " + message);
    }
}

/// <summary>
/// This class shall keep all the information and path of one single game story.
/// </summary>
[System.Serializable]
public class GameStory
{
    public string storyName;
    public int storyId;

    public GameMessageNode root;
    private GameMessageNode[] orderedNodes;

    public GameStory(string storyName, int storyId, JSONObject singleStoryJSON)
    {
        this.storyName = storyName;
        this.storyId = storyId;
        Init(singleStoryJSON);
    }

    private void Init(JSONObject singleStoryJSON)
    {
        List<JSONObject> jsons = singleStoryJSON["Messages"].list;
        List<JSONObject> options;
        List<JSONObject> parentsJSON;
        GameMessageNode pointer;
        int index;
        int order;
        int hpath;

        orderedNodes = new GameMessageNode[jsons.Count];

        foreach (JSONObject obj in jsons)
        {
            index = (int)obj["Idh"].i;
            order = (int)obj["Order"].i;
            hpath = (int)obj["Hpath"].i;
            parentsJSON = obj["Before"].list;

            if (root == null)
            {
                root = new GameMessageNode(index, order, hpath, obj["Msg"].str);
                orderedNodes[0] = root;
                pointer = root;
            }
            else
            {
                GameMessageNode newMessageNode = new GameMessageNode(index, order, hpath, obj["Msg"].str);
                for (int i = 0; i < parentsJSON.Count; i++)
                {
                    orderedNodes[(int)parentsJSON[i].i].nodes.Add(newMessageNode);
                }
                orderedNodes[index] = newMessageNode;
                pointer = newMessageNode;
            }
            options = obj["opts"].list;
            for (int i = 0; i < options.Count; i++)
            {
                pointer.options[i] = new GamePhraseOption(options[i]["opts"].str, (int)options[i]["effect"].i, (int)options[i]["effect"].i);
            }
        }
    }
}

public class GameGraph : MonoBehaviour
{
    public GameNode rootNode;
    public GameStory[] stories;
    public static string gameJsonFile = "gameStories0.json";

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

        List<JSONObject> jsons = mainObject.list[0].list;
        stories = new GameStory[jsons.Count];

        int i = 0;
        foreach (JSONObject obj in jsons)
        {
            stories[i] = new GameStory(obj["StoryName"].str, (int)obj["StoryID"].i, obj);
        }
    }
}