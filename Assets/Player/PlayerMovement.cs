using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{ 
    public Enums.ColourState currentColourState;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private BoxCollider2D playerCollider;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private float dirX;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 6f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask groundRed;
    [SerializeField] private LayerMask groundBlue;
    [SerializeField] private LayerMask groundGreen;
    [SerializeField] private Sprite blue_sprite;
    [SerializeField] private Sprite green_sprite;
    [SerializeField] private Sprite red_sprite;
    [SerializeField] private Tilemap terrain_red;
    [SerializeField] private Tilemap terrain_blue;
    [SerializeField] private Tilemap terrain_green;
    [SerializeField] private TerrainController terrainController;
    private enum MovementState {idle, running, jumping, falling, attack}
    private MovementState currentMovementState = MovementState.idle;
    private bool isGrounded;
    private Tilemap[] terrainTilemaps;
    private bool isAttacking = false;
    private float lastDirX;


    // Start is called before the first frame update
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        terrainTilemaps = FindObjectsOfType<Tilemap>();
        currentColourState = Enums.ColourState.red;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAttacking)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            myRigidbody.velocity = new Vector2(dirX* moveSpeed, myRigidbody.velocity.y);
            if (dirX != 0)
            {
                lastDirX = dirX;
            }
            if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpPower);
            }
            CheckColourState();
            UpdateAnimationState();
        }

    }
    private bool IsGrounded()
    {
        // Define the layers to check based on the current shoe color
        LayerMask groundLayers;

        switch (currentColourState)
        {
            case Enums.ColourState.red:
                // Check collisions with terrain_neutral and terrain_red
                groundLayers = groundLayer | LayerMask.GetMask("terrain_red");
                break;
            case Enums.ColourState.blue:
                // Check collisions with terrain_neutral and terrain_blue
                groundLayers = groundLayer | LayerMask.GetMask("terrain_blue");
                break;
            case Enums.ColourState.green:
                // Check collisions with terrain_neutral and terrain_green
                groundLayers = groundLayer | LayerMask.GetMask("terrain_green");
                break;
            default:
                // Check collisions with terrain_neutral only
                groundLayers = groundLayer;
                break;
        }

        // Define the position to check for collisions (player's feet)
        Vector2 checkPosition = groundCheck.position;

        // Perform a circle cast downwards to check for collisions at the player's feet
        float castRadius = 0.35f; // Adjust the radius based on your player's size
        RaycastHit2D hit = Physics2D.CircleCast(checkPosition, castRadius, Vector2.down, 0.1f, groundLayers);

        // Check if the circle cast hit something
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    private void CheckColourState()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            currentColourState = Enums.ColourState.red;
            terrainController.SetColliderActive(TerrainController.ColourState.red, true);
        }
        else if (Input.GetKeyDown(KeyCode.Period))
        {
            currentColourState = Enums.ColourState.blue;
            terrainController.SetColliderActive(TerrainController.ColourState.blue, true);
        }
        else if (Input.GetKeyDown(KeyCode.Slash))
        {
            currentColourState = Enums.ColourState.green;
            terrainController.SetColliderActive(TerrainController.ColourState.green, true);
        }
        anim.SetInteger("colour_state", (int)currentColourState);
    }

    private void UpdateAnimationState()
    {
        MovementState state = currentMovementState;
        anim.ResetTrigger("return");
        isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("attack");
            // Set a delay before transitioning back to idle
            StartCoroutine(TransitionToIdleAfterDelay());
        }
        if (isGrounded)
        {
            if (dirX > 0f)
            {
                state = MovementState.running;
                spriteRenderer.flipX = false;
            }
            else if (dirX < 0f)
            {
                state = MovementState.running;
                spriteRenderer.flipX = true;
            }
            else
            {
                state = MovementState.idle;
            }
        }
        else
        {
            if (myRigidbody.velocity.y > .1f)
            {
                state = MovementState.jumping;
                spriteRenderer.flipX = lastDirX < 0f;
            }
            else if (myRigidbody.velocity.y < -.1f)
            {
                state = MovementState.falling;
                spriteRenderer.flipX = lastDirX < 0f;
            }
        }
        anim.SetInteger("state", (int)state);

        string animationName = currentColourState.ToString() + "_" + state.ToString();
        anim.Play(animationName);
    }

    private IEnumerator TransitionToIdleAfterDelay()
    {
        isAttacking = true;
        // Wait for the attack animation to finish playing (adjust the delay time as needed)
        yield return new WaitForSeconds(0.39f); // Adjust the delay time as needed
        anim.ResetTrigger("attack");
        anim.SetTrigger("return");
        isAttacking = false;
    }
}
