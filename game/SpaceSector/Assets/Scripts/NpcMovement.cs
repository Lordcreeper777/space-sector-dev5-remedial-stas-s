using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float movementDistance = 8f;

    private Vector3 startPosition;
    private Vector3 movementDirection;
    private float travelledDistance;
    private int direction = 1;

    private void Start()
    {
        startPosition = transform.position;
        movementDirection = transform.forward.normalized;
    }

    private void Update()
    {
        travelledDistance += speed * direction * Time.deltaTime;

        if (travelledDistance >= movementDistance)
        {
            travelledDistance = movementDistance;
            direction = -1;
        }
        else if (travelledDistance <= 0f)
        {
            travelledDistance = 0f;
            direction = 1;
        }

        transform.position =
            startPosition + movementDirection * travelledDistance;
    }
}