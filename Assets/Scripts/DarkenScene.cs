using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DarkenScene : MonoBehaviour
{
    public Light directionalLight;
    public float ambientIntensity = 0.6f;
    void Start()
    {
        RenderSettings.ambientLight = new Color(0.05f, 0.05f, 0.05f);

        RenderSettings.ambientIntensity = 0.2f;

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

        if (directionalLight != null)
            directionalLight.enabled = false;

        Debug.Log("Scene darkened but less pitch black.");
    }

}
