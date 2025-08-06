using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightLight;
    public float maxBattery = 100f;
    public float batteryDrainRate = 100f;
    public float minIntensity = 100f;
    public float maxIntensity = 3000f;
    public KeyCode toggleKey = KeyCode.F;

    [Header("Fallback Light")]
    public Light fallbackLight;
    public float fallbackDelay = 2f;
    public float flickerSpeed = 0.2f;

    private bool isOn = false;
    private float currentBattery;
    private float fallbackTimer;
    private bool flashlightDead = false;

    [Header("UI")]
    public Slider batterySlider;
    private InventoryManager inventoryManager;

    void Start()
    {
        currentBattery = maxBattery;
        flashlightLight.enabled = false;
        fallbackLight.enabled = false;
        Debug.Log("FlashlightController started");
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found by FlashlightController");
        }

    }
    void Update()
    {
        if (inventoryManager != null && inventoryManager.IsInventoryOpen())
        {
            return;
        }
        if (Input.GetKeyDown(toggleKey))
        {
            Debug.Log("Toggle key pressed");
        }

        HandleToggle();

        if (isOn && currentBattery > 0f)
        {
            DrainBattery();
            UpdateLightIntensity();
            Debug.Log($"Flashlight ON. Battery: {currentBattery}");
        }
        else if (isOn && currentBattery <= 0f)
        {
            TurnOffFlashlight();
            flashlightDead = true;
            Debug.Log("Flashlight battery dead");
        }

        HandleFallbackLight();
    }
    private void HandleToggle()
    {
        if (Input.GetKeyDown(toggleKey) && !flashlightDead)
        {
            isOn = !isOn;
            flashlightLight.enabled = isOn;
            Debug.Log($"Flashlight toggled {(isOn ? "ON" : "OFF")}");
        }
    }
    private void DrainBattery()
    {
        currentBattery -= batteryDrainRate * Time.deltaTime;
        currentBattery = Mathf.Max(currentBattery, 0f);
        UpdateBatteryUI();
    }
    private void UpdateLightIntensity()
    {
        float t = currentBattery / maxBattery;
        flashlightLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
    private void TurnOffFlashlight()
    {
        isOn = false;
        flashlightLight.enabled = false;
        fallbackTimer = fallbackDelay;
    }
    private void HandleFallbackLight()
    {
        if (!isOn && flashlightDead)
        {
            fallbackTimer -= Time.deltaTime;
            Debug.Log("Fallback countdown: " + fallbackTimer.ToString("F2"));

            if (fallbackTimer <= 0f)
            {
                fallbackLight.enabled = Random.value > 0.8f;
                fallbackLight.intensity = Random.Range(50f, 500f);
                Debug.Log("Fallback light flickering. Enabled: " + fallbackLight.enabled);
            }
        }
    }
    public void RechargeBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
        flashlightDead = currentBattery <= 0f;
        if (currentBattery > 0f)
        {
            fallbackLight.enabled = false;
        }
        UpdateBatteryUI();
    }
    public float GetBatteryPercent()
    {
        return currentBattery / maxBattery;
    }
    private void UpdateBatteryUI()
    {
        if (batterySlider != null)
        {
            batterySlider.value = currentBattery / maxBattery;
        }
    }

}
