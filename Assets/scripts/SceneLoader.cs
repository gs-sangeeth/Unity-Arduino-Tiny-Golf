using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public void LoadPlayScene()
    {
        AudioManager.instance.Pause("title");
        SceneManager.LoadScene("playScene");
    }

    public void LoadNewScene()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        LoadPlayScene();
    }
}
