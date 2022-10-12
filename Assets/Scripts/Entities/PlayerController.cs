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

    private Vector3 v;
    private Vector3 h;
    private Vector3 movement;
    private float horizontal;
    private float vertical;

    private int powerupTime;

    private bool hasTripleShot = false;
    private bool hasHeatSeeking = false;

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
                Move();
                GetInput();
                Animate();
                Shoot();
                DodgeRoll();
                break;
            case State.DodgeRollSliding:
                DodgeRollSliding();
                break;
        }
    }

    //Gets the vertical and horizontal input to see if the player is trying to move the character
    void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        v = vertical * Camera.main.transform.up;
        h = horizontal * Camera.main.transform.right;

        v.y = 0;
        h.y = 0;

        movement = (h + v);
    }

    //Sets the correct animation state
    void Animate()
    {   
        Vector3 localMove = transform.InverseTransformDirection(movement);

        animator.SetFloat("forward", localMove.z);
        animator.SetFloat("sideways", localMove.x);
    }

    private void Move()
    {
        if (!mainManager.m_GameOver)
        {
            //Gets the direction to move in
            Vector3 direction = new Vector3(horizontal * moveSpeed, 0, vertical * moveSpeed);

            //Transforms the local coordinates of the direction to world coordinates so that the character always moves in the same direction, regardless of it's rotation
            Vector3 worldCord = transform.InverseTransformDirection(direction);

            //Moves player character at a fixed rate
            transform.Translate(worldCord * Time.deltaTime);
        }
    }

    //Finds the direction to dodge roll in and sets the speed and state
    private void DodgeRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = State.DodgeRollSliding;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Plane plane = new Plane(Vector3.up, Vector3.zero);

            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 target = ray.GetPoint(distance);

                slideDir = (target - transform.position).normalized;

                slideSpeed = 8f;
            }
        }
    }

    //Rolls the character toward the mouse
    private void DodgeRollSliding()
    {
        transform.position += slideDir * slideSpeed * Time.deltaTime;

        slideSpeed -= slideSpeed * 0.75f * Time.deltaTime;

        animator.SetFloat("rollSpeed", slideSpeed);

        if (slideSpeed < 4f)
        {
            state = State.Normal;
        }
    }

    private void UpdateHealth()
    {
        //Sets life to max number of hearts if it exceeds the value
        if (life > numOfHearts)
        {
            life = numOfHearts;
        }
        
        for (int i = 0; i < hearts.Length; i++)
        {
            //Sets sprite of the heart to show if its full or empty
            if (i < life)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            //Enables or disables the heart depending on if it's supposed to be shown or not
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

    private void Shoot()
    {
        int rotationOffset = 15;

        if (!mainManager.m_GameOver)
        {
            //Fires ray from the mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Plane to intersect with the ray
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            //Gets the distance of the ray if it has has intersected with a collider
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                //Gets the target point at the distance along the ray
                Vector3 target = ray.GetPoint(distance);

                //The direction to rotate towards
                Vector3 direction = target - transform.position;

                //Gets desired rotation
                float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                //Rotates transform towards mouse
                transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            //Instantiates a bullet if left mouse button is clicked
            if (Input.GetMouseButtonDown(0) && !hasTripleShot)
            {
                Vector3 bulletRotation = transform.eulerAngles;
                Vector3 bulletPosition = transform.position;

                bulletPosition.y += 0.3f;

                bulletRotation.x = 90;

                StartCoroutine(cameraController.Shake(0.15f, 0.02f));

                Instantiate(bulletPrefab, bulletPosition, Quaternion.Euler(bulletRotation));
            }
            else if (Input.GetMouseButtonDown(0) && hasTripleShot)
            {
                StartCoroutine(cameraController.Shake(0.15f, 0.04f));

                //Instantiates three different bullets, each with different rotation
                for (int i = 0; i < 3; i++)
                {
                    Vector3 playerRotation = new Vector3 (transform.localEulerAngles.x + 90, transform.localEulerAngles.y, transform.localEulerAngles.z);
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
    }
    public void OnSlowMotionEnter()
    {
        powerupTime = 5;
        StartCoroutine(SlowmoRoutine());
    }

    public void OnSpeedBoostEnter()
    {
        powerupTime = 5;
        StartCoroutine(SpeedBoostRoutine());
    }

    public void OnTripleShotEnter()
    {
        powerupTime = 5;
        StartCoroutine(TripleShotRoutine());
    }

    public void OnHeatSeekingEnter()
    {
        powerupTime = 5;
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
}
