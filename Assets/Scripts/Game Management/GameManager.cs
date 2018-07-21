using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MEC;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public TimeManager timeManager;

    public struct TimeManager
    {
        public float myDelta;
        public float myFixedDelta;
        public float myTimeScale;
    }

    [HideInInspector]
    Vector3 playerStartPosition = new Vector3(1,5,1);

    GameObject player;

    [SerializeField]
    int currentLevel;

    bool lvlLoaded;
    public float delayTime = 2f;

    public Image overlayImage;

    private void Awake()
    {
        Debug.developerConsoleVisible = true;
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
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        player = GameObject.FindGameObjectWithTag("Player");
        lvlLoaded = true;

        timeManager.myTimeScale = 1;

        //player = FindObjectOfType<RaycastPlayer>().gameObject;

        if (playerStartPosition == null)
        {
            Debug.Log("No start tile in the level");
            return;
        }

        //player.transform.position = playerStartPosition;
    }

    private void Update()
    {
        timeManager.myDelta = Time.deltaTime * timeManager.myTimeScale;

        //Closing the Game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //Next Scene
        if (Input.GetKeyDown(KeyCode.N))
            LevelComplete();

        //Previous Scene
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentLevel -= 2;
            currentLevel = Mathf.Clamp(currentLevel, -1, SceneManager.sceneCountInBuildSettings);
            LevelComplete();
        }

        //Start from title screen
        if(currentLevel == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            LevelComplete();
        }

        //Check the overlay isnt showing
        if (lvlLoaded)
        {
            if(overlayImage != null)
            overlayImage.color = Vector4.zero;
        }
    }

    private void FixedUpdate()
    {
        timeManager.myFixedDelta = Time.fixedDeltaTime * timeManager.myTimeScale;
    }

    public void LevelComplete()
    {
        if (lvlLoaded)
        {
            lvlLoaded = false;
            currentLevel = (currentLevel + 1) % SceneManager.sceneCountInBuildSettings;

            Timing.RunCoroutine(_FadeToSceneChange(Color.black));
           
            
            //Timing.RunCoroutine(_FadeToColour(Vector4.zero));
        }
    }

    public void ResetPlayer()
    {
        /*Transform startTile = FindObjectOfType<StartTile>().transform;
        if(startTile != null)
            playerStartPosition = startTile.position;
        */

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerStartPosition;
    }

    public void KillPlayer()
    {
        player.GetComponent<MeshRenderer>().enabled = false;
        Invoke("ResetPlayer", 1f);
    }

    IEnumerator<float> _FadeToSceneChange(Color targetColour)
    {
        Color startColour = overlayImage.color;

        float journey = 0f;

        while (journey <= delayTime)
        {
            journey += Time.deltaTime;
            float percentage = Mathf.Clamp01(journey / delayTime);
            overlayImage.color = Color.Lerp(startColour, targetColour, percentage);
            yield return Timing.WaitForOneFrame;
        }

        if (!lvlLoaded)
        {
            SceneManager.LoadScene(currentLevel);
            lvlLoaded = true;
        }
    }
}
