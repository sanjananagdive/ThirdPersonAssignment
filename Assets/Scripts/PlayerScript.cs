using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;//player speed variable
    public float turnSpeed = 10f;//player turning speed variable
    public float jumpForce = 5f;//force applied during jump

    private Quaternion targetRotation;//rotation reference
    private Animator anim;//animator reference
    private Rigidbody rb;//rigidbody reference

    private bool isGrounded;//bool to check if player is over the ground

    public int maxHealth = 100; // Maximum health value
    private int currentHealth; // Current health value

    public int laserDamage = 20; //damage variable for laser
    public bool gameover=false;//initially setting gameover bool false

    public static PlayerScript instance;//instance of this script

    void Awake()
    {
        //singleton pattern for this script
        if(instance==null)
        {
            instance=this;
        }
    }

    void Start()
    {
        //storing initial rotation in targetRotation variable
        targetRotation = transform.rotation;
        //initialization
        rb  = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // Initialize health
        currentHealth = maxHealth;
    }

    void Update()
    {
        //calling move and jump function only if game is not over
        if(!gameover)
        {
            Move();

            Jump();
        }
    }
    //function to move the player
    public void Move()
    {
        //key "W" : to provide forward speed
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //run anim parameter setting to true
            anim.SetBool("run",true);
        }
        else if (Input.GetKey(KeyCode.S)) // 
        {
            transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime); //
            anim.SetBool("run", true); // 
        }
        //setting run anim parameter false to depict idle state
        else
        {
            anim.SetBool("run",false);
        }

        // key "A" : to provide left rotation
        if (Input.GetKeyDown(KeyCode.A))
        {
            targetRotation *= Quaternion.Euler(0, -90f, 0);
        }

        // key "D" : to provide right rotation
        if (Input.GetKeyDown(KeyCode.D))
        {
            targetRotation *= Quaternion.Euler(0, 90f, 0);
        }

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    
    }
    //function to jump
    public void Jump()
    {
        //key "spacebar" : to provide jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetBool("jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Player is now in the air
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the player collides with something, check if it’s the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("jump",false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //checking if player passes through gem
        if(col.gameObject.CompareTag("gem"))
        {
            UIManager.instance.UpdateScore();//updating score for which calling the updatescore funtion from ui script
            print("Colleted gem");
            Destroy(col.gameObject);//destroying the gem
        }
        //checking if player passes through laser beam
        if(col.gameObject.CompareTag("Laser"))
        {
            print("Collided with laser");
            //calling damage function
            TakeDamage(laserDamage);
        }
    }
    //function to take damage
    private void TakeDamage(int damage)
    {
        // Reduce current health by damage amount
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        print(currentHealth);

        UIManager.instance.UpdateHealthBar(currentHealth);

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }

        // Optional: Update UI or other systems with current health
    }

    private void Die()
    {
        // Handle death (e.g., disable player, play animation, etc.)
        print("Player died");

        anim.SetTrigger("die");
        gameover= true;//setting game over true 

        
    }
}
