using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPooler : MonoBehaviour
{
    public GameObject[] poolObjects;
    public float distanceMin;
    public float distanceMax;
    public PlayerControllerWaypoint playerContWP;

    [SerializeField] private int objCount = 0;
    [SerializeField] private List<GameObject> poolObjectsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "SceneMaker")
        {
            if (poolObjects[0].CompareTag("PowerObj") || poolObjects[0].CompareTag("CoinPivot"))
                MakeObjects(poolObjects[0]);
            else if (poolObjects[0].CompareTag("ObstacleObj"))
            {
                int oddTemp = Random.Range(0, 10);
                if (oddTemp < 5)
                    MakeObjects(poolObjects[0]);
                else if (oddTemp < 8)
                    MakeObjects(poolObjects[1]);
                else
                    MakeObjects(poolObjects[2]);
            }
        }

        playerContWP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerWaypoint>();
    }

    void MakeObjects(GameObject obj)
    {
        GameObject tempObj;
        for (int i = 0; i < objCount; i++)
        {
            tempObj = Instantiate(obj, Vector3.zero, Quaternion.identity);
            tempObj.transform.SetParent(gameObject.transform);
            tempObj.SetActive(true);
            poolObjectsList.Add(tempObj);
        }
        MoveObjects(poolObjectsList);
    }

    void MoveObjects(List<GameObject> objList)
    {
        float posZ = 0;
        float posX = 0;
        float posY = 0;
        int posXYodds = 0;
        float angle = 0;
        foreach (var obj in objList)
        {
            posX = posZ += Random.Range(distanceMin, distanceMax);
            posXYodds = Random.Range(0, 4);
            switch (posXYodds)
            {
                case 0:
                    //posX = 1.5f;
                    posY = 0;
                    angle = 0f;
                    break;
                case 1:
                    //posX = -1.5f;
                    posY = 0;
                    angle = 90.0f;
                    break;
                case 2:
                    //posX = 0;
                    posY = 1.5f;
                    angle = 180.0f;
                    break;
                case 3:
                    //posX = 0;
                    posY = -1.5f;
                    angle = 270.0f;
                    break;
            }
            if (obj.CompareTag("PowerObj") || obj.CompareTag("CoinPivot"))
            {
                obj.transform.localEulerAngles = new Vector3(angle, 90, 0); ;
                obj.transform.localPosition = new Vector3(0, 0, posZ);
            }
            else if (obj.CompareTag("ObstacleObj"))
            {
                obj.transform.localEulerAngles = new Vector3(0,0, angle);
                obj.transform.localPosition = new Vector3(0, 0, posZ);
            }

        }
    }
}
