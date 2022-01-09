using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public GameObject[] levels;
    [HideInInspector]
    public static LevelLoader instance;
    public GameObject player;
    public GameObject knockModeUI;

    private GameObject loadedLevel;
    private GameObject loadedPlayer;
    private int currentLevel = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadLevel();
    }

    private void Update()
    {
        if (Player.instance.knockMode)
        {
            knockModeUI.SetActive(true);
        }
        else
        {
            knockModeUI.SetActive(false);
        }
    }

    public void LoadLevel()
    {
        Destroy(loadedLevel);
        Destroy(loadedPlayer);
        if (currentLevel != 0)
        {
            currentLevel++;
        }
        loadedLevel = Instantiate(levels[currentLevel], transform.position, Quaternion.identity);
        loadedPlayer = Instantiate(player, transform.position + new Vector3(0, 10, 0), Quaternion.identity);
    }
}
