using UnityEngine;

public class NotificationHUDManager : MonoBehaviour
{
    [Tooltip("UI panel containing the layoutGroup for displaying notifications")]
    public RectTransform notificationPanel;
    [Tooltip("Prefab for the notifications")]
    public GameObject notificationPrefab;


    public void CreateNotification(string text)
    {
        GameObject notificationInstance = Instantiate(notificationPrefab, notificationPanel);
        notificationInstance.transform.SetSiblingIndex(0);

        NotificationToast toast = notificationInstance.GetComponent<NotificationToast>();
        if (toast)
        {
            toast.Initialize(text);
        }
    }

    public void RegisterObjective(Objective objective)
    {
        objective.onUpdateObjective += OnUpdateObjective;
    }

    public void UnregisterObjective(Objective objective)
    {
        objective.onUpdateObjective -= OnUpdateObjective;
    }

    void OnUpdateObjective(UnityActionUpdateObjective updateObjective)
    {
        if (!string.IsNullOrEmpty(updateObjective.notificationText))
            CreateNotification(updateObjective.notificationText);
    }

}
