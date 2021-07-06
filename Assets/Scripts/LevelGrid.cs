using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    private Vector2Int _foodGridPos;
    private GameObject _foodGameObj;
    private int width;
    private int height;
    private Snake _snake;

    public LevelGrid(int wid, int hei)
    {
        width = wid;
        height = hei;
    }

    public void Setup(Snake s)
    {
        _snake = s;
        SpawnFood();

    }

    private void SpawnFood()
    {
        do
        {
            _foodGridPos = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (_snake.GetAllGridPos().IndexOf(_foodGridPos) != -1);

        _foodGameObj = new GameObject("Food", typeof(SpriteRenderer));
        _foodGameObj.GetComponent<SpriteRenderer>().sprite = GameAssets.ob.foodSprite;
        _foodGameObj.transform.position = new Vector3(_foodGridPos.x, _foodGridPos.y,1);
    }

    public bool SnakeMove(Vector2Int SnakeGridPos)
    {
        if (SnakeGridPos == _foodGridPos)
        {
            Destroy(_foodGameObj);
            SpawnFood();
            GameHandler.updateScore();
            return true;
        }

        return false;
    }

    public Vector2Int ValidateGridPos(Vector2Int gridPos)
    {
        if (gridPos.x > width)
        {
            gridPos.x = gridPos.x % width;
        }else if(gridPos.x < 0)
        {
            gridPos.x = width - 1;
        }
        else if (gridPos.y > height)
        {
            gridPos.y = gridPos.y % height;
        }
        else if (gridPos.y < 0)
        {
            gridPos.y = height - 1;
        }
        return gridPos;
    }
    
}
