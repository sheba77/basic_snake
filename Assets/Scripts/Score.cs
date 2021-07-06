using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text _score;

    private void Awake()
    {
        _score = GameObject.FindGameObjectWithTag("scoreText").GetComponent<Text>();
        Debug.Log("Score is: "+ _score.text);
    }

    private void Update()
    {
        _score.text = GameHandler.getScore().ToString();
    }
}

