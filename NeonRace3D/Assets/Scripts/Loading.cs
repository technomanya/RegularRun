using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Transform player;
    public Transform finishLine;

    private float maxDistance;
    private float currDistance;

    public Transform loadingBar;

    AsyncOperation async;
    [SerializeField]
    public float currentAmount, currentAmountpointer;
    [SerializeField]
    private float speed;
    public static Loading load;
    
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //finishLine = GameObject.FindGameObjectWithTag("Finish").transform;

        load = this;
        currentAmountpointer = 0;
    }

    public void Start()
    {
        maxDistance = Vector3.Distance(player.position , finishLine.position);
        currDistance = 0;
    }

    void Update()
    {
        currDistance = Vector3.Distance(player.position, finishLine.position);
        loadingBar.GetComponent<Image>().fillAmount = (maxDistance - currDistance) / maxDistance;

    }

    
    public void BarFullControl()//Nextbutton
    {
        if (loadingBar.GetComponent<Image>().fillAmount == 1)
        {
            currentAmount = 0;
            currentAmountpointer = currentAmount;
        }
        else
            currentAmountpointer = currentAmount;
    }

    public void BarRepeat()//Repeatbutton
    {
        if(currentAmountpointer != 0)
        {
            Debug.Log(currentAmountpointer);
           
            currentAmount = currentAmountpointer;
           // UIManager.uiTexts.L_SumPointInfo.text = currentAmount.ToString();
        }

        else
        {
          
            currentAmount = 0;
            currentAmountpointer = currentAmount;
            //UIManager.uiTexts.L_SumPointInfo.text = currentAmount.ToString();
        }
       
    }

}

