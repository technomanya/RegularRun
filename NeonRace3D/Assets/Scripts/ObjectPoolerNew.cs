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
    public GameObject[] ObstaclesX15;
    public GameObject[] PowersX5;
    public GameObject[] PowersX10;
    public GameObject[] PowersX15;

    public ObjectType Type;

    public enum ObjectType
    {
        Obj5 = 0,
        Obj10 = 1,
        Obj15 = 2
    }

    void Awake()
    {

    }

    public void FillTheRoad(ObjectType type, bool power, Transform newParent)
    {
        Debug.Log("***Fillin Road***");

        GameObject tempPow = new GameObject();
        GameObject tempObs = new GameObject();

        if (power)
        {
            int angle = Random.Range(0, 2);
            switch (type)
            {
                case ObjectType.Obj15:
                    tempPow = Instantiate(PowersX15[0]);
                    tempPow.transform.parent = newParent;
                    tempPow.transform.localPosition = Vector3.zero;
                    tempPow.transform.localRotation = Quaternion.identity;
                    break;
                case ObjectType.Obj10:
                    tempPow = Instantiate(PowersX10[0]);
                    tempPow.transform.parent = newParent;
                    tempPow.transform.localPosition = Vector3.zero;
                    tempPow.transform.localRotation = Quaternion.identity;

                    tempObs = Instantiate(ObstaclesX5[angle]);
                    tempObs.transform.parent = newParent;
                    tempObs.transform.localPosition = Vector3.zero;
                    tempObs.transform.localRotation = Quaternion.identity;
                    break;
                case ObjectType.Obj5:
                    tempPow = Instantiate(PowersX5[0]);
                    tempPow.transform.parent = newParent;
                    tempPow.transform.localPosition = Vector3.zero;
                    tempPow.transform.localRotation = Quaternion.identity;

                    tempObs = Instantiate(ObstaclesX10[angle]);
                    tempObs.transform.parent = newParent;
                    tempObs.transform.localPosition = Vector3.zero;
                    tempObs.transform.localRotation = Quaternion.identity;
                    break;
                default:
                    break;
            }
        }
        else
        {
            int angle = Random.Range(0, 2);
            switch (type)
            {
                case ObjectType.Obj15:
                    tempObs = Instantiate(ObstaclesX15[angle]);
                    tempObs.transform.parent = newParent;
                    tempObs.transform.localPosition = Vector3.zero;
                    tempObs.transform.localRotation = Quaternion.identity;
                    break;
                case ObjectType.Obj10:
                    tempObs = Instantiate(ObstaclesX10[angle],newParent);
                    tempObs.transform.parent = newParent;
                    tempObs.transform.localPosition = Vector3.zero;
                    tempObs.transform.localRotation = Quaternion.identity;

                    tempPow = Instantiate(PowersX5[0],newParent);
                    tempPow.transform.parent = newParent;
                    tempPow.transform.localPosition = Vector3.zero;
                    tempPow.transform.localRotation = Quaternion.identity;
                    break;
                case ObjectType.Obj5:
                    tempObs = Instantiate(ObstaclesX5[angle]);
                    tempObs.transform.parent = newParent;
                    tempObs.transform.localPosition = Vector3.zero;
                    tempObs.transform.localRotation = Quaternion.identity;

                    tempPow = Instantiate(PowersX10[0]);
                    tempPow.transform.parent = newParent;
                    tempPow.transform.localPosition = Vector3.zero;
                    tempPow.transform.localRotation = Quaternion.identity;
                    break;
                default:
                    break;
            }
        }
        tempPow.SetActive(true);
        tempObs.SetActive(true);
    }

}
