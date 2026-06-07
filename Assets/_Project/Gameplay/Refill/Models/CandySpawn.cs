using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawn 
{
    public int X;
    public int Y;

    public Candy Candy;

    public CandySpawn(int x, int y, Candy candy)
    {
        X = x;
        Y = y;
        Candy = candy;
    }
}
