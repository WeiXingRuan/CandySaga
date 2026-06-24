using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardResolver
{
    private readonly MatchFinder matchFinder;
    private readonly BoardDestroyer boardDestroyer;
    private readonly BoardGravity boardGravity;
    private readonly BoardRefiller boardRefiller;
    private readonly BoardView boardView;

    public BoardResolver(
        MatchFinder matchFinder,
        BoardDestroyer boardDestroyer,
        BoardGravity boardGravity,
        BoardRefiller boardRefiller,
        BoardView boardView)
    {
        this.matchFinder = matchFinder;
        this.boardDestroyer = boardDestroyer;
        this.boardGravity = boardGravity;
        this.boardRefiller = boardRefiller;
        this.boardView = boardView;
    }

    public bool HasMatch(BoardState boardState)
    {
        List<MatchGroup> matches = matchFinder.FindMatches(boardState);
        return matches.Count > 0;
    }

    public IEnumerator Resolve(BoardState boardState)
    {
        while (true)
        {
            List<MatchGroup> matches = matchFinder.FindMatches(boardState);

            if (matches.Count == 0)
                break;

            yield return boardView.DestroyCandyViews(matches);

            boardDestroyer.DestroyMatches(matches);

            List<CandyMove> moves = boardGravity.ApplyGravity(boardState);
            yield return boardView.ApplyCandyMoves(moves);

            List<CandySpawn> spawns = boardRefiller.Refill(boardState);
            yield return boardView.SpawnCandyViews(spawns);
        }
    }
}