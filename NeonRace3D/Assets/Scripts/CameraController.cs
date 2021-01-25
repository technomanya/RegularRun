using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private bool effectOn = false;
    [SerializeField]private bool effectGameOver = false;

    public List<int> list = new List<int>();

    public float speed;

    public Transform Castle;
    

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

    void Update()
    {
        Castle.localRotation = Quaternion.Euler(0,180, -gameObject.transform.parent.rotation.eulerAngles.z);
    }

    void LateUpdate()
    {
        if(effectOn)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0,-60,0), Time.deltaTime * speed);

        if(effectGameOver)
        {
            transform.localPosition =
                Vector3.Slerp(transform.localPosition, new Vector3(0, -3, -7), Time.deltaTime * speed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-5, -180, 0),
                Time.deltaTime * speed);
        }
    }

    public void CameraEffect()
    {
        effectOn = true;
        //transform.localEulerAngles = new Vector3(0, 45, 0);
        
        // StartCoroutine(Reset());
    }

    public void GameOverEffect()
    {
        effectGameOver = true;
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2.0f);
        effectOn = false;
        transform.localEulerAngles =new Vector3(0,-90,0);
    }
}
