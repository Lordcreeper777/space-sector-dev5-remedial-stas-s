using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float movementDistance = 8f;

    private Vector3 startPosition;
    private int direction = 1;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * direction * Time.deltaTime);

        var distanceFromStart = Vector3.Distance(startPosition, transform.position);

        if (distanceFromStart >= movementDistance)
        {
            direction *= -1;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}