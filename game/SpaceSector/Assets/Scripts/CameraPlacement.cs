using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
    [SerializeField] private GameObject cameraPrefab;

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

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Ground"))
        {
            var position = hit.point;
            position.y = 0.5f;

           var placedCamera = Instantiate(cameraPrefab, position, Quaternion.identity);
placedCamera.name = $"SurveillanceCamera_{FindObjectsByType<SurveillanceCameraView>(FindObjectsSortMode.None).Length}";
        }
    }
}