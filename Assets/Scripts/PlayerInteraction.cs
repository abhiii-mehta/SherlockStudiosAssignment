using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactableLayer;
    public TextMeshProUGUI interactionPrompt;

    private Camera cam;
    private InspectableItem currentTarget;
    void Start()
    {
        cam = Camera.main;
        interactionPrompt.text = "";
    }
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            InspectableItem inspectable = hit.collider.GetComponent<InspectableItem>();

            if (inspectable != null)
            {
                currentTarget = inspectable;
                interactionPrompt.text = $"Press [E] to Inspect {inspectable.itemName}";

                if (Input.GetKeyDown(interactKey))
                {
                    InspectItem(currentTarget);
                }
            }
        }
        else
        {
            currentTarget = null;
            interactionPrompt.text = "";
        }
    }
    void InspectItem(InspectableItem item)
    {
        Debug.Log($"Inspecting: {item.itemName}");
        FindObjectOfType<InspectionManager>().StartInspection(item.gameObject);
    }

}
