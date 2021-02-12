using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using NUnit.Framework.Constraints;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private bool effectOn = false;
    [SerializeField]private bool effectGameOver = false;
    [SerializeField] private bool failGameOver = false;

    private Vector3 camLocalPos;
    private Quaternion camParentLocalRot;

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

        //foreach (var item in list)
        //{
        //    Debug.Log(item);
        //}
    }

    void Start()
    {
        camLocalPos = Camera.main.transform.localPosition;
        camParentLocalRot = transform.localRotation;
        //Castle = GameObject.FindGameObjectWithTag("Finish").transform;
        //list[0] = list[list.Count - 1];
        //foreach (var item in list)
        //{
        //    //Debug.Log(item);
        //}
    }

    void Update()
    {
        Castle.localRotation = Quaternion.Euler(0,180, -gameObject.transform.parent.rotation.eulerAngles.z);
    }

    void LateUpdate()
    {
        if(effectOn)
        {
            Camera.main.transform.localPosition =
                Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0, 2, -15), Time.deltaTime * speed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(15, -60, 0),
                Time.deltaTime * speed);
        }
        if(effectGameOver)
        {
            Camera.main.transform.localPosition =
                Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0,3 , -7.5f), Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(30, 0, 0),
                Time.deltaTime * speed);
        }

        if (failGameOver)
        {
            Camera.main.transform.localPosition =
                Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0, 2, -10), Time.deltaTime * speed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(30, 180, 0),
                Time.deltaTime * speed);
        }
    }

    public void CameraEffect()
    {
        effectOn = true;
        //transform.localEulerAngles = new Vector3(0, 45, 0);
        
        // StartCoroutine(Reset());
    }

    public void GameOverEffect(bool win)
    {
        if(win)
        {
            if (effectOn)
                effectOn = false;

            effectGameOver = true;
        }
        else
        {
            failGameOver = true;
        }
    }

    //public IEnumerator Reset(Vector3 rPos, Quaternion rRot)
    //{
    //    yield return new WaitUntil(true);
    //    effectOn = false;
    //    Camera.main.transform.localPosition = rPos;
    //    transform.localRotation = rRot;
    //}
}
