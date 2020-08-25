using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public GameObject Grid;
    public GameManager GM;
    private GameObject oldGrid;
    [SerializeField] private GameObject player;
    [SerializeField] private float allSideRotAngle = 15.0f;

    public Animator tronRunning;
 
    public float speed;

    public float rotDuration;

    public bool _canTurn;

    public void ChangeBaseGrid(GameObject newGrid)
    {
        oldGrid = Grid;
        Grid = newGrid;
        //Invoke("DestroyGrid",2);
    }

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Grid = GM.gridList[0];
        _canTurn = false;
        tronRunning = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player");
        //allSideRotAngle = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TurnRight();
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            TurnLeft();
        }
    }

    IEnumerator Rotate(GameObject rotateObj, Vector3 axis, float angle, float duration = 1.0f)
    {
        Quaternion from = rotateObj.transform.rotation;
        Quaternion to = rotateObj.transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            rotateObj.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rotateObj.transform.rotation = to;
        _canTurn = true;
    }

    public void TurnRight()
    {
        if (_canTurn)
        {
            tronRunning.GetComponent<Animation>().GetClip("Running@Running Left Turn");
            //tronRunning.SetTrigger("RightTurnSingle");
            _canTurn = false;
            StartCoroutine(Rotate(gameObject, Vector3.forward, -90, rotDuration));
        }
        
    }

    public void TurnLeft()
    {
        if (_canTurn)
        {
            //tronRunning.SetTrigger("LeftTurnSingle");
            _canTurn = false;
            StartCoroutine(Rotate(gameObject, Vector3.forward, 90, rotDuration));
        }
        
    }

    public void ContiniousSideTurn(float mousePosX)
    {
        if (_canTurn)
        {
            if(mousePosX > 0)
            {
                //tronRunning.SetTrigger("RightTurn");
                gameObject.transform.Rotate(Vector3.forward, allSideRotAngle * -1 * Time.deltaTime);
                //if(GameObject.FindGameObjectWithTag("Animator"))
                //{

                //    GameObject.FindGameObjectWithTag("Animator").transform.localEulerAngles = Vector3.up*30;
                //}
            }
            else if(mousePosX < 0)
            {
                //tronRunning.SetTrigger("LeftTurn");
                gameObject.transform.Rotate(Vector3.forward, allSideRotAngle * 1 * Time.deltaTime);
                //if (GameObject.FindGameObjectWithTag("Animator"))
                //{

                //    GameObject.FindGameObjectWithTag("Animator").transform.localEulerAngles = Vector3.up * -30;
                //}
            }
            else
            {
                tronRunning.ResetTrigger("RightTurn");
                tronRunning.ResetTrigger("LeftTurn");
                tronRunning.SetTrigger("RunIdle");
                gameObject.transform.Rotate(Vector3.forward, 0.0f);
            }
        }

    }

    void DestroyGrid()
    {
        Destroy(oldGrid);
    }
}
