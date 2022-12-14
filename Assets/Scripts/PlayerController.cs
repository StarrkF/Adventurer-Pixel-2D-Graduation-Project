using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private enum State { idle, run, jump, fall, hurt};
    private State state = State.idle;
    private Collider2D coll;
    

    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int elmalar = 0;
    [SerializeField] private Text ElmaSayisi;


    public GameObject[] hearts;
    private int life;
    private bool dead;
    



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

   
        //DontDestroyOnLoad(gameObject);
        life = hearts.Length;
    }

    
    void Update()
    {
        anim.SetInteger("State", (int)state);
        //char movements
        
            movement();
        
        
        VelocityState();
        

    }

    //received points

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag== "Collectible")
        {
            Destroy(collision.gameObject);
            elmalar += 1;
            ElmaSayisi.text = elmalar.ToString();
        }
    }

    // killed enemy action
    private void OnCollisionEnter2D(Collision2D other)
    {

      

        if (other.gameObject.tag == "Enemy")
        {

            if (state == State.fall)
            {
                Destroy(other.gameObject);
                // enemy touching trigger
                rb.velocity = new Vector2(0, 4);
            }
            else
            {
                takeDamage();
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-5, 7);
                   
                }
                else
                {
                    rb.velocity = new Vector2(5, 7);
                }

                
            }

        }

        if (other.gameObject.tag=="MovingPlatform")
        {
            this.transform.parent = other.transform;

        }

        //spike toucking trigger
        if ((other.gameObject.tag == "Spike"))
        {
            takeDamage();
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-3, 5);
                state = State.hurt;
            }
            else
            {
                rb.velocity = new Vector2(3, 5);
                state = State.hurt;
            }
        }

        // killzone settings
        if(other.gameObject.tag=="KillZone")
        {
            Scene sc = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sc.name);

        }

        if (other.gameObject.tag == "FinishFlag")
        {
            Scene sc = SceneManager.GetActiveScene();
            SceneManager.LoadScene(2);

        }

        if (other.gameObject.tag == "FinishFlag_2")
        {
            Scene sc = SceneManager.GetActiveScene();
            SceneManager.LoadScene(3);

        }

        if (other.gameObject.tag == "Finish_Flag_3")
        {
            Scene sc = SceneManager.GetActiveScene();
            SceneManager.LoadScene(0);

        }



    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null;
        }
    }


    void VelocityState()
    {
        if (state==State.jump)
        {

            if (rb.velocity.y<2f)
            {   
                    state = State.fall;
               
                
            }
 
        }

        else if (state == State.fall)
        {


            if (coll.IsTouchingLayers(Ground))
            {
                state = State.idle;
            }
        }


    }

    private void movement()
    {
        float yanal_hareket = Input.GetAxis("Horizontal");




        if (yanal_hareket > 0)
        {
            //rb.velocity = new Vector2(speed, 0);
            rb.transform.Translate(yanal_hareket * speed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector2(1, 1);

            if (state == State.idle)
            {
                state = State.run;
            }



        }


        else if (yanal_hareket < 0)
        {
            //rb.velocity = new Vector2(-speed.no, 0);
            rb.transform.Translate(yanal_hareket * speed * Time.deltaTime, 0, 0);
            transform.localScale = new Vector2(-1, 1);

            if (state == State.idle)
            {
                state = State.run;
            }


        }

        else if (yanal_hareket == 0 && state != State.jump && state != State.fall)
        {
            state = State.idle;

        }


        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jump;

        }
    }

    public void takeDamage()
    {
        
        if (life > 0)
        {
            life -= 1;
            Destroy(hearts[life].gameObject);
        }

        if(life==0)
        {
            dead = true;
        }

        if(dead)
        {
            Scene sc = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sc.name);
        }
    }
}
