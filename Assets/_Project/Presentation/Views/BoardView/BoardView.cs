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

                CandyView candyView = Instantiate(
                    candyPrefab,
                    GetWorldPosition(x, y),
                    Quaternion.identity,
                    transform
                );

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

    public IEnumerator DestroyCandyViews(List<MatchGroup> matches)
    {
        CandyAnimator animator = new CandyAnimator();

        foreach (MatchGroup group in matches)
        {
            foreach (Cell cell in group.Cells)
            {
                CandyView view = candyViews[cell.X, cell.Y];

                if (view == null)
                    continue;

                StartCoroutine(animator.AnimateDestroy(view.transform));
            }
        }

        yield return new WaitForSeconds(0.15f);

        RemoveCandyViews(matches);
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

    private void RemoveCandyView(int x, int y)
    {
        CandyView view = candyViews[x, y];

        if (view == null)
            return;

        Destroy(view.gameObject);
        candyViews[x, y] = null;
    }

    public IEnumerator ApplyCandyMoves(List<CandyMove> moves)
    {
        CandyAnimator animator = new CandyAnimator();

        foreach (CandyMove move in moves)
        {
            CandyView view = candyViews[move.FromX, move.FromY];

            if (view == null)
                continue;

            candyViews[move.ToX, move.ToY] = view;
            candyViews[move.FromX, move.FromY] = null;

            view.SetGridPosition(move.ToX, move.ToY);

            float distance = Mathf.Abs(move.FromY - move.ToY);
            float duration = Mathf.Max(0.12f, distance * 0.08f);

            StartCoroutine(
                animator.AnimateMove(
                    view.transform,
                    GetWorldPosition(move.ToX, move.ToY),
                    duration
                )
            );
        }

        yield return new WaitForSeconds(0.25f);
    }

    public IEnumerator SpawnCandyViews(List<CandySpawn> spawns)
    {
        CandyAnimator animator = new CandyAnimator();

        int boardHeight = candyViews.GetLength(1);
        Dictionary<int, int> spawnCountByColumn = new Dictionary<int, int>();

        foreach (CandySpawn spawn in spawns)
        {
            if (!spawnCountByColumn.ContainsKey(spawn.X))
                spawnCountByColumn[spawn.X] = 0;

            int spawnIndex = spawnCountByColumn[spawn.X];
            spawnCountByColumn[spawn.X]++;

            Vector3 targetPosition = GetWorldPosition(spawn.X, spawn.Y);
            Vector3 spawnPosition = GetWorldPosition(spawn.X, boardHeight + spawnIndex);

            CandyView candyView = Instantiate(
                candyPrefab,
                spawnPosition,
                Quaternion.identity,
                transform
            );

            candyView.Setup(spawn.Candy, spawn.X, spawn.Y);

            candyViews[spawn.X, spawn.Y] = candyView;

            float distance = Vector3.Distance(spawnPosition, targetPosition);
            float duration = Mathf.Max(0.15f, distance * 0.06f);

            StartCoroutine(
                animator.AnimateMove(
                    candyView.transform,
                    targetPosition,
                    duration
                )
            );
        }

        yield return new WaitForSeconds(0.45f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize, y * cellSize, 0);
    }
}