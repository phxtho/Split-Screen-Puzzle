using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject playerLeft;
    public GameObject playerRight;

    public GameObject canvas;

    public float restartDelay = 1f;
    int level;
    bool sceneLoaded = true;

    public TimeManager timeManager;

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

        timeManager.myTimeScale = 1;
    }

    private void Update()
    {
        timeManager.myDelta = Time.deltaTime * timeManager.myTimeScale;

        //Closing the Game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.R))
            Restart();

        if (BothPlayersOnGoal() && sceneLoaded)
        {
            sceneLoaded = false;
            Invoke("NextLevel", restartDelay);
        }
    }

    private void FixedUpdate()
    {
        timeManager.myFixedDelta = Time.fixedDeltaTime * timeManager.myTimeScale;
    }

    bool BothPlayersOnGoal()
    {
        if ((playerLeft.GetComponent<PlayerController>().onGoal) && (playerRight.GetComponent<PlayerController>().onGoal))
            return true;

        else
            return false;
    }

    #region Scene Switching
    public void Restart()
    {
        level = 0;
        SceneManager.LoadScene(level);
    }

    void NextLevel()
    {
        if (!sceneLoaded)
        {
            level++;

            if (level < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(level);
                sceneLoaded = true;
            }
            else
            {
               GameObject canvasInstance = Instantiate(canvas, Vector3.zero, Quaternion.identity);
               canvasInstance.SetActive(true);
            }
        }
    }
    #endregion
}
