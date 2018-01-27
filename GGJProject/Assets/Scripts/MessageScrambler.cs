﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MessageScrambler {

    const string NOISE = "~~~~";
    
    Dictionary<string, string> m_dict;

    /// <summary>
    /// Define the whole dictionary.
    /// </summary>
    /// <param name="dict">the defined dictionary</param>
    public void DefDict (Dictionary<string, string> dict)
    {
        m_dict = dict;
    } // DefDict

    /// <summary>
    /// Set the code for a word in the dictionary.
    /// </summary>
    /// <param name="word">Key word</param>
    /// <param name="code">the code to replace the word in the message.</param>
    public void SetDict(string word, string code)
    {
        if (m_dict.ContainsKey(word))
        {
            m_dict[word] = code;
        } else
        {
            m_dict.Add(word, code); 
        }
    } // SetDict

    /// <summary>
    /// Replace all word in the mesasge with its corresponding code, if existent in the dictionary.
    /// </summary>
    /// <param name="message">the message to be encoded</param>
    /// <returns>the encoded message</returns>
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

    /// <summary>
    /// Randomly selects the maximum count of words and replaces them in the message.
    /// </summary>
    /// <param name="message">the message to be encoded</param>
    /// <param name="count">the maximum count of words to be replaced</param>
    /// <returns>the encoded message</returns>
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
                + NOISE + 
                ( first < 0 ? "" : right.Substring(first) )
            ;
            Debug.Log(ret);
        }

        return ret;
    } // ScrambleRandomly

    /// <summary>
    /// Replaces one word in each of the numbered groups.
    /// </summary>
    /// <param name="message">message to be encoded</param>
    /// <returns>the encoded message</returns>
    public string ScrambleNumbered(string message)
    {
        string ret = message;

        for (char c = '1'; c <= '9'; c++)
        {
            string[] words = ExtractGroupedWords(ret, c);
            if (words.Length == 0) continue;

            int selectedWordIdx = UnityEngine.Random.Range(0, words.Length);
            string selectedWord = words[selectedWordIdx];
            Debug.Log("Selected word: " + selectedWord);
            ret = ret.Replace(c.ToString(), "");
            ret = ret.Replace(selectedWord, NOISE);
            Debug.Log(ret);
        }

        return ret;
    } // ScrambleNumbered

    /// <summary>
    /// Replaces all delimited words in the message.
    /// </summary>
    /// <param name="message">the source message</param>
    /// <returns>the encoded message</returns>
    public string ScrambleDelimited(string message)
    {
        List<char[]> delimiters = new List<char[]>
        {
            new char[] { '[', ']' }, 
            new char[] { '@' }
        };

        string ret = message;
        foreach (char[] delimiter in delimiters)
        {
            string[] words = ExtractGroupedWords(message, delimiter);
            foreach(string word in words)
            {
                ret = ret.Replace(word, NOISE);
            }
            foreach(char c in delimiter)
            {
                ret = ret.Replace(c.ToString(), "");
            }
        }

        Debug.Log(ret);
        return ret;
    } // ScrambleDelimited

    /// <summary>
    /// Extracts the group of words from the message.
    /// </summary>
    /// <param name="message">source message</param>
    /// <param name="sep">the separator</param>
    /// <returns>the array of extracted words</returns>
    private string[] ExtractGroupedWords(string message, char sep)
    {
        return ExtractGroupedWords(message, new char[] { sep });
    } // ExtractGroupedWords

    /// <summary>
    /// Extracts the group of words from the message.
    /// </summary>
    /// <param name="message">source message</param>
    /// <param name="sep">the separators array</param>
    /// <returns>the array of extracted words</returns>
    private string[] ExtractGroupedWords(string message, char[] sep)
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