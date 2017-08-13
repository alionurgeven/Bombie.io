using UnityEngine;

public class Bomber : MonoBehaviour, IScoreBehavior
{
    private int score;

    public void AddScore(int increaseAmount)
    {
        score += increaseAmount;
    }

    public void DropScore(int decreaseAmount)
    {
        score -= decreaseAmount;
    }

    public int GetScore()
    {
        return score;
    }
}
