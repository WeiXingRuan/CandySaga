using UnityEngine;
using System.Collections;

public class BoardInputHandler : MonoBehaviour
{
    [SerializeField] private BoardInputReader inputReader;
    [SerializeField] private BoardView boardView;

    private SwapSystem swapSystem;
    private CandyAnimator candyAnimator;
    private CandyView selectedCandy;
    private Coroutine selectedLoopCoroutine;
    private bool isBusy;

    public void Initialize(BoardState board, CandyDatabase candyDatabase)
    {
        BoardResolver boardResolver = new BoardResolver(
            new MatchFinder(),
            new BoardDestroyer(),
            new BoardGravity(),
            new BoardRefiller(candyDatabase),
            boardView
        );

        swapSystem = new SwapSystem(
            board,
            new BoardSwapper(),
            boardResolver,
            boardView
        );

        candyAnimator = new CandyAnimator();
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
        if (isBusy)
            return;

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
            SelectNewCandy(candy);
            Debug.Log($"Selected: {candy.X}, {candy.Y}");
            return;
        }

        if (selectedCandy == candy)
        {
            UnselectCurrentCandy();
            Debug.Log("Unselected");
            return;
        }

        if (!AreAdjacent(selectedCandy, candy))
        {
            ChangeSelection(candy);
            Debug.Log($"Changed selection: {candy.X}, {candy.Y}");
            return;
        }

        Debug.Log($"Can swap: {selectedCandy.X},{selectedCandy.Y} <-> {candy.X},{candy.Y}");

        StartCoroutine(SwapSelectedCandy(candy));
    }

    private void SelectNewCandy(CandyView candy)
    {
        selectedCandy = candy;

        StartCoroutine(candyAnimator.AnimateSelect(selectedCandy.transform));

        selectedLoopCoroutine =
            StartCoroutine(candyAnimator.AnimateSelectedLoop(selectedCandy.transform));
    }

    private void UnselectCurrentCandy()
    {
        StopSelectedLoop();

        StartCoroutine(candyAnimator.AnimateUnselect(selectedCandy.transform));

        selectedCandy = null;
    }

    private void ChangeSelection(CandyView newCandy)
    {
        StopSelectedLoop();

        StartCoroutine(candyAnimator.AnimateUnselect(selectedCandy.transform));

        selectedCandy = newCandy;

        StartCoroutine(candyAnimator.AnimateSelect(selectedCandy.transform));

        selectedLoopCoroutine =
            StartCoroutine(candyAnimator.AnimateSelectedLoop(selectedCandy.transform));
    }

    private IEnumerator SwapSelectedCandy(CandyView targetCandy)
    {
        isBusy = true;

        CandyView first = selectedCandy;
        CandyView second = targetCandy;

        StopSelectedLoop();

        yield return candyAnimator.AnimateUnselect(first.transform);

        selectedCandy = null;

        yield return swapSystem.TrySwap(first, second);

        isBusy = false;
    }

    private void StopSelectedLoop()
    {
        if (selectedLoopCoroutine == null)
            return;

        StopCoroutine(selectedLoopCoroutine);
        selectedLoopCoroutine = null;
    }

    private bool AreAdjacent(CandyView a, CandyView b)
    {
        int distanceX = Mathf.Abs(a.X - b.X);
        int distanceY = Mathf.Abs(a.Y - b.Y);

        return distanceX + distanceY == 1;
    }
}