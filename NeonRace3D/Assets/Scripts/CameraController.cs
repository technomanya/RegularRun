using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distanceMain;

    private float offsetX;
    private float offsetY;
    private float offsetZ;
    private bool side = true;

    public Vector3 angleBefore;
    public Vector3 angleLast;

    private void Start()
    {
        player = transform.parent.gameObject;
        offset = transform.position - player.transform.position;
        distanceMain = Mathf.Abs(offset.magnitude);
    }

    private void Update()
    {
        angleBefore = player.transform.eulerAngles;
    }

    private void LateUpdate()
    {
        //angleLast = player.transform.eulerAngles;
        //if(angleLast.x > angleBefore.x || angleLast.y > angleBefore.y )
        //    CameraEffect(true);
        //else if(angleLast.x < angleBefore.x || angleLast.y < angleBefore.y)
        //    CameraEffect(false);
    }

    public void CameraEffect()
    {
        if (Random.Range(-3, 2) < 0)
            side = false;
        else
            side = true;

        if (side)
        {
          transform.localPosition = Vector3.Slerp(transform.localPosition,new Vector3(-2, 0, -1),Time.deltaTime);
          transform.localEulerAngles = new Vector3(0,45,0);
        }
        else
        {
          transform.localPosition = new Vector3(2, 0, -1);
          transform.localEulerAngles = new Vector3(0,-45,0);
        }

        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.5f);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles =Vector3.zero;
    }
}
