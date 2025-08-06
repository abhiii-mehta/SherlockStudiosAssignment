using UnityEngine;
using UnityEngine.UI;

public class BatteryPickup : MonoBehaviour
{
    public int batteryAmount = 20;
    public KeyCode pickupKey = KeyCode.E;

    private bool playerInRange = false;
    private InventoryManager inventory;
    public GameObject pickupHintUI;

    private void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        if (inventory == null)
            Debug.LogError("InventoryManager not found in scene!");

        if (pickupHintUI != null)
            pickupHintUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (pickupHintUI != null)
                pickupHintUI.SetActive(true);

            if (Input.GetKeyDown(pickupKey))
            {
                Debug.Log("Battery picked up!");
                inventory.AddBattery(batteryAmount);
                if (pickupHintUI != null)
                    pickupHintUI.SetActive(false);

                Destroy(gameObject);
            }
        }
        else
        {
            if (pickupHintUI != null)
                pickupHintUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player in battery pickup range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left battery pickup range");
        }
    }
}
