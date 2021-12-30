using UnityEngine;
using System.IO.Ports;

public class Player : MonoBehaviour
{
    public float rotateSpeed = 60f;
    SerialPort sp;

    void Start()
    {
        sp = new SerialPort("COM4", 9600);
        sp.Open();
        sp.ReadTimeout = 1;
    }


    void Update()
    {
        var x = Input.GetAxisRaw("Horizontal");
        if (x != 0)
        {
            transform.Rotate(rotateSpeed * Time.deltaTime * x * transform.up);
        }
    }
}
