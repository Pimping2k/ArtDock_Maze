using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class MazeGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject cellPrefab;

    private readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    private CellController[,] grid;
    private int[,] maze;

    private void Awake()
    {
        grid = new CellController[width, height];
    }

    private void Start()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        if (grid != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] != null)
                    {
                        Destroy(grid[x, y].gameObject);
                    }
                }
            }
        }

        grid = new CellController[width, height];

        InstantiateMaze();
        RemoveWallWithBacktracking(grid);
    }

    private void InstantiateMaze()
    {
        CellController cellControllerComponent;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var instance = Instantiate(cellPrefab, new Vector3(x, 0, y), Quaternion.identity);
                cellControllerComponent = instance.GetComponent<CellController>();
                cellControllerComponent.X = x;
                cellControllerComponent.Y = y;
                grid[x, y] = cellControllerComponent;
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
}