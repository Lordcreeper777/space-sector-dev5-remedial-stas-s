using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SpaceSectorApiClient : MonoBehaviour
{
    [SerializeField] private string baseUrl = "http://localhost:8081";
    [SerializeField] private NpcIdentity[] npcsToRegister;
    [SerializeField] private SurveillanceCameraView[] camerasToRegister;
    private SurveillanceSummaryResponse latestSummary;
    private NpcResponse[] existingNpcs;
    private CameraResponse[] existingCameras;

    public string CurrentSessionId { get; private set; }
    public float CoveragePercentage =>
    latestSummary != null ? latestSummary.coveragePercentage : 0f;
    public int BlindSpotCount =>
        latestSummary != null ? latestSummary.blindSpotNpcs : 0;
    public int Score =>
        latestSummary != null ? latestSummary.score : 0;
    public int TotalNpcCount =>
        latestSummary != null ? latestSummary.totalNpcs : 0;
    public int CoveredNpcCount =>
        latestSummary != null ? latestSummary.coveredNpcs : 0;

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
    private class SurveillanceSummaryResponse
    {
        public int totalNpcs;
        public int coveredNpcs;
        public int blindSpotNpcs;
        public float coveragePercentage;
        public int score;
    }

        [System.Serializable]
    private class CreateDetectionRequest
    {
        public string simulationSessionId;
        public string cameraId;
        public string npcId;
    }
        [System.Serializable]
    private class NpcListResponse
    {
        public NpcResponse[] items;
    }

    [System.Serializable]
    private class CameraListResponse
    {
        public CameraResponse[] items;
    }

    private IEnumerator Start()
{
    yield return CheckHealth();
    yield return StartSimulationSession();
    yield return GetExistingNpcs();
    yield return GetExistingCameras();

    foreach (var npc in npcsToRegister)
    {
         NpcResponse existingNpc = null;

    if (existingNpcs != null)
    {
        foreach (var candidate in existingNpcs)
        {
            if (candidate.name == npc.DisplayName)
            {
                existingNpc = candidate;
                break;
            }
        }
    }

    if (existingNpc != null)
    {
        npc.SetBackendId(existingNpc.id);

        Debug.Log(
            $"NPC reused: {existingNpc.name} - {existingNpc.id}");
    }
    else
    {
        yield return CreateNpc(npc);
    }
    }

    foreach (var cameraView in camerasToRegister)
    {
      CameraResponse existingCamera = null;

    if (existingCameras != null)
    {
        foreach (var candidate in existingCameras)
        {
            if (candidate.name == cameraView.name)
            {
                existingCamera = candidate;
                break;
            }
        }
    }

    if (existingCamera != null)
    {
        cameraView.SetBackendId(existingCamera.id);

        Debug.Log(
            $"Camera reused: {existingCamera.name} - {existingCamera.id}");
    }
    else
    {
        yield return CreateCamera(cameraView);
    }
    }

    foreach (var cameraView in camerasToRegister)
    {
        cameraView.NpcDetected += HandleNpcDetected;
    }

    Debug.Log("Detection events connected.");

    yield return GetSurveillanceSummary();
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

    private IEnumerator GetSurveillanceSummary()
{
    using var request =
        UnityWebRequest.Get($"{baseUrl}/surveillance/summary");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        var summary =
            JsonUtility.FromJson<SurveillanceSummaryResponse>(
                request.downloadHandler.text);

        latestSummary = summary;

        Debug.Log(
            $"Summary: {summary.coveragePercentage}% coverage, " +
            $"{summary.blindSpotNpcs} blind spots, score {summary.score}");
    }
    else
    {
        Debug.LogError(
            $"Summary request failed: {request.downloadHandler.text}");
    }
}

    public void RegisterRuntimeCamera(SurveillanceCameraView cameraView)
    {
        StartCoroutine(RegisterRuntimeCameraCoroutine(cameraView));
    }

    private IEnumerator RegisterRuntimeCameraCoroutine(
        SurveillanceCameraView cameraView)
    {
        yield return CreateCamera(cameraView);

        cameraView.NpcDetected += HandleNpcDetected;

        yield return GetSurveillanceSummary();
    }
    private IEnumerator GetExistingNpcs()
{
    using var request = UnityWebRequest.Get($"{baseUrl}/npcs");

    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        var wrappedJson =
            $"{{\"items\":{request.downloadHandler.text}}}";

        var response =
            JsonUtility.FromJson<NpcListResponse>(wrappedJson);

        existingNpcs = response.items;

        Debug.Log($"Loaded {existingNpcs.Length} existing NPCs.");
    }
    else
    {
        Debug.LogError(
            $"Failed to load NPCs: {request.downloadHandler.text}");
    }
}
    private IEnumerator GetExistingCameras()
    {
        using var request = UnityWebRequest.Get($"{baseUrl}/cameras");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var wrappedJson =
                $"{{\"items\":{request.downloadHandler.text}}}";

            var response =
                JsonUtility.FromJson<CameraListResponse>(wrappedJson);

            existingCameras = response.items;

            Debug.Log($"Loaded {existingCameras.Length} existing cameras.");
        }
        else
        {
            Debug.LogError(
                $"Failed to load cameras: {request.downloadHandler.text}");
        }
}

}
