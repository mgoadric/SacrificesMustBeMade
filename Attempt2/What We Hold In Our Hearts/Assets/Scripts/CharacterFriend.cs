using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum State { NEWS, WAIT, RUN, DEAD, WIN}

public class CharacterFriend : MonoBehaviour {

	private Animator animator;
	public float speed;
    public GameObject window;
    public GameObject goalhouse;
    public GameObject enemies;
    public GameObject item;
    public GameObject dialogbox;
    public GameObject dialogbox2;
    public GameObject player;
    public List<GameObject> mentioned;
    public State mystate;

    AudioSource source;


    // Use this for initialization
    void Start () {
        mystate = State.NEWS;
        mentioned = new List<GameObject>();
		animator = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        StartRunning();
	}

    void StartRunning()
    {
        StartCoroutine("DoCheck");
    }

    // Update is called once per frame
    void Update () {

    }

    void Point(float zangle, float yangle)
    {
        Vector3 eulerAngles = gameObject.transform.localEulerAngles;
        eulerAngles.z = zangle;
        eulerAngles.y = yangle;
        transform.localRotation = Quaternion.Euler(eulerAngles);
    }



    void FixedUpdate() {
		Vector3 pos = transform.position;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterIdle"))
        {
            pos.x += speed;
            transform.position = pos;
        }
		
	}

    void SetState(State s)
    {
        mystate = s;
    }

    IEnumerator DoCheck()
    {
        speed = 0.04f;
        while (mystate == State.NEWS)
        {
            animator.SetTrigger("run");
            yield return new WaitForSeconds(0.5f);
        }

        // DIALOG TIME
        dialogbox.GetComponent<TextMeshPro>().text = "IT IS NOT SAFE! WE HAVE TO GO!\nGRAB ONLY WHAT MATTERS!";
        source.Stop();
        window.GetComponent<Window>().Activate();
        yield return new WaitForSeconds(1f);
        while (mystate == State.WAIT)
        {
            yield return new WaitForSeconds(0.03f);
            if (player.GetComponent<CharacterMover>().items.Count == 1)
            {
                dialogbox.GetComponent<TextMeshPro>().text = "I CAN'T WAIT FOREVER!";

            }
            else if (player.GetComponent<CharacterMover>().items.Count == 2)
            {
                dialogbox.GetComponent<TextMeshPro>().text = "HURRY UP!";
            }
            else if (player.GetComponent<CharacterMover>().items.Count >= 3)
            {
                mystate = State.RUN;

            }
            else
            {
                dialogbox2.GetComponent<TextMeshPro>().text = "[Hit the number to grab the item]";
            }
        }
        Vector3 pos = transform.position;
        pos.x += 1.3f;
        transform.position = pos;
        window.GetComponent<Window>().Deactivate();
        enemies.GetComponent<Enemies>().Activate();
        dialogbox.GetComponent<TextMeshPro>().text = "TOO LATE, RUN!";
        dialogbox2.GetComponent<TextMeshPro>().text = "'<' left [keep hitting] right '>'";
        player.GetComponent<CharacterMover>().mystate = State.RUN;
        speed = 0.08f;
        source.Play();

        int count = 0;

        while (mystate == State.RUN)
        {
            animator.SetTrigger("run");
            yield return new WaitForSeconds(0.5f);
            speed *= 1.003f;
            count++;
            if (count == 10 || count == 30 || count == 55 || count == 80)
            {
                dialogbox.GetComponent<TextMeshPro>().text = "";
            }
            else if (count == 20 || count == 45 || count == 70)
            {
                int savednum = player.GetComponent<CharacterMover>().items.Count;
                if (savednum > 0)
                {
                    foreach (GameObject item in player.GetComponent<CharacterMover>().items)
                    {
                        if (!mentioned.Contains(item))
                        {
                            mentioned.Add(item);
                            dialogbox.GetComponent<TextMeshPro>().text = item.GetComponent<Item>().description;
                            break;
                        }
                    }
                }
            }

            if (count > 100)
            {
                goalhouse.transform.parent = transform.parent;
                dialogbox.GetComponent<TextMeshPro>().text = "We can hide here, quiet!";

            }

            if (player.GetComponent<CharacterMover>().mystate == State.DEAD)
            {
                mystate = State.DEAD;
                GameManager.S.GetComponent<GameManager>().gamestate = State.DEAD;
                dialogbox.GetComponent<TextMeshPro>().text = "OH NO!";

                dialogbox2.GetComponent<TextMeshPro>().text = "[press space to restart]";
            }
        }

        if (mystate == State.WIN)
        {
            source.Stop();
            pos = transform.position;
            pos.y += 21.3f;
            transform.position = pos;

            while (player.GetComponent<CharacterMover>().mystate == State.RUN)
            {
                yield return new WaitForSeconds(0.03f);
            }

            if (player.GetComponent<CharacterMover>().mystate == State.DEAD)
            {
                mystate = State.DEAD;
                GameManager.S.GetComponent<GameManager>().gamestate = State.DEAD;
                dialogbox.GetComponent<TextMeshPro>().text = "OH NO!";

                dialogbox2.GetComponent<TextMeshPro>().text = "[press space to restart]";

            }
            else
            {
                dialogbox.GetComponent<TextMeshPro>().text = "You made it!";

                yield return new WaitForSeconds(2.03f);
                int savednum = player.GetComponent<CharacterMover>().items.Count;

                dialogbox.GetComponent<TextMeshPro>().text = "You saved " + savednum + " treasured items.";

                dialogbox2.GetComponent<TextMeshPro>().text = "[press space to restart]";
                GameManager.S.GetComponent<GameManager>().gamestate = State.WIN;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (mystate == State.NEWS && coll.gameObject.tag == "Character")
        {
            
            speed = 0.08f;
            mystate = State.WAIT;
        
        } else if (coll.gameObject.tag == "GoalHouse")
        {
            mystate = State.WIN;
            
        }
    }

}
