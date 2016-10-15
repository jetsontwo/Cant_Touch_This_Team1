using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Load_Level : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void load_level()
    {
        SceneManager.LoadScene("game_scene");
    }
}
