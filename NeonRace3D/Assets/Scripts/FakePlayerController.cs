using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private float tempSpeed;

    public Transform mainCam;
    public GridController Gcontrol;
    public float speed;
    public int jumpForce;


    void Start()
    {
        Gcontrol = GetComponent<GridController>();
        mainCam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        tempSpeed = speed;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        float mouseX = Input.mousePosition.x;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (mouseX < Screen.width / 2)
        //        Gcontrol.TurnLeft();
        //    else if (mouseX > Screen.width / 2)
        //        Gcontrol.TurnRight();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collide");
        if (other.transform.CompareTag("StartTip") && other.GetType() == typeof(SphereCollider))
        {
            //Debug.Log(other.transform.parent.name);
            StartCoroutine(RotateOverTime(gameObject.transform, gameObject.transform.rotation, Quaternion.Euler(other.transform.parent.eulerAngles.x, other.transform.parent.eulerAngles.y, gameObject.transform.eulerAngles.z), 2 / speed));
        }

        if (other.transform.CompareTag("TurnBanner"))
        {
            Gcontrol._canTurn = false;
            tempSpeed = speed;
            speed = 25.0f;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("TurnBanner"))
        {
            Gcontrol._canTurn = false;
        }

        if (other.transform.CompareTag("Corrector"))
        {
            Gcontrol._canTurn = true;
            speed = tempSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("StartTip"))
        {
            gameObject.transform.position = other.transform.parent.Find("Corrector").position;
        }

        if (other.transform.CompareTag("TurnBanner"))
        {
            Gcontrol._canTurn = true;
            speed = tempSpeed;
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
