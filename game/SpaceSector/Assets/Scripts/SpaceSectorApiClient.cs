using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SpaceSectorApiClient : MonoBehaviour
{
    [SerializeField] private string baseUrl = "http://localhost:8081";
    [SerializeField] private NpcIdentity[] npcsToRegister;
    [SerializeField] private SurveillanceCameraView[] camerasToRegister;

    public string CurrentSessionId { get; private set; }

    [System.Serializable]
    private class SimulationSessionResponse
    {
        public string id;
    }

    [System.Serializable]
    private class CreateNpcRequest
    {
        public string name;
        public float positionX;
        public float positionY;
        public float positionZ;
    }

    [System.Serializable]
    private class NpcResponse
    {
        public string id;
        public string name;
    }

        [System.Serializable]
    private class CreateCameraRequest
    {
        public string name;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float rotationY;
        public float range;
        public float fieldOfView;
    }

    [System.Serializable]
    private class CameraResponse
    {
        public string id;
        public string name;
    }

        [System.Serializable]
    private class CreateDetectionRequest
    {
        public string simulationSessionId;
        public string cameraId;
        public string npcId;
    }

    private IEnumerator Start()
    {
    yield return CheckHealth();
    yield return StartSimulationSession();

    foreach (var npc in npcsToRegister)
    {
        yield return CreateNpc(npc);
    }

    foreach (var cameraView in camerasToRegister)
    {
        yield return CreateCamera(cameraView);
    }

    foreach (var cameraView in camerasToRegister)
    {
        cameraView.NpcDetected += HandleNpcDetected;
    }

    Debug.Log("Detection events connected.");
}

    private IEnumerator CheckHealth()
    {
        using var request = UnityWebRequest.Get($"{baseUrl}/health");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"API connected: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"API connection failed: {request.error}");
        }
    }

    private IEnumerator StartSimulationSession()
    {
        using var request = new UnityWebRequest(
            $"{baseUrl}/sessions",
            UnityWebRequest.kHttpVerbPOST);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var session = JsonUtility.FromJson<SimulationSessionResponse>(
                request.downloadHandler.text);

            CurrentSessionId = session.id;

            Debug.Log($"Simulation session ID: {CurrentSessionId}");
        }
        else
        {
            Debug.LogError($"Session creation failed: {request.error}");
        }

    }
    private IEnumerator CreateNpc(NpcIdentity npc)
{
    if (npc == null)
    {
        Debug.LogError("No NPC assigned for API registration.");
        yield break;
    }

    var position = npc.transform.position;

    var npcRequest = new CreateNpcRequest
    {
        name = npc.DisplayName,
        positionX = position.x,
        positionY = position.y,
        positionZ = position.z
    };

    var json = JsonUtility.ToJson(npcRequest);

    using var request = new UnityWebRequest(
        $"{baseUrl}/npcs",
        UnityWebRequest.kHttpVerbPOST);

    request.uploadHandler = new UploadHandlerRaw(
        System.Text.Encoding.UTF8.GetBytes(json));

    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        var createdNpc =
            JsonUtility.FromJson<NpcResponse>(request.downloadHandler.text);

        npc.SetBackendId(createdNpc.id);

        Debug.Log($"NPC registered: {createdNpc.name} - {createdNpc.id}");
    }
    else
    {
        Debug.LogError($"NPC registration failed: {request.downloadHandler.text}");
    }
}

private IEnumerator CreateCamera(SurveillanceCameraView cameraView)
{
    if (cameraView == null)
    {
        Debug.LogError("No camera assigned for API registration.");
        yield break;
    }

    var position = cameraView.transform.position;

    var cameraRequest = new CreateCameraRequest
    {
        name = cameraView.name,
        positionX = position.x,
        positionY = position.y,
        positionZ = position.z,
        rotationY = cameraView.transform.eulerAngles.y,
        range = cameraView.Range,
        fieldOfView = cameraView.FieldOfView
    };

    var json = JsonUtility.ToJson(cameraRequest);

    using var request = new UnityWebRequest(
        $"{baseUrl}/cameras",
        UnityWebRequest.kHttpVerbPOST);

    request.uploadHandler = new UploadHandlerRaw(
        System.Text.Encoding.UTF8.GetBytes(json));

    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        var createdCamera =
            JsonUtility.FromJson<CameraResponse>(request.downloadHandler.text);

        cameraView.SetBackendId(createdCamera.id);

        Debug.Log(
            $"Camera registered: {createdCamera.name} - {createdCamera.id}");
    }

    
    else
    {
        Debug.LogError(
            $"Camera registration failed: {request.downloadHandler.text}");
    }
}
   private void HandleNpcDetected(
    SurveillanceCameraView cameraView,
    NpcIdentity npc)
{
    StartCoroutine(CreateDetection(cameraView, npc));
}
    private IEnumerator CreateDetection(
    SurveillanceCameraView cameraView,
    NpcIdentity npc)
{
    if (string.IsNullOrWhiteSpace(CurrentSessionId) ||
        string.IsNullOrWhiteSpace(cameraView.BackendId) ||
        string.IsNullOrWhiteSpace(npc.BackendId))
    {
        Debug.LogError("Detection cannot be saved because a backend ID is missing.");
        yield break;
    }

    var detectionRequest = new CreateDetectionRequest
    {
        simulationSessionId = CurrentSessionId,
        cameraId = cameraView.BackendId,
        npcId = npc.BackendId
    };

    var json = JsonUtility.ToJson(detectionRequest);

    using var request = new UnityWebRequest(
        $"{baseUrl}/detections",
        UnityWebRequest.kHttpVerbPOST);

    request.uploadHandler = new UploadHandlerRaw(
        System.Text.Encoding.UTF8.GetBytes(json));

    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log($"Detection saved: {request.downloadHandler.text}");
    }
    else
    {
        Debug.LogError(
            $"Detection save failed: {request.downloadHandler.text}");
    }
}

}
