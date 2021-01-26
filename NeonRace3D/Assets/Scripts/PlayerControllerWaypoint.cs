﻿using System;
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
    private float tempSpeed;
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
        tempSpeed = PlayerSpeed;
        rb = GetComponent<Rigidbody>();
        _gridController = GetComponent<GridController>();
    }

    void Update()
    {

        if (Vector3.Distance(_wayPoints[current].transform.position, transform.position) < 0.5 && Time.timeScale > 0)
        {
            current++;
            if (current >= _wayPoints.Length)
            {
                current = 0;
            }
            
        }
        MovePlayer(transform, _wayPoints[current].transform, PlayerSpeed);
        InputControlWayPoint();
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
        Vector2 touchDeltaPosition;
#if UNITY_ANDROID
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            // Get movement of the finger since last frame
            touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            mouseX = touchDeltaPosition.x;
        }

#endif

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

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collide");
        if (other.transform.CompareTag("StartTip") && other.GetType() == typeof(SphereCollider))
        {
            StartCoroutine(RotateOverTime(gameObject.transform, gameObject.transform.rotation, Quaternion.Euler(other.transform.parent.eulerAngles.x, other.transform.parent.eulerAngles.y, gameObject.transform.eulerAngles.z), 2 / PlayerSpeed));
            //camControl.CameraEffect();
        }

        if (other.transform.CompareTag("TurnBanner"))
        {
            _gridController._canTurn = false;
            
            PlayerSpeed += 30;
        }

        if (other.transform.CompareTag("EndTip"))
        {
            PlayerSpeed = tempSpeed;
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
            _gridController._canTurn = false;
        }

        else if (other.transform.CompareTag("Corrector"))
        {
            _gridController._canTurn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("EndTip"))
        {
            //Debug.Log(other.transform.parent.name);
            _gridController.ChangeBaseGrid(other.transform.parent.gameObject);
        }
        if (other.transform.CompareTag("StartTip"))
        {
            gameObject.transform.position = other.transform.parent.Find("Corrector").position;
        }

        if (other.transform.CompareTag("TurnBanner"))
        {
            _gridController._canTurn = true;
            PlayerSpeed = tempSpeed;
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
}
