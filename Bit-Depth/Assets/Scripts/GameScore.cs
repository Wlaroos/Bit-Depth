using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{

    public static GameScore Instance { get; private set;  }

    public int scoreMult = 1;
    private int scoreTotal;

    private TMP_Text scoreText;

    void Awake()
    {

        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        scoreText = GetComponent<TMP_Text>();
    }

    public void AddScore(int shit)
    {
        scoreTotal += (shit * scoreMult);
        scoreText.SetText(scoreTotal.ToString().PadLeft(7, '0'));
    }

    public void ChangeMult(int comboLevel)
    {
        //scoreMult = (int)(Mathf.Pow(2f, (comboLevel - 1.0f)));
        scoreMult = comboLevel;
    }

    public int GetScore()
    {
        return scoreTotal;
    }

}
