using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    [SerializeField] CapsuleCollider2D  playerCollider; 
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Ground Check Settings")]
    [Tooltip("Assign the separate empty GameObject with the Circle Collider here")]
    [SerializeField] CircleCollider2D groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    [Header("Materials")]
    [SerializeField] PhysicsMaterial2D movingMaterial; 
    [SerializeField] PhysicsMaterial2D stoppingMaterial; 

    [Header("Audio")]
    public AudioSource audioSource;        // walk sound
    public AudioSource sfxAudioSource;     // land/jump sounds (separate!)
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip walkSound;
    
    public float minWalkPitch = 1.0f;
    public float maxWalkPitch = 2.5f;
    public float animationSpeedMultiplier = 1.5f;

    [Header("Movement")]
    private float landingGraceTimer;
    private float airTime;
    private float coyoteTime;
    private float jumpCooldownTimer;
    bool wasGrounded;
    bool successfullyJumped;
    public float acceleration = 10f;
    public float speed = 5f;
    public float jumpForce = 10f;
    public float coyoteTimeDuration = 0.2f;
    [Header("Animation")]
    public Animator animator;

    [Header("Effects")]
    public GameObject walkParticle;
    public GameObject landingParticle;
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerCollider == null) playerCollider = GetComponent<CapsuleCollider2D>();
        if (groundCheckCollider == null)
            Debug.LogError("Please assign the Ground Check Collider in the inspector!", this);

        animator = GetComponent<Animator>();
        playerCollider.sharedMaterial = movingMaterial;
    }

    void Update()
    {
        bool isGrounded = IsGrounded();
        
        if (!wasGrounded && isGrounded)
        {
            Landed();
            sfxAudioSource.PlayOneShot(landSound); // separate source, can't be stopped
        }

        wasGrounded = isGrounded;

        Animator();
        Jump();
        Audio();
    }

    void FixedUpdate()
    {
        Movement();
        Gravity();
    }

    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (x != 0)
        {
            Vector2 targetVelocity = new Vector2(x * speed, rb.linearVelocity.y);
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else if (IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.8f, rb.linearVelocity.y);
        }

        walkParticle.SetActive(x != 0 && IsGrounded());

        if (x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
    bool IsGrounded()
    {
        if (groundCheckCollider == null) return false;

        return Physics2D.OverlapCircle(groundCheckCollider.bounds.center, groundCheckCollider.radius, groundLayer);
    }
    void Landed()
    {
        landingParticle.SetActive(true);
        ParticleSystem ps = landingParticle.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Invoke(nameof(DeactivateLandingParticle), ps.main.duration);
        }
    }

    void DeactivateLandingParticle()
    {
        landingParticle.SetActive(false);
    }
    void Jump()
    {
        if (jumpCooldownTimer > 0)
            jumpCooldownTimer -= Time.deltaTime;

        if (IsGrounded() && !successfullyJumped)
            coyoteTime = coyoteTimeDuration;
        else
            coyoteTime -= Time.deltaTime;

        if (IsGrounded() && jumpCooldownTimer <= 0)
            successfullyJumped = false;

        if (Input.GetButtonDown("Jump") && coyoteTime > 0)
        {
            coyoteTime = 0f;
            airTime = 0f;
            successfullyJumped = true;
            jumpCooldownTimer = 0.2f;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            airTime += Time.deltaTime;
            rb.gravityScale = 1f + airTime;
        }
        else
        {
            airTime = 0f;
            rb.gravityScale = 1f;
        }
    }
    void Animator()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f && IsGrounded();
        animator.SetBool("isRunning", isRunning);
    }
    public void PlayFootstep()
    {
        audioSource.PlayOneShot(walkSound);
    }
    void Audio()
    {
        bool isGrounded = IsGrounded();
        bool isWalking = isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        if (isWalking)
        {
            if (!audioSource.isPlaying || audioSource.clip != walkSound)
            {
                audioSource.clip = walkSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == walkSound)
            {
                audioSource.Stop();
            }
        }
    }
}
