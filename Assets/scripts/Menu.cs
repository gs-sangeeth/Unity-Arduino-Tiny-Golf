using UnityEngine;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.Play("title");
    }
}
