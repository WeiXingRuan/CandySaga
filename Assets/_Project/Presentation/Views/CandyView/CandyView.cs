using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CandyView : MonoBehaviour
{
    public Candy Candy { get; private set; }
    private SpriteRenderer spriteRenderer;
    public int X { get; private set; }
    public int Y { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(Candy candy, int x, int y)
    {
        Candy = candy;

        X = x;
        Y = y;

        spriteRenderer.sprite = candy.Data.Sprite;
    }
    public void SetGridPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}
