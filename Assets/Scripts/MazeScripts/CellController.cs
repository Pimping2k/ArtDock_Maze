using System;
using UnityEngine;


public class CellController : MonoBehaviour
{
    [SerializeField] private GameObject northWall;
    [SerializeField] private GameObject southWall;
    [SerializeField] private GameObject eastWall;
    [SerializeField] private GameObject westWall;
    [SerializeField] private GameObject floor;

    public enum Direction
    {
        North,
        South,
        West,
        East
    }

    public bool Visited = false;
    public int X;
    public int Y;

    public void SetCellAsPath()
    {
        northWall.SetActive(false);
        southWall.SetActive(false);
        eastWall.SetActive(false);
        westWall.SetActive(false);
    }

    public void SetCellAsWall()
    {
        northWall.SetActive(true);
        southWall.SetActive(true);
        eastWall.SetActive(true);
        westWall.SetActive(true);
    }

    public void OpenWallTowards(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                if (northWall.activeSelf) northWall.SetActive(false);
                break;
            case Direction.South:
                if (southWall.activeSelf) southWall.SetActive(false);
                break;
            case Direction.East:
                if (eastWall.activeSelf) eastWall.SetActive(false);
                break;
            case Direction.West:
                if (westWall.activeSelf) westWall.SetActive(false);
                break;
        }
    }
}