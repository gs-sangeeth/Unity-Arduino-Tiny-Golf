using UnityEngine;
using System;
using System.IO.Ports;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject forwardDirection;
    public GameObject directionArrow;
    public float basicKnockForce = 10f;
    public GameObject levelComplete;
    public Transform levelCompletePos;
    public GameObject canvas;
    public GameObject knockNumberText;

    [HideInInspector]
    public bool knockMode = false;
    [HideInInspector]
    public static Player instance;

    SerialPort sp;
    Rigidbody body;

    private int touchPreviousValue = 0;
    private int knockCount = 0;

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

        knockNumberText.GetComponent<TMP_Text>().text = knockCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            LevelLoader.instance.LoadLevel(same: true);
        }

        if (other.CompareTag("Goal"))
        {
            var obj = Instantiate(levelComplete, levelCompletePos.position, Quaternion.identity);
            obj.transform.parent = canvas.transform;

            LevelLoader.instance.LoadLevel();
        }
    }
}
