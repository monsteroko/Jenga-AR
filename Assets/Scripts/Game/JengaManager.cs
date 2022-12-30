using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;
using TMPro;

public class JengaManager : Singleton<JengaManager>
{
    //Jenga Stuff
    public GameObject table;
    private GameObject poddon;
    public GameObject jengaPiece;
    public Vector3 addPoint;
    private Vector3 spawnPoint;
    private float pieceOffsetZ = 0.06f;
    private float pieceOffsetY = 0.04f;
    public int layers = 9;
    private int currentLayer;
    private float spawnDelay = 0.5f;
    public string jengaPieceTag = "JengaPiece";
    public Button b1;
    public TextMeshProUGUI text;
    public float tmr= 0;

    [HideInInspector]
    public bool pieceSelected;
    public bool isPaused;
    public bool canMove;
    private bool gameInProgress;


    [Header(@"AR Objects")]
    public Camera ARcamera;
    public GameObject PlaneFinder;
    public GameObject GroundPlane;

    //Pause Stuff
    public Canvas PauseCanvas;
    //public PauseManager myPause;
    public Button startButton, ExitButton;

    protected JengaManager() { }
    
    private void Start()
    {
        PauseCanvas.enabled = false;
        poddon= table.transform.GetChild(2).gameObject;
        //startButton.onClick.AddListener(ResumeGame);
        ExitButton.onClick.AddListener(CloseGame);
        poddon.GetComponent<TableTouching>().piecesTouching = -3;
        currentLayer = 0;
        pieceSelected = false;
        isPaused = false;
        gameInProgress = false;
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (poddon.GetComponent<TableTouching>().piecesTouching >= 5)
            {
                GameOver();
            }
        }
    }

    public void SpawnJengaPieces()
    {
        spawnPoint = table.transform.position + addPoint;
        if (currentLayer < layers)
        {
            if (currentLayer % 2 == 0)
            {
                SpawnHorizontalLayer(currentLayer);
            }
            else
            {
                SpawnVerticalLayer(currentLayer);
            }
            currentLayer++;
            Invoke("SpawnJengaPieces", spawnDelay);
        }
        gameInProgress = true;
        if (currentLayer == layers)
        {
            var objs = GameObject.FindGameObjectsWithTag("JengaPiece");
            for (int i = 0; i < objs.Length; i++)
                objs[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Debug.Log("Change");
        }
    }
    private void SpawnHorizontalLayer(int layer)
    {
        Vector3 center = new Vector3(spawnPoint.x, spawnPoint.y + pieceOffsetY * layer, spawnPoint.z);
        Quaternion rotation = new Quaternion();
        var myJP1 = Instantiate(jengaPiece, center, rotation);
        myJP1.transform.parent = table.transform;
        var myJP2 = Instantiate(jengaPiece, new Vector3(center.x, center.y, center.z + pieceOffsetZ), rotation);
        myJP2.transform.parent = table.transform;
        var myJP3 = Instantiate(jengaPiece, new Vector3(center.x, center.y, center.z - pieceOffsetZ), rotation);
        myJP3.transform.parent = table.transform;
    }
    private void SpawnVerticalLayer(int layer)
    {
        Vector3 center = new Vector3(spawnPoint.x, spawnPoint.y + pieceOffsetY * layer, spawnPoint.z);
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        var myJR1 = Instantiate(jengaPiece, center, rotation);
        myJR1.transform.parent = table.transform;
        var myJR2 = Instantiate(jengaPiece, new Vector3(center.x + pieceOffsetZ, center.y, center.z), rotation);
        myJR2.transform.parent = table.transform;
        var myJR3 = Instantiate(jengaPiece, new Vector3(center.x - pieceOffsetZ, center.y, center.z), rotation);
        myJR3.transform.parent = table.transform;
    }

    public void ResetPieces()
    {
        ClearPieces();
        poddon.GetComponent<TableTouching>().piecesTouching = -3;
        currentLayer = 0;
        SpawnJengaPieces();
    }
    private void ClearPieces()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag(jengaPieceTag);
        foreach (GameObject piece in pieces)
        {
            Destroy(piece);
        }
    }

    private void GameOver()
    {
        PauseCanvas.enabled = true;
        canMove = false;
        gameInProgress = false;
        TextMeshProUGUI b1text = b1.GetComponentInChildren<TextMeshProUGUI>();
        b1text.text = "Restart";
        text.text = "You lose!";
        isPaused = true;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        PauseCanvas.enabled = true;
        canMove = false;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        PauseCanvas.enabled = false;
        canMove = true;
        isPaused = false;

        if (!gameInProgress)
        {
            ResetPieces();
            TextMeshProUGUI b1text = b1.GetComponentInChildren<TextMeshProUGUI>();
            b1text.text = "Continue";
            text.text = "Pause";
            gameInProgress = true;
        }
    }
    public void SpawnFaced(bool isTracked)
    {
        if (isTracked)
        {
            float yRotation = ARcamera.transform.eulerAngles.y;
            GroundPlane.transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            PlaneFinder.GetComponent<PlaneFinderBehaviour>().OnInteractiveHitTest.RemoveAllListeners();
            SpawnJengaPieces();
        }
        else
            PlaneFinder.GetComponent<PlaneFinderBehaviour>().OnInteractiveHitTest.AddListener(PlaneFinder.GetComponent<ContentPositioningBehaviour>().PositionContentAtPlaneAnchor);
    }
    public void CloseGame()
    {
        Application.Quit();
    }

}
