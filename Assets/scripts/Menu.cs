using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadPlayScene()
    {
        SceneManager.LoadScene("playScene");
    }
}
