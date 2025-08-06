using UnityEngine;
using UnityEngine.UI;

public class InspectionManager : MonoBehaviour
{
    public Transform inspectionSpot;
    public GameObject cancelButton;

    private GameObject currentObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    private bool inspecting = false;

    [Header("Rotation Settings")]
    public float scrollRotationSpeed = 10000f;

    void Update()
    {
        if (!inspecting || currentObject == null) return;

        HandleScrollRotation();
    }

    private void HandleScrollRotation()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.001f)
        {
            Vector3 rotationAxis = Camera.main.transform.up;
            currentObject.transform.Rotate(rotationAxis, scroll * scrollRotationSpeed, Space.World);
        }
    }

    public void StartInspection(GameObject obj)
    {
        if (inspecting) return;

        inspecting = true;
        currentObject = obj;

        originalPosition = obj.transform.position;
        originalRotation = obj.transform.rotation;
        originalParent = obj.transform.parent;

        obj.transform.SetParent(inspectionSpot);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        cancelButton.SetActive(true);
    }

    public void CancelInspection()
    {
        if (!inspecting || currentObject == null) return;

        currentObject.transform.SetParent(originalParent);
        currentObject.transform.position = originalPosition;
        currentObject.transform.rotation = originalRotation;

        currentObject = null;
        inspecting = false;
        cancelButton.SetActive(false);
    }
}
