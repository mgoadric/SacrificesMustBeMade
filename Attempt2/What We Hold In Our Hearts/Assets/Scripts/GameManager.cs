﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum State { NEWS, WAIT, RUN, FRIENDSAFE, DEAD, WIN }

public class GameManager : MonoBehaviour {

    public GameObject gamePrefab;
    public State gamestate;
    public GameObject game;

    public GameObject dialogbox;
    public GameObject instructions;
    public GameObject button;
    public Camera mcamera;
    public GameObject logo;

    public List<GameObject> mentioned;

    public static GameManager S;

    private void Awake()
    {
        S = this;
        Debug.Log("Starting");
        mentioned = new List<GameObject>();

        //StartCoroutine("GameScript");
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void StartGame()
    {
        if (game)
        {
            Destroy(game);
        }
        game = Instantiate(gamePrefab);
        StartCoroutine("GameScript");
        button.SetActive(false);
        logo.SetActive(false);
    }

    IEnumerator GameScript()
    {

        // SETUP

        game.GetComponent<GameItems>().friend.GetComponent<CharacterAI>().SetBoost(1.003f);
        game.GetComponent<GameItems>().enemies.GetComponent<Enemies>().SetBoost(1.004f);
        // set player boost too!!! TODO
        dialogbox.GetComponent<TextMeshProUGUI>().text = "";
        instructions.GetComponent<TextMeshProUGUI>().text = "";
        mcamera.GetComponent<FollowCam>().playerSprite = game.GetComponent<GameItems>().player;
        gamestate = State.NEWS;

        // NEWS - FRIEND RUNS IN

        game.GetComponent<GameItems>().friend.GetComponent<CharacterAI>().StartRunning(0.04f);
        while (gamestate == State.NEWS)
        {
            yield return new WaitForSeconds(0.05f);
        }

        game.GetComponent<GameItems>().friend.GetComponent<CharacterAI>().StopRunning();
        dialogbox.GetComponent<TextMeshProUGUI>().text = "... NOT SAFE! WE HAVE TO GO! GRAB THINGS!";
        game.GetComponent<GameItems>().window.GetComponent<Window>().Activate();
        yield return new WaitForSeconds(1f);

        // WAIT -- WINDOW ITEM PICKUP

        while (gamestate == State.WAIT)
        {
            instructions.GetComponent<TextMeshProUGUI>().text = "[Hit the number to grab the item]";
            if (game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().items.Count == 1)
            {
                dialogbox.GetComponent<TextMeshProUGUI>().text = "CAN'T WAIT FOREVER!";

            }
            else if (game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().items.Count == 2)
            {
                dialogbox.GetComponent<TextMeshProUGUI>().text = "HURRY UP!";
            }
            else if (game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().items.Count >= 3)
            {
                gamestate = State.RUN;

            }
            yield return new WaitForSeconds(0.03f);
        }
        game.GetComponent<GameItems>().window.GetComponent<Window>().Deactivate();

        // FRIEND AND ENEMIES START RUNNING

        game.GetComponent<GameItems>().friend.GetComponent<CharacterAI>().ForceMove(.8f, 0);
        game.GetComponent<GameItems>().friend.GetComponent<CharacterAI>().StartRunning(0.08f);

        game.GetComponent<GameItems>().enemies.GetComponent<Enemies>().Activate(0.082f);

        dialogbox.GetComponent<TextMeshProUGUI>().text = "TOO LATE, RUN!";
        instructions.GetComponent<TextMeshProUGUI>().text = "'<' left [keep hitting] right '>'";

        game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().mystate = State.RUN;

        // PLAYER RUNS, ABOUT 1 MINUTE UNTIL NEXT SAFEHOUSE

        int count = 0;
        while (gamestate == State.RUN)
        {
            yield return new WaitForSeconds(0.5f);
            count++;
            if (count == 10 || count == 30 || count == 55 || count == 80)
            {
                dialogbox.GetComponent<TextMeshProUGUI>().text = "";
            }
            else if (count == 20 || count == 45 || count == 70)
            {
                int savednum = game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().items.Count;
                if (savednum > 0)
                {
                    foreach (GameObject item in game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().items)
                    {
                        if (!mentioned.Contains(item))
                        {
                            mentioned.Add(item);
                            dialogbox.GetComponent<TextMeshProUGUI>().text = item.GetComponent<Item>().description;
                            break;
                        }
                    }
                }
            }

            if (count > 100)
            {
                // DROP OFF THE SAFEHOUSE AHEAD OF THE FRIEND
                game.GetComponent<GameItems>().goalhouse.transform.parent = transform.parent;
                dialogbox.GetComponent<TextMeshProUGUI>().text = "We can hide here, quiet!";
            }
        }

        // FRIEND IS SAFE, THEY STOP AND WAIT

        if (gamestate == State.FRIENDSAFE)
        {
            game.GetComponent<GameItems>().friend.GetComponent<CharacterAI>().ForceMove(0, 21.3f);
            game.GetComponent<GameItems>().friend.GetComponent<CharacterAI>().StopRunning();
        }
        while (gamestate == State.FRIENDSAFE)
        {
            yield return new WaitForSeconds(0.03f);
        }

        // PLAYER DIED

        if (gamestate == State.DEAD)
        {
            dialogbox.GetComponent<TextMeshProUGUI>().text = "OH NO!";
            instructions.GetComponent<TextMeshProUGUI>().text = "";
            button.SetActive(true);
        }

        // PLAYER MADE IT TO THE SAFEHOUSE

        else if (gamestate == State.WIN)
        {
            if (game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().items.Count == 0)
            {
                dialogbox.GetComponent<TextMeshProUGUI>().text = "Where are your things?";
            }
            else
            {
                dialogbox.GetComponent<TextMeshProUGUI>().text = "You made it!";
            }

            yield return new WaitForSeconds(2.03f);
            int savednum = game.GetComponent<GameItems>().player.GetComponent<CharacterMover>().items.Count;

            dialogbox.GetComponent<TextMeshProUGUI>().text = "You saved " + savednum + " treasured items.";
            instructions.GetComponent<TextMeshProUGUI>().text = "";
            button.SetActive(true);
        }
    }
}