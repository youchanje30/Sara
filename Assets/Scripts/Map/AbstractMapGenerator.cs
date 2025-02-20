using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapGenerator : MonoBehaviour
{
    [SerializeField] protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;


    public void GenerateMap()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
