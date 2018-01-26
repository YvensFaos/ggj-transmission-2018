using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
    public Text FPSText;

    private void Start()
    {
        if(FPSText == null)
        {
            FPSText = GetComponent<Text>();
        }

        if (!StaticData.Instance.DEBUG)
        {
            FPSText.enabled = false;
        }
    }

    private void Update()
    {
        if (StaticData.Instance.DEBUG)
        {
            if (Time.deltaTime == 0.0)
            {
                FPSText.text = "[Pause]";
                return;
            }
            float fps = 1.0f / Time.deltaTime;
            FPSText.text = fps.ToString();

            fps = (fps > 60.0f) ? 60.0f : fps;
            Color fpsColor = Color.Lerp(Color.red, Color.green, 60.0f / fps);
            FPSText.color = fpsColor;
        }
    }
}