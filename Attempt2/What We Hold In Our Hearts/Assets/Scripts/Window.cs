﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    public GameObject player;
    public bool[] taken;
    public GameObject[] images;
    public bool active;
    public GameObject item;

    string[] descriptions =
    {
        "Your Dad always did look good in that photo.",
        "Couldn't leave Mom's war medals behind, eh?",
        "If we survive, we need to pawn the jewelery.",
        "Why do you keep those diaries?",
        "Hold your passport tight, you need it!",
        "Rosie would be happy you kept the bear."
    };

    // Use this for initialization
    void Start()
    {
        taken = new bool[6];
        foreach (GameObject img in images)
        {
            img.SetActive(false);
        }
    }


    public void Deactivate()
    {
        active = false;
        foreach (GameObject img in images)
        {
            img.transform.GetChild(0).gameObject.SetActive(false);
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

    void TakeSomething(int which)
    {
        taken[which] = true;
        images[which].SetActive(false);
        Debug.Log("Took " + (which + 1));
        GameObject it = Instantiate(item);
        it.GetComponent<Item>().description = descriptions[which];
        it.GetComponent<SpriteRenderer>().sprite = images[which].GetComponent<SpriteRenderer>().sprite;
        player.GetComponent<CharacterMover>().AddItem(it);
    }

    // Update is called once per frame
    void Update()
    {
        if (active && player.GetComponent<CharacterMover>().items.Count < 3)
        {
            if ((Input.GetKeyDown("1") || Input.GetKeyDown("[1]")) && !taken[0])
            {
                TakeSomething(0);
            }
            else if ((Input.GetKeyDown("2") || Input.GetKeyDown("[2]")) && !taken[1])
            {
                TakeSomething(1);
            }
            else if ((Input.GetKeyDown("3") || Input.GetKeyDown("[3]")) && !taken[2])
            {
                TakeSomething(2);
            }
            else if ((Input.GetKeyDown("4") || Input.GetKeyDown("[4]")) && !taken[3])
            {
                TakeSomething(3);
            }
            else if ((Input.GetKeyDown("5") || Input.GetKeyDown("[5]")) && !taken[4])
            {
                TakeSomething(4);
            }
            else if ((Input.GetKeyDown("6") || Input.GetKeyDown("[6]")) && !taken[5])
            {
                TakeSomething(5);
            }
        }
    }
}
