﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Button_Commands : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void load_level()
    {
        SceneManager.LoadScene("Main");
    }

    public void quit()
    {
        Application.Quit();
    }
}