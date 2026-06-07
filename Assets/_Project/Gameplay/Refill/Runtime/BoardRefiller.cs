using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardRefiller
{
    private readonly CandyDatabase candyDatabase;
    private readonly System.Random random;

    public BoardRefiller(CandyDatabase candyDatabase)
    {
        this.candyDatabase = candyDatabase;
        random = new System.Random();
    }

    public List<CandySpawn> Refill(BoardState board)
    {
        List<CandySpawn> spawns = new();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Cell cell = board.Cells[x, y];

                if (cell.Candy != null)
                    continue;

                CandyData data = GetRandomSpawnableCandy();
                Candy candy = new Candy(data);

                cell.Candy = candy;

                spawns.Add(new CandySpawn(x, y, candy));
            }
        }

        return spawns;
    }

    private CandyData GetRandomSpawnableCandy()
    {
        List<CandyData> spawnableCandies = new();

        foreach (CandyData candy in candyDatabase.Candies)
        {
            if (candy.CanSpawn)
                spawnableCandies.Add(candy);
        }

        return spawnableCandies[
            random.Next(spawnableCandies.Count)
        ];
    }
}