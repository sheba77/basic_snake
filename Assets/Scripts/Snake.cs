using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    private enum Direction
    {
        Left, Right, Up, Dowm
    }
    private enum State
    {
        Alive, Dead
    }
    private Vector2Int gridPosition;
    private Direction gridMoveDir;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid _levelGrid;
    private int _snakeBodySize;
    private List<SnakeMovePosition> _SnakeMovmentPosList;
    private List<SnakeBodyPart> _SnakeBodyPartsList;
    private State state;
    

    public void setup(LevelGrid lvlGr)
    {
        _levelGrid = lvlGr;
    }
    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = .5f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDir = Direction.Right;
        _SnakeMovmentPosList = new List<SnakeMovePosition>();
        _SnakeBodyPartsList = new List<SnakeBodyPart>();
        _snakeBodySize = 0;
        state = State.Alive;

    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                move();
                handelMovement();
                break;
            case State.Dead:
                break;
        }


        
    }

    private void move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && gridMoveDir != Direction.Dowm)
        {
            gridMoveDir = Direction.Up;

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)&& gridMoveDir != Direction.Up)
        {
            gridMoveDir = Direction.Dowm;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && gridMoveDir != Direction.Right)
        {
            gridMoveDir = Direction.Left;

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && gridMoveDir != Direction.Left)
        {
            gridMoveDir = Direction.Right;

        }
        
    }

    private void handelMovement()
    {
        Vector2Int gridMoveDirVector = new Vector2Int(0,0);
        gridMoveTimer += Time.deltaTime * 2;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            SnakeMovePosition prev = null; //this is the problem 
            if (_SnakeMovmentPosList.Count > 0)
            {
                prev = _SnakeMovmentPosList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(prev, gridPosition, gridMoveDir);

            _SnakeMovmentPosList.Insert(0, snakeMovePosition) ;
            switch (gridMoveDir)
            {
                case Direction.Right:
                    gridMoveDirVector = new Vector2Int(1, 0);
                    break;
                case Direction.Left:
                    gridMoveDirVector = new Vector2Int(-1, 0);
                    break;
                case Direction.Up:
                    gridMoveDirVector = new Vector2Int(0, 1);
                    break;
                case Direction.Dowm:
                    gridMoveDirVector = new Vector2Int(0, -1);
                    break;

            }
            gridPosition += gridMoveDirVector;
            gridPosition = _levelGrid.ValidateGridPos(gridPosition);
            // Debug.Log(gridPosition.ToString());

            bool SnakeAtFood = _levelGrid.SnakeMove(gridPosition);
            if (SnakeAtFood)
            {
                //grow body
                _snakeBodySize++;
                CreatPartSnakeBody();
            }
            if (_SnakeMovmentPosList.Count >= _snakeBodySize + 1)
            {
                _SnakeMovmentPosList.RemoveAt(_SnakeMovmentPosList.Count - 1);
            }
            
            //check where body pos is 
            foreach (SnakeBodyPart snbp in _SnakeBodyPartsList)
            {
                Vector2Int bodyPartPosition = snbp.GetGridPosition();
                if (gridPosition == bodyPartPosition)
                {
                    state = State.Dead;
                    Loader.Load(Loader.Scene.mainScene);
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, getAngleFromFloat(gridMoveDirVector) - 90);

            UpdateSnakeBodyPart();
        }
    }


    private void UpdateSnakeBodyPart()
    {
        for (int i = 0; i < _SnakeBodyPartsList.Count; i++)
        {
            _SnakeBodyPartsList[i].SetSnakeMovePos(_SnakeMovmentPosList[i]);
        }
    }
    private void CreatPartSnakeBody()
    {
        _SnakeBodyPartsList.Add(new SnakeBodyPart(_SnakeBodyPartsList.Count));
    }

    private float getAngleFromFloat(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }

        return n;
    }
    public Vector2Int GetGridPos()
    {
        return gridPosition;
    }
    public List<Vector2Int> GetAllGridPos()
    {
        List<Vector2Int> gridPosList = new List<Vector2Int>(){gridPosition};
        foreach (SnakeMovePosition snp in _SnakeMovmentPosList)
        {
            gridPosList.Add(snp.GetGridPos());
        }
        return gridPosList;
    }
    
    private class SnakeBodyPart
    {
        private Transform _transform;
        private Vector2Int _gridPos;
        private SnakeMovePosition _snakeMovePosition;
        public SnakeBodyPart(int bodyIDX)
        {
            GameObject _bodyPart = new GameObject("snakeBody", typeof(SpriteRenderer));
            _bodyPart.GetComponent<SpriteRenderer>().sprite = GameAssets.ob.SnakeBody;
            _bodyPart.GetComponent<SpriteRenderer>().sortingOrder = -bodyIDX;
            _transform = _bodyPart.transform;
        }

        public void SetSnakeMovePos(SnakeMovePosition snakeMove)
        {
            _gridPos = snakeMove.GetGridPos();
            _snakeMovePosition = snakeMove;
            _transform.position = new Vector3(_gridPos.x, _gridPos.y);
            float angle = 0;
            switch (snakeMove.GetDirection())
            {
                case Direction.Up:
                    switch (_snakeMovePosition.getPrevDirection())
                    {
                        default:              angle = 0;   break;
                        case Direction.Left:  angle = 45;  break;
                        case Direction.Right: angle = -45; break;
                    }
                    break;
                case Direction.Dowm:
                    switch (_snakeMovePosition.getPrevDirection())
                    {
                        default:               angle = 180;      break;
                        case Direction.Left:   angle = 180 + 45; break;
                        case Direction.Right:  angle = 180 - 45; break;
                    }
                    break;
                case Direction.Left:
                    switch (_snakeMovePosition.getPrevDirection())
                    {
                        default:             angle = -90; break;
                        case Direction.Dowm: angle = -45; break; 
                        case Direction.Up:   angle = 45;  break;
                    }
                    break;
                case Direction.Right:
                    switch (_snakeMovePosition.getPrevDirection())
                    {
                        default:             angle = 90;  break;
                        case Direction.Dowm: angle = 45;  break;
                        case Direction.Up:   angle = -45; break;;
                    }
                    break;
            }
            _transform.eulerAngles = new Vector3(0, 0, angle);

        }

        public Vector2Int GetGridPosition()
        {
            return this._gridPos;
        }
    }
    

    private class SnakeMovePosition
    {
        private Vector2Int gridPos;
        private Direction _direction;
        private SnakeMovePosition preSnakeMovePosition;

        public SnakeMovePosition(SnakeMovePosition psmp, Vector2Int gridPos, Direction _direction)
        {
            preSnakeMovePosition = psmp;
            this.gridPos = gridPos;
            this._direction = _direction;
        }

        public Vector2Int GetGridPos()
        {
            return gridPos;
        }
        public Direction GetDirection()
        {
            return _direction;
        }
        public Direction getPrevDirection()
        {
            if (preSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            return preSnakeMovePosition._direction;
            
        }

    }

}
