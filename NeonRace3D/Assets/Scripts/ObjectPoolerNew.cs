using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerNew : MonoBehaviour
{
    public GameObject poolObj;
    public float distanceMin;
    public float distanceMax;
    public PlayerControllerWaypoint playerContWP;

    [SerializeField] private int objCount = 0;
    [SerializeField] private List<GameObject> poolObjectsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        playerContWP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerWaypoint>();

        MakeObjects(poolObj);
    }

    // Update is called once per frame
    void Update()
    {

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
        float rotZ = 0;

        int posXYodds = 0;
        for (int i = 0; i < objList.Count; i++)
        {
            rotZ = Random.Range(0, 180);
            objList[i].transform.position = playerContWP._wayPoints[i].transform.position;
            objList[i].transform.rotation = Quaternion.identity;
            objList[i].transform.localEulerAngles = new Vector3(0,0, 0);
            //obj.transform.parent.rotation;
        }
    }
}
