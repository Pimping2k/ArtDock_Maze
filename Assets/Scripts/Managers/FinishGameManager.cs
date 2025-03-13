using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float duration;

    private Coroutine fadeInRoutine;
    private static event Action FinishGame;
    public static void InvokeFinishGame() => FinishGame?.Invoke();

    private void Awake()
    {
        FinishGame += HandleFadeIn;
        _canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            InvokeFinishGame();
    }

    private void HandleFadeIn()
    {
        if (fadeInRoutine == null)
        {
            fadeInRoutine = StartCoroutine(FadeIn());
        }
        else
        {
            StopCoroutine(fadeInRoutine);
            fadeInRoutine = null;
        }
    }

    private void OnDestroy()
    {
        FinishGame -= HandleFadeIn;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (duration > elapsedTime)
        {
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = 1f;
    }
}