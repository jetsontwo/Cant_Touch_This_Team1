using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onClick()
    {
        SceneManager.LoadScene("game_scene");
    }
}
