using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private int score;

    private void OnEnable()
    {
        score = 0;
    }

    public void AddScore(int increaseAmount)
    {
        score += increaseAmount;
    }

    public int GetScore()
    {
        return score;
    }
}
