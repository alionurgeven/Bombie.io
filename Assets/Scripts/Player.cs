using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface IScoreBehavior
{
    void AddScore(int increaseAmount);
    void DropScore(int decreaseAmount);
}

public class Player : MonoBehaviour,IScoreBehavior, IKillable {

    [SerializeField]
    private int score;
    
    public Text PlayerScoreText;

    private void OnEnable()
    {
        //TODO ALi StartBigger score ayaralaması
        score = 0;
        PlayerScoreText.text = "" + score;
    }

    void IScoreBehavior.AddScore(int increaseAmount)
    {
        score += increaseAmount;
        PlayerScoreText.text = "" + score;
    }

    void IScoreBehavior.DropScore(int decreaseAmount)
    {
        score -= decreaseAmount;
        PlayerScoreText.text = "" + score;
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
