using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaFrogAI : MonoBehaviour
{

    [SerializeField] float rightCap;
    [SerializeField] float leftCap;

    [SerializeField] float jumpLenght=2;
    [SerializeField] float jumpHeight=2;
    [SerializeField] LayerMask Ground;

    private bool facingLeft = true;
    private Collider2D coll;
    private Rigidbody2D rb;

    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if(facingLeft)
        {
            if (transform.position.x > leftCap)
            {

                if (transform.localScale.x!=1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                


                if(coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(-jumpLenght, jumpHeight);
                }
            }
            else
            {
                facingLeft = false;

            }
        }

        else
        {
            if (transform.position.x < rightCap)
            {

                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }



                if (coll.IsTouchingLayers(Ground))
                {
                    rb.velocity = new Vector2(jumpLenght, jumpHeight);
                }
            }
            else
            {
                facingLeft = true;

            }
        }
    }
}
