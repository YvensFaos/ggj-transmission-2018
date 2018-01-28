using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MessageScrambler
{
    private const string noise = "~~~~";

    public static string NOISE
    {
        get
        {
            return noise;
        }
    }

    private Dictionary<string, string> noiseDictionary;

    /// <summary>
    /// This method shall call the scramble methods, keeping the number of scrambled words according to the reveleadWords of the messageNode parameter.
    /// It must first reveal the [ ] words, and then the 1 1 words. The 1 1 words must always keep at least 1 word visible.
    /// </summary>
    /// <param name="messageNode"></param>
    /// <returns></returns>
    public string Scramble(GameMessageNode messageNode)
    {
        //public string ScrambleNumbered(string message, int phrasesPerGroup = 1)

        string scrambledMessage = messageNode.message;
        scrambledMessage = ScrambleNumbered(scrambledMessage);
        //scrambledMessage = ScrambleDelimited(scrambledMessage);

        return scrambledMessage;
    }

    /// <summary>
    /// Define the whole dictionary.
    /// </summary>
    /// <param name="dict">the defined dictionary</param>
    public void DefDict(Dictionary<string, string> dict)
    {
        noiseDictionary = dict;
    } // DefDict

    /// <summary>
    /// Set the code for a word in the dictionary.
    /// </summary>
    /// <param name="word">Key word</param>
    /// <param name="code">the code to replace the word in the message.</param>
    public void SetDict(string word, string code)
    {
        if (noiseDictionary.ContainsKey(word))
        {
            noiseDictionary[word] = code;
        }
        else
        {
            noiseDictionary.Add(word, code);
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
            ret.Add(noiseDictionary.ContainsKey(word) ? noiseDictionary[word] : word);
        }

        return string.Join(" ", ret.ToArray());
    } // ScrambleFromDict

    /// <summary>
    /// Randomly selects the maximum count of words and replaces them in the message.
    /// </summary>
    /// <param name="message">the message to be encoded</param>
    /// <param name="count">the maximum count of words to be replaced</param>
    /// <returns>the encoded message</returns>
    public string ScrambleRandomly(string message, int count = 1)
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
                (last < 0 ? "" : left.Substring(0, last + 1))
                + NOISE +
                (first < 0 ? "" : right.Substring(first));
        }

        return ret;
    } // ScrambleRandomly

    /// <summary>
    /// Replaces one word in each of the numbered groups.
    /// </summary>
    /// <param name="message">message to be encoded</param>
    /// <returns>the encoded message</returns>
    public string ScrambleNumbered(string message, int phrasesPerGroup = 1)
    {
        int[] foundsCases = new int[10];
        int numericalValue = -1;
        for (int i = 0; i < message.Length; i++)
        {
            numericalValue = (int)char.GetNumericValue(message[i]);
            if (numericalValue >= 0 && numericalValue <= 9)
            {
                foundsCases[numericalValue]++;
            }
        }

        for (int i = 0; i < foundsCases.Length; i++)
        {
            foundsCases[i] /= 2;
        }

        int[] safeWords = new int[10];
        for (int i = 0; i < foundsCases.Length; i++)
        {
            safeWords[i] = (foundsCases[i] != 0) ? phrasesPerGroup : 0;
        }

        bool removing = false;
        bool ignore = false;
        StringBuilder stringBuilder = new StringBuilder(message);
        for (int i = 0; i < message.Length; i++)
        {
            numericalValue = (int)char.GetNumericValue(message[i]);
            if (numericalValue >= 0 && numericalValue <= 9)
            {
                if (removing)
                {
                    removing = false;
                    ignore = false;
                }
                else if (!ignore)
                {
                    bool remove = false;
                    if (safeWords[numericalValue] < foundsCases[numericalValue])
                    {
                        if (safeWords[numericalValue] > 0)
                        {
                            remove = Random.Range(0, 101) <= 25;
                        }
                        else
                        {
                            remove = true;
                        }
                    }
                    else
                    {
                        remove = false;
                    }

                    --foundsCases[numericalValue];
                    if (remove)
                    {
                        removing = true;
                    }
                    else
                    {
                        --safeWords[numericalValue];
                        ignore = true;
                    }
                }
                else if (ignore)
                {
                    ignore = false;
                }
                stringBuilder[i] = '§';
            }
            if (removing)
            {
                stringBuilder[i] = '~';
            }
        }

        stringBuilder.Replace("§", "");
        return stringBuilder.ToString(); ;
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
            foreach (string word in words)
            {
                ret = ret.Replace(word, NOISE);
            }
            foreach (char c in delimiter)
            {
                ret = ret.Replace(c.ToString(), "");
            }
        }

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
            string exceptionMessage = "The " + sep + " markings don't match right in " + message;
            StaticData.Instance.Log(exceptionMessage);
        }

        List<string> ret = new List<string>();
        for (int i = 1; i < words.Length - 1; i += 2)
        {
            ret.Add(words[i]);
        }

        return ret.ToArray();
    } // ExtractGroupedWords
}