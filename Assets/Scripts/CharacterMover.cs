using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour {

	private Animator animator;
	public float speed;
	private bool forward;


	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
        forward = true;
	}

	// Update is called once per frame
	void Update () {

		
		var horizontal = Input.GetAxis("Horizontal");

		if (Input.GetKeyDown(".")) {
            animator.SetTrigger("run");
            if (!forward)
            {
                speed *= -1;
            }
            forward = true;
		}
        else if (Input.GetKeyDown(","))
        {
            animator.SetTrigger("run");
            if (forward)
            {
                speed *= -1;
            }
            forward = false;
        }
    }

	void FixedUpdate() {
		Vector3 pos = transform.position;
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("SkeletonWest")) {
			pos.x -= speed;
		} else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterIdle")) {
			pos.x += speed;
            transform.position = pos;
        } 
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			//grounded = true;
		}
	}
}
