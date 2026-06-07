using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardGenerator
{
    private readonly CandyDatabase database;
    private readonly System.Random random;

    public BoardGenerator(CandyDatabase database)
    {
        this.database = database;
        random = new System.Random();
    }

    public BoardState Generate(int width, int height)
    {
        BoardState board = new BoardState(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CandyData data = GetRandomCandyWithoutStartingMatch(board, x, y);

                board.Cells[x, y] = new Cell
                {
                    X = x,
                    Y = y,
                    Candy = new Candy(data)
                };
            }
        }

        return board;
    }

    private CandyData GetRandomCandyWithoutStartingMatch(BoardState board, int x, int y)
    {
        List<CandyData> validCandies = GetSpawnableCandies();

        RemoveHorizontalMatchType(board, x, y, validCandies);
        RemoveVerticalMatchType(board, x, y, validCandies);

        return validCandies[random.Next(validCandies.Count)];
    }

    private List<CandyData> GetSpawnableCandies()
    {
        List<CandyData> spawnable = new List<CandyData>();

        foreach (CandyData candy in database.Candies)
        {
            if (candy.CanSpawn)
                spawnable.Add(candy);
        }

        return spawnable;
    }

    private void RemoveHorizontalMatchType(BoardState board, int x, int y, List<CandyData> validCandies)
    {
        if (x < 2) return;

        Candy left1 = board.Cells[x - 1, y].Candy;
        Candy left2 = board.Cells[x - 2, y].Candy;

        if (left1 == null || left2 == null) return;

        if (left1.Data.Type == left2.Data.Type)
        {
            validCandies.RemoveAll(candy =>
                candy.Type == left1.Data.Type);
        }
    }

    private void RemoveVerticalMatchType(BoardState board, int x, int y, List<CandyData> validCandies)
    {
        if (y < 2) return;

        Candy down1 = board.Cells[x, y - 1].Candy;
        Candy down2 = board.Cells[x, y - 2].Candy;

        if (down1 == null || down2 == null) return;

        if (down1.Data.Type == down2.Data.Type)
        {
            validCandies.RemoveAll(candy =>
                candy.Type == down1.Data.Type);
        }
    }
}