using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSwapper
{
    public void Swap(BoardState board, int firstX, int firstY, int secondX, int secondY)
    {
        Candy firstCandy = board.Cells[firstX, firstY].Candy;
        Candy secondCandy = board.Cells[secondX, secondY].Candy;

        board.Cells[firstX, firstY].Candy = secondCandy;
        board.Cells[secondX, secondY].Candy = firstCandy;
    }
}
