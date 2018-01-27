using System.Collections.Generic;

public enum GameOptionResult
{
    POSITIVE, NEUTER, NEGATIVE
}

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

public class GameNode
{
    public string phrase;
    public List<GamePhraseOption> options;

    public GameNode(string phrase, List<GamePhraseOption> options)
    {
        this.phrase = phrase;
        this.options.AddRange(options);
    }
}

public class GameGraph
{

}
