using UnityEngine;

public class NpcIdentity : MonoBehaviour
{
    [SerializeField] private string displayName;

    public string DisplayName => displayName;

    private void OnGUI()
    {
        if (Camera.main == null || string.IsNullOrWhiteSpace(displayName))
        {
            return;
        }

        var worldPosition = transform.position + Vector3.up * 2f;
        var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        if (screenPosition.z <= 0f)
        {
            return;
        }

        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        style.normal.textColor = Color.white;

        var labelPosition = new Rect(
            screenPosition.x - 75f,
            Screen.height - screenPosition.y - 15f,
            150f,
            30f);

        GUI.Label(labelPosition, displayName, style);
    }
}