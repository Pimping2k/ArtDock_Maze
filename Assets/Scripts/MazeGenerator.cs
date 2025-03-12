using System;
using Unity.Mathematics;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject cellPrefab;

    private int[,] maze;

    public void GenerateMaze()
    {
        maze = new int[width, height];
        
    }

    private void Start()
    {
        InstantiateMaze();
    }

    private void InstantiateMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var instance = Instantiate(cellPrefab, new Vector3(x, 0, y), quaternion.identity);
            }
        }
    }
}