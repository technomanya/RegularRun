using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Grid Objects")]
    public GameObject[] GridPrefabs;
    public GameObject grid;
    public GameObject finishLine;
    public GameObject[] gridList;
    [SerializeField] private GridController Gcontroller;

    [SerializeField] private int gridCount;
    public GameObject ControlButtons;
    [Space(20)]

    [Header("Game UI Objects")]
    public GameObject StartImage;
    public GameObject InGameImage;
    public GameObject GameOverObj;
    public Text CoinText;
    public Text ScoreText;
    public Text IngameScoreText;
    [SerializeField] private Image youWin;
    [SerializeField] private Image youLose;
    [Space(20)]

    [Header("Game Attributes")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int _positivePoint = 0;
    [SerializeField] private int _negativePoint = 0;
    [SerializeField] private int _gameSeconds = 0;
    public int RealCoins = 0;
    public int CoinGeneral;
    private bool _isLevelBegin = false;

    [Header("Player Attributes")]
    public PlayerControllerWaypoint playerControllerWP;
    //public FakePlayerController rivalController;
    private float tempSpeedPlayer;
    //private float tempSpeedRival;
    private Vector3 _playerPosStart;
    //private Vector3 _fakePlayerPosStart;
    private Quaternion _playerRotStart;
    //private Quaternion _fakePlayerRotStart;
    public GameObject _player;



    public enum  SceneIndexConstant
    {
        MainScene = 0,
        AllSidedScene = 1,
        FourSidedScene = 2
        
    }

    public enum PointSystem
    {
        PositivePoint,
        NegativePoint,
        Seconds
    }

    void Awake()
    {
        //CoinText.gameObject.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 0;


        gridList = GameObject.FindGameObjectsWithTag("Grid");
        if (SceneManager.GetActiveScene().name == "SceneMaker")
        {
            finishLine = GameObject.FindGameObjectWithTag("Finish");
            GameObject beforeGrid = new GameObject();
            float randomAngle = 0f;
            int randomAngleEnum = 0;
            int randomAxis = 0;
            for (int i = 0; i < gridCount; i++)
            {
                randomAngleEnum = Random.Range(0, 8);
                randomAxis = Random.Range(0, 2);
                if (i == 0)
                {
                    beforeGrid = Instantiate(grid, Vector3.zero, Quaternion.identity);
                    beforeGrid.GetComponentInChildren<SphereCollider>().enabled = false;
                }
                else
                {
                    //beforeGrid = Instantiate(grid, beforeGrid.transform.Find("EndTip").position, Quaternion.Euler(new Vector3(randomAngle, 0, 0)));
                    beforeGrid = Instantiate(grid, beforeGrid.transform.Find("EndTip").position, beforeGrid.transform.rotation);
                    switch (randomAngleEnum)
                    {
                        case 0:
                            randomAngle = 30f;
                            break;
                        case 1:
                            randomAngle = 45f;
                            break;
                        case 2:
                            randomAngle = 60f;
                            break;
                        case 3:
                            randomAngle = 75f;
                            break;
                        case 4:
                            randomAngle = -30f;
                            break;
                        case 5:
                            randomAngle = -45f;
                            break;
                        case 6:
                            randomAngle = -60f;
                            break;
                        case 7:
                            randomAngle = -75f;
                            break;
                    }
                    if (randomAxis == 0)
                    {
                        beforeGrid.transform.Rotate(Vector3.up, randomAngle);
                    }
                    else
                    {
                        beforeGrid.transform.Rotate(Vector3.right, randomAngle);
                    }
                }
                beforeGrid.SetActive(true);
            }
            gridList = GameObject.FindGameObjectsWithTag("Grid");
            foreach (var grid in gridList)
            {
                grid.transform.eulerAngles = new Vector3(grid.transform.eulerAngles.x, grid.transform.eulerAngles.y, 0);
            }

            finishLine.transform.position = gridList[gridList.Length - 1].transform.Find("EndTip").position;
            finishLine.transform.rotation = gridList[gridList.Length - 1].transform.rotation;
        }
        //GameOverObj.gameObject.transform.parent.gameObject.SetActive(false);
        //GameAnalyticsSDK.GameAnalytics.Initialize();

    }

    void Start()
    {
        if (PlayerPrefs.HasKey("Coin"))
        {
            CoinGeneral = PlayerPrefs.GetInt("Coin");
        }
        else
        {
            PlayerPrefs.SetInt("Coin", 0);
            CoinGeneral = 0;
        }
        audioSource = GetComponent<AudioSource>();
        playerControllerWP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerWaypoint>();
        //rivalController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<FakePlayerController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        //_fakePlayer = GameObject.FindGameObjectWithTag("Enemy");
        _playerPosStart = _player.transform.position;
        _playerRotStart = _player.transform.rotation;
        //_fakePlayerPosStart = _fakePlayer.transform.position;
        //_fakePlayerRotStart = _fakePlayer.transform.rotation;
    }

    void Update()
    {
    #if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene((int)SceneIndexConstant.MainScene);
#endif
        if (_isLevelBegin == false)
        {
            float mouseXdif = playerControllerWP.mouseX;
            if (Input.GetMouseButton(0) && mouseXdif != 0.0f)
            {
                Begin();
                StartImage.SetActive(false);
            }
        }
    }

    public void GameOver(string playerName)
    {
        //Time.timeScale = 0;
        playerControllerWP.PlayerSpeed = 0;
        _player.GetComponent<SphereCollider>().enabled = false;
        
        //rivalController.speed = 0;

        var animS = _player.GetComponentInChildren<PlayerImageControllerCiftAdam>().animPlayer;
        if (playerName == "Player")
        {
            youLose.gameObject.SetActive(false);
            youWin.gameObject.SetActive(true);


            foreach (var anim in animS)
            {
                anim.SetInteger("DanceMode", 1);
            }
        }
        else
        {
            youWin.gameObject.SetActive(false);
            youLose.gameObject.SetActive(true);
            //foreach (var anim in animS)
            //{
            //    anim.SetBool("GameOver", true);
            //}
        }
        PointAddByType(PointSystem.Seconds, (int)Time.realtimeSinceStartup);

        var coin = Mathf.Clamp((int)(_positivePoint - _negativePoint), 0, Mathf.Infinity);
        var score = (int)(coin * 7);

        CoinGeneral +=score;
        CoinText.text = CoinGeneral.ToString();
        ScoreText.text = score.ToString();
        PlayerPrefs.SetInt("Coin",CoinGeneral);

        
        //CoinText.gameObject.transform.parent.gameObject.SetActive(true);
        InGameImage.SetActive(false);
        StartCoroutine("GameOverUiDelay");
        
        audioSource.Stop();
    }

    public void Restart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);

        Start();
        Time.timeScale = 1;
        audioSource.Play();
        if(_isLevelBegin == true)
            _isLevelBegin = false;
    }

    public void Begin()
    {
        
        //PointAddByType(PointSystem.Seconds, (int)Time.realtimeSinceStartup);
        Gcontroller._canTurn = true;
        audioSource.Play();
        _isLevelBegin = true;
        InGameImage.SetActive(true);
        GameOverObj.SetActive(false);
        _player.transform.position = _playerPosStart;
        _player.transform.rotation = _playerRotStart;
        //_fakePlayer.transform.position = _fakePlayerPosStart;
        //_fakePlayer.transform.rotation = _fakePlayerRotStart;
        //_player.GetComponentInChildren<PlayerImageController>().ChangeAnimator(true);
        tempSpeedPlayer = playerControllerWP.PlayerSpeed;
        //tempSpeedRival = rivalController.speed;
        gridList = GameObject.FindGameObjectsWithTag("Grid");
        Gcontroller.GridControllerBegin();
        playerControllerWP.PlayerControlBegin();
        Time.timeScale = 1;
    }

    public void PauseContinue(bool state)
    {
        
        if (state)
        {
            //Time.timeScale = 0;
            playerControllerWP.PlayerSpeed = 0;
            //rivalController.speed = 0;
        }
        else
        {
            //  Time.timeScale = 1;
            playerControllerWP.PlayerSpeed = tempSpeedPlayer;
            //rivalController.speed = tempSpeedRival;
        }
    }

    public void PointAddByType(PointSystem type, int points)
    {
        switch (type)
        {
            case PointSystem.PositivePoint:
                _positivePoint += points;
                IngameScoreText.text = "X" + _positivePoint.ToString();
                break;
            case PointSystem.NegativePoint:
                _negativePoint += points;
                break;
            case PointSystem.Seconds:
                _gameSeconds = Mathf.Clamp(points - _gameSeconds, 1 , 60);
                break;
        }
    }


    IEnumerator GameOverUiDelay()
    {
        yield return new WaitForSeconds(3);
        GameOverObj.SetActive(true);
    }
}
