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

    void Point(float zangle, float yangle)
    {
        Vector3 eulerAngles = gameObject.transform.localEulerAngles;
        eulerAngles.z = zangle;
        eulerAngles.y = yangle;
        transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    // Update is called once per frame
    void Update () {

		if (Input.GetKeyDown(".")) {
            if (animator.GetBool("run"))
            {
                Debug.Log("Testing");
            }
            animator.SetTrigger("run");
            if (!forward)
            {
                speed *= -1;
                Point(0, 0);
                Vector3 pos = transform.position;
                pos.x += 1f;
                transform.position = pos;
            }
            forward = true;
		}
        else if (Input.GetKeyDown(","))
        {
            animator.SetTrigger("run");
            if (forward)
            {
                speed *= -1;
                Point(0, 180);
                Vector3 pos = transform.position;
                pos.x += -1f;
                transform.position = pos;
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
