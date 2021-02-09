using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImageControllerCiftAdam : MonoBehaviour
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


    public PlayerControllerWaypoint PlayerControllerWP;
    public GameManager GM;
    public float maxSpeed = 20.0f;
    public float minSpeed = 5.0f;
    public Camera MainCamera;
    public GameObject[] SpeedBars;
    public GameObject JetObj;

    [Header("StackProperties")]
    public List<GameObject> ShieldStack = new List<GameObject>();
    public List<GameObject> ShieldStackDisable = new List<GameObject>();
    public List<GameObject> FallenAnimated;
    private int fallenIndex=0;
    private Vector3 fallenMainPosLeft;
    private Vector3 fallenMainPosRight;

    public GameObject ShieldStackPrefabLeft;
    public GameObject ShieldStackPrefabRight;
    public GameObject ShieldStackParent;
    public GameObject ShieldMain;
    public int stackCount;
    public Animator[] animPlayer;

    void Start()
    {
        fallenMainPosLeft = FallenAnimated[0].transform.localPosition;
        fallenMainPosRight = FallenAnimated[1].transform.localPosition;

        animPlayer = GetComponentsInChildren<Animator>();
        for (int i = 0; i < stackCount; i++)
        {
            MakeStack(true);
        }

        //JetObj = GameObject.FindGameObjectWithTag("Jet");
        //JetObj.SetActive(false);
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (gameObject.GetComponentInParent<PlayerControllerWaypoint>())
        {
            PlayerControllerWP = GetComponentInParent<PlayerControllerWaypoint>();
            boostSpeed = maxSpeed;
        }
        else
        {
            boostSpeed = 30;
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
        RenderSettings.skybox.SetFloat("_Rotation", transform.root.eulerAngles.y);
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
            //JetObj.SetActive(true);
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
                    //JetObj.SetActive(false);


                    cameraMove = 2;
                    //Camera.main.transform.localPosition = new Vector3(0, 3.0f, -5.0f);
                    bar.SetActive(false);
                }
                PlayerControllerWP.PlayerSpeed = mainSpeed;
            }
        }
        else if (slowBegin > 0)
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
        if (other.transform.CompareTag("PowerObj"))
        {
            powerFX.Play();
            audios[1].Play();
            ChangeStack(true);
            other.gameObject.SetActive(false);
        }
        else if (other.transform.CompareTag("ObstacleObj"))
        {
            audios[0].Play();
            ChangeStack(false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("PowerObj"))
        {
            Debug.Log("POWEROBJ");

            audios[1].Play();
            ChangeStack(true);
            col.gameObject.SetActive(false);
        }
        else if (col.transform.CompareTag("ObstacleObj"))
        {
            Debug.Log("OBSTACLEOBJ");
            obstacleFX.Play();
            audios[0].Play();
            ChangeStack(false);
        }

        else if (col.transform.CompareTag("FinalTrigger"))
        {
            MainCamera.gameObject.GetComponentInParent<CameraController>().CameraEffect();
            PlayerControllerWP.PlayerSpeed = 25.0f;
        }
    }


    void MakeStack(bool cond)
    {
        //ShieldMain.SetActive(false);
        if (cond)
        {
            var shieldCount = GameObject.FindGameObjectsWithTag("ShieldStack").Length;
            GameObject shieldInstance;
            if (shieldCount % 2 == 1)
            {
                shieldInstance = Instantiate(ShieldStackPrefabLeft, ShieldStackPrefabLeft.transform.position, Quaternion.identity);

                shieldInstance.transform.parent = ShieldStackParent.transform;
                //shieldInstance.transform.localScale = new Vector3(0.2f,0.5f,0.2f);
                shieldInstance.transform.localEulerAngles = new Vector3(90, 0, 0);
                if (ShieldStack.Count > 0)
                {
                    shieldInstance.transform.position = ShieldStack[ShieldStack.Count - 2].transform.position;
                    shieldInstance.transform.Translate(Vector3.down * 0.5f);

                }
            }
            else
            {
                shieldInstance = Instantiate(ShieldStackPrefabRight, ShieldStackPrefabRight.transform.position, Quaternion.identity);

                shieldInstance.transform.parent = ShieldStackParent.transform;
                //shieldInstance.transform.localScale = new Vector3(0.2f,0.5f,0.2f);
                shieldInstance.transform.localEulerAngles = new Vector3(90, 0, 0);
                if (ShieldStack.Count > 0)
                {
                    shieldInstance.transform.position = ShieldStack[ShieldStack.Count - 2].transform.position;
                    shieldInstance.transform.Translate(Vector3.down * 0.5f);

                }
            }
            
            ShieldStack.Add(shieldInstance);
        }
        else
        {
            if (ShieldStack.Count > 0)
            {
                ShieldStack.RemoveAt(ShieldStack.Count - 1);
            }
            else
            {
                GM.GameOver("LOSE");
            }
        }
    }

    void ChangeStack(bool cond)
    {
        if (cond)
        {
            GM.PointAddByType(GameManager.PointSystem.PositivePoint, 10);
            if (ShieldStack.Count > 0)
            {

                var shieldCount = GameObject.FindGameObjectsWithTag("ShieldStack").Length;
                
                if (ShieldStackDisable.Count > 0)
                {
                    var shieldTemp = ShieldStackDisable[ShieldStackDisable.Count - 1];
                    shieldTemp.SetActive(true);
                    ShieldStackDisable.Remove(shieldTemp);
                    ShieldStack.Add(shieldTemp);

                }
                else 
                {
                    GameObject shieldInstance;

                    if (shieldCount % 2 == 0)
                    {
                        shieldInstance = Instantiate(ShieldStackPrefabLeft, ShieldStackPrefabLeft.transform.position, Quaternion.identity);

                        shieldInstance.transform.parent = ShieldStackParent.transform;
                        //shieldInstance.transform.localScale = new Vector3(0.2f,0.5f,0.2f);
                        shieldInstance.transform.localEulerAngles = new Vector3(90, 0, 0);
                        if (ShieldStack.Count > 0)
                        {
                            shieldInstance.transform.position = ShieldStack[ShieldStack.Count - 2].transform.position;
                            shieldInstance.transform.Translate(Vector3.down * 0.5f);

                        }
                    }
                    else
                    {
                        shieldInstance = Instantiate(ShieldStackPrefabRight, ShieldStackPrefabRight.transform.position, Quaternion.identity);
                        shieldInstance.transform.parent = ShieldStackParent.transform;
                        //shieldInstance.transform.localScale = new Vector3(0.2f,0.5f,0.2f);
                        shieldInstance.transform.localEulerAngles = new Vector3(90, 0, 0);
                        if (ShieldStack.Count > 0)
                        {
                            shieldInstance.transform.position = ShieldStack[ShieldStack.Count - 2].transform.position;
                            shieldInstance.transform.Translate(Vector3.down * 0.5f);

                        }
                    }


                    powerFX.transform.position = shieldInstance.transform.position;
                    powerFX.Play();
                    ShieldStack.Add(shieldInstance);

                }
                animPlayer = GetComponentsInChildren<Animator>();
            }
        }
        else
        {
            GM.PointAddByType(GameManager.PointSystem.NegativePoint, 10);
            if (ShieldStack.Count > 1)
            {
                if(fallenIndex < FallenAnimated.Count)
                {
                    var fallenItem = FallenAnimated[fallenIndex];
                    fallenItem.SetActive(true);
                    fallenItem.GetComponentInChildren<Animator>().SetTrigger("Stumble");
                    fallenItem.transform.parent = null;
                    if (fallenIndex % 2 == 0)
                    {
                        var coroutine = DisableFallen(fallenItem, fallenMainPosLeft, 2);
                        StartCoroutine(coroutine);
                        
                    }
                    else
                    {
                        var coroutine = DisableFallen(fallenItem, fallenMainPosLeft, 2);
                        StartCoroutine(coroutine);
                    }

                    fallenIndex++;
                }
                else
                {
                    fallenIndex = 0;
                }
                var lastObj = ShieldStack[ShieldStack.Count - 1];
                lastObj.SetActive(false);
                ShieldStack.Remove(lastObj);
                ShieldStackDisable.Add(lastObj);
                

            }
            else
            {
                ShieldMain.SetActive(true);
                animPlayer = GetComponentsInChildren<Animator>();

                foreach (var anim in animPlayer)
                {
                    anim.SetBool("GameOver", true);
                }

                MainCamera.gameObject.GetComponentInParent<CameraController>().GameOverEffect();
                GM.GameOver("LOSE");
            }
        }

        if (ShieldStack.Count == 1)
            ShieldMain.SetActive(true);
        else if(ShieldStack.Count > 1)
        {
            ShieldMain.SetActive(false);
        }
    }

    IEnumerator DisableFallen(GameObject fallen, Vector3 fallenPos, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        fallen.transform.parent = ShieldStackParent.transform;
        fallen.transform.localPosition = fallenPos;
        fallen.transform.localRotation = Quaternion.Euler(90,0,0);
        fallen.SetActive(false);
    }
}
