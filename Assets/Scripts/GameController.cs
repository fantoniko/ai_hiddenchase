using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] Transform PlayerSpawnPoint;
    
    [Header("Controllers")]
    [SerializeField] CoverController CoverController;
    [SerializeField] AiController AiController;

    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        AiController.OnFixedUpdate();
    }
    
    void Init()
    {
        var player = SpawnPlayer();
        
        CoverController.Init();
        AiController.Init(CoverController, player.transform);
    }
    
    GameObject SpawnPlayer()
    {
        return Instantiate(PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
    }
}
