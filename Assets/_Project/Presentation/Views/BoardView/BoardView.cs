using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CandyView candyPrefab;
    [SerializeField] private CandyDatabase candyDatabase;
    [SerializeField] private float cellSize = 1.4f;

    private CandyView[,] candyViews;
    public float CellSize => cellSize;
    public void Render(BoardState board)
    {
        candyViews = new CandyView[board.Width, board.Height];
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Cell cell = board.Cells[x, y];
                CandyData candyData = candyDatabase.GetCandyData(cell.Candy.Type);
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                CandyView candyView = Instantiate(candyPrefab,position, Quaternion.identity, transform);
                candyView.Setup(candyData,x, y);
                candyViews[x, y] = candyView;

            }
        }
    }
    public void SwapViews(CandyView first, CandyView second)
    {
        int firstX = first.X;
        int firstY = first.Y;

        int secondX = second.X;
        int secondY = second.Y;

        candyViews[firstX, firstY] = second;
        candyViews[secondX, secondY] = first;

        first.SetGridPosition(secondX, secondY);
        second.SetGridPosition(firstX, firstY);

        first.transform.position = GetWorldPosition(secondX, secondY);
        second.transform.position = GetWorldPosition(firstX, firstY);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, y * cellSize, 0);
    }
}
