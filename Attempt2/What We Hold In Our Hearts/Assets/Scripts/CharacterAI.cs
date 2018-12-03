using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : MonoBehaviour {

	private Animator animator;
	public float speed;
    public GameObject item;
    public AudioClip scream;

    AudioSource source;


    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public void StartRunning()
    {
        StartCoroutine("DoCheck");
    }

    // Update is called once per frame
    void Update () {

    }

	void FixedUpdate() {
		Vector3 pos = transform.position;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterIdle")) {
			pos.x += speed;
            transform.position = pos;
        } 
		
	}

    IEnumerator DoCheck()
    {
        for (; ; )
        {
            animator.SetTrigger("run");
            yield return new WaitForSeconds(0.5f);
            speed *= 1.004f;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Character")
        {
            source.PlayOneShot(scream);
        }
        else if (coll.gameObject.tag == "Item")
        {
            Destroy(coll.gameObject);
        }
    }
}
