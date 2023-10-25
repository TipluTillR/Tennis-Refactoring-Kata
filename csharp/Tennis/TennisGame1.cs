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
        return Score switch
        {
            0 => ScoreConstants.Love,
            1 => ScoreConstants.Fifteen,
            2 => ScoreConstants.Thirty,
            3 => ScoreConstants.Forty,
            _ => throw new NotImplementedException()
        };
    }

    public void IncreaseScore(int amount)
    {
        Score += amount;
    }

    public string GetNeutralScoreText()
    {
        return Score switch
        {
            0 => ScoreConstants.LoveAll,
            1 => ScoreConstants.FifteenAll,
            2 => ScoreConstants.ThirtyAll,
            _ => ScoreConstants.Deuce
        };
    }
    
    public string GetPlayerWinText()
    {
        return $"{ScoreConstants.Win} {Name}";
    }

    public string GetPlayerAdvantageText()
    {
        return $"{ScoreConstants.Advantage} {Name}";
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

    public string GetScore()
    {
        if (IsNeutralScore())
        {
            return players[0].GetNeutralScoreText();
        }

        return IsMatchmakingScore() 
            ? GetMatchMakingScoreText() 
            : GetIntermittentScoreText();
    }
    
    private bool IsNeutralScore()
    {
        return players[0].Score == players[1].Score;
    }

    private bool IsMatchmakingScore()
    {
        return players.Any(player => player.Score >= 4);
    }

    private bool IsAdvantageScore()
    {
        int scoreDifference = players[0].Score - players[1].Score;
        
        return Math.Abs(scoreDifference) < 2;
    }

    private string GetIntermittentScoreText()
    {
        return $"{players[0].GetScoreText()}-{players[1].GetScoreText()}";
    }

    private string GetMatchMakingScoreText()
    {
        PlayerEntity leadingPlayer = players.MaxBy(player => player.Score);
        bool isAdvantage = IsAdvantageScore();

        return isAdvantage
            ? leadingPlayer.GetPlayerAdvantageText()
            : leadingPlayer.GetPlayerWinText();
    }
}