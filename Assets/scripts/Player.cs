using UnityEngine;
using System;
using System.IO.Ports;

public class Player : MonoBehaviour
{
    public GameObject forwardDirection;
    public GameObject knockModeUI;
    public GameObject directionArrow;
    [HideInInspector]
    public bool knockMode = false;
    public float basicKnockForce = 10f;

    SerialPort sp;
    Rigidbody body;

    private int touchPreviousValue = 0;

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

            if (knockMode)
            {
                print(knockForce);
                if (knockForce > 20)
                {
                    body.AddForce(basicKnockForce * knockForce * forwardDirection.transform.forward);
                }
                knockModeUI.SetActive(true);
                directionArrow.SetActive(false);
            }
            else
            {
                knockModeUI.SetActive(false);
                directionArrow.SetActive(true);
            }
        }
        catch (Exception)
        {

        }
    }
}
