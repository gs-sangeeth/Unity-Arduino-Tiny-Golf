using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [HideInInspector]
    public static LevelLoader instance;

    public GameObject[] levels;
    public GameObject player;
    public GameObject knockModeUI;

    private GameObject loadedLevel;
    private int currentLevel = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        if (!same)
        {
            currentLevel++;
        }
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = Vector3.zero + new Vector3(0, 10, 0);
        loadedLevel = Instantiate(levels[currentLevel], transform.position, Quaternion.identity);
    }
}
