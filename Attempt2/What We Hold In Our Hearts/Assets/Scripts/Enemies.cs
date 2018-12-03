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
        StartCoroutine("MakeActiveRandom");
    }

    IEnumerator MakeActiveRandom()
    {
        foreach (GameObject e in enemies)
        {
            e.SetActive(true);
            e.GetComponent<CharacterAI>().StartRunning();
            yield return new WaitForSeconds(Random.Range(0.01f, 0.04f));
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
