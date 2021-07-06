using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameAssets : MonoBehaviour
{

    public static GameAssets ob;
    private void Awake()
    {
        ob = this;
    }
    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite SnakeBody;


}
