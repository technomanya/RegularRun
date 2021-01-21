using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreakBehaviour : MonoBehaviour
{
    [SerializeField]private List<GameObject> glassParts;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var part in glassParts)
        {
            //if(part.activeInHierarchy)
            //    part.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        foreach (var part in glassParts)
        {
            //if (part.activeInHierarchy)
            //    part.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            foreach (var part in glassParts)
            {
                part.transform.parent = null;
                part.SetActive(true);
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("**Breaking**");
            foreach (var part in glassParts)
            {
                part.transform.parent = null;
                part.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
