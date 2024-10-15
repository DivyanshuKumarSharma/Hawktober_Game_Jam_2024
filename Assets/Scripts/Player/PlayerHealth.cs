using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float playerHP = 100f;
    public float playerStamina = 100f;
    public float maxStamina = 100f;
    public float staminaDrainRate = 10f;
    public float staminaRestoreRate = 5f;
    public float staminaRestoreDelay = 2f;
    [SerializeField] private float waitForDeath = 3f;
    private float restoreTimer;
    public bool isRunning = false;

    public Healthbar healthbar;
    private PlayerController playerMovement;
    [SerializeField] private GameObject deathUI;
    private DeathMenu deathMenu;

    void Awake()
    {
        deathUI.SetActive(false);    
    }
    void Start()
    {
        playerMovement = GetComponent<PlayerController>();
        healthbar.setMaxHealth(playerHP);
        healthbar.setMaxStamina(maxStamina);
        deathMenu = deathUI.GetComponent<DeathMenu>();
    }

    void Update()
    {
        if (isRunning)
        {
            deductStamina(staminaDrainRate * Time.deltaTime);
        }
        else
        {
            restoreTimer += Time.deltaTime;
            if (restoreTimer >= staminaRestoreDelay)
            {
                RestoreStamina();
            }
        }

        if (playerStamina <= 0)
        {
            playerMovement.canRun = false;
        }
        else if (playerStamina >= maxStamina)
        {
            playerMovement.canRun = true;
        }
    }

    public void TakeDamage(float damageTaken)
    {
        playerHP -= damageTaken;
        healthbar.setHealth(playerHP);
        if (playerHP <= 0)
        {
            StartCoroutine(Die());
        }
        Debug.Log("Player took damage. Current HP: " + playerHP);
        // Play damage sound
        // Play damage animation
    }

    IEnumerator Die()
    {
        //play death anim
        Debug.Log("Player died.");
        yield return new WaitForSeconds(waitForDeath);
        deathUI.SetActive(true);
    }

    public void deductStamina(float staminaDamage)
    {
        playerStamina -= staminaDamage;
        if (playerStamina < 0)
        {
            playerStamina = 0;
        }
        restoreTimer = 0;
        healthbar.setStamina(playerStamina);
        //play huffing sound
    }

    public void RestoreStamina()
    {
        playerStamina += staminaRestoreRate * Time.deltaTime;
        if (playerStamina > maxStamina)
        {
            playerStamina = maxStamina;
        }
        healthbar.setStamina(playerStamina);
    }

}
