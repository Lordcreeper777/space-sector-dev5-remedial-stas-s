using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCameraView : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private bool showDetectionLines = true;

    private NpcMovement[] npcs;
    private readonly HashSet<NpcMovement> detectedNpcs = new();

    public float Range => range;
    public float FieldOfView => fieldOfView;

    private void Start()
    {
        npcs = FindObjectsByType<NpcMovement>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        foreach (var npc in npcs)
        {
            var canSeeNpc = CanSeeNpc(npc.transform);

            if (canSeeNpc)
            {
                if (detectedNpcs.Add(npc))
                {
                    Debug.Log($"{name} detected {npc.name}");
                }

                if (showDetectionLines)
                {
                    Debug.DrawLine(transform.position, npc.transform.position);
                }
            }
            else if (detectedNpcs.Remove(npc))
            {
                Debug.Log($"{name} lost {npc.name}");
            }
        }
    }

    private bool CanSeeNpc(Transform npc)
{
    var flatDirection = npc.position - transform.position;
    flatDirection.y = 0f;

    var distance = flatDirection.magnitude;

    if (distance > range)
    {
        return false;
    }

    var angle = Vector3.Angle(transform.forward, flatDirection);

    if (angle > fieldOfView / 2f)
    {
        return false;
    }

    var directionToNpc = npc.position - transform.position;

    if (Physics.Raycast(
        transform.position,
        directionToNpc.normalized,
        out RaycastHit hit,
        directionToNpc.magnitude))
    {
        return hit.transform == npc || hit.transform.IsChildOf(npc);
    }

    return true;
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);

        var halfFov = fieldOfView / 2f;

        var leftDirection =
            Quaternion.Euler(0f, -halfFov, 0f) * transform.forward;

        var rightDirection =
            Quaternion.Euler(0f, halfFov, 0f) * transform.forward;

        Gizmos.DrawRay(transform.position, leftDirection * range);
        Gizmos.DrawRay(transform.position, rightDirection * range);
    }
}