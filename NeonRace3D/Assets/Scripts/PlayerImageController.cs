using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

//using Random = System.Random;

public class PlayerImageController : MonoBehaviour
{
    private float mainSpeed;
    public float boostSpeed;
    private int powerCounter = 0;
    private float sprintBegin = 0;
    private float slowBegin = 0;
    private int cameraMove = 0;
    private ParticleSystem powerFX;
    private ParticleSystem obstacleFX;
    private GridController gridCon;
    private float endCounterSec;
    private bool _isGameOver = false;
    private AudioSource[] audios;
    //[SerializeField] private AnimatorController dances;
    //[SerializeField] private AnimatorController inGame;

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
            //gameObject.transform.localPosition = new Vector3(0,3.0f,0);
            //gameObject.transform.localEulerAngles = new Vector3(60, 0, 0);
            cameraMove = 1;
            //Camera.main.transform.localPosition = new Vector3(0, 4.0f, -3.0f);
            PlayerControllerWP.PlayerSpeed = boostSpeed;
            if (Time.timeSinceLevelLoad - sprintBegin > 2.0f)
            {
                sprintBegin = 0;
                powerCounter = 0;
                //gameObject.transform.localPosition = new Vector3(0, 1.5f, 0);
                //gameObject.transform.localEulerAngles = Vector3.zero;
                foreach (var bar in SpeedBars)
                {
                    JetObj.SetActive(false);
                    
                    
                    cameraMove = 2;
                    //Camera.main.transform.localPosition = new Vector3(0, 3.0f, -5.0f);
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

        switch (cameraMove)
        {
            case 1:
                cameraMove = 0;
                CameraEffect(Camera.main.transform.localPosition, new Vector3(0, 4.5f, -5.0f));
                CharacterMove(gameObject.transform.localPosition, new Vector3(0, 3.0f, transform.localPosition.z));
                gameObject.transform.localEulerAngles = new Vector3(60, 0, 0);
                break;
            case 2:
                
                cameraMove = 0;
                //gameObject.transform.localPosition = new Vector3(0, 1.5f, transform.localPosition.z);
                //Camera.main.transform.localPosition = new Vector3(0, 3.0f, -5.0f);
                CameraEffect(Camera.main.transform.localPosition, new Vector3(0, 3.0f, -5.0f));
                CharacterMove(gameObject.transform.localPosition, new Vector3(0, 1.5f, transform.localPosition.z));
                gameObject.transform.localEulerAngles = Vector3.zero;
                break;
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

    void CameraEffect(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(TranslateOverTime(Camera.main.transform,startPos,endPos, 0.1f));
    }

    void CharacterMove(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(TranslateOverTime(gameObject.transform, startPos, endPos, 0.1f));
    }

    IEnumerator TranslateOverTime(Transform movingHandle, Vector3 originalPosition, Vector3 finalPosition, float duration)
    {
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            movingHandle.localPosition = originalPosition;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                // progress will equal 0 at startTime, 1 at endTime.
                movingHandle.localPosition = Vector3.Slerp(originalPosition, finalPosition, progress);
                yield return null;
            }
        }
        movingHandle.localPosition = finalPosition;
    }

    public void ChangeAnimator(bool inGame)
    {
        int rand = 0;
        var anim = GameObject.FindGameObjectWithTag("Animator").GetComponent<Animator>();
        

        if (inGame)
        {
            anim.SetInteger("DanceMode", rand);
        }
        else
        {
            rand = Random.Range(1, 2);
            transform.Rotate(Vector3.up, 180);
            anim.SetInteger("DanceMode", rand);
        }
    }
}
