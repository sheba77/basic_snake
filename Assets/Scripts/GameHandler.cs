using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    private static int _score;
    [SerializeField] private Snake _snake;
    private LevelGrid _levelGrid;

    private void Awake()
    {
        instance = this;
        initStatic();
    }

    void Start()
    {
        _levelGrid = new LevelGrid(20, 20);
        _snake.setup(_levelGrid);
        _levelGrid.Setup(_snake);
    }

    public static int getScore()
    {
        return _score;
    }
    public static void updateScore()
    {
         _score += 100;
    }

    public void initStatic()
    {
        _score = 0;
    }
    
}
