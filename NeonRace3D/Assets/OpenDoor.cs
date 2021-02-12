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
            Camera.main.gameObject.GetComponentInParent<CameraController>().GameOverEffect();
            openDoorAnim = GetComponent<Animator>();
            openDoorAnim.SetTrigger("OpenDoor");
        }
    }
}
