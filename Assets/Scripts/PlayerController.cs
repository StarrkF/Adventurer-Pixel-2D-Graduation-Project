using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private enum State { idle, run, jump, fall, hurt };
    private State state = State.idle;
    private Collider2D coll;

    public FixedJoystick joystick;


    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int apples = 0;
    [SerializeField] private Text ElmaSayisi;


    public GameObject[] hearts;
    private int life;
    private bool dead;

    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        //DontDestroyOnLoad(gameObject);
        life = hearts.Length;
    }

     private void movement()
    {
        float xMov = joystick.Horizontal;

        if (xMov > 0) {
            rb.transform.Translate(xMov * speed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector2(1, 1);

            if (state == State.idle) state = State.run;
        } else if (xMov < 0) {
            rb.transform.Translate(xMov * speed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector2(-1, 1);

            if (state == State.idle) {} state = State.run;
        } else if (xMov == 0 && state != State.jump && state != State.fall) {
            state = State.idle;
        }

    }

    private void movementKB() {
        float xMovKB = Input.GetAxis("Horizontal");

        if (xMovKB > 0) {
            rb.transform.Translate(xMovKB * speed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector2(1, 1);

            if (state == State.idle) state = State.run;
        } else if (xMovKB < 0) {
            rb.transform.Translate(xMovKB * speed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector2(-1, 1);

            if (state == State.idle) state = State.run;
        } else if (xMovKB == 0 && state != State.jump && state != State.fall) state = State.idle;
        
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground) && state != State.jump && state != State.fall) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;
        }
    }


    void Update()
    {
        anim.SetInteger("State", (int)state);
        movement();
        // movementKB();
        VelocityState();
    }

    //received points
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            apples += 1;
            ElmaSayisi.text = apples.ToString();
        }
    }

    // killed enemy action
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {

            if (state == State.fall) {
                Destroy(other.gameObject);
                // enemy touching trigger
                rb.velocity = new Vector2(0, 4);
            } else {
                takeDamage();
                state = State.hurt;

                if (other.gameObject.transform.position.x > transform.position.x) {
                    rb.velocity = new Vector2(-5, 7);
                } else {
                    rb.velocity = new Vector2(5, 7);
                }

            }

        }

        if (other.gameObject.tag == "MovingPlatform") {
            this.transform.parent = other.transform;
        }

        //spike toucking trigger
        if (other.gameObject.tag == "Spike")
        {
            takeDamage();
            if (other.gameObject.transform.position.x > transform.position.x) {
                rb.velocity = new Vector2(-3, 5);
                state = State.hurt;
            } else {
                rb.velocity = new Vector2(3, 5);
                state = State.hurt;
            }
        }


        if (other.gameObject.tag == "KillZone"){
            Scene sc = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sc.name);
        }

        if (other.gameObject.tag == "FinishFlag") {
            SceneManager.LoadScene(3);
        }

        if (other.gameObject.tag == "FinishFlag_2")
        {
            SceneManager.LoadScene(4);
        }

        if (other.gameObject.tag == "Finish_Flag_3")
        {
            SceneManager.LoadScene(0);
        }

    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null;
        }
    }

    void VelocityState()
    {
        if (state == State.jump)
        {

            if (rb.velocity.y < 2f) {
                state = State.fall;
            }

        } else if (state == State.fall) {

            if (coll.IsTouchingLayers(Ground)) {
                state = State.idle;
            }
        }
    }


    public void jumpButton() {

        if (coll.IsTouchingLayers(Ground) && state != State.jump && state != State.fall)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;
        }
    }

    public void takeDamage()
    {

        if (life > 0) {
            life -= 1;
            Destroy(hearts[life].gameObject);
        }

        if (life == 0) dead = true;

        if (dead) {
            Scene sc = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sc.name);
        }
    }
}
