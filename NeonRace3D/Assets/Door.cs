using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorPivot;
    [SerializeField] private bool doorFall = false;
    [SerializeField] private float speed = 1; 


    void Update()
    {
        if (doorFall)
        {
            doorPivot.localRotation = Quaternion.Slerp(doorPivot.localRotation, Quaternion.Euler(-90, -180, 320), Time.deltaTime * speed);

        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Player"))
        {
            doorFall = true;
            //GetComponent<Rigidbody>().useGravity = true;
            //GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
