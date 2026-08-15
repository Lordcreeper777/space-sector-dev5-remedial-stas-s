using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private SpaceSectorApiClient apiClient;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceCamera();
        }
    }

    private void TryPlaceCamera()
{
    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out RaycastHit hit) &&
        hit.collider.CompareTag("Ground"))
    {
        var position = hit.point;
        position.y = 5f;

        var rotation = Quaternion.Euler(0f, 180f, 0f);

        var placedCamera =
            Instantiate(cameraPrefab, position, rotation);

        placedCamera.name =
             $"SurveillanceCamera_{System.Guid.NewGuid().ToString("N")[..8]}";

        var cameraView =
            placedCamera.GetComponent<SurveillanceCameraView>();

        if (cameraView != null && apiClient != null)
        {
            apiClient.RegisterRuntimeCamera(cameraView);
        }
    }
}

}