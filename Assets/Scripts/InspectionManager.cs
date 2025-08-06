using UnityEngine;
using UnityEngine.UI;

public class InspectionManager : MonoBehaviour
{
    public Transform inspectionSpot;
    public GameObject cancelButton;
    public GameObject playerController;
    public FPSController fpsController;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;
    [Header("Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minZoomDistance = 0.5f;
    public float maxZoomDistance = 2.5f;

    private GameObject currentObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    private bool inspecting = false;

    private Vector3 lastMousePosition;
    private float currentZoomDistance;

    void Start()
    {
        cancelButton.SetActive(false);
    }

    void Update()
    {
        if (!inspecting || currentObject == null) return;

        HandleRotation();
        HandleZoom();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            float rotX = -mouseDelta.y * rotationSpeed * Time.deltaTime;
            float rotY = mouseDelta.x * rotationSpeed * Time.deltaTime;

            currentObject.transform.Rotate(Camera.main.transform.right, rotX, Space.World);
            currentObject.transform.Rotate(Vector3.up, rotY, Space.World);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            currentZoomDistance -= scroll * zoomSpeed;
            currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);

            currentObject.transform.localPosition = Vector3.forward * currentZoomDistance;
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
        currentZoomDistance = 1.5f;
        obj.transform.localPosition = Vector3.forward * currentZoomDistance;
        obj.transform.localRotation = Quaternion.identity;

        cancelButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (fpsController != null)
            fpsController.canLook = false;
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
        if (fpsController != null)
            fpsController.canLook = true;
        TogglePlayerControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void TogglePlayerControl(bool state)
    {
        if (playerController != null)
        {
            playerController.SetActive(state);
        }
    }
}
