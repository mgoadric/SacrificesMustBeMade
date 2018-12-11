using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour 
{

	private Animator animator;
    private AudioSource source;

    public float speed;
    public float boost;

	private bool forward;
    public int pressure;
    public List<GameObject> items;
    public AudioClip breathing;
    public AudioClip scream;
    public State mystate;

    // Use this for initialization
    void Start () {
		animator = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        mystate = State.WAIT;
        items = new List<GameObject>();
        forward = true;
        speed = 0;
        boost = 1.01f;
	}

    void Point(float zangle, float yangle)
    {
        Vector3 eulerAngles = gameObject.transform.localEulerAngles;
        eulerAngles.z = zangle;
        eulerAngles.y = yangle;
        transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    // Sometimes we just need to be somewhere else
    public void ForceMove(float deltax, float deltay)
    {
        Vector3 pos = transform.position;
        pos.x += deltax;
        pos.y += deltay;
        transform.position = pos;
    }

    public void AddItem(GameObject item)
    {
        items.Add(item);
        item.GetComponent<Item>().grounded = false;
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        item.transform.parent = transform;
        item.transform.position = transform.position;
        // TODO Put it in the right coordinates
    }

    public void DropItem()
    {
        GameObject item = items[0];
        items.RemoveAt(0);
        item.transform.parent = transform.parent;
        Vector3 ipos = item.transform.position;
        ipos += new Vector3(-1, 0, 0);
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        item.transform.position = ipos;
    }

    public void Die()
    {
        animator.SetTrigger("death");
    }

    // Update is called once per frame
    void Update () {
        if (mystate == State.RUN)
        {
            if (Input.GetKeyDown(".") || Input.GetKeyDown("right"))
            {
                if (animator.GetBool("run"))
                {
                    pressure++;
                    speed *= boost;
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
                }

                animator.SetTrigger("run");
                if (!forward)
                {
                    speed = 0.1f;
                    //pressure = 0;
                    Point(0, 0);
                }
                forward = true;
            }
            else if (Input.GetKeyDown(",") || Input.GetKeyDown("left"))
            {
                animator.SetTrigger("run");
                if (forward)
                {
                    speed = -0.1f;
                    //pressure = 0;
                    Point(0, 180);
                }
                forward = false;
            }
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun2"))
            {
                source.Stop();

            }
        }
    }

	void FixedUpdate() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun2"))
        {
            ForceMove(speed, 0);

            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
        {
            speed = Mathf.Sign(speed) * 0.1f;
        }
        
    }

	void OnCollisionEnter2D(Collision2D coll) {
        Debug.Log("Hit something...");
        if (coll.gameObject.tag == "Item")
        {
            Debug.Log("Hit an item...");
            Item itemhit = coll.collider.gameObject.GetComponent<Item>();
            if (itemhit.grounded)
            {
                pressure = 0;
                AddItem(coll.gameObject);
            }
        }
        else if (coll.gameObject.tag == "GoalHouse")
        {
            Debug.Log("Hit the goal house!");
            mystate = State.WIN;
            GameManager.S.gamestate = State.WIN;
            ForceMove(0, 21.3f);
        }
        else if (coll.gameObject.tag == "Enemy")
        {
            Debug.Log("Playing scream");
            source.PlayOneShot(scream);
            mystate = State.DEAD;
            GameManager.S.gamestate = State.DEAD;
            Die();
        }
    }
}
