using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    public void GoToTitle()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
