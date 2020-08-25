using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
    public void OpenSceneByBuildId(int sceneId)
    {
        Debug.Log(sceneId);
        SceneManager.LoadScene(sceneId);
    }
}
