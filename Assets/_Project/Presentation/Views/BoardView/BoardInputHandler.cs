using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour
{
    [SerializeField] BoardInputReader inputReader;
    [SerializeField] BoardView boardView;

    private BoardState boardState;
    private BoardSwapper boardSwapper;
    private MatchFinder matchFinder;
    private CandyView selectedCandy;
    private BoardDestroyer boardDestroyer;
    private BoardGravity boardGravity;

    public void Initialize(BoardState board)
    {
        boardState = board;
        boardSwapper = new BoardSwapper();
        matchFinder = new MatchFinder();
        boardDestroyer = new BoardDestroyer();
        boardGravity = new BoardGravity();
    }

    private void OnEnable()
    {
        inputReader.PointerDown += HandlePointerDown;
    }
    private void OnDisable()
    {
        inputReader.PointerDown -= HandlePointerDown;
    }
    private void HandlePointerDown(Vector2 screenPosition)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider == null)
            return;
        CandyView candyView = hit.collider.GetComponent<CandyView>();
        if (candyView == null)
            return;
        SelectCandy(candyView);
    }

    private void SelectCandy(CandyView candy)
    {
        if (selectedCandy == null)
        {
            selectedCandy = candy;
            Debug.Log($"Selected: {candy.X}, {candy.Y}");
            return;
        }

        if (selectedCandy == candy)
        {
            selectedCandy = null;
            Debug.Log("Unselected");
            return;
        }

        if (!AreAdjacent(selectedCandy, candy))
        {
            selectedCandy = candy;
            Debug.Log($"Changed selection: {candy.X}, {candy.Y}");
            return;
        }

        Debug.Log($"Can swap: {selectedCandy.X},{selectedCandy.Y} <-> {candy.X},{candy.Y}");
        SwapCandies(selectedCandy, candy);
        var matches = matchFinder.FindMatches(boardState);

        if (matches.Count == 0)
        {
            Debug.Log("No matches found, swapping back");
            SwapCandies(selectedCandy, candy);
        }    
        else
        {
            ResolveBoard(); 
        }    
        
        selectedCandy = null;
    }
    private void SwapCandies(CandyView first, CandyView second)
    {
        boardSwapper.Swap(
            boardState,
            first.X,
            first.Y,
            second.X,
            second.Y);

        boardView.SwapViews(first, second);

    }
    private void ResolveBoard()
    {
        while (true)
        {
            List<MatchGroup> matches = matchFinder.FindMatches(boardState);

            if (matches.Count == 0)
                break;

            boardDestroyer.DestroyMatches(matches);
            boardView.RemoveCandyViews(matches);

            List<CandyMove> moves = boardGravity.ApplyGravity(boardState);
            boardView.ApplyCandyMoves(moves);
        }
    }
    private bool AreAdjacent(CandyView a, CandyView b)
    {
        int distanceX = Mathf.Abs(a.X - b.X);
        int distanceY = Mathf.Abs(a.Y - b.Y);

        return distanceX + distanceY == 1;
    }
}

