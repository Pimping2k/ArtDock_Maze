using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject enemyPool;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject playerInstance;

    public GameObject PlayerInstance => playerInstance;

    private GameObject enemyPoolInstance;

    public GameObject EnemyPoolInstance
    {
        get => enemyPoolInstance;
        private set => enemyPoolInstance = value;
    }

    public Transform SpawnPoint
    {
        get => spawnPoint;
        set => spawnPoint = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeEnemyPool();
    }

    private void InitializeEnemyPool()
    {
        enemyPoolInstance = Instantiate(enemyPool);
    }

    public void SpawnPlayer()
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
    
    public void RespawnPlayer()
    {
        playerInstance.transform.position = spawnPoint.position;
    }

    public void FinishGame()
    {
        MazeGenerator.InvokeRegenerateMaze(true);
        
    }
}