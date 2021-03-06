﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFriend : MonoBehaviour {

	private Animator animator;
    private AudioSource source;

    public float speed;
    public float boost;

    // Use this for initialization
    void Start () {
		animator = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        speed = 0;
        boost = 1.003f;
	}

    // Sometimes we just need to be somewhere else
    public void ForceMove(float deltax, float deltay)
    {
        Vector3 pos = transform.position;
        pos.x += deltax;
        pos.y += deltay;
        transform.position = pos;
    }

    public void StartRunning(float speed)
    {
        this.speed = speed;
        StartCoroutine("AIRun");
        source.Play();
    }

    public void StopRunning()
    {
        StopCoroutine("AIRun");
        speed = 0;
        source.Stop();
    }

    IEnumerator AIRun()
    {
        for (; ; )
        {
            animator.SetTrigger("run");
            yield return new WaitForSeconds(0.5f);
            speed *= boost;
        }
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun2"))
        {
            Vector3 pos = transform.position;
            pos.x += speed;
            transform.position = pos;
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if ((GameManager.S.gamestate == State.RUN || GameManager.S.gamestate == State.FRIENDSAFE) && coll.gameObject.tag == "Item")
        {
            Destroy(coll.gameObject);
        }
        else if (GameManager.S.gamestate == State.NEWS && coll.gameObject.tag == "Character")
        {
            GameManager.S.gamestate = State.WAIT;

        } else if ((GameManager.S.gamestate == State.RUN || GameManager.S.gamestate == State.FRIENDSAFE) && coll.gameObject.tag == "GoalHouse")
        {
            if (gameObject.tag == "Friend")
            {
                GameManager.S.gamestate = State.FRIENDSAFE;
            } else if (gameObject.tag == "Enemy")
            {
                coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
