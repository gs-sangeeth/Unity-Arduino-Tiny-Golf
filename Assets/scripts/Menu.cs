using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.Play("title");
    }
    public void LoadPlayScene()
    {
        AudioManager.instance.Pause("title");
        SceneManager.LoadScene("playScene");
    }
}
