using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder
{
    public List<MatchGroup> FindMatches(BoardState board)
    {
        List<MatchGroup> matches = new();

        FindHorizontal(board, matches);
        FindVertical(board, matches); 
        return matches;
    }
     private void FindHorizontal(BoardState board, List<MatchGroup> matches)
    {
        for (int y =0; y< board.Height; y++)
        {
            int count = 1; 
            for (int x= 1; x<board.Width; x++)
            {
                CandyType current = board.Cells[x, y].Candy.Type;
                CandyType previous = board.Cells[x - 1, y].Candy.Type;
                if (current ==previous)
                {
                    count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        CreateHorizontalMatch(board, matches, x-count,y, count);


                    }
                    count = 1;
                }
                if (count >= 3)
                {
                    CreateHorizontalMatch(
                        board,
                        matches,
                        board.Width - count,
                        y,
                        count);
                }
            }

        }
    }
    private void FindVertical(
        BoardState board,
        List<MatchGroup> matches)
    {
        for (int x = 0; x < board.Width; x++)
        {
            int count = 1;

            for (int y = 1; y < board.Height; y++)
            {
                CandyType current =
                    board.Cells[x, y].Candy.Type;

                CandyType previous =
                    board.Cells[x, y - 1].Candy.Type;

                if (current == previous)
                {
                    count++;
                }
                else
                {
                    if (count >= 3)
                    {
                        CreateVerticalMatch(
                            board,
                            matches,
                            x,
                            y - count,
                            count);
                    }

                    count = 1;
                }
            }

            if (count >= 3)
            {
                CreateVerticalMatch(
                    board,
                    matches,
                    x,
                    board.Height - count,
                    count);
            }
        }
    }

    private void CreateHorizontalMatch(
        BoardState board,
        List<MatchGroup> matches,
        int startX,
        int y,
        int count)
    {
        MatchGroup group = new();

        for (int i = 0; i < count; i++)
        {
            group.Cells.Add(
                board.Cells[startX + i, y]);
        }

        matches.Add(group);
    }

    private void CreateVerticalMatch(
        BoardState board,
        List<MatchGroup> matches,
        int x,
        int startY,
        int count)
    {
        MatchGroup group = new();

        for (int i = 0; i < count; i++)
        {
            group.Cells.Add(
                board.Cells[x, startY + i]);
        }

        matches.Add(group);
    }
}
