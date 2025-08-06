using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    public int batteryCount = 0;
    public FlashlightController flashlight;

    [Header("UI")]
    public TextMeshProUGUI batteryCountText;
    public Button useBatteryButton;
    public GameObject inventoryPanel;
    private bool inventoryOpen = false;
    void Start()
    {
        UpdateUI();
        useBatteryButton.onClick.AddListener(UseBattery);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        inventoryPanel.SetActive(inventoryOpen);

        if (inventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddBattery(int amount)
    {
        batteryCount += amount;
        Debug.Log($"Picked up {amount} batteries. Total now: {batteryCount}");
        UpdateUI();
    }

    public void UseBattery()
    {
        if (batteryCount > 0)
        {
            batteryCount--;
            flashlight.RechargeBattery(25f);
            Debug.Log("Used one battery to recharge flashlight.");
            UpdateUI();
        }
        else
        {
            Debug.Log("No batteries to use!");
        }
    }
    private void UpdateUI()
    {
        if (batteryCountText != null)
            batteryCountText.text = $"Batteries: {batteryCount}";

        if (useBatteryButton != null)
        {
            useBatteryButton.gameObject.SetActive(batteryCount > 0);
        }
    }
    public bool IsInventoryOpen()
    {
        return inventoryOpen;
    }


}
