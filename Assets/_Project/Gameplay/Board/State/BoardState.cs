using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState 
{
    public int Width { get; }
    public int Height { get; }

    
    public Cell[,] Cells;
    public BoardState(int width, int height)
    {
        Width = width;
        Height = height;
        
        Cells = new Cell[width, height];
    }
    
}
