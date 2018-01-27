using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MessageScrambler {

    Dictionary<string, string> m_dict;

    public void DefDict (Dictionary<string, string> dict)
    {
        m_dict = dict;
    } // DefDict

    public void SetDict(string msg, string code)
    {
        if (m_dict.ContainsKey(msg))
        {
            m_dict[msg] = code;
        } else
        {
            m_dict.Add(msg, code); 
        }
    } // SetDict

    public string ScrambleFromDict(string message)
    {
        string[] words = message.Split(' ');
        List<string> ret = new List<string>(words.Length);
        foreach (string word in words)
        {
            ret.Add(m_dict.ContainsKey(word) ? m_dict[word] : word);
        }

        return String.Join(" ", ret.ToArray());
    } // ScrambleFromDict

    public string ScrambleRandomly(string message, int count)
    {
        string ret = message;

        for (int i = 0; i < count; i++)
        {
            int point = UnityEngine.Random.Range(0, ret.Length);
            if (message[point] == ' ') point++;
            string left = ret.Substring(0, point);
            string right = ret.Substring(point, ret.Length - point);
            int last = left.LastIndexOf(' ');
            int first = right.IndexOf(' ');
            ret = 
                ( last < 0 ? "" : left.Substring(0, last + 1) ) 
                + "$$$$" + 
                ( first < 0 ? "" : right.Substring(first) )
            ;
            Debug.Log(ret);
        }

        return ret;
    } // ScrambleRandomly

    public string ScrambleNumbered(string message)
    {
        string ret = message;

        for (char c = '1'; c <= '9'; c++)
        {
            string[] words;
            try
            {
                words = ExtractGroupedWords(ret, c);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                throw;
            }
            if (words.Length == 0) continue;

            int selectedWordIdx = UnityEngine.Random.Range(0, words.Length);
            string selectedWord = words[selectedWordIdx];
            Debug.Log("Selected word: " + selectedWord);
            ret = ret.Replace(c.ToString(), "");
            ret = ret.Replace(selectedWord, "$$$$");
            Debug.Log(ret);
        }

        return ret;
    } // ScrambleNumbered

    string[] ExtractGroupedWords(string message, char sep)
    {
        string[] words = message.Split(sep);
        if (words.Length % 2 == 0)
        {
            throw new Exception("The " + sep + " markings don't match right in " + message);
        }

        List<string> ret = new List<string>();
        for (int i = 1; i < words.Length - 1; i+=2)
        {
            ret.Add(words[i]);
        }

        return ret.ToArray();
    } // ExtractGroupedWords
} 
