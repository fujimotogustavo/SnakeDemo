using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Transform gridParent;
    public float padding = 1.5f;

    private void Awake()
    {
        mainCamera = gameObject.GetComponent<Camera>();
    }

    public void AdjustOrthographicCameraForGrid()
    {
        float gridHeight = GridController.cols;
        float gridWidth = GridController.rows;

        mainCamera.orthographicSize = (gridHeight / 2f) + padding;
        mainCamera.transform.position = new Vector3(gridWidth / 2f, 10, gridHeight / 2f);
        mainCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
