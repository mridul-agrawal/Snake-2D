using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private LevelGrid levelGridInstance;
    [SerializeField] private Snake snakeInstance;

    void Start()
    {
        levelGridInstance = new LevelGrid(20,20);
        snakeInstance.Setup(levelGridInstance);
        levelGridInstance.Setup(snakeInstance);

        levelGridInstance.SpawnMassGainer();
        levelGridInstance.SpawnMassBurner();
    }

}
