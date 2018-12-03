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
    public AudioClip scream;
    public bool forward;
    public State mystate;

    AudioSource source;


    // Use this for initialization
    void Start () {
        mystate = State.NEWS;
		animator = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        Point(0, 180);
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
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterIdle")) {
            if (forward)
            {
                pos.x += speed;
            }
            else
            {
                pos.x -= speed;
            }
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
                dialogbox2.GetComponent<TextMeshPro>().text = "Hit the number to grab the item.";
            }
        }
        window.GetComponent<Window>().Deactivate();
        enemies.GetComponent<Enemies>().Activate();
        dialogbox.GetComponent<TextMeshPro>().text = "TOO LATE, RUN!";
        dialogbox2.GetComponent<TextMeshPro>().text = "< to move left, and > to move right.";
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
            if (count > 1)
            {
                goalhouse.transform.parent = null;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!forward && coll.gameObject.tag == "Character")
        {
            Point(0, 0);
            Vector3 pos = transform.position;
            pos.x -= 0.3f;
            transform.position = pos;
            speed = 0.08f;
            forward = true;
            mystate = State.WAIT;
        }
        else if (coll.gameObject.tag == "Item")
        {
            Destroy(coll.gameObject);
        } else if (coll.gameObject.tag == "GoalHouse")
        {
            mystate = State.WIN;
        }
    }
}
