using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{


    public Transform loadingBar;

    AsyncOperation async;
    [SerializeField]
    public float currentAmount, currentAmountpointer;
    [SerializeField]
    private float speed;
    public static Loading load;
    
    public void Awake()
    {
        load = this;
        currentAmountpointer = 0;
    }


    void Update()
    { 
        
       loadingBar.GetComponent<Image>().fillAmount = currentAmount / 1000;

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

