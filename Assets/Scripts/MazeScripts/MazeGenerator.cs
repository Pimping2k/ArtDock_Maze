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
    
    
    [Header("Properties")] public int width;
    public int height;
    [SerializeField, Range(0, 10)] private int trapCount = 3;
    
    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private CellController[,] grid;

    private CellController exitCell;
    
    private static event Action<bool> RegenerateMaze;
    public static void InvokeRegenerateMaze(bool newMaze) => RegenerateMaze?.Invoke(newMaze);

    private void Awake()
    {
        RegenerateMaze += GenerateMaze;
    }

    private void Start()
    {
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

        if (newMaze)
        {
            width = Random.Range(width + 2, width + 7);
            height = Random.Range(width + 2, width + 7);
        }
        
        grid = new CellController[width, height];

        InstantiateMaze();
        UpdatePlayerSpawnPoint();
        RemoveWallWithBacktracking(grid);
        SetExit();
        SpawnTraps();
    }

    private void UpdatePlayerSpawnPoint()
    {
        var startPos = grid[0, 0].transform.localPosition;
        GameManager.Instance.SpawnPoint.SetPositionAndRotation(new Vector3(startPos.x, 1, startPos.z),quaternion.identity);
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
        for (int i = 0; i < trapCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            var cell = grid[x, y];
            var trapPrefab = Random.value > 0.5f ? resetPosTrap : regenerateMazeTrap;
            Instantiate(trapPrefab, cell.transform.position, Quaternion.identity, mazeContainer.transform);
        }
    }
    
    private void OnDestroy()
    {
        RegenerateMaze -= GenerateMaze;
    }
}