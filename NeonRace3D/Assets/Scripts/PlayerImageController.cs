using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerImageController : MonoBehaviour
{
    private float mainSpeed;
    public float boostSpeed;
    private int powerCounter = 0;
    private float sprintBegin = 0;
    private float slowBegin = 0;
    private ParticleSystem powerFX;
    private ParticleSystem obstacleFX;
    private GridController gridCon;
    private float endCounterSec;
    private bool _isGameOver = false;
    private AudioSource[] audios;

    public PlayerController PlayerController;
    public PlayerControllerWaypoint PlayerControllerWP;
    public GameManager GM;
    public float maxSpeed = 20.0f;
    public float minSpeed = 5.0f;
    public Camera MainCamera;
    public GameObject[] SpeedBars;
    public GameObject JetObj;

    void Start()
    {
        JetObj = GameObject.FindGameObjectWithTag("Jet");
        JetObj.SetActive(false);
        PlayerController = GetComponentInParent<PlayerController>();
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (gameObject.GetComponentInParent<PlayerControllerWaypoint>())
        {
            PlayerControllerWP = GetComponentInParent<PlayerControllerWaypoint>();
            boostSpeed = maxSpeed;
        }
        else
        {
            PlayerController = GetComponentInParent<PlayerController>();
            boostSpeed = PlayerController.speed;
        }

        foreach (var effectFX in GetComponentsInChildren<ParticleSystem>())
        {
            if (effectFX.CompareTag("PowerFX"))
                powerFX = effectFX;
            else if (effectFX.CompareTag("ObstacleFX"))
                obstacleFX = effectFX;
            else
                Debug.Log("There is no power or obstacle effects !");
        }

        audios = GetComponents<AudioSource>();

        gridCon = GetComponentInParent<GridController>();

        MainCamera = Camera.main;

        mainSpeed = PlayerControllerWP.PlayerSpeed;
    }

    void Update()
    {
        endCounterSec += Time.deltaTime;
        int seconds = (int)endCounterSec % 60;
        switch (powerCounter)
        {
            case 1:
                SpeedBars[0].SetActive(true);
                break;
            case 2:
                SpeedBars[1].SetActive(true);
                break;
            case 3:
                SpeedBars[2].SetActive(true);
                break;
            case 4:
                SpeedBars[3].SetActive(true);
                sprintBegin = Time.timeSinceLevelLoad;
                powerCounter = 0;
                break;
            case 5:
                sprintBegin = Time.timeSinceLevelLoad;
                powerCounter = 0;
                break;
        }

        if (sprintBegin > 0)
        {
            JetObj.SetActive(true);
            gameObject.transform.localPosition = new Vector3(0,2.0f,0);
            gameObject.transform.localEulerAngles = new Vector3(60, 0, 0);
            PlayerControllerWP.PlayerSpeed = boostSpeed;
            if (Time.timeSinceLevelLoad - sprintBegin > 2.0f)
            {
                sprintBegin = 0;
                powerCounter = 0;
                foreach (var bar in SpeedBars)
                {
                    JetObj.SetActive(false);
                    gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
                    gameObject.transform.localEulerAngles = Vector3.zero;
                    bar.SetActive(false);   
                }
                PlayerControllerWP.PlayerSpeed = mainSpeed;
            }
        }
        else if(slowBegin > 0)
        {
            PlayerControllerWP.PlayerSpeed = minSpeed;
            if (Time.timeSinceLevelLoad - slowBegin > 1.0f)
            {
                //Debug.Log("Slow Correction");
                slowBegin = 0;
                PlayerControllerWP.PlayerSpeed = mainSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Obstacle"))
        {
            
            if(sprintBegin == 0 && slowBegin == 0)
            {
                //Debug.Log("ObstacleHit");
                slowBegin = Time.timeSinceLevelLoad;
                //PlayerControllerWP.PlayerSpeed = minSpeed;
                //if (speed >= minSpeed)
                //{
                //    speed -= speed * 0.3f;
                //    PlayerControllerWP.PlayerSpeed = minSpeed;
                //}
                //Destroy(other.gameObject);
                other.gameObject.GetComponent<Renderer>().enabled = false;
                obstacleFX.Play();
                audios[0].Play();
                gridCon.tronRunning.SetTrigger("Stumble");
            }

            GM.PointAddByType(GameManager.PointSystem.NegativePoint, 5);
            //gameObject.GetComponent<GridController>().speed -= gameObject.GetComponent<GridController>().speed * 0.2f;
        }
        else if (other.transform.parent.CompareTag("Power"))
        {
            //Debug.Log("PowerHit");
            if(sprintBegin == 0 )
            {
                if (powerCounter < 4)
                {
                    powerCounter++;
                }
                else
                {
                    powerCounter = 0;
                }

                //if (speed <= maxSpeed)
                //{
                //    speed += speed;
                //    PlayerControllerWP.PlayerSpeed = speed;
                //}
                //Destroy(other.gameObject);
                other.gameObject.GetComponent<Renderer>().enabled = false;
                powerFX.Play();
                audios[1].Play();
                gridCon.tronRunning.SetTrigger("Sprint");
            }

            GM.PointAddByType(GameManager.PointSystem.PositivePoint, 10);
            //gameObject.GetComponent<GridController>().speed *= 2;
        }
        else if (other.transform.CompareTag("CoinObj"))
        {
            GM.RealCoins++;
            other.GetComponent<Renderer>().enabled = false;
        }
    }
}
