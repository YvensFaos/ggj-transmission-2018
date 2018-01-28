﻿using System;
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

/// <summary>
///
/// </summary>
[System.Serializable]
public class GameMessageNode : IComparable<GameMessageNode>
{
    public int index;
    public int order;
    public int hpath;
    public int repeat;
    public string message;
    public GamePhraseOption[] options;
    public List<GameMessageNode> nodes;
    public List<GameMessageNode> parents;

    public float callTime;
    public bool isActive;
    public bool alreadyAdded;
    public bool wasPlayed;

    public int reveleadWords;

    public GameMessageNode(int index, int order, int hpath, int repeat, string message)
    {
        this.index = index;
        this.order = order;
        this.hpath = hpath;
        this.repeat = repeat;
        this.message = message;
        options = new GamePhraseOption[3];
        nodes = new List<GameMessageNode>();
        parents = new List<GameMessageNode>();
        isActive = false;
        alreadyAdded = false;
        wasPlayed = false;

        reveleadWords = 1;
        StaticData.Instance.Log("Created node: " + index + " " + order + " " + hpath + " " + message);
    }

    public int CompareTo(GameMessageNode other)
    {
        return callTime.CompareTo(other.callTime);
    }

    public void Activate()
    {
        isActive = true;
        StaticData.Instance.coreLogic.CallPhone(this);
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
        int repeat;

        orderedNodes = new GameMessageNode[jsons.Count];

        foreach (JSONObject obj in jsons)
        {
            index = (int)obj["Idh"].i;
            order = (int)obj["Order"].i;
            hpath = (int)obj["Hpath"].i;
            repeat = 1;

            if (obj.HasField("Repeat"))
            {
                repeat = (int)obj["Repeat"].i;
            }

            parentsJSON = obj["Before"].list;

            if (root == null)
            {
                root = new GameMessageNode(index, order, hpath, repeat, obj["Msg"].str);
                orderedNodes[0] = root;
                pointer = root;
            }
            else
            {
                GameMessageNode newMessageNode = new GameMessageNode(index, order, hpath, repeat, obj["Msg"].str);
                for (int i = 0; i < parentsJSON.Count; i++)
                {
                    orderedNodes[(int)parentsJSON[i].i].nodes.Add(newMessageNode);
                    newMessageNode.parents.Add(orderedNodes[(int)parentsJSON[i].i]);
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

[System.Serializable]
public class GameTimeline
{
    public GameStory[] stories;
    public float clock;
    public float minimalInterval = 1.0f;
    public float maximumInterval = 5.0f;
    public bool isActive;
    public bool isWaiting;
    private bool hasFinished;

    public bool HasFinished
    {
        get
        {
            return hasFinished;
        }

        private set
        {
            hasFinished = value;
        }
    }

    public List<GameMessageNode> timedNodes;

    private int currentIndex;

    public GameTimeline(int[] storyIndexes, GameStory[] stories)
    {
        this.stories = new GameStory[storyIndexes.Length];
        for (int i = 0; i < storyIndexes.Length; i++)
        {
            this.stories[i] = stories[storyIndexes[i]];
        }

        timedNodes = new List<GameMessageNode>();
        Queue<GameMessageNode> messageNodeQueue = new Queue<GameMessageNode>();

        //Set tutorial story
        GameStory tutorial = this.stories[0];
        messageNodeQueue.Enqueue(tutorial.root);
        GameMessageNode msgPointer;

        float tutorialMinimalInterval = 2.0f;
        float tutorialMaximumInterval = 3.2f;
        while (messageNodeQueue.Count > 0)
        {
            msgPointer = messageNodeQueue.Dequeue();
            if (!msgPointer.alreadyAdded)
            {
                msgPointer.callTime = UnityEngine.Random.Range(tutorialMinimalInterval, tutorialMaximumInterval);
                msgPointer.alreadyAdded = true;
                foreach (GameMessageNode msgNode in msgPointer.nodes)
                {
                    if (!msgNode.alreadyAdded)
                    {
                        messageNodeQueue.Enqueue(msgNode);
                    }
                }
            }
        }
        timedNodes.Add(tutorial.root);

        float startTime = timedNodes[timedNodes.Count - 1].callTime + UnityEngine.Random.Range(minimalInterval, maximumInterval);

        //Set another stories
        GameStory pointer;
        for (int i = 1; i < storyIndexes.Length; i++)
        {
            pointer = this.stories[i];

            //TODO
        }

        timedNodes.Sort();

        isActive = false;
        isWaiting = false;
        currentIndex = 0;
    }

    public void CallNextEvent(GameMessageNode messageNode, GamePhraseOption gamePhraseOption)
    {
        GameMessageNode nextMessage;
        int i = 0;
        do
        {
            nextMessage = messageNode.nodes[i];
            if(nextMessage.hpath == gamePhraseOption.path)
            {
                break;
            }
        } while (++i < messageNode.nodes.Count);

        messageNode.isActive = false;
        messageNode.wasPlayed = true;

        nextMessage.callTime = clock + UnityEngine.Random.Range(minimalInterval, maximumInterval);
        nextMessage.alreadyAdded = true;

        timedNodes.Add(nextMessage);
        //TODO adicionar método que remove todas as mensagens que foram jogadas
        timedNodes.Sort();

        isWaiting = false;
    }

    public void Update()
    {
        clock += Time.deltaTime;

        if (!isWaiting && clock >= timedNodes[currentIndex].callTime)
        {
            timedNodes[currentIndex].Activate();
            ++currentIndex;
            if (currentIndex >= timedNodes.Count)
            {
                isWaiting = true;
            }
        }
    }
}

public class GameGraph : MonoBehaviour
{
    public GameTimeline timeline;
    public GameStory[] stories;

    public static string gameJsonFile = "gameStories0.json";
    public static int maxStoriesSize = 1;//5;

    private void Awake()
    {
        InitGameGraph();
        StaticData.Instance.gameGraph = this;
    }

    private void InitGameGraph()
    {
        string path = Path.Combine(Application.dataPath, gameJsonFile);
        StreamReader reader = new StreamReader(path);
        string jsonString = reader.ReadToEnd();
        JSONObject mainObject = new JSONObject(jsonString);

        List<JSONObject> jsons = mainObject.list[0].list;
        stories = new GameStory[jsons.Count];

        StaticData.Instance.Log("Reading JSON file.");

        int i = 0;
        foreach (JSONObject obj in jsons)
        {
            stories[i] = new GameStory(obj["StoryName"].str, (int)obj["StoryID"].i, obj);
        }

        StaticData.Instance.Log("Generating Game Graph.");
        int[] storyIndexes = new int[maxStoriesSize];
        storyIndexes[0] = 0;

        timeline = new GameTimeline(storyIndexes, stories);
        timeline.isActive = true;
    }

    private void Update()
    {
        if (timeline.isActive)
        {
            timeline.Update();
        }
    }

    private void OnDestroy()
    {
        StaticData.Instance.gameGraph = null;
    }
}