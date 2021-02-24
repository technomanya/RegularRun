using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreakBehaviour : MonoBehaviour
{
    [SerializeField]private Rigidbody[] glassParts;
    // Start is called before the first frame update
    void Start()
    {
        glassParts = GetComponentsInChildren<Rigidbody>();
        foreach (var part in glassParts)
        {
            if (!part.isKinematic)
                part.isKinematic = true;
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
                part.isKinematic = false;
            }

            //gameObject.GetComponent<MeshRenderer>().enabled = false;
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
                part.isKinematic = false;
            }
        }
    }
}
