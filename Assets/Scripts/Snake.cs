using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    public float gridMoveTimermax;
    private float gridMoveTimer;
    private Vector3 snakeHeadRotation;
    private LevelGrid levelGrid;
    public int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public void Setup(LevelGrid lg)
    {
        levelGrid = lg;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        gridMoveDirection = Direction.Right;
        gridMoveTimermax = 0.1f;
        gridMoveTimer = gridMoveTimermax;
        snakeHeadRotation = new Vector3(0,0,90);
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();
    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }

    public void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
                snakeHeadRotation.Set(0,0,180);
            }

        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
                snakeHeadRotation.Set(0, 0, -90);
            }

        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
                snakeHeadRotation.Set(0, 0, 0);
            }

        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
                snakeHeadRotation.Set(0, 0, 90);
            }

        }

    }

    public void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimermax)
        {
            gridMoveTimer -= gridMoveTimermax;

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;

            switch(gridMoveDirection)
            {
                default:
                case Direction.Up:      gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down:    gridMoveDirectionVector = new Vector2Int(0, -1); break;
                case Direction.Right:   gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left:    gridMoveDirectionVector = new Vector2Int(-1, 0); break;
            }

            gridPosition += gridMoveDirectionVector;

            if (levelGrid.HasEatenFood(gridPosition))
            {
                snakeBodySize++;
                CreateSnakeBodyPart();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

/*            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMoveGridPosition = snakeMovePositionList[i].gridPosition;
                GameObject SnakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
                SnakeBody.GetComponent<SpriteRenderer>().sprite = GameAssetsReference.Instance.snakeBodySprite;
                SnakeBody.transform.position = new Vector3(snakeMoveGridPosition.x, snakeMoveGridPosition.y);

                Destroy(SnakeBody, gridMoveTimermax);
            }*/

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = snakeHeadRotation;

            UpdateSnakeBodyParts();

        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i].gridPosition);
        }
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetAllGridPosition()
    {
        List<Vector2Int> allGridPosition = new List<Vector2Int>() { gridPosition };
        foreach(SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            allGridPosition.Add(snakeMovePosition.gridPosition);
        }
        return allGridPosition;
    }


    //Handles one Move Position from Snake.
    private class SnakeMovePosition
    {
        public Vector2Int gridPosition;
        public Direction direction;

        public SnakeMovePosition(Vector2Int gridPosition, Direction direction)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }
    }


    // Handles a single Snake Body Part:
    private class SnakeBodyPart
    {
        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject SnakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
            SnakeBody.GetComponent<SpriteRenderer>().sprite = GameAssetsReference.Instance.snakeBodySprite;
            SnakeBody.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = SnakeBody.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x,gridPosition.y);
        }

    }

}

