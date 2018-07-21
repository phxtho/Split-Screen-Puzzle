using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour {
    public Canvas canvas;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex < 1)
        {
            canvas.enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.H))
            canvas.enabled = !canvas.enabled;
    }
}
