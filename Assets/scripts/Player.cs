using UnityEngine;
using System;
using System.IO.Ports;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject forwardDirection;
    public GameObject directionArrow;
    public float basicKnockForce = 10f;

    public GameObject levelCompleteText;
    public GameObject outOfBoundsText;
    public Transform uiCenterPos;
    public GameObject circleScaleAnimation;
    public GameObject canvas;

    public GameObject knockCountText;
    public GameObject deathCountText;

    [HideInInspector]
    public bool knockMode = false;
    [HideInInspector]
    public static Player instance;

    SerialPort sp;
    Rigidbody body;

    private int touchPreviousValue = 0;
    private int knockCount = 0;
    private int deathCount = 0;

    private readonly float timerSpeed = .5f;
    private float elapsed;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();

        sp = new SerialPort("COM4", 9600);
        sp.Open();
    }

    void Update()
    {
        try
        {
            var sensorsInput = sp.ReadLine().Split(',');
            var knockForce = int.Parse(sensorsInput[0]);
            var rotationAngle = int.Parse(sensorsInput[1]);
            var touchInput = int.Parse(sensorsInput[2]);


            if (touchInput != touchPreviousValue)
            {
                if (touchPreviousValue == 0 && touchInput == 1)
                {
                    AudioManager.instance.Play("knockMode");
                    knockMode = !knockMode;

                }
                touchPreviousValue = touchInput;
            }

            if (rotationAngle != 0 && !knockMode)
            {
                transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            }

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

        knockCountText.GetComponent<TMP_Text>().text = knockCount.ToString();
        deathCountText.GetComponent<TMP_Text>().text = deathCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            deathCount++;
            AudioManager.instance.Play("outOfBounds");
            var obj = Instantiate(outOfBoundsText, uiCenterPos.position, Quaternion.identity);
            var obj2 = Instantiate(circleScaleAnimation, uiCenterPos.position, Quaternion.identity);
            obj.transform.parent = canvas.transform;
            obj2.transform.parent = canvas.transform;
            LevelLoader.instance.LoadLevel(same: true);
        }

        if (other.CompareTag("Goal"))
        {
            AudioManager.instance.Play("goal");
            var obj = Instantiate(levelCompleteText, uiCenterPos.position, Quaternion.identity);
            var obj2 = Instantiate(circleScaleAnimation, uiCenterPos.position, Quaternion.identity);
            obj.transform.parent = canvas.transform;
            obj2.transform.parent = canvas.transform;

            LevelLoader.instance.LoadLevel();
        }
    }
}
