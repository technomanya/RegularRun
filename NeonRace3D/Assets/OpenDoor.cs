using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Animator openDoorAnim;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            openDoorAnim = GetComponent<Animator>();
            openDoorAnim.SetTrigger("OpenDoor");
        }
    }
}
