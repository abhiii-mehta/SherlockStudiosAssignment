using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlightLight;
    public float maxBattery = 100f;
    public float batteryDrainRate = 10f; // per minute
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

    void Start()
    {
        currentBattery = maxBattery;
        flashlightLight.enabled = false;
        fallbackLight.enabled = false;
        Debug.Log("FlashlightController started");
    }

    void Update()
    {
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
        currentBattery -= (batteryDrainRate / 60f) * Time.deltaTime;
        currentBattery = Mathf.Max(currentBattery, 0f);
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
            if (fallbackTimer <= 0f)
            {
                fallbackLight.enabled = Random.value > 0.5f;
                fallbackLight.intensity = Random.Range(50f, 150f);
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
    }

    public float GetBatteryPercent()
    {
        return currentBattery / maxBattery;
    }
}
