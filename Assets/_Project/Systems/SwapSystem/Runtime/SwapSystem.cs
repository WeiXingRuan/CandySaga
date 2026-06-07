using System.Collections;
using UnityEngine;

public class SwapSystem
{
    private readonly BoardState boardState;
    private readonly BoardSwapper boardSwapper;
    private readonly BoardResolver boardResolver;
    private readonly BoardView boardView;

    public SwapSystem(
        BoardState boardState,
        BoardSwapper boardSwapper,
        BoardResolver boardResolver,
        BoardView boardView)
    {
        this.boardState = boardState;
        this.boardSwapper = boardSwapper;
        this.boardResolver = boardResolver;
        this.boardView = boardView;
    }

    public IEnumerator TrySwap(CandyView first, CandyView second)
    {
        yield return SwapCandies(first, second);

        bool hasMatch = boardResolver.HasMatch(boardState);

        if (!hasMatch)
        {
            Debug.Log("No matches found, swapping back");
            yield return SwapCandies(first, second);
            yield break;
        }

        boardResolver.Resolve(boardState);
    }

    private IEnumerator SwapCandies(CandyView first, CandyView second)
    {
        boardSwapper.Swap(
            boardState,
            first.X,
            first.Y,
            second.X,
            second.Y
        );

        yield return boardView.SwapViews(first, second);
    }
}