using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 3f;

    private float verticalInput;
    private float horizontalInput;

    private int powerupTime;

    private bool hasTripleShot = false;
    public bool hasHeatSeeking = false;

    private PowerUp powerupScript;

    public MainManager mainManager;

    public int life = 5;
    public int numOfHearts = 5;

    public GameObject bulletPrefab;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    

    private void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        UpdateHealth();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!mainManager.m_GameOver)
        {
            //Gets the vertical and horizontal input to see if the player is trying to move the character
            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");

            //Gets the direction to move in
            Vector3 direction = new Vector3(horizontalInput * moveSpeed, 0, verticalInput * moveSpeed);

            //Transforms the local coordinates of the direction to world coordinates so that the character always moves in the same direction, regardless of it's rotation
            Vector3 worldCord = transform.InverseTransformDirection(direction);

            //Moves player character at a fixed rate
            transform.Translate(worldCord * Time.fixedDeltaTime);
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
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
            else if (Input.GetMouseButtonDown(0) && hasTripleShot)
            {
                //Instantiates three different bullets, each with different rotation
                for (int i = 0; i < 3; i++)
                {
                    Vector3 playerRotation = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);

                    switch (i)
                    {
                        case 0: 
                            playerRotation.y -= rotationOffset;
                            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(playerRotation));
                            break;
                        case 1:
                            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(playerRotation));
                            break;
                        case 2:
                            playerRotation.y += rotationOffset;
                            Instantiate(bulletPrefab, transform.position, Quaternion.Euler(playerRotation));
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
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(powerupTime);
        Time.timeScale = 1;
    }

    IEnumerator HeatSeekingRoutine()
    {
        hasHeatSeeking = true;
        yield return new WaitForSeconds(powerupTime);
        hasHeatSeeking = false;
    }

    IEnumerator TripleShotRoutine()
    {
        hasTripleShot = true;
        yield return new WaitForSeconds(powerupTime);
        hasTripleShot = false;
    }

    IEnumerator SpeedBoostRoutine()
    {
        moveSpeed = 6f;
        yield return new WaitForSeconds(powerupTime);
        moveSpeed = 3f;
    }
}
