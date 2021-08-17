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

    private bool alive;
    private bool ShieldActive;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private Vector3 snakeHeadRotation;
    public int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    private bool powerUpPresent;
    public float powerUpCoolDownTimer;
    public float DelayPowerUpTimerMax;
    public float DelayPowerUpTimer;
    private float powerUpTimer;
    public float powerUpTimerMax;

    public float gridMoveTimermax;
    private float gridMoveTimer;
    private LevelGrid levelGrid;


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
        alive = true;
        ShieldActive = false;
        DelayPowerUpTimerMax = 10f;
        DelayPowerUpTimer = DelayPowerUpTimerMax;
        powerUpTimerMax = 5f;
        powerUpTimer = powerUpTimerMax;
        powerUpPresent = false;
    }

    private void Update()
    {
        if (alive) 
        {
            HandleInput();
            HandleGridMovement();
        }
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
        HandlePowerUps();

        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimermax)
        {
            gridMoveTimer -= gridMoveTimermax;

            UpdateSnakeMovePositions();
            UpdateGridPosition();
            HasEaten();
            UpdateSnakeMovePositionList();
            UpdateSnakeTransform();
            UpdateSnakeBodyParts();
            CheckIfDead();

        }
    }

    private void HandlePowerUps()
    {
        if (!powerUpPresent)
        {
            DelayPowerUpTimer -= Time.deltaTime;
            if (DelayPowerUpTimer <= 0)
            {
                SpawnPowerUps();
                DelayPowerUpTimer = DelayPowerUpTimerMax;
                powerUpPresent = true;
            }
        }
        else
        {
            powerUpTimer -= Time.deltaTime;

            if (powerUpTimer <= 0)
            {
                DestroyPowerUps();
                powerUpPresent = false;
                powerUpTimer = powerUpTimerMax;
            }
        }
    }

    private void UpdateSnakeMovePositions()
    {
        SnakeMovePosition snakeMovePreviousPosition = null;
        if (snakeMovePositionList.Count > 0)
        {
            snakeMovePreviousPosition = snakeMovePositionList[0];
        }

        SnakeMovePosition snakeMovePosition = new SnakeMovePosition(snakeMovePreviousPosition, gridPosition, gridMoveDirection);
        snakeMovePositionList.Insert(0, snakeMovePosition);
    }

    private void UpdateGridPosition()
    {
        Vector2Int gridMoveDirectionVector;

        switch (gridMoveDirection)
        {
            default:
            case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
            case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
            case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
        }

        gridPosition += gridMoveDirectionVector;
        gridPosition = levelGrid.ValidateGridPosition(gridPosition);
    }

    private void HasEaten()
    {
        if (levelGrid.HasEatenFood(gridPosition))
        {
            snakeBodySize++;
            CreateSnakeBodyPart();
        }

        if (levelGrid.HasEatenBurner(gridPosition))
        {
            if (snakeBodySize <= 0)
            {
                GameOver();
                return;
            }
            snakeBodySize--;
            DestroySnakeBodyPart();
        }

        if (levelGrid.HasEatenShield(gridPosition))
        {
            powerUpPresent = false;
            powerUpTimer = powerUpTimerMax;
            StartCoroutine(ActivateShield());
        }
    }

    private void UpdateSnakeMovePositionList()
    {
        if (snakeMovePositionList.Count >= snakeBodySize + 1)
        {
            snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
        }
    }

    private void UpdateSnakeTransform()
    {
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        transform.eulerAngles = snakeHeadRotation;
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

    private void CheckIfDead()
    {
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
        {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
            if (gridPosition == snakeBodyPartGridPosition)
            {
                GameOver();
            }
        }
    }


    private void GameOver()
    {
        if(ShieldActive) { return; }

        alive = false; 
        Debug.Log("Game Over");
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
        public SnakeMovePosition previousSnakeMovePosition;
        public Vector2Int gridPosition;
        public Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Direction GetPreviousDirection()
        {
            if(previousSnakeMovePosition == null)
            {
                return Direction.Right;
            } else
            {
                return previousSnakeMovePosition.direction;
            }
        }

    }


    // Handles a single Snake Body Part:
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public GameObject SnakeBody;

        public SnakeBodyPart(int bodyIndex)
        {
            SnakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
            SnakeBody.GetComponent<SpriteRenderer>().sprite = GameAssetsReference.Instance.snakeBodySprite;
            SnakeBody.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = SnakeBody.transform;
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.gridPosition;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.gridPosition.x, snakeMovePosition.gridPosition.y);

            float angle;

            switch(snakeMovePosition.direction)
            {
                default:
                case Direction.Right:               //Going Right.
                    switch(snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 0; break;
                        case Direction.Up:      angle = 45; break;              //Previously Going Up.
                        case Direction.Down:    angle = -45; break;              //Previously Going Down.
                    }
                    break;
                case Direction.Down:                      //Going Down.
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 90; break;
                        case Direction.Left: angle = 45; break;              //Previously Going Left.
                        case Direction.Right: angle = -45; break;             //Previously Going Right.
                    }
                    break;
                case Direction.Left:                     //Going Left.
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 0; break;
                        case Direction.Up: angle = -45; break;              //Previously Going Left.
                        case Direction.Down: angle = 45; break;              //Previously Going Left.
                    }
                    break;
                case Direction.Up:                      //Going Up.         
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 90; break;
                        case Direction.Left: angle = -45; break;              //Previously Going Left.
                        case Direction.Right: angle = 45; break;              //Previously Going Left.
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0,0,angle);
            
        }

    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void DestroySnakeBodyPart()
    {
        Object.Destroy(snakeBodyPartList[snakeBodyPartList.Count-1].SnakeBody);
        snakeBodyPartList.RemoveAt(snakeBodyPartList.Count-1);
    }


    IEnumerator ActivateShield()
    {
        ShieldActive = true;
        yield return new WaitForSeconds(powerUpCoolDownTimer);
        ShieldActive = false;
    }

    private void SpawnPowerUps()
    {
        levelGrid.SpawnRandomPowerUps();
    }

    private void DestroyPowerUps()
    {
        levelGrid.DestroyPowerUps();
    }

}

