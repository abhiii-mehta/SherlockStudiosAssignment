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

    void Update()
    {
        if (inspecting && currentObject != null)
        {
            float rotX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

            currentObject.transform.Rotate(Vector3.up, -rotX, Space.World);

            currentObject.transform.Rotate(Camera.main.transform.right, rotY, Space.World);
        }
    }
    private Camera cam => Camera.main;

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
        if (!inspecting) return;

        currentObject.transform.SetParent(originalParent);
        currentObject.transform.position = originalPosition;
        currentObject.transform.rotation = originalRotation;

        currentObject = null;
        inspecting = false;
        cancelButton.SetActive(false);
    }
}
