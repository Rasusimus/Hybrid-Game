using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator anim;

    [SerializeField] private float moveSpeed = 5f;

    private float horizontalInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            Move();
        }
        if (horizontalInput != 0f && rb.bodyType == RigidbodyType2D.Dynamic) 
        { anim.SetBool("moving", true); }
        
        else { anim.SetBool("moving", false); }
    }
    private void Move()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

            if (horizontalInput > 0f) { sprite.flipX = false; }

            else if (horizontalInput < 0f) { sprite.flipX = true; }
        }
    }
}
