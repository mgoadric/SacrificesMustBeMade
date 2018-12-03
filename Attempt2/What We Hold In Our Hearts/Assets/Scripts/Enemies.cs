using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

    public GameObject[] enemies;

	// Use this for initialization
	void Start () {
        Deactivate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject e = enemies[i];
            e.SetActive(true);
            e.GetComponent<CharacterAI>().StartRunning();
        }
    }

    public void Deactivate()
    {
        foreach (GameObject e in enemies)
        {
            e.SetActive(false);
        }
    }


}
