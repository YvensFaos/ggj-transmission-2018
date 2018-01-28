using DigitalRuby.SimpleLUT;
using UnityStandardAssets.ImageEffects;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;

    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = FindObjectOfType<GameObject>();
                if (gameObject != null)
                {
                    instance = gameObject.AddComponent<EffectManager>();
                }
            }
            return instance;
        }
    }

    [Header("References")]
    public GlitchEffect GlitchRef;
    public SimpleLUT LUTRef;
    public VignetteAndChromaticAberration VignetteRef;

    [Header("Settings")]
    public bool isEnabledGlitchEffect = false;
    public bool isEnabledLUT = false;
    public bool isEnabledVignette = false;
    public Color LUTColor = Color.red;
    [Range(0, 1)]
    public float VignetteIntensity = 0.2f;
    public float ChromaticAberration = 0.1f;

    // Use this for initialization
    void Start()
    {
        if (!GlitchRef)
            GlitchRef = GetComponent<GlitchEffect>();

        if (!LUTRef)
            LUTRef = GetComponent<SimpleLUT>();

        if (!VignetteRef)
            VignetteRef = GetComponent<VignetteAndChromaticAberration>();

        GlitchRef.enabled = isEnabledGlitchEffect;
        LUTRef.enabled = isEnabledLUT;
        VignetteRef.enabled = isEnabledVignette;
    }

    // Update is called once per frame
    void Update()
    {
        GlitchRef.enabled = isEnabledGlitchEffect;
        LUTRef.enabled = isEnabledLUT;
        VignetteRef.enabled = isEnabledVignette;

        LUTRef.TintColor = LUTColor;

        VignetteRef.intensity = VignetteIntensity;
        VignetteRef.chromaticAberration = ChromaticAberration;
    }
}