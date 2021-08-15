using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsReference : MonoBehaviour
{
    public static GameAssetsReference Instance;

    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;
    public Sprite foodSprite;

    private void Awake()
    {
        Instance = this;
    }

 /*   void Start()
    {
        if (Instance = null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        Debug.Log(snakeHeadSprite.texture);
    }
 */


}
