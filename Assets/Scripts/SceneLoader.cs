using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public GameObject loadingScreen;

    private void Start()
    {
        loadingScreen.SetActive(false);
    }

    public void OnButtonStart()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("SpaceWalk");
    }
}
