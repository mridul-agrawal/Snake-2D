using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsReference : MonoBehaviour
{
    public static GameAssetsReference Instance;

    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;
    public Sprite massGainerSprite;
    public Sprite massBurnerSprite;
    public Sprite shieldSprite;
    public Sprite speedUpSprite;
    public Sprite scoreBoostSprite;

    private void Awake()
    {
        Instance = this;
    }



}
