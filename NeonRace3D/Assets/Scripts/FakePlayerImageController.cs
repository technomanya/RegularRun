using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class FakePlayerImageController : MonoBehaviour
{
    [SerializeField] private GridController Gcontrol;
    private float speed;
    public FakePlayerController PlayerController;

    void Start()
    {
        Gcontrol = GetComponentInParent<GridController>();
        PlayerController = GetComponentInParent<FakePlayerController>();
        speed = PlayerController.speed;
    }

    // Update is called once per frame
    void Update()
    {
        float rand = Random.value;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.parent != null && hit.transform.parent.CompareTag("Obstacle"))
            {
                if(rand >= 0.5f)
                    Gcontrol.TurnRight();
                else
                {
                    Gcontrol.TurnLeft();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collide");
        if (other.transform.parent.CompareTag("Obstacle") && speed > 5.0f)
        {
            //Destroy(other.gameObject);
            speed -= speed * 0.2f;
            PlayerController.speed = speed;
            //gameObject.GetComponent<GridController>().speed -= gameObject.GetComponent<GridController>().speed * 0.2f;
        }
        else if (other.transform.parent.CompareTag("Power") && speed < 20.0f)
        {
            //Destroy(other.gameObject);
            speed += speed * 0.5f;
            PlayerController.speed = speed;
            //gameObject.GetComponent<GridController>().speed *= 2;

        }
    }
}
