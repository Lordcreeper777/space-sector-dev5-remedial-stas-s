using UnityEngine;

public class SurveillanceSummaryDisplay : MonoBehaviour
{
    [SerializeField] private SpaceSectorApiClient apiClient;
    private void OnGUI()
    {
        if (apiClient == null)
        {
            return;
        }

        GUI.Box(new Rect(20, 20, 260, 180), "Surveillance Status");

        GUI.Label(
            new Rect(35, 55, 230, 25),
            $"Coverage: {apiClient.CoveragePercentage:F1}%");

        GUI.Label(
            new Rect(35, 80, 230, 25),
            $"Covered NPCs: {apiClient.CoveredNpcCount}/{apiClient.TotalNpcCount}");

        GUI.Label(
            new Rect(35, 105, 230, 25),
            $"Blind spots: {apiClient.BlindSpotCount}");

        GUI.Label(
            new Rect(35, 130, 230, 25),
            $"Score: {apiClient.Score}/100");

        var barWidth = 210f;
        var coverageWidth =
            barWidth * Mathf.Clamp01(apiClient.CoveragePercentage / 100f);

        GUI.Box(
            new Rect(35, 160, barWidth, 18),
            GUIContent.none);

        var previousColor = GUI.color;
        GUI.color = Color.green;

        GUI.DrawTexture(
            new Rect(35, 160, coverageWidth, 18),
            Texture2D.whiteTexture);

        GUI.color = previousColor;
    }
    }