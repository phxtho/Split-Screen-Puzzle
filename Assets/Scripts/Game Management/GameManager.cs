using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public TimeManager timeManager;

    [HideInInspector]
    Vector3 playerStartPosition = new Vector3(1,5,1);

    GameObject player;

    [SerializeField]
    int currentLevel;

    public struct TimeManager
    {
        public float myDelta;
        public float myFixedDelta;
        public float myTimeScale;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject); 
    }

    private void Start()
    {
        timeManager.myTimeScale = 1;
        player = FindObjectOfType<RaycastPlayer>().gameObject;

        if (playerStartPosition == null)
        {
            Debug.Log("No start tile in the level");
            return;
        }

        player.transform.position = playerStartPosition;
    }

    private void Update()
    {
        timeManager.myDelta = Time.deltaTime * timeManager.myTimeScale;

        //Closing the Game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void FixedUpdate()
    {
        timeManager.myFixedDelta = Time.fixedDeltaTime * timeManager.myTimeScale;
    }

    public void LevelComplete()
    {
        currentLevel++;
    }

    public void ResetPlayer()
    {
        playerStartPosition = FindObjectOfType<StartTile>().transform.position;
        player.transform.position = playerStartPosition;
    }
}
