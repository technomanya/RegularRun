using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScaler : MonoBehaviour
{
    [SerializeField] private float seconds = 1.0f;
    [SerializeField] private float scaleAmount = 0.5f;

    [SerializeField]private Vector3 mainScale;

    void Start()
    {
        mainScale = gameObject.transform.localScale;
    }

    void Update()
    {
        
        if (gameObject.transform.localScale.x >= mainScale.x * scaleAmount)
            StartCoroutine(DelayedReturn());
        else
        {
            gameObject.transform.localScale = Vector3.Slerp(gameObject.transform.localScale, mainScale * this.scaleAmount, Time.deltaTime);
        }
    }

    public IEnumerator DelayedReturn()
    {
        yield return new WaitForSeconds(seconds);
        gameObject.transform.localScale = mainScale;
    }
}
