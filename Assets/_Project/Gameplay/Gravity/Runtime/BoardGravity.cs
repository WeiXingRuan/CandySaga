using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGravity : MonoBehaviour
{
    public List<CandyMove> ApplyGravity(BoardState board)
    {
        List<CandyMove> moves = new();

        for (int x = 0; x < board.Width; x++)
        {
            int emptyY = -1;

            for (int y = 0; y < board.Height; y++)
            {
                Cell cell = board.Cells[x, y];

                if (cell.Candy == null)
                {
                    if (emptyY == -1)
                        emptyY = y;
                }
                else if (emptyY != -1)
                {
                    board.Cells[x, emptyY].Candy = cell.Candy;
                    cell.Candy = null;

                    moves.Add(new CandyMove(
                        x,
                        y,
                        x,
                        emptyY
                    ));

                    emptyY++;
                }
            }
        }

        return moves;
    }
}
