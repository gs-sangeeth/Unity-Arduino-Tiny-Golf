using UnityEngine;
using System;
using System.IO.Ports;
using TMPro;

public class Player : MonoBehaviour
{
    public float basicKnockForce = 10f;
    public GameObject forwardDirection;
    public GameObject directionArrow;

    public GameObject levelCompleteText;
    public GameObject outOfBoundsText;
    public GameObject circleScaleAnimation;
    public GameObject canvas;
    public Transform uiCenterPos;

    public GameObject knockCountText;
    public GameObject deathCountText;

    [HideInInspector]
    public bool knockMode = false;
    [HideInInspector]
    public static Player instance;

    private SerialPort sp;
    private Rigidbody body;
    private int touchPreviousValue = 0;
    private int knockCount = 0;
    private int deathCount = 0;

    // timer for knock delay
    private readonly float timerSpeed = .5f;
    private float elapsed;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();

        sp = new SerialPort("COM4", 9600); // Specify port used for connecting arduino (eg.COM4)
        sp.Open();
    }

    void Update()
    {
        try
        {
            // Arduino output comes as : knockSensorOP,potentiometerOP,TouchSensorOP,20
            var sensorsInput = sp.ReadLine().Split(',');
            var knockForce = int.Parse(sensorsInput[0]);
            var rotationAngle = int.Parse(sensorsInput[1]);
            var touchInput = int.Parse(sensorsInput[2]);

            // Enabling knock mode based on touch sensor input
            if (touchInput != touchPreviousValue)
            {
                if (touchPreviousValue == 0 && touchInput == 1)
                {
                    AudioManager.instance.Play("knockMode");
                    knockMode = !knockMode;

                }
                touchPreviousValue = touchInput;
            }

            // changing ball rotation based on potentiometer input
            if (rotationAngle != 0 && !knockMode)
            {
                transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            }

            // giving ball force based on knock sensor input
            elapsed += Time.deltaTime;
            if (knockMode)
            {
                if (knockForce > 20 && elapsed >= timerSpeed)
                {
                    elapsed = 0f;
                    AudioManager.instance.Play("knock");
                    body.AddForce(basicKnockForce * knockForce * Time.deltaTime * forwardDirection.transform.forward);
                    knockCount++;
                    directionArrow.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else
            {
                directionArrow.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        catch (Exception)
        {
        }

        // Update knock count and death count in game UI
        knockCountText.GetComponent<TMP_Text>().text = knockCount.ToString();
        deathCountText.GetComponent<TMP_Text>().text = deathCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if ball goes out of bounds
        if (other.CompareTag("Finish"))
        {
            deathCount++;
            AudioManager.instance.Play("outOfBounds");
            var outOFBoundsTextObj = Instantiate(outOfBoundsText, uiCenterPos.position, Quaternion.identity);
            var circleAnimationObj = Instantiate(circleScaleAnimation, uiCenterPos.position, Quaternion.identity);
            outOFBoundsTextObj.transform.SetParent(canvas.transform);
            circleAnimationObj.transform.SetParent(canvas.transform);

            LevelLoader.instance.LoadLevel(same: true);
        }

        // check if level completed
        if (other.CompareTag("Goal"))
        {
            AudioManager.instance.Play("goal");
            var levelCompleteTextObj = Instantiate(levelCompleteText, uiCenterPos.position, Quaternion.identity);
            var circleAnimationObj = Instantiate(circleScaleAnimation, uiCenterPos.position, Quaternion.identity);
            levelCompleteTextObj.transform.SetParent(canvas.transform);
            circleAnimationObj.transform.SetParent(canvas.transform);

            LevelLoader.instance.LoadLevel();
        }
    }
}
