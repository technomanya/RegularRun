using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerControllerWaypoint : MonoBehaviour
{
    [SerializeField] private GridController _gridController;
    private int current = 0;
    private Rigidbody rb;

    public GameObject[] _wayPoints;
    public float PlayerSpeed;
    public float frequency = 1.0f;
    public float mouseX = 0.0f;
    public Text speedText;
    public InputType InputT;

    public enum InputType
    {
        Swipe,
        Hold
    }

    void Awake()
    {
        _wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
        foreach (var wp in _wayPoints)
        {
            wp.transform.SetParent(null);
        }

        _wayPoints.OrderBy(_wayPoints => _wayPoints.transform.position.z);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _gridController = GetComponent<GridController>();
    }

    void Update()
    {
        //Debug.Log("Distance= "+ Vector3.Distance(_wayPoints[current].transform.position, transform.position));
        
        if (Vector3.Distance(_wayPoints[current].transform.position, transform.position) < 0.5 && Time.timeScale > 0)
        {
            //Debug.Log(_wayPoints[current].transform.position);
            //Debug.Log(current);
            current++;
            if (current >= _wayPoints.Length)
            {
                current = 0;
            }
            
        }
        MovePlayer(transform, _wayPoints[current].transform, PlayerSpeed);
        InputControlWayPoint();
        //speedText.text = "SPEED: " + (PlayerSpeed*10) + " mph";
    }

    void MovePlayer(Transform _here, Transform _there, float speed)
    {
        transform.position = Vector3.MoveTowards(_here.position, _there.position,
           speed*Time.deltaTime);

        //rb.velocity = Vector3.MoveTowards(_here.position, _there.position,
        //    speed*Time.deltaTime);
        //transform.position = Vector3.Slerp(_here.position, _there.position, speed * Time.deltaTime);
        //Debug.Log("PlayerPosition= "+transform.position);
    }

    void InputControlWayPoint()
    {
        mouseX = Input.mousePosition.x;

        if (Input.GetMouseButton(0))
        {
            if (InputT == InputType.Hold)
            {
                mouseX = mouseX - Screen.width;
            }
            else if(InputT == InputType.Swipe)
            {
                mouseX = Input.GetAxis("Mouse X");
            }
                
            _gridController.ContiniousSideTurn(mouseX);
        }
        else if(Input.GetMouseButtonUp(0) )
        {
            if (GameObject.FindGameObjectWithTag("Animator"))
            {

                GameObject.FindGameObjectWithTag("Animator").transform.localEulerAngles = Vector3.zero;
            }
        }

    }

    public void InputChange()
    {
        if (InputT == InputType.Hold)
            InputT = InputType.Swipe;
        else if (InputT == InputType.Swipe)
            InputT = InputType.Hold;

    }

    public void PlayerControlBegin()
    {
        if (_wayPoints.Length > 0)
        {
            _wayPoints = null;
        }
        _wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
        foreach (var wp in _wayPoints)
        {
            wp.transform.SetParent(null);
        }

        _wayPoints.OrderBy(_wayPoints => _wayPoints.transform.position.z);
    }
}
