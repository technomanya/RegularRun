using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPoolerNew : MonoBehaviour
{
    [Header("Objects in Grid")]
    public GameObject[] ObstaclesX5;
    public GameObject[] ObstaclesX10;
    public GameObject[] PowersX5;
    public GameObject[] PowersX10;

    public ObjectType Type;

    public enum ObjectType
    {
        Lane1,
        Lane2,
        Lane3,
        Lane4
    }

    void Awake()
    {

    }

    public void FillTheRoad(ObjectType type, bool power, Transform newParent)
    {
        Debug.Log("***Fillin Road***");

        int anglePow = Random.Range(0, PowersX5.Length);
        int angleObs = Random.Range(0, ObstaclesX5.Length);
        switch (type)
        {
            case ObjectType.Lane1:
                var tempPow = Instantiate(PowersX10[anglePow]);
                tempPow.transform.parent = newParent;
                tempPow.transform.localPosition = Vector3.zero;
                tempPow.transform.localRotation = Quaternion.identity;

                var tempObs = Instantiate(ObstaclesX5[angleObs]);
                tempObs.transform.parent = newParent;
                tempObs.transform.localPosition = Vector3.zero + Vector3.forward * 60;
                tempObs.transform.localRotation = Quaternion.identity;

                tempPow.SetActive(true);
                tempObs.SetActive(true);
                break;
            case ObjectType.Lane2:
                tempPow = Instantiate(PowersX5[anglePow]);
                tempPow.transform.parent = newParent;
                tempPow.transform.localPosition = Vector3.zero;
                tempPow.transform.localRotation = Quaternion.identity;

                tempObs = Instantiate(ObstaclesX10[angleObs]);
                tempObs.transform.parent = newParent;
                tempObs.transform.localPosition = Vector3.zero + Vector3.forward * 25;
                tempObs.transform.localRotation = Quaternion.identity;

                tempPow.SetActive(true);
                tempObs.SetActive(true);
                break;
            case ObjectType.Lane3:
                tempPow = Instantiate(PowersX5[anglePow]);
                tempPow.transform.parent = newParent;
                tempPow.transform.localPosition = Vector3.zero;
                tempPow.transform.localRotation = Quaternion.identity;

                tempObs = Instantiate(ObstaclesX10[angleObs]);
                tempObs.transform.parent = newParent;
                tempObs.transform.localPosition = Vector3.zero + Vector3.forward * 35;
                tempObs.transform.localRotation = Quaternion.identity;

                tempPow.SetActive(true);
                tempObs.SetActive(true);
                break;
            case ObjectType.Lane4:
                tempPow = Instantiate(PowersX5[anglePow]);
                tempPow.transform.parent = newParent;
                tempPow.transform.localPosition = Vector3.zero;
                tempPow.transform.localRotation = Quaternion.identity;

                tempObs = Instantiate(ObstaclesX10[angleObs]);
                tempObs.transform.parent = newParent;
                tempObs.transform.localPosition = Vector3.zero + Vector3.forward * 35;
                tempObs.transform.localRotation = Quaternion.identity;

                tempPow.SetActive(true);
                tempObs.SetActive(true);
                break;
            default:
                break;
        }

        
    }

}
