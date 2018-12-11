using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{

    private Animator animator;
    private AudioSource source;

    public float speed;
    public float boost;

    // Use this for initialization
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        speed = 0;
        boost = 1.003f; // ENEMY is 1.004f
    }

    // Sometimes we just need to be somewhere else
    public void ForceMove(float deltax, float deltay)
    {
        Vector3 pos = transform.position;
        pos.x += deltax;
        pos.y += deltay;
        transform.position = pos;
    }

    public void SetBoost(float boost)
    {
        this.boost = boost;
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
        animator.SetTrigger("stop");

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
    void Update()
    {

    }

    void Point(float zangle, float yangle)
    {
        Vector3 eulerAngles = gameObject.transform.localEulerAngles;
        eulerAngles.z = zangle;
        eulerAngles.y = yangle;
        transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun2"))
        {
            ForceMove(speed, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if ((GameManager.S.gamestate == State.RUN || GameManager.S.gamestate == State.FRIENDSAFE) && coll.gameObject.tag == "Item" && gameObject.tag == "Enemy")
        {
            Destroy(coll.gameObject);
            GameManager.S.lost++;
        }
        else if (GameManager.S.gamestate == State.NEWS && coll.gameObject.tag == "Character")
        {
            GameManager.S.gamestate = State.WAIT;
            StopRunning();
        }
        else if ((GameManager.S.gamestate != State.NEWS && GameManager.S.gamestate != State.WAIT) && coll.gameObject.tag == "GoalHouse")
        {
            if (gameObject.tag == "Friend")
            {
                GameManager.S.gamestate = State.FRIENDSAFE;
            }
            else if (gameObject.tag == "Enemy")
            {
                coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
