using System;
using UnityEngine;

public class FinishGameManager : MonoBehaviour
{
    private static event Action FinishGame;
    public static void InvokeFinishGame() => FinishGame?.Invoke();

    private void Awake()
    {
        FinishGame += HandleFinish;
    }

    private void HandleFinish()
    {
        MazeGenerator.InvokeRegenerateMaze(true);
        GameManager.Instance.InvokeRespawnPlayer();
    }
}