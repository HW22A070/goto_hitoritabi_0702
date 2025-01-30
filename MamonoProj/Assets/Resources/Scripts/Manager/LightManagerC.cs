using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManagerC : MonoBehaviour
{
    private Light2D _lightGlobal;

    private Light2D _lightPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _lightPlayer = GameObject.Find("Player").GetComponent<Light2D>();
        _lightGlobal = GetComponent<Light2D>();
    }

    /// <summary>
    /// グローバルライトの明るさチェンジ（平常１）
    /// </summary>
    /// <param name="intensity"></param>
    public void SetGlobalLightIntensity(float intensity)
    {
        _lightGlobal.intensity = intensity;
        if (intensity < 1) _lightPlayer.intensity = 1.0f - intensity;
        else _lightPlayer.intensity = 0.0f;
    }

    /// <summary>
    /// グローバルライトの色チェンジ
    /// </summary>
    public void SetGlobalLightColor(Color32 color)
    {
        _lightGlobal.color = color;
    }
}
