using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownController : MonoBehaviour
{
    public float speed = 1;
    public Vector3 direction = new Vector3(1, 0, 0);

    public Transform playerTransform;

    public Transform rivalTransform;

    public Transform endLine;

    private float distancePlayer;
    private float distanceRival;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rivalTransform = GameObject.FindGameObjectWithTag("Enemy").transform;
        endLine = GameObject.FindGameObjectWithTag("Finish").transform;

        transform.parent = playerTransform;
        transform.localPosition = new Vector3(0, 2.4f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        distancePlayer = Vector3.Distance(endLine.position, playerTransform.position);
        distanceRival = Vector3.Distance(endLine.position, rivalTransform.position);
        if (distancePlayer < distanceRival && transform.parent == rivalTransform)
        {
            ChangeOwner(playerTransform);
        }
        else if (distanceRival < distancePlayer && transform.parent == playerTransform)
        {
            ChangeOwner(rivalTransform);
        }

        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(direction * speed * Time.deltaTime);
    }

    void ChangeOwner(Transform other)
    {
        Debug.Log(other.tag);
        transform.parent = other;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = new Vector3(0, 2.4f, 0.2f);
        
    }

    IEnumerator TranslateOverTime(Transform movingHandle, Vector3 originalPosition, Vector3 finalPosition, float duration)
    {
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            movingHandle.position = originalPosition;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                // progress will equal 0 at startTime, 1 at endTime.
                movingHandle.position = Vector3.Slerp(originalPosition, finalPosition, progress);
                yield return null;
            }
        }
        movingHandle.position = finalPosition;
    }
}
