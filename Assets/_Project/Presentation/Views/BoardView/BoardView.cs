using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CandyView candyPrefab;
    
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

                if (cell.Candy == null)
                    continue;

                Vector3 position =
                    new Vector3(x * cellSize, y * cellSize, 0);

                CandyView candyView =
                    Instantiate(
                        candyPrefab,
                        position,
                        Quaternion.identity,
                        transform);

                candyView.Setup(cell.Candy, x, y);

                candyViews[x, y] = candyView;
            }
        }
    }
    public IEnumerator SwapViews(CandyView first, CandyView second)
    {
        int firstX = first.X;
        int firstY = first.Y;

        int secondX = second.X;
        int secondY = second.Y;

        Vector3 firstTargetPosition = GetWorldPosition(secondX, secondY);
        Vector3 secondTargetPosition = GetWorldPosition(firstX, firstY);

        CandyAnimator animator = new CandyAnimator();

        yield return animator.AnimateSwap(
            first.transform,
            firstTargetPosition,
            second.transform,
            secondTargetPosition
        );

        candyViews[firstX, firstY] = second;
        candyViews[secondX, secondY] = first;

        first.SetGridPosition(secondX, secondY);
        second.SetGridPosition(firstX, firstY);
    }
    public void RemoveCandyView(int x, int y)
    {
        CandyView view = candyViews[x, y];

        if (view == null)
            return;

        Destroy(view.gameObject);
        candyViews[x, y] = null;
    }

    public void RemoveCandyViews(List<MatchGroup> matches)
    {
        foreach (MatchGroup group in matches)
        {
            foreach (Cell cell in group.Cells)
            {
                RemoveCandyView(cell.X, cell.Y);
            }
        }
    }
    public void ApplyCandyMoves(List<CandyMove> moves)
    {
        foreach (CandyMove move in moves)
        {
            CandyView view = candyViews[move.FromX, move.FromY];

            if (view == null)
                continue;

            candyViews[move.ToX, move.ToY] = view;
            candyViews[move.FromX, move.FromY] = null;

            view.SetGridPosition(move.ToX, move.ToY);
            view.transform.position = GetWorldPosition(move.ToX, move.ToY);
        }
    }
    public void SpawnCandyViews(List<CandySpawn> spawns)
    {
        foreach (CandySpawn spawn in spawns)
        {
            Vector3 position = GetWorldPosition(spawn.X, spawn.Y);

            CandyView candyView = Instantiate(
                candyPrefab,
                position,
                Quaternion.identity,
                transform);

            candyView.Setup(spawn.Candy, spawn.X, spawn.Y);

            candyViews[spawn.X, spawn.Y] = candyView;
        }
    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, y * cellSize, 0);
    }
}
