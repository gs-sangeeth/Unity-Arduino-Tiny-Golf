using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [HideInInspector]
    public static LevelLoader instance;

    public GameObject[] levels;
    public GameObject player;
    public GameObject knockModeUI;
    public Material[] skyboxes;

    private GameObject loadedLevel;
    private int currentLevel = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AudioManager.instance.Play("theme");

        RenderSettings.skybox = skyboxes[currentLevel];
        loadedLevel = GameObject.FindGameObjectWithTag("Level");
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

    public void LoadLevel(bool same = false)
    {
        Destroy(loadedLevel);
        if (currentLevel < levels.Length)
        {
            if (!same)
            {
                currentLevel++;
            }
            RenderSettings.skybox = skyboxes[currentLevel];
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = Vector3.zero + new Vector3(0, 10, 0);
            loadedLevel = Instantiate(levels[currentLevel], transform.position, Quaternion.identity);
        }
    }
}
