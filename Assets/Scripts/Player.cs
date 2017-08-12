using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IScoreBehavior
{
    void AddExperience(int increaseAmount);
}

public class Player : MonoBehaviour,IScoreBehavior, IKillable {

    [SerializeField]
    private int score;

    private void OnEnable()
    {
        score = 0;
    }

    void IScoreBehavior.AddExperience(int increaseAmount)
    {
        score += increaseAmount;
    }

    void IKillable.Die()
    {
        GM.Instance.EndGame();
    }

    public int GetScore()
    {
        return score;
    }
}
