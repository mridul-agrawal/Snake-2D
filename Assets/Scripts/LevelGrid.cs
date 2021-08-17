using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid 
{
    private int width;
    private int height;
    private Vector2Int foodGridPosition;
    private Vector2Int burnerGridPosition;
    private Vector2Int shieldGridPosition;
    private GameObject Food;
    private GameObject MassBurner;
    private GameObject Shield;
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

    public void SpawnMassGainer()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetAllGridPosition().Contains(foodGridPosition));

        Food = new GameObject("Food", typeof(SpriteRenderer));
        Food.GetComponent<SpriteRenderer>().sprite = GameAssetsReference.Instance.massGainerSprite;
        Food.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public void SpawnMassBurner()
    {
        List<Vector2Int> OccupiedPositions = new List<Vector2Int>();
        OccupiedPositions = snake.GetAllGridPosition();
        OccupiedPositions.Add(foodGridPosition);
        
        do
        {
            burnerGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (OccupiedPositions.Contains(burnerGridPosition));

        MassBurner = new GameObject("MassBurner", typeof(SpriteRenderer));
        MassBurner.GetComponent<SpriteRenderer>().sprite = GameAssetsReference.Instance.massBurnerSprite;
        MassBurner.transform.position = new Vector3(burnerGridPosition.x, burnerGridPosition.y);

    }

    public void SpawnShield()
    {
        List<Vector2Int> OccupiedPositions = new List<Vector2Int>();
        OccupiedPositions = snake.GetAllGridPosition();
        OccupiedPositions.Add(foodGridPosition);
        OccupiedPositions.Add(burnerGridPosition);

        do
        {
            shieldGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (OccupiedPositions.Contains(shieldGridPosition));

        Shield = new GameObject("Shield", typeof(SpriteRenderer));
        Shield.GetComponent<SpriteRenderer>().sprite = GameAssetsReference.Instance.shieldSprite;
        Shield.transform.position = new Vector3(shieldGridPosition.x, shieldGridPosition.y);
    }

    public bool HasEatenFood(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(Food);
            SpawnMassGainer();
            Object.Destroy(MassBurner);
            SpawnMassBurner();
            return true;
        } else
        {
            return false;
        }
    }

    public bool HasEatenBurner(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == burnerGridPosition)
        {
            Object.Destroy(MassBurner);
            SpawnMassBurner();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasEatenShield(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == shieldGridPosition)
        {
            Object.Destroy(Shield);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyPowerUps()
    {
        Object.Destroy(Shield);
    }

    public void SpawnRandomPowerUps()
    {
        SpawnShield();
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if(gridPosition.x < 0)
        {
            gridPosition.x = width - 1;
        }
        if(gridPosition.y < 0)
        {
            gridPosition.y = height - 1;
        }
        if(gridPosition.x > width-1)
        {
            gridPosition.x = 0; 
        }
        if(gridPosition.y > height - 1)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}
