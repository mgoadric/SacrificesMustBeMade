using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    public GameObject player;
    public bool[] taken;
    public GameObject[] images;
    public bool active;

    // Use this for initialization
    void Start()
    {
        taken = new bool[6];
        foreach (GameObject img in images)
        {
            img.SetActive(false);
        }
    }

    public void Activate()
    {
        active = true;
        foreach (GameObject img in images)
        {
            img.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown("1") && !taken[0])
            {
                taken[0] = true;
                images[0].SetActive(false);
                Debug.Log("Took 1");
                player.GetComponent<CharacterMover>().items++;
            }
            else if (Input.GetKeyDown("2") && !taken[1])
            {
                taken[1] = true;
                images[1].SetActive(false);
                Debug.Log("Took 2");
                player.GetComponent<CharacterMover>().items++;

            }
            else if (Input.GetKeyDown("3") && !taken[2])
            {
                taken[2] = true;
                images[2].SetActive(false);
                Debug.Log("Took 3");
                player.GetComponent<CharacterMover>().items++;

            }
            else if (Input.GetKeyDown("4") && !taken[3])
            {
                taken[3] = true;
                images[3].SetActive(false);
                Debug.Log("Took 4");
                player.GetComponent<CharacterMover>().items++;

            }
            else if (Input.GetKeyDown("5") && !taken[4])
            {
                taken[4] = true;
                images[4].SetActive(false);
                Debug.Log("Took 5");
                player.GetComponent<CharacterMover>().items++;

            }
            else if (Input.GetKeyDown("6") && !taken[5])
            {
                taken[5] = true;
                images[5].SetActive(false);
                Debug.Log("Took 6");
                player.GetComponent<CharacterMover>().items++;

            }
        }
    }
}
