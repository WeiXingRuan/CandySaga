using UnityEngine;

public class BoardCameraFitter : MonoBehaviour
{
    private Camera targetCamera;

    [SerializeField]
    private float padding ;

    private void Awake()
    {
        targetCamera = Camera.main;
    }

    public void Fit(int boardWidth, int boardHeight, float cellSize)
    {
        float boardWorldWidth = boardWidth * cellSize;
        float boardWorldHeight = boardHeight * cellSize;

        float screenAspect =
            (float)Screen.width / Screen.height;

        float boardAspect =
            boardWorldWidth / boardWorldHeight;

        if (screenAspect >= boardAspect)
        {
            targetCamera.orthographicSize =
                boardWorldHeight / 2f + padding;
        }
        else
        {
            targetCamera.orthographicSize =
                boardWorldWidth /
                (2f * screenAspect) +
                padding;
        }

        targetCamera.transform.position =
            new Vector3(
                boardWorldWidth / 2f - cellSize / 2f,
                boardWorldHeight / 2f - cellSize / 2f,
                -10f);
    }
}