using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLight : MonoBehaviour
{
    [SerializeField]
    private Light whiteLight;

    [SerializeField]
    private Light orangeLight;

    [SerializeField]
    private float whiteLightMaxIntensity;
    [SerializeField]
    private float orangeLightMaxIntensity;

    private int whiteMultiplier;
    private int orangeMultiplier;

    private void Start()
    {
        whiteMultiplier = 1;
        orangeMultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        whiteLight.intensity += whiteMultiplier * 0.02f;
        orangeLight.intensity += orangeMultiplier * 0.01f;

        if (whiteLight.intensity > whiteLightMaxIntensity || whiteLight.intensity < 1.0f)
            whiteMultiplier *= -1;

        if (orangeLight.intensity > orangeLightMaxIntensity || orangeLight.intensity < 1.0f)
            orangeMultiplier *= -1;
    }
}
