using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    private event Action SpawnPlayer;
    public void InvokeSpawnPlayer() => SpawnPlayer?.Invoke();

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
    }

    private void OnPlayerSpawn()
    {
        Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        SpawnPlayer -= OnPlayerSpawn;
    }
}