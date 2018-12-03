using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterFriend : MonoBehaviour {

	private Animator animator;
	public float speed;
    public GameObject item;
    public GameObject dialogbox;
    public AudioClip scream;
    public bool forward;

    AudioSource source;


    // Use this for initialization
    void Start () {
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

    IEnumerator DoCheck()
    {
        for (; ; )
        {
            animator.SetTrigger("run");
            yield return new WaitForSeconds(0.5f);
            speed *= 1.002f;
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

            // DIALOG TIME
            dialogbox.GetComponent<TextMeshPro>().text="THEY FOUND US, GRAB WHAT YOU CAN AND RUN!";
        }
        else if (coll.gameObject.tag == "Item")
        {
            Destroy(coll.gameObject);
        }
    }
}
