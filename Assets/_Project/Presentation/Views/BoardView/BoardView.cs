using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CandyView candyPrefab;
    [SerializeField] private CandyDatabase candyDatabase;
    [SerializeField] private float cellSize = 1.4f;
    public float CellSize => cellSize;
    public void Render(BoardState board)
    {
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Cell cell = board.Cells[x, y];
                CandyData candyData = candyDatabase.GetCandyData(cell.Candy.Type);
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                CandyView candyView = Instantiate(candyPrefab,position, Quaternion.identity, transform);
                candyView.Setup(candyData.Sprite);
                
            }
        }
    }
}
