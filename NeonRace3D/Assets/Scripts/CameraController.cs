using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private bool effectOn = false;

    public List<int> list = new List<int>();

    public float speed;

    void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            list.Add(5);
        }

        list[4] = 0;

        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }

    void Start()
    {
        list[0] = list[list.Count - 1];
        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }

    void LateUpdate()
    {
        if(effectOn)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0,-60,0), Time.deltaTime * speed);
    }

    public void CameraEffect()
    {
        effectOn = true;
        //transform.localEulerAngles = new Vector3(0, 45, 0);
        
        // StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2.0f);
        effectOn = false;
        transform.localEulerAngles =new Vector3(0,-90,0);
    }
}
