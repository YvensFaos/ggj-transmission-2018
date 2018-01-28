using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MessageBarController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("If the message system is active or not")]
    public bool isActive = false;

    [Tooltip("Time to a new message event happen")]
    public float NewMessageTimer = 20f;

    private float NewMessageCounter;

    [Tooltip("The factor is a multiplier of the NewMessageTimer")]
    public float NewMessageTimeFactor = 1f;

    [Tooltip("Color of the bar when the Time is close to full")]
    public Color FullTimeColor;

    [Tooltip("Color of the bar when the Time is close to medium")]
    public Color MediumTimeColor;

    [Tooltip("Color of the bar when the Time is close to low")]
    public Color LowTimeColor;

    private Color CurrentColor;

    [Tooltip("Speed to transition between colors")]
    public float ColorTransitionSpeed = 5f;

    [Tooltip("Percentage of the bar that represents full")]
    public float FullTimerCoef = 0.8f;

    [Tooltip("Percentage of the bar that represents low")]
    public float LowTimerCoef = 0.2f;

    //Reference to Image Component
    public Image MessageTimerImage;

    [Header("Events To Invoke")]
    public UnityEvent EventsListener;

    // Use this for initialization
    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            NewMessageCounter -= Time.deltaTime;

            float MessageTimerNormalized = NewMessageCounter / NewMessageTimer;

            MessageTimerImage.fillAmount = MessageTimerNormalized;

            if (MessageTimerNormalized >= FullTimerCoef)//Red
            {
                CurrentColor = FullTimeColor;
            }
            else if (MessageTimerNormalized >= LowTimerCoef)//Yellow
            {
                CurrentColor = MediumTimeColor;
            }
            else //Red
            {
                CurrentColor = LowTimeColor;
            }

            if (NewMessageCounter < 0)
            {
                NewMessageCounter = NewMessageTimer;

                //INVOKE EVENT
                EventsListener.Invoke();
                isActive = false;
            }
        }
        LerpBarColor();
    }

    /// <summary>
    /// For variables initialization
    /// </summary>
    private void Init()
    {
        Restart();
        MessageTimerImage = GetComponent<Image>();
    }

    public void Restart()
    {
        NewMessageCounter = NewMessageTimer * NewMessageTimeFactor;
    }

    public void StopBar()
    {
        isActive = false;
        MessageTimerImage.color = Color.white;
    }

    private void LerpBarColor()
    {
        MessageTimerImage.color = Color.Lerp(MessageTimerImage.color, CurrentColor, ColorTransitionSpeed * Time.deltaTime);
    }
}