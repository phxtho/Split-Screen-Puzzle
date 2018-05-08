using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    Rigidbody rb;

    Vector3 movement;
    public float speed = 3f;
    public float jumpForce = 75f;

    public enum Player {Left, Right }
    public Player playerSide;

    public bool onGoal = false;

    [SerializeField]
    int health = 3;
    GameObject[] healthObjects;
    public GameObject healthObjectPrefab;

    float inputX, inputY;
    bool lerpComplete;

    public Vector3 startPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        InstantiateHealthPrefabs();
        SetPlayer();

        startPos = transform.position;
    }
    private void Update()
    {
        //Movement Input
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        //Detecting no input to snap to grid
        if (Mathf.Abs(inputX) < Mathf.Epsilon && Mathf.Abs(inputY) < Mathf.Epsilon)
            SnapToGrid();

        if (transform.position.y < -10f)
        {
            transform.position = startPos;
            Invoke("LoseHealth", 0.3f);
        }
    }

    void FixedUpdate()
    {
        movement = new Vector3(inputX, 0f, inputY); 
        rb.MovePosition(transform.position + movement.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Goal")
            onGoal = true;
    }

    private void OnTriggerExit(Collider other)
    {
        onGoal = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.tag == "Undefined")
        {
            LoseHealth();
        }
    }

    void SetPlayer()
    {
        if (playerSide == Player.Right)
            speed *= -1f;

        if (playerSide == Player.Left)
        {
            GameManager.instance.playerLeft = this.gameObject;
        }

        if (playerSide == Player.Right)
            GameManager.instance.playerRight = this.gameObject;
    }

    void  SnapToGrid()
    {
        float lerpTime = 0.5f;
        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(Mathf.Round(currentPos.x), currentPos.y, Mathf.Round(currentPos.z));
        transform.position = Vector3.Lerp(currentPos, targetPos, lerpTime);
    }

    #region Health Management
    void InstantiateHealthPrefabs()
    {
        float objectHeight = 1f;
        healthObjects = new GameObject[health];
        float gap = 0.3f;
        float scale = ((1f / health) - 0.1f);

        for (int i = 0; i < health; i++)
        {
            Vector3 position = (new Vector3((gap - gap * i), objectHeight, 0f));//transform.localScale.x;

            healthObjects[i] = Instantiate(healthObjectPrefab, transform.position, Quaternion.identity);
            healthObjects[i].transform.parent = transform;
            healthObjects[i].transform.localPosition = position;
            healthObjects[i].transform.localScale = Vector3.one * scale;

        }
    }

    void LoseHealth()
    {
        health--;
        health = Mathf.Clamp(health, 0, healthObjects.Length);
        healthObjects[health].GetComponent<MeshRenderer>().enabled = false;

        if (health <= 0)
            GameManager.instance.Restart();
    }

    public void GainHealth()
    {
        health ++;
        health = Mathf.Clamp(health, 0, healthObjects.Length);
        healthObjects[health].GetComponent<MeshRenderer>().enabled = false;
    }
    #endregion
}
