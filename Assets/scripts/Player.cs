using UnityEngine;
using System;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject forwardDirection;
    public GameObject directionArrow;
    [HideInInspector]
    public bool knockMode = false;
    public float basicKnockForce = 10f;

    [HideInInspector]
    public static Player instance;

    SerialPort sp;
    Rigidbody body;

    private int touchPreviousValue = 0;


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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                knockMode = !knockMode;
            }

            if (rotationAngle != 0 && !knockMode)
            {
                transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            }

            //transform.Rotate(new Vector3(0, Input.GetAxisRaw("Horizontal") * 20f, 0));

            if (knockMode)
            {
                if (knockForce > 20)
                {
                    body.AddForce(basicKnockForce * knockForce * Time.deltaTime * forwardDirection.transform.forward);
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
            //print(e);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            LevelLoader.instance.LoadLevel(same: true);
        }

        if (other.CompareTag("Goal"))
        {
            LevelLoader.instance.LoadLevel();
        }
    }
}
