using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grip : MonoBehaviour {

    public GameObject[] fingers;

    public int level = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < level; i++)
        {
            fingers[i].SetActive(true);
        }
        for (int i = level; i < fingers.Length; i++)
        {
            fingers[i].SetActive(false);

        }
    }

    public void Reset()
    {
        foreach (GameObject f in fingers)
        {
            f.SetActive(false);
        }
        level = 0;
    }
}
