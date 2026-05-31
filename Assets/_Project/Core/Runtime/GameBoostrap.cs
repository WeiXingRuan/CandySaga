using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoostrap : MonoBehaviour
{
    [SerializeField] private BoardView boardView;
    [SerializeField] private BoardCameraFitter cameraFitter;
    private void Start()
    {
        BoardState board = new BoardState
        {
            Width = 8,
            Height = 8,
            Cells = new Cell[8,8]

        };
        BoardGenerator generator = new BoardGenerator();
        generator.Generate(board);
        boardView.Render(board);
        cameraFitter.Fit(board.Width, board.Height, boardView.CellSize);
    }
   
}
