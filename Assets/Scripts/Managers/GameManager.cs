using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject enemyPool;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private event Action SpawnPlayer;
    private event Action RespawnPlayer;
    public void InvokeSpawnPlayer() => SpawnPlayer?.Invoke();
    public void InvokeRespawnPlayer() => RespawnPlayer?.Invoke();

    private GameObject playerInstance;
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
        SpawnPlayer += OnPlayerSpawn;
        RespawnPlayer += OnPlayerRespawn;
        InitializeEnemyPool();
    }

    private void InitializeEnemyPool()
    {
        enemyPoolInstance = Instantiate(enemyPool);
    }

    private void OnPlayerSpawn()
    {
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log(playerInstance, this);
    }

    private void OnDestroy()
    {
        SpawnPlayer -= OnPlayerSpawn;
        RespawnPlayer -= OnPlayerRespawn;
    }

    private void OnPlayerRespawn()
    {
        playerInstance.transform.position = spawnPoint.position;
    }
}