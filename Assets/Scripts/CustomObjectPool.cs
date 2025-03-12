using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject poolPrefab;
    [SerializeField] private int initialSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            AddObjecToPool();
        }
    }

    private void AddObjecToPool()
    {
        GameObject go = Instantiate(poolPrefab);
        go.SetActive(false);
        pool.Add(go);
    }

    public GameObject GetFromPool()
    {
        foreach (var go in pool)
        {
            if (!go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }

        GameObject newObj = Instantiate(poolPrefab);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
    }
}