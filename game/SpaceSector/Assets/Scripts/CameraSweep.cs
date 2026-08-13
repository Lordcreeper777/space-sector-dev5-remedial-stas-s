using UnityEngine;

public class CameraSweep : MonoBehaviour
{
    [SerializeField] private float sweepAngle = 45f;
    [SerializeField] private float sweepSpeed = 30f;

    private float startRotationY;

    private void Start()
    {
        startRotationY = transform.eulerAngles.y;
    }

    private void Update()
    {
        var offset = Mathf.Sin(Time.time * sweepSpeed * Mathf.Deg2Rad) * sweepAngle;

        transform.rotation = Quaternion.Euler(
            0f,
            startRotationY + offset,
            0f);
    }
}