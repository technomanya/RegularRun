using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private float tempSpeed;

    public Transform mainCam;
    public GridController GridControl;
    public GameManager GM;
    public float speed;
    public int jumpForce;
    public CameraController camControl;
    public PlayerControllerWaypoint PlayerWP;

    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GridControl = GetComponent<GridController>();
        mainCam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        PlayerWP = GetComponent<PlayerControllerWaypoint>();
        speed = PlayerWP.PlayerSpeed;
        tempSpeed = speed;
        camControl = GetComponentInChildren<CameraController>();
    }

    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);

        InputControl(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collide");
        if(other.transform.CompareTag("StartTip") && other.GetType() == typeof(SphereCollider))
        {
            StartCoroutine(RotateOverTime(gameObject.transform, gameObject.transform.rotation, Quaternion.Euler(other.transform.parent.eulerAngles.x,other.transform.parent.eulerAngles.y,gameObject.transform.eulerAngles.z), 2/speed));
            //camControl.CameraEffect();
        }

        if (other.transform.CompareTag("TurnBanner"))
        {
            GridControl._canTurn = false;
            tempSpeed = speed;
            PlayerWP.PlayerSpeed = speed + 30;
        }

        if (other.transform.CompareTag("EndTip"))
        {
            PlayerWP.PlayerSpeed = tempSpeed;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            gameObject.transform.position = other.transform.position;
            //GM.GameOver(gameObject.tag);
        }

        else if (other.transform.CompareTag("TurnBanner"))
        {
            GridControl._canTurn = false;
        }

        else if (other.transform.CompareTag("Corrector"))
        {
            GridControl._canTurn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("EndTip"))
        {
            //Debug.Log(other.transform.parent.name);
            GridControl.ChangeBaseGrid(other.transform.parent.gameObject);
        }
        if (other.transform.CompareTag("StartTip"))
        {
            gameObject.transform.position = other.transform.parent.Find("Corrector").position;
        }

        if (other.transform.CompareTag("TurnBanner"))
        {
            GridControl._canTurn = true;
            PlayerWP.PlayerSpeed = tempSpeed;
        }
    }

    IEnumerator RotateOverTime(Transform movingHandle, Quaternion originalRotation, Quaternion finalRotation, float duration)
    {
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            movingHandle.rotation = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                // progress will equal 0 at startTime, 1 at endTime.
                movingHandle.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        movingHandle.localRotation = finalRotation;
    }

    void InputControl(int sceneIndex)
    {
        float mouseX = Input.mousePosition.x;

        switch (sceneIndex)
        {
            case (int)GameManager.SceneIndexConstant.FourSidedScene:
                if (Input.GetMouseButtonDown(0))
                {
                    if (mouseX < Screen.width / 2)
                        GridControl.TurnLeft();
                    else if (mouseX > Screen.width / 2)
                        GridControl.TurnRight();
                }
                break;
            case (int)GameManager.SceneIndexConstant.AllSidedScene:
                float mouseInitialX = Input.mousePosition.x; 
                if (Input.GetMouseButton(0))
                {
                    mouseX = Input.GetAxis("Mouse X");
                    GridControl.ContiniousSideTurn(mouseX);
                }
                break;
            default:
                break;
        }
        
        
    }
}
