using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

    public GameObject[] enemies;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject e = enemies[i];
            e.GetComponent<CharacterAI>().StartRunning();
        }
    }

}
