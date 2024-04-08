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
    [SerializeField] private GameObject floatingPoints250;
    [SerializeField] private GameObject floatingPoints100;
    private bool isFacingRight = true;
    private float attackRange = 2.5f;
    [SerializeField] private LayerMask enemyLayer;
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
    [SerializeField] private AudioClip attackSound; 
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip runSound;
    private bool isJumpSoundPlaying = false;
    private AudioSource audioSource;
    private enum MovementState {idle, running, jumping, falling, attack}
    private MovementState currentMovementState = MovementState.idle;
    private bool isGrounded;
    private Tilemap[] terrainTilemaps;
    private bool isAttacking = false;
    private float lastDirX;
    public CoinManager cm;
    private bool isPaused = false;
    // Start is called before the first frame update
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        terrainTilemaps = FindObjectsOfType<Tilemap>();
        currentColourState = Enums.ColourState.red;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = attackSound;
        Health healthComponent = GetComponent<Health>();
        healthComponent.Awake();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isPaused)
        {
            if (!isAttacking)
            {
                dirX = Input.GetAxisRaw("Horizontal");
                myRigidbody.velocity = new Vector2(dirX * moveSpeed, myRigidbody.velocity.y);
                if (dirX != 0)
                {
                    lastDirX = dirX;
                    if (currentMovementState == MovementState.jumping || currentMovementState == MovementState.falling)
                    {
                        // Flip sprite based on direction while jumping or falling
                        spriteRenderer.flipX = dirX < 0f;
                    }
                }
                if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
                {
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpPower);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetTrigger("attack");
                    currentMovementState = MovementState.attack;
                    Attack();
                    // Set a delay before transitioning back to idle
                    StartCoroutine(TransitionToIdleAfterDelay());
                }

                CheckColourState();
                UpdateAnimationState();
            }
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
        if (isGrounded)
        {
            if (dirX != 0)
            {
                state = MovementState.running;
                spriteRenderer.flipX = dirX < 0f;
                isFacingRight = dirX > 0f;
            }
            else
            {
                state = MovementState.idle;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = MovementState.attack;
                }
            }
        }
        else
        {
            if (myRigidbody.velocity.y > 0.1f)
            {
                state = MovementState.jumping;
            }
            else if (myRigidbody.velocity.y < -0.1f)
            {
                state = MovementState.falling;
            }
            spriteRenderer.flipX = lastDirX < 0f;
        }
        

        anim.SetInteger("state", (int)state);

        string animationName = currentColourState.ToString() + "_" + state.ToString();
        anim.Play(animationName);

        PlayAudio(state); // Play appropriate audio based on the state
        
    }

    private void PlayRunSound()
    {
        audioSource.clip = runSound;
        audioSource.loop = true;
        audioSource.Play();

    }

    private void PlayAudio(MovementState state)
    {
        switch (state)
        {
            case MovementState.running:
                if (!audioSource.isPlaying)
                    PlayRunSound();
                break;
            case MovementState.jumping:
                if (!isJumpSoundPlaying)
                {
                    audioSource.PlayOneShot(jumpSound);
                    isJumpSoundPlaying = true;
                }
                break;
            case MovementState.attack:
                audioSource.PlayOneShot(attackSound);
                break;
            default:
                audioSource.Stop(); // Stop any other sound
                break;
        }
        if (state != MovementState.jumping)
        {
            isJumpSoundPlaying = false;
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            ScoreManager.instance.AddPoints(100);
            DestroyFloatingPoints(other.gameObject, 3f);
            cm.coinCount++;
            audioSource.PlayOneShot(coinSound);
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    private void Attack()
    {
        PlayAudio(MovementState.attack);
        // Define the direction of the attack based on the player's facing direction
        Vector2 attackDirection = isFacingRight ? Vector2.right : Vector2.left;

        // Define the size of the ellipse based on the attack range
        Vector2 ellipseSize = new Vector2(attackRange * 2f, 2f);

        // Cast an ellipse in the attack direction
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + (Vector3)attackDirection * attackRange, ellipseSize, 0f, enemyLayer);

        // Damage enemies if they are hit
        foreach (Collider2D collider in colliders)
        {
            // Check if the collided object is an enemy
            if (collider.CompareTag("Enemy"))
            {
                // Destroy the enemy object
                collider.gameObject.SendMessage("TakeDamage");
                ScoreManager.instance.AddPoints(250);
                CameraController cameraController = FindObjectOfType<CameraController>();
                cameraController.AttackShake();
                Vector3 floatingScorePosition = collider.transform.position + Vector3.up * 1.5f;
                
                // Pass the position to destroy floating points at the player's position
                DestroyFloatingPoints(collider.gameObject, 3f);
            }
        }
    }

    public void DestroyFloatingPoints(GameObject pointsInstance, float delay)
    {
        // Determine which prefab to use based on the pointsInstance tag
        GameObject floatingPointsPrefab = pointsInstance.CompareTag("Coin") ? floatingPoints100 : floatingPoints250;

        // Instantiate the floating points prefab at the same position as the pointsInstance
        GameObject floatingPoints = Instantiate(floatingPointsPrefab, pointsInstance.transform.position, Quaternion.identity);

        // Destroy the original pointsInstance
        Destroy(pointsInstance);

        // Start coroutine to destroy floating points after a delay
        StartCoroutine(DestroyPointsAfterDelay(floatingPoints, delay));
    }

    private IEnumerator DestroyPointsAfterDelay(GameObject pointsInstance, float delay)
    {
        // Move the floating points up immediately
        float startTime = Time.time;
        while (Time.time - startTime < delay)
        {
            float newY = pointsInstance.transform.position.y + 1.0f * Time.deltaTime;
            pointsInstance.transform.position = new Vector3(pointsInstance.transform.position.x, newY, pointsInstance.transform.position.z);
            yield return null;
        }

        // Ensure the final position is reached
        pointsInstance.transform.position = new Vector3(pointsInstance.transform.position.x, pointsInstance.transform.position.y + 1.0f * (delay - (Time.time - startTime)), pointsInstance.transform.position.z);

        // Wait for the remaining time before destroying
        yield return new WaitForSeconds(Mathf.Max(0, delay - (Time.time - startTime)));

        // Destroy the floating points
        Destroy(pointsInstance);
    }
}
