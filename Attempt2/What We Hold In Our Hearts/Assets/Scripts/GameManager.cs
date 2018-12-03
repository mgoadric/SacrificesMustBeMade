using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject gamePrefab;
    public State gamestate;
    public GameObject game;

    public static GameManager S;

    private void Awake()
    {
        S = this;
        Debug.Log("Starting");
        game = Instantiate(gamePrefab);
        gamestate = State.NEWS;
    }
	
	// Update is called once per frame
	void Update () {
		if (gamestate == State.DEAD || gamestate == State.WIN)
        {
            Debug.Log("Can Restart!");
            if (Input.GetKeyDown("space"))
            {
                Destroy(game);
                game = Instantiate(gamePrefab);
                gamestate = State.NEWS;
            }
        }
	}
}
