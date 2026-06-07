using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoostrap : MonoBehaviour
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private BoardCameraFitter cameraFitter;
    [SerializeField] private BoardInputHandler boardInputHandler;
    [SerializeField] private CandyDatabase candyDatabase;
    [Header("Board Settings")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    private void Start()
    {
        BoardGenerator generator = new BoardGenerator(candyDatabase);

        BoardState board = generator.Generate(width, height);

        boardView.Render(board);
        cameraFitter.Fit(board.Width, board.Height, boardView.CellSize);
        boardInputHandler.Initialize(board, candyDatabase);
    }

}
