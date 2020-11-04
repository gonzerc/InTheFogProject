using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    private int playerHealth;
    private int playerStamina;
    private int playerCurrency;

    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    public float walkRadius;
    public float sprintRadius;
    public float crouchRadius;
    public float shootRadius;

    public float regCameraSpeed;
    public float adsCameraSpeed;
    public float upperPitchLimit;
    public float lowerPitchLimit;
    public GameObject gun;
    public Transform bulletSpawnPos;
    public GameObject bullet;
    public float fireRate;
    public int bulletsInMag;
    public int ammoRemaining;
    public int magazineSize;

    private float moveSpeed;
    private float camSpeed;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Rigidbody rb;
    private Camera playerCam;
    private float nextFire;

    private bool reloading;
    private bool crouching;
    private bool sprinting;

    private int moneyEarned;
    private int moneySpent;

    private UIController ui;
    private MusicController musicController;
    private DetectionScript detectionScript;

    private void Awake()
    {
        playerHealth = 100;
        playerStamina = 100;
        playerCurrency = moneySpent = moneyEarned = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        detectionScript = GetComponentInChildren<DetectionScript>();
        rb = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<Camera>();
        ui = FindObjectOfType<UIController>();
        musicController = FindObjectOfType<MusicController>();
        nextFire = 0.0f;
        magazineSize = 10;
        bulletsInMag = 10;
        ammoRemaining = 40;

        moveSpeed = walkSpeed;
        camSpeed = regCameraSpeed;
        reloading = false;
        crouching = false;
        sprinting = false;
    }


    private void FixedUpdate()
    {

        if (!GameController.terminalOpen)
        {
            //movement on WASD keys
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 movement = transform.forward * v * moveSpeed * Time.deltaTime;
            Vector3 sidestep = transform.right * h * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + movement + sidestep);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(playerHealth <= 0)
        {
            StartCoroutine(FindObjectOfType<GameController>().EndGame(true));
        }
        else if (!GameController.gamePaused && !GameController.terminalOpen)
        {
            //camera move on mouse movement
            yaw += camSpeed * Input.GetAxis("Mouse X");
            pitch -= camSpeed * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, lowerPitchLimit, upperPitchLimit);

            //rotate player left/right
            transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
            //rotate camera up/down
            playerCam.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            gun.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);


            //================================================Player Inputs=================================================\\

            //fire
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                if (bulletsInMag > 0 && !reloading)
                {
                    StartCoroutine(FireShot());
                }
                if (bulletsInMag < 3 && ammoRemaining > 0)
                {
                    //Debug.Log("Out of ammo");
                    ui.DisplayReloadMessage("RELOAD");
                }
                if (bulletsInMag == 0 && ammoRemaining == 0)
                {
                    //Debug.Log("Out of ammo");
                    ui.DisplayReloadMessage("NO AMMO");
                    musicController.PlayEmptyMag();
                }
            }

            //reload gun
            if ( (Input.GetKeyDown(KeyCode.R) || bulletsInMag == 0) && ammoRemaining > 0)
            {
                StartCoroutine(Reload());
            }


            //sprint on left shift down
            if (Input.GetKeyDown(KeyCode.LeftShift) && !crouching)
            {
                if (playerStamina > 0)
                {
                    sprinting = true;
                    moveSpeed = sprintSpeed;
                    detectionScript.ChangeRadius(sprintRadius);
                    StartCoroutine(DecreaseStamina());
                }
            }

            //release sprint
            if ( (Input.GetKeyUp(KeyCode.LeftShift) && !crouching && sprinting) || playerStamina == 0)
            {
                sprinting = false;
                moveSpeed = walkSpeed;
                detectionScript.ChangeRadius(walkRadius);
                StartCoroutine(IncreaseStamina());
            }

            //crouch down
            if (Input.GetKeyDown(KeyCode.LeftControl) && !sprinting)
            {
                //Debug.Log("Crouch");
                moveSpeed = crouchSpeed;
                transform.localScale -= new Vector3(0.0f, 0.25f, 0.0f);
                crouching = true;
                detectionScript.ChangeRadius(crouchRadius);
            }

            //stand up
            if (Input.GetKeyUp(KeyCode.LeftControl) && !sprinting)
            {
                //Debug.Log("Stand");
                moveSpeed = walkSpeed;
                transform.localScale += new Vector3(0.0f, 0.25f, 0.0f);
                crouching = false;
                detectionScript.ChangeRadius(walkRadius);
            }

            ////ADS
            if (Input.GetButtonDown("Fire2"))
            {
                //Debug.Log("ADS on");
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, new Vector3(0.0f, -0.15f, 0.5f), 60);
                playerCam.fieldOfView = Mathf.Lerp(60f, 40f, Time.time);
                camSpeed = adsCameraSpeed;
                ui.ToggleADS();
            }
            if (Input.GetButtonUp("Fire2"))
            {
                //Debug.Log("ADS off");
                ui.ToggleADS();
                camSpeed = regCameraSpeed;
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, new Vector3(0.35f, -0.5f, 1.0f), 30);
                playerCam.fieldOfView = Mathf.Lerp(40f, 60f, Time.time);
            }
        }
    }

    //====================================== Update Player Stats ===============================\\
    
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetPlayerStamina()
    {
        return playerStamina;
    }

    public int GetPlayerCurrency()
    {
        return playerCurrency;
    }
    
    public void DamagePlayer(int damage)
    {
        playerHealth -= damage;
        Debug.Log("Player took " + damage + " damage");
    }

    public void HealPlayer(int amount)
    {
        playerHealth += amount;

        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
    }

    public void AddCurrency(int amount)
    {
        playerCurrency += amount;
        moneyEarned += amount;
    }

    public void RemoveCurrency(int amount)
    {
        playerCurrency -= amount;
        moneySpent += amount;
    }

    public int GetMoneyEarned()
    {
        return moneyEarned;
    }

    public int GetMoneySpent()
    {
        return moneySpent;
    }

    public void RefillAmmo(int amount)
    {
        ammoRemaining += amount;
    }

    private IEnumerator DecreaseStamina()
    {
        while (playerStamina > 0 && sprinting)
        {
            int value = 1;
            playerStamina -= value;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator IncreaseStamina()
    {
        yield return new WaitForSeconds(3.0f);
        while (playerStamina < 100 && !sprinting)
        {
            int value = 1;
            playerStamina += value;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator FireShot()
    {
        nextFire = Time.time + fireRate;
        Instantiate(bullet, bulletSpawnPos.position, bulletSpawnPos.rotation);
        musicController.PlayGunshot();
        bulletsInMag--;

        float oldRadius = detectionScript.GetCurrentRadius();

        detectionScript.ChangeRadius(shootRadius);
        yield return new WaitForSeconds(0.5f);
        detectionScript.ChangeRadius(oldRadius);
    }


    private IEnumerator Reload()
    {
        reloading = true;
        StartCoroutine(ui.ReloadAnimation(1.0f));
        yield return new WaitForSeconds(1.0f);

        musicController.PlayReload();

        int bulletsToReload = magazineSize - bulletsInMag;

        if(bulletsToReload > ammoRemaining)
        {
            bulletsToReload = ammoRemaining;
        }

        ammoRemaining -= bulletsToReload;
        bulletsInMag += bulletsToReload;
        reloading = false;
    }
}
