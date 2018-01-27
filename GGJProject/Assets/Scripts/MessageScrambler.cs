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
            Debug.Log(point);
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

} 
