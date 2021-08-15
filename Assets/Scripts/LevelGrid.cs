using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid 
{
    private int width;
    private int height;
    private Vector2Int foodGridPosition;
    private GameObject Food;
    private Snake snake;

    public LevelGrid(int w, int h)
    {
        width = w;
        height = h;
    }

    public void Setup(Snake s)
    {
        snake = s;
    }

    public void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetAllGridPosition().Contains(foodGridPosition));

        Food = new GameObject("Food", typeof(SpriteRenderer));
        Food.GetComponent<SpriteRenderer>().sprite = GameAssetsReference.Instance.foodSprite;
        Food.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool HasEatenFood(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(Food);
            SpawnFood();
            return true;
        } else
        {
            return false;
        }
    }


}
