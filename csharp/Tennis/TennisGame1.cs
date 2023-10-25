using System;
using System.Linq;

namespace Tennis;

public record PlayerEntity
{
    public string Name { get; }

    public int Score { get; private set; }

    public PlayerEntity(string Name, int Score)
    {
        this.Name = Name;
        this.Score = Score;
    }

    public string GetScoreText()
    {
        return (ScoreValue)Score switch
        {
            ScoreValue.Love => ScoreConstants.Love,
            ScoreValue.Fifteen => ScoreConstants.Fifteen,
            ScoreValue.Thirty => ScoreConstants.Thirty,
            ScoreValue.Forty => ScoreConstants.Forty,
            _ => throw new NotImplementedException()
        };
    }

    public void IncreaseScore(int amount)
    {
        Score += amount;
    }

    public string GetNeutralScoreText()
    {
        var score = (ScoreValue)Score;

        return score switch
        {
            ScoreValue.Love => ScoreConstants.LoveAll,
            ScoreValue.Fifteen => ScoreConstants.FifteenAll,
            ScoreValue.Thirty => ScoreConstants.ThirtyAll,
            _ => ScoreConstants.Deuce
        };
    }
}

public static class ScoreConstants
{
    public const string Win = "Win for";
    public const string Advantage = "Advantage";
    public const string LoveAll = "Love-All";
    public const string FifteenAll = "Fifteen-All";
    public const string ThirtyAll = "Thirty-All";
    public const string Deuce = "Deuce";
    public const string Love = "Love";
    public const string Fifteen = "Fifteen";
    public const string Thirty = "Thirty";
    public const string Forty = "Forty";
}

public enum ScoreValue
{
    Love = 0,
    Fifteen = 1,
    Thirty = 2,
    Forty = 3
}

public class TennisGame1 : ITennisGame
{
    private readonly PlayerEntity[] players = new PlayerEntity[2];

    public TennisGame1(string player1Name, string player2Name)
    {
        players[0] = new PlayerEntity(player1Name, 0);
        players[1] = new PlayerEntity(player2Name, 0);
    }

    public void WonPoint(string playerName)
    {
        PlayerEntity foundPlayer = players.FirstOrDefault(player => player.Name == playerName);

        foundPlayer?.IncreaseScore(1);
    }

    private static string GetPlayerWinText(string playerName)
    {
        return $"{ScoreConstants.Win} {playerName}";
    }

    private static string GetPlayerAdvantageText(string playerName)
    {
        return $"{ScoreConstants.Advantage} {playerName}";
    }

    public string GetScore()
    {
        if (IsNeutralScore())
        {
            return players[0].GetNeutralScoreText();
        }

        if (IsMatchmakingScore())
        {
            return GetMatchMakingScoreText();
        }

        return GetIntermittentScoreText();
    }

    private string GetIntermittentScoreText()
    {
        return $"{players[0].GetScoreText()}-{players[1].GetScoreText()}";
    }

    private string GetMatchMakingScoreText()
    {
        int scoreDifference = players[0].Score - players[1].Score;
        string leadingPlayerName = scoreDifference > 0 
            ? players[0].Name 
            : players[1].Name;

        int absScoreDifference = Math.Abs(scoreDifference);

        return absScoreDifference < 2
            ? GetPlayerAdvantageText(leadingPlayerName)
            : GetPlayerWinText(leadingPlayerName);
    }

    private bool IsMatchmakingScore()
    {
        return players.Any(player => player.Score >= 4);
    }

    private bool IsNeutralScore()
    {
        return players[0].Score == players[1].Score;
    }
}