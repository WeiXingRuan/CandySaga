using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CandyView : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    public int X { get; private set; }
    public int Y { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(CandyData data, int x, int y)
    {
        spriteRenderer.sprite = data.Sprite;
        X = x;
        Y = y;
    }
    public void SetGridPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}
