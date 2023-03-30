using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private State state;
    private enum State
    {
        Normal, 
        DodgeRollSliding,
    }
    private float slideSpeed;
    private Vector3 slideDir;
    private Animator animator;

    private float moveSpeed = 3f;

    private Vector3 movement;
    private float horizontal;
    private float vertical;

    private int powerupTime;

    private bool hasTripleShot = false;
    private bool hasHeatSeeking = false;
    private bool canShoot = true;

    [SerializeField] private AudioClip healSound, shootSound, rollSound, powerupSound;

    public bool HasHeatSeeking
    {
        get
        {
            return hasHeatSeeking;
        }

        private set
        {
            hasHeatSeeking = value;
        }
    }

    public CameraController cameraController;
    public MainManager mainManager;

    public int life = 5;
    public int numOfHearts = 5;

    public GameObject bulletPrefab;
    public GameObject[] powerupEffectPrefab;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Slider rollSlider;

    private void Awake()
    {
        state = State.Normal;
    }

    private void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        switch (state)
        {
            case State.Normal:
                DodgeRoll();
                Move();
                GetInput();
                Animate();
                Shoot();
                break;
            case State.DodgeRollSliding:
                DodgeRollSliding();
                break;
        }
    }

    // Gets the vertical and horizontal input to see if the player is trying to move the character.
    void GetInput()
    {
        Vector3 v;
        Vector3 h;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        v = vertical * Camera.main.transform.up;
        h = horizontal * Camera.main.transform.right;

        v.y = 0;
        h.y = 0;

        movement = (h + v);
    }

    // Sets the correct animation state.
    void Animate()
    {   
        Vector3 localMove = transform.InverseTransformDirection(movement);

        animator.SetFloat("forward", localMove.z);
        animator.SetFloat("sideways", localMove.x);
    }

    // Moves the player at a fixed rate if it doesn't collide with obstacles.
    private void Move()
    {
        if (!mainManager.m_GameOver)
        {
            // Gets the direction to move in.
            Vector3 direction = new Vector3(horizontal * moveSpeed, 0, vertical * moveSpeed);

            // Transforms the local coordinates of the direction to world coordinates so that the character always moves in the same direction, regardless of it's rotation.
            Vector3 worldCord = transform.InverseTransformDirection(direction);

            float distance = 1;

            Vector3 raycastDirection = direction;
            raycastDirection.y += 0.2f;

            int layerMask = 1 << 7;

            // Checks for collisions.
            if (!Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, distance, layerMask))
            {
                transform.Translate(worldCord * Time.deltaTime);
            }
        }
    }

    // Finds the direction to dodge roll in and sets the speed and state.
    private void DodgeRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = State.DodgeRollSliding;

            animator.SetBool("isRolling", true);

            SoundManager.instance.PlaySound(rollSound, 1);

            slideDir = movement;

            slideSpeed = 7f;
        }
    }

    // Rolls the character in the direction of travel.
    private void DodgeRollSliding()
    {
        rollSlider.gameObject.SetActive(true);

        transform.position += slideDir * slideSpeed * Time.deltaTime;

        rollSlider.value += slideSpeed * 1f * Time.deltaTime;

        slideSpeed -= slideSpeed * 1f * Time.deltaTime;

        if (slideSpeed < 3f)
        {
            state = State.Normal;
            animator.SetBool("isRolling", false);
            rollSlider.value = 0;
            rollSlider.gameObject.SetActive(false);
        }
    }

    private void UpdateHealth()
    {
        // Sets life to max number of hearts if it exceeds the value.
        if (life > numOfHearts)
        {
            life = numOfHearts;
        }
        
        for (int i = 0; i < hearts.Length; i++)
        {
            // Sets sprite of the heart to show if it's full or empty.
            if (i < life)
            {
                hearts[i].sprite = fullHeart;
            }

            else
            {
                hearts[i].sprite = emptyHeart;
            }

            // Enables or disables the heart depending on if it's supposed to be shown or not.
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }

            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    // Makes the player shoot towards the location of the mouse on the screen.
    private void Shoot()
    {
        if (!mainManager.m_GameOver)
        {
            // Fires ray from the mouse.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Plane to intersect with the ray.
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            // Gets the distance of the ray if it has intersected with a collider.
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                // Gets the target point at the distance along the ray.
                Vector3 target = ray.GetPoint(distance);

                // The direction to rotate towards.
                Vector3 direction = target - transform.position;

                // Gets desired rotation.
                float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                // Rotates transform towards mouse.
                transform.rotation = Quaternion.Euler(0, rotation, 0);
            }

            // Instantiates a bullet if left mouse button is clicked.
            if (Input.GetMouseButton(0) && !hasTripleShot && canShoot)
            {
                StartCoroutine(ShootingCD());

                Vector3 bulletRotation = transform.eulerAngles;
                Vector3 bulletPosition = transform.position;

                bulletPosition.y += 0.3f;

                bulletRotation.x = 90;

                StartCoroutine(cameraController.Shake(0.15f, 0.02f));

                Instantiate(bulletPrefab, bulletPosition, Quaternion.Euler(bulletRotation));
                SoundManager.instance.PlaySound(shootSound, 0.05f);
            }

            // Instantiates three bullets if the player has TripleShot.
            else if (Input.GetMouseButton(0) && hasTripleShot && canShoot)
            {
                StartCoroutine(cameraController.Shake(0.15f, 0.03f));
                StartCoroutine(ShootingCD());
                SoundManager.instance.PlaySound(shootSound, 0.05f);

                // The bullets are instantiated at different rotations relative to the player.
                for (int i = 0; i < 3; i++)
                {
                    int rotationOffset = 15;

                    Vector3 playerRotation = transform.eulerAngles;
                    playerRotation.x = 90;

                    Vector3 bulletPosition = transform.position;

                    bulletPosition.y += 0.3f;

                    switch (i)
                    {
                        case 0: 
                            playerRotation.y -= rotationOffset;
                            Instantiate(bulletPrefab, bulletPosition, Quaternion.Euler(playerRotation));
                            break;
                        case 1:
                            Instantiate(bulletPrefab, bulletPosition, Quaternion.Euler(playerRotation));
                            break;
                        case 2:
                            playerRotation.y += rotationOffset;
                            Instantiate(bulletPrefab, bulletPosition, Quaternion.Euler(playerRotation));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void OnHealthPickupEnter()
    {
        life++;
        SoundManager.instance.PlaySound(healSound, 1);
    }

    public void OnHealthIncreaseEnter()
    {
        numOfHearts++;
        life++;
        SoundManager.instance.PlaySound(healSound, 1);
    }

    public void OnSlowMotionEnter()
    {
        powerupTime = 5;
        SoundManager.instance.PlaySound(powerupSound, 0.1f);
        StartCoroutine(SlowmoRoutine());
    }

    public void OnSpeedBoostEnter()
    {
        powerupTime = 5;
        SoundManager.instance.PlaySound(powerupSound, 0.1f);
        StartCoroutine(SpeedBoostRoutine());
    }

    public void OnTripleShotEnter()
    {
        powerupTime = 5;
        SoundManager.instance.PlaySound(powerupSound, 0.1f);
        StartCoroutine(TripleShotRoutine());
    }

    public void OnHeatSeekingEnter()
    {
        powerupTime = 5;
        SoundManager.instance.PlaySound(powerupSound, 0.1f);
        StartCoroutine(HeatSeekingRoutine());
    }

    IEnumerator SlowmoRoutine()
    {
        powerupEffectPrefab[2].SetActive(true);

        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(powerupTime);

        powerupEffectPrefab[2].SetActive(false);

        Time.timeScale = 1;
    }

    IEnumerator HeatSeekingRoutine()
    {
        hasHeatSeeking = true;

        powerupEffectPrefab[3].SetActive(true);

        yield return new WaitForSeconds(powerupTime);
        hasHeatSeeking = false;

        powerupEffectPrefab[3].SetActive(false);
    }

    IEnumerator TripleShotRoutine()
    {
        powerupEffectPrefab[1].SetActive(true);

        hasTripleShot = true;
        yield return new WaitForSeconds(powerupTime);

        powerupEffectPrefab[1].SetActive(false);

        hasTripleShot = false;
    }

    IEnumerator SpeedBoostRoutine()
    {
        powerupEffectPrefab[0].SetActive(true);

        moveSpeed = 6f;
        yield return new WaitForSeconds(powerupTime);

        powerupEffectPrefab[0].SetActive(false);

        moveSpeed = 3f;
    }

    IEnumerator ShootingCD()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }
}
