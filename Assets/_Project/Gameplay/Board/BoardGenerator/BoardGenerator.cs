using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator 
{
    public void Generate(BoardState board)
    { 
        for (int x = 0; x < board.Width; x++)
        {
            var randomType = (CandyType)Random.Range(0, System.Enum.GetValues(typeof(CandyType)).Length);
            for (int y = 0; y < board.Height; y++)
            {
                board.Cells[x, y] = new Cell
                {
                    X = x,
                    Y = y,
                    Candy = new Candy(randomType)
                };
            }
        }
    }
}
