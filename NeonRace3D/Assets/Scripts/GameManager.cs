using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
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
    public List<GameObject> waypointAll = new List<GameObject>();
    [SerializeField]private GameObject[] waypointGrid;
    [SerializeField] private GridController Gcontroller;
    public GameObject[] finalWP;

    [SerializeField] private int gridCount;
    public GameObject ControlButtons;
    [Space(10)]

    [Header("Game UI Objects")]
    public GameObject StartImage;
    public GameObject InGameImage;
    public GameObject GameOverObj;
    public Text CoinText;
    public Text ScoreText;
    public Text IngameScoreText;
    [SerializeField] private Image youWin;
    [SerializeField] private Image youLose;
    [SerializeField] private Text levelName;

    [Space(10)] [Header("Game Attributes")]
    [SerializeField] private bool deleteSave = false;
    [SerializeField] private bool SceneMaker = true;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int _positivePoint = 0;
    [SerializeField] private int _negativePoint = 0;
    [SerializeField] private int _gameSeconds = 0;
    public int RealCoins = 0;
    public int CoinGeneral;
    private bool _isLevelBegin = false;
    [Space(10)]

    [Header("Player Attributes")]
    public PlayerControllerWaypoint playerControllerWP;
    private float tempSpeedPlayer;
    private Vector3 _playerPosStart;
    private Quaternion _playerRotStart;
    public GameObject _player;

    [Header("Level Attributes")]
    public List<Level> Levels = new List<Level>();
    
    public Level currLevel = new Level();
    [SerializeField] private int levelCountAll = 50;
    
    public class Level
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int GridCount { get; set; }

        public Level LevelMaker(string name, int id)
        {
            
            Name = name;
            Id = id;
            switch (id)
            {
                case int n when n >= 0 && n < 5:
                    GridCount = 3;
                    break;
                case int n when n >= 5 && n < 10:
                    GridCount = 4;
                    break;
                case int n when n >= 10 && n < 20:
                    GridCount = 5;
                    break;
                case int n when n >= 20 && n < 30:
                    GridCount = 6;
                    break;
                case int n when n >= 30 /*&& n < 5*/:
                    GridCount = 7;
                    break;
            }
            return this;
        }
    }

    public enum  SceneIndexConstant
    {
        MainScene = 0,
        GameScene = 1,
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

        for (int i = 0; i < levelCountAll; i++)
        {
            Level tLevel = new Level();

            tLevel.LevelMaker("Level " + (i + 1), i);
            Levels.Add(tLevel);
        }

        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            currLevel = Levels[PlayerPrefs.GetInt("SavedLevel")];
        }
        else
        {
            currLevel = Levels[0];
        }

        gridList = GameObject.FindGameObjectsWithTag("Grid");
        GameObject beforeGrid = new GameObject();
        beforeGrid = gridList[0];
        int randCase = Random.Range(0, 4);
        GetComponent<ObjectPoolerNew>().FillTheRoad((ObjectPoolerNew.ObjectType)randCase, true, beforeGrid.transform);
        if (SceneMaker)
        {
            finishLine = GameObject.FindGameObjectWithTag("Finish");
            //finishLine.SetActive(false);
            
            float randomAngle = 0f;
            int randomAngleEnum = 0;
            int randomAxis = 0;
            for (int i = 0; i < gridCount; i++)
            {
                randomAngleEnum = Random.Range(0, 4);
                randomAxis = Random.Range(0, 2);
                beforeGrid = Instantiate(grid, beforeGrid.transform.Find("EndTip").position, Quaternion.identity);

                if (randomAxis == 0)
                {
                    switch (randomAngleEnum)
                    {
                        case 0:
                            randomAngle = 30f;
                            break;
                        case 1:
                            randomAngle = 15f;
                            break;
                        case 2:
                            randomAngle = -30f;
                            break;
                        case 3:
                            randomAngle = -15f;
                            break;
                    }
                    beforeGrid.transform.Rotate(Vector3.up, randomAngle);
                }
                else
                {
                    switch (randomAngleEnum)
                    {
                        case 0:
                            randomAngle = 15f;
                            break;
                        case 1:
                            randomAngle = 30f;
                            break;
                        case 2:
                            randomAngle = -30f;
                            break;
                        case 3:
                            randomAngle = -15f;
                            break;
                    }
                    beforeGrid.transform.Rotate(Vector3.right, randomAngle);
                }
                
                beforeGrid.SetActive(true);
                waypointGrid = GameObject.FindGameObjectsWithTag("WayPoint");
                waypointGrid = waypointGrid.OrderBy(wp => wp.transform.localPosition.z).ToArray();
                foreach (var item in waypointGrid)
                {
                    if(item.transform.parent == beforeGrid.transform)
                        waypointAll.Add(item);
                }
                randCase = Random.Range(0,4);
                GetComponent<ObjectPoolerNew>().FillTheRoad((ObjectPoolerNew.ObjectType)randCase, true, beforeGrid.transform);
            }
            gridList = GameObject.FindGameObjectsWithTag("Grid");
            //foreach (var grid in gridList)
            //{
            //    grid.transform.eulerAngles = new Vector3(grid.transform.eulerAngles.x, grid.transform.eulerAngles.y, 0);
            //}
       
            finalWP = finalWP.OrderBy(wp => wp.transform.localPosition.z).ToArray();
            foreach (var item in finalWP)
            {
                waypointAll.Add(item);
            }

            var exp = gridList[gridList.Length - 1].transform.Find("EndTip");
            finishLine.transform.parent = exp;
            finishLine.transform.localPosition = Vector3.zero;
            finishLine.transform.localRotation = Quaternion.Euler(0,180,0) ;
            
            //finishLine.SetActive(true);
            playerControllerWP._wayPoints = waypointAll;
        }
        //GameOverObj.gameObject.transform.parent.gameObject.SetActive(false);
        //GameAnalyticsSDK.GameAnalytics.Initialize();

    }

    void Start()
    {
        if(deleteSave)
            PlayerPrefs.DeleteAll();
        levelName.text = currLevel.Name;
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
        Gcontroller._canTurn = false;
        //Time.timeScale = 0;
        playerControllerWP.PlayerSpeed = 0;
        _player.GetComponent<SphereCollider>().enabled = false;
        
        //rivalController.speed = 0;

        var animS = _player.GetComponentInChildren<PlayerImageControllerCiftAdam>().animPlayer;
        if (playerName == "Player")
        {
            youLose.gameObject.SetActive(false);
            youWin.gameObject.SetActive(true);

            PlayerPrefs.SetInt("SavedLevel", currLevel.Id + 1);
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
