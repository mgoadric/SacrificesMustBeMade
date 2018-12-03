using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour {

	private Animator animator;
	public float speed;
	private bool forward;
    public int pressure;
    public List<GameObject> items;
    public AudioClip breathing;
    public State mystate;

    AudioSource source;


    // Use this for initialization
    void Start () {
		animator = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        mystate = State.WAIT;
        items = new List<GameObject>();
        forward = true;
	}

    void Point(float zangle, float yangle)
    {
        Vector3 eulerAngles = gameObject.transform.localEulerAngles;
        eulerAngles.z = zangle;
        eulerAngles.y = yangle;
        transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        item.transform.parent = transform;
        item.transform.position = transform.position;
        // TODO Put it in the right coordinates
    }

    public void DropItem()
    {

    }

    // Update is called once per frame
    void Update () {
        if (mystate == State.RUN)
        {
            if (Input.GetKeyDown("."))
            {
                if (animator.GetBool("run"))
                {
                    pressure++;
                    speed *= 1.01f;
                }

                if (pressure % 14 == 1)
                {
                    source.PlayOneShot(breathing);
                    pressure++;
                }

                // Drop something
                if (pressure > 30 && Random.Range(0.0f, 1.0f) < (0.1 + (pressure - 30) * 0.05) && items.Count > 0)
                {
                    DropItem();

                    GameObject item = items[0];
                    items.RemoveAt(0);
                    item.transform.parent = null;
                    Vector3 ipos = item.transform.position;
                    ipos += new Vector3(-1, 0, 0);
                    item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    item.transform.position = ipos;
                }

                animator.SetTrigger("run");
                if (!forward)
                {
                    speed = 0.1f;
                    pressure = 0;
                    Point(0, 0);
                }
                forward = true;
            }
            else if (Input.GetKeyDown(","))
            {
                animator.SetTrigger("run");
                if (forward)
                {
                    speed = -0.1f;
                    pressure = 0;
                    Point(0, 180);
                }
                forward = false;
            }
        }
    }

	void FixedUpdate() {
		Vector3 pos = transform.position;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun2"))
        {
            pos.x += speed;
            transform.position = pos;
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
        {
            source.Stop();
            speed = Mathf.Sign(speed) * 0.1f;
        }
        
    }

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Item") {
            Item itemhit = coll.collider.gameObject.GetComponent<Item>();
            if (itemhit.grounded)
            {
                AddItem(coll.gameObject);
            }
        }
        else if (coll.gameObject.tag == "GoalHouse")
        {
            mystate = State.WIN;
            animation.SetTrigger("win");
        }
        else if (coll.gameObject.tag == "Enemy")
        {
            mystate = State.DEAD;
            animator.SetTrigger("death");
        }
    }
}
