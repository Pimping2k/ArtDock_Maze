using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class MazeGenerator : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private GameObject mazeContainer;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private GameObject resetPosTrap;
    [SerializeField] private GameObject regenerateMazeTrap;

    [Header("Settings")] [SerializeField, Range(1, 100)]
    private int width;

    [SerializeField, Range(1, 100)] private int height;
    [SerializeField, Range(0, 10)] private int trapCount;
    [SerializeField, Range(0, 10)] private int enemyCount;

    private CellController[,] grid;
    private CellController exitCell;
    private CustomObjectPool enemyPool;

    private static event Action<bool> RegenerateMaze;
    public static void InvokeRegenerateMaze(bool newMaze) => RegenerateMaze?.Invoke(newMaze);

    private void Awake()
    {
        RegenerateMaze += GenerateMaze;
    }

    private void Start()
    {
        enemyPool = GameManager.Instance.EnemyPoolInstance.GetComponent<CustomObjectPool>();
        GenerateMaze(false);
        HandlePlayerSpawn();
    }

    private void HandlePlayerSpawn()
    {
        GameManager.Instance.InvokeSpawnPlayer();
        UpdatePlayerSpawnPoint();
    }

    public void GenerateMaze(bool newMaze)
    {
        ClearMaze();
        enemyPool.ReturnEverythingToPool(); 
        
        if (newMaze)
        {
            width = Random.Range(width + 2, width + 3);
            height = Random.Range(width + 2, width + 3);
        }

        grid = new CellController[width, height];

        InstantiateMaze();
        UpdatePlayerSpawnPoint();
        RemoveWallWithBacktracking(grid);
        SetExit();
        SpawnTraps();
        SpawnEnemies();
    }

    private void UpdatePlayerSpawnPoint()
    {
        var startPos = grid[0, 0].transform.localPosition;
        GameManager.Instance.SpawnPoint.SetPositionAndRotation(new Vector3(startPos.x, 1, startPos.z),
            quaternion.identity);
    }

    private void ClearMaze()
    {
        foreach (Transform child in mazeContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void InstantiateMaze()
    {
        CellController cellControllerComponent;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var instance = Instantiate(cellPrefab, new Vector3(x * 2, 0, y * 2), Quaternion.identity);
                cellControllerComponent = instance.GetComponent<CellController>();
                cellControllerComponent.X = x;
                cellControllerComponent.Y = y;
                grid[x, y] = cellControllerComponent;
                grid[x, y].transform.parent = mazeContainer.transform;
            }
        }
    }

    private void RemoveWallWithBacktracking(CellController[,] grid)
    {
        var current = grid[0, 0];
        current.Visited = true;

        Stack<CellController> stack = new Stack<CellController>();

        do
        {
            List<CellController> unvisitedNeighbours = new List<CellController>();

            int x = current.X;
            int y = current.Y;

            if (y > 0 && !grid[x, y - 1].Visited) // North
            {
                unvisitedNeighbours.Add(grid[x, y - 1]);
            }

            if (y < height - 1 && !grid[x, y + 1].Visited) // South
            {
                unvisitedNeighbours.Add(grid[x, y + 1]);
            }

            if (x > 0 && !grid[x - 1, y].Visited) // West
            {
                unvisitedNeighbours.Add(grid[x - 1, y]);
            }

            if (x < width - 1 && !grid[x + 1, y].Visited) // East
            {
                unvisitedNeighbours.Add(grid[x + 1, y]);
            }

            if (unvisitedNeighbours.Count > 0)
            {
                CellController neighbour = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, neighbour);
                stack.Push(current);
                neighbour.Visited = true;
                current = neighbour;
            }
            else if (stack.Count > 0)
            {
                current = stack.Pop();
            }
        } while (stack.Count > 0);
    }

    private void RemoveWall(CellController current, CellController neighbour)
    {
        if (current.X > neighbour.X)
        {
            current.OpenWallTowards(CellController.Direction.West);
            neighbour.OpenWallTowards(CellController.Direction.East);
        }
        else if (current.X < neighbour.X)
        {
            current.OpenWallTowards(CellController.Direction.East);
            neighbour.OpenWallTowards(CellController.Direction.West);
        }
        else if (current.Y > neighbour.Y)
        {
            current.OpenWallTowards(CellController.Direction.South);
            neighbour.OpenWallTowards(CellController.Direction.North);
        }
        else if (current.Y < neighbour.Y)
        {
            current.OpenWallTowards(CellController.Direction.North);
            neighbour.OpenWallTowards(CellController.Direction.South);
        }
    }

    private void SetExit()
    {
        exitCell = grid[width - 1, height - 1];
        exitCell.OpenWallTowards(CellController.Direction.East);
        exitCell.OpenWallTowards(CellController.Direction.West);
        exitCell.OpenWallTowards(CellController.Direction.South);
        exitCell.OpenWallTowards(CellController.Direction.North);
        Instantiate(exitPrefab, exitCell.transform.position, Quaternion.identity, mazeContainer.transform);
    }

    private void SpawnTraps()
    {
        HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();
        int totalRegenerateTraps = 0;
        int totalRespawnTraps = 0;

        for (int i = 0; i < trapCount; i++)
        {
            Vector2Int pos;
            do
            {
                int x = Random.Range(1, width - 1);
                int y = Random.Range(1, height - 1);
                pos = new Vector2Int(x, y);
            } while (occupiedCells.Contains(pos));

            occupiedCells.Add(pos);
            var cell = grid[pos.x, pos.y];

            if (totalRespawnTraps < trapCount / 2)
            {
                Instantiate(resetPosTrap, cell.transform.position, Quaternion.identity, mazeContainer.transform);
                totalRespawnTraps++;
            }
            else
            {
                Instantiate(regenerateMazeTrap, cell.transform.position, Quaternion.identity, mazeContainer.transform);
                totalRegenerateTraps++;
            }
        }
    }

    private void SpawnEnemies()
    {
        HashSet<Vector3> usedPositions = new HashSet<Vector3>();

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy;
            Vector3 spawnPos;
            do
            {
                int x = Random.Range(3, width - 1);
                int y = Random.Range(3, height - 1);
                var cell = grid[x, y];
                spawnPos = new Vector3(cell.transform.position.x, 1f, cell.transform.position.z);
            } while (usedPositions.Contains(spawnPos));

            enemy = enemyPool.GetFromPool();
            enemy.transform.position = spawnPos;
            enemy.SetActive(true);
            usedPositions.Add(spawnPos);
        }
    }

    private void OnDestroy()
    {
        RegenerateMaze -= GenerateMaze;
    }
}