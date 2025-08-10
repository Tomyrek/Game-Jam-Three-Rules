using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RuleManager : MonoBehaviour
{
    public static RuleManager instance;

    [Header("Rule Settings")]
    public bool canMoveLeft = true;
    public bool canKillEnemies = true;
    public bool canStand = true;
    public bool canJumpMoreThanLimit = true;
    public bool canEnterForbiddenZone = true;

    [Header("Rule Limits")]
    public float standingTimeLimit = 0.3f;
    public int maxJumps = 3;

    [Header("Rule Tracking")]
    private bool brokeLeftMovement = false;
    private bool brokeKilling = false;
    private bool brokeStanding = false;
    private bool brokeJumpLimit = false;
    private bool brokeForbiddenZone = false;

    [Header("Standing Detection")]
    private float standingTimer = 0f;
    private Vector3 lastPlayerPosition;
    private bool hasStartedMoving = false;

    [Header("Jump Tracking")]
    private int jumpCount = 0;

    [Header("UI References - Individual Rule Slots")]
    public Text rule1Text; // Prvi slot za pravilo
    public Text rule2Text; // Drugi slot za pravilo
    public Text rule3Text; // Treći slot za pravilo

    [Header("UI References - Violation Slots")]
    public Text violation1Text; // Prvi slot za prekršaj
    public Text violation2Text; // Drugi slot za prekršaj
    public Text violation3Text; // Treći slot za prekršaj

    [Header("Rule Display Settings")]
    public GameObject ruleDisplayPanel;
    public float ruleDisplayTime;
    
    [Header("Game Over Settings")]
    public bool shouldReturnToLevelSelect = false; // Da li da vrati u Level Select ili restart level

    private bool isKillingPlayer = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        lastPlayerPosition = PlayerController.instance.transform.position;
        StartCoroutine(ShowRulesAtStart());
    }

    void Update()
    {
        if (!PauseMenu.instance.isPaused && !PlayerController.instance.stopInput && !isKillingPlayer)
        {
            CheckLeftMovement();
            CheckStanding();
            CheckMovementStarted();
            UpdateUI();
            CheckAllRulesBroken();
        }
    }

    private void CheckLeftMovement()
    {
        if (!canMoveLeft && !brokeLeftMovement)
        {
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                brokeLeftMovement = true;
                ShowRuleViolation(1, "Violation!");
                AudioManager.instance.PlaySFX(9);
            }
        }
    }

    private void CheckMovementStarted()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            hasStartedMoving = true;
        }
    }

    private void CheckStanding()
    {
        if (!canStand && !brokeStanding && hasStartedMoving)
        {
            Vector3 currentPosition = PlayerController.instance.transform.position;
            
            if (Vector3.Distance(currentPosition, lastPlayerPosition) < 0.1f && 
                Mathf.Abs(PlayerController.instance.theRB.linearVelocity.x) < 0.1f)
            {
                standingTimer += Time.deltaTime;
                
                if (standingTimer >= standingTimeLimit)
                {
                    brokeStanding = true;
                    ShowRuleViolation(2, "Violation!");
                    AudioManager.instance.PlaySFX(9);
                }
            }
            else
            {
                standingTimer = 0f;
            }
            
            lastPlayerPosition = currentPosition;
        }
    }

    public void OnJump()
    {
        if (!canJumpMoreThanLimit)
        {
            jumpCount++;
            
            if (jumpCount >= maxJumps && !brokeJumpLimit)
            {
                brokeJumpLimit = true;
                ShowRuleViolation(2, "PREKRŠENO!");
                AudioManager.instance.PlaySFX(9);
            }
        }
    }

    public void OnEnemyKilled()
    {
        if (!canKillEnemies && !brokeKilling)
        {
            brokeKilling = true;
            ShowRuleViolation(1, "Violation!");
            AudioManager.instance.PlaySFX(9);
        }
    }

    public void OnForbiddenZoneEntered()
    {
        if (!canEnterForbiddenZone && !brokeForbiddenZone)
        {
            brokeForbiddenZone = true;
            ShowRuleViolation(3, "Violation!");
            AudioManager.instance.PlaySFX(9);
        }
    }

    private void CheckAllRulesBroken()
    {
        int rulesBroken = 0;
        
        // aktivna pravila koja su prekršena
        if (!canMoveLeft && brokeLeftMovement) rulesBroken++;
        if (!canKillEnemies && brokeKilling) rulesBroken++;
        if (!canStand && brokeStanding) rulesBroken++;
        if (!canJumpMoreThanLimit && brokeJumpLimit) rulesBroken++;
        if (!canEnterForbiddenZone && brokeForbiddenZone) rulesBroken++;

        // koliko ukupno pravila imamo
        int totalActiveRules = 0;
        if (!canMoveLeft) totalActiveRules++;
        if (!canKillEnemies) totalActiveRules++;
        if (!canStand) totalActiveRules++;
        if (!canJumpMoreThanLimit) totalActiveRules++;
        if (!canEnterForbiddenZone) totalActiveRules++;

        // Ubij samo ako su prekršena SVI aktivni rulovi (3)
        if (rulesBroken >= 3 && !isKillingPlayer)
        {
            StartCoroutine(KillPlayerForRuleBreaking());
        }
    }

    private IEnumerator KillPlayerForRuleBreaking()
    {
        isKillingPlayer = true;
        
        // Prikaži poruku
        ShowAllViolations("GAME OVER!");
        
        // INSTANT RESTART - BEZ ČEKANJA
        if (RuleGameOver.instance != null)
        {
            RuleGameOver.instance.TriggerRuleGameOver();
        }
        else
        {
            // Fallback - direktan restart
            DirectRestartLevel();
        }
        
        yield return null; // Samo da zadovolji IEnumerator
    }

    private void DirectRestartLevel()
    {
        // Opcionalno: Vrati u Level Select ili restartuj trenutni level
        if (shouldReturnToLevelSelect)
        {
            // Vrati u level select scenu
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level Select"); // Promeni ime scene
        }
        else
        {
            // Restartuj trenutni level
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    private void ShowRuleViolation(int ruleNumber, string message)
    {
        Text targetViolation = null;
        
        switch(ruleNumber)
        {
            case 1:
                targetViolation = violation1Text;
                break;
            case 2:
                targetViolation = violation2Text;
                break;
            case 3:
                targetViolation = violation3Text;
                break;
        }
        
        if (targetViolation != null)
        {
            targetViolation.text = message;
            targetViolation.color = Color.red;
        }
    }

    private void ShowAllViolations(string message)
    {
        if (violation1Text != null)
        {
            violation1Text.text = message;
            violation1Text.color = Color.red;
        }
        if (violation2Text != null)
        {
            violation2Text.text = message;
            violation2Text.color = Color.red;
        }
        if (violation3Text != null)
        {
            violation3Text.text = message;
            violation3Text.color = Color.red;
        }
    }

    private IEnumerator ShowRulesAtStart()
    {
        if (ruleDisplayPanel != null)
        {
            UpdateRulesDisplay();
            ruleDisplayPanel.SetActive(true);
            
            yield return new WaitForSeconds(ruleDisplayTime);
            
            ruleDisplayPanel.SetActive(true);
        }
    }

    private void UpdateRulesDisplay()
    {
        // rule1Text, rule2Text, rule3Text će prikazati šta god da je unešeno u njih
        
        // Ova funkcija samo osigurava da su tekstovi vidljivi ako postoje
        if (rule1Text != null && !string.IsNullOrEmpty(rule1Text.text))
        {
            rule1Text.gameObject.SetActive(true);
        }
        if (rule2Text != null && !string.IsNullOrEmpty(rule2Text.text))
        {
            rule2Text.gameObject.SetActive(true);
        }
        if (rule3Text != null && !string.IsNullOrEmpty(rule3Text.text))
        {
            rule3Text.gameObject.SetActive(true);
        }
    }


    // Tekst se postavlja direktno u Unity Inspector-u

    private void UpdateUI()
    {
        // Ažuriraj violation tekstove na osnovu trenutnog stanja
        if (!canMoveLeft)
        {
            if (violation1Text != null)
            {
                violation1Text.text = brokeLeftMovement ? "Violation!" : "OK";
                violation1Text.color = brokeLeftMovement ? Color.red : Color.green;
            }
        }
        
        if (!canKillEnemies)
        {
            Text targetText = !canMoveLeft ? violation2Text : violation1Text;
            if (targetText != null)
            {
                targetText.text = brokeKilling ? "Violation!" : "OK";
                targetText.color = brokeKilling ? Color.red : Color.green;
            }
        }
        
        if (!canStand)
        {
            int index = 0;
            if (!canMoveLeft) index++;
            if (!canKillEnemies) index++;
            
            Text targetText = index == 0 ? violation1Text : (index == 1 ? violation2Text : violation3Text);
            if (targetText != null)
            {
                targetText.text = brokeStanding ? "Violation!" : "OK";
                targetText.color = brokeStanding ? Color.red : Color.green;
            }
        }
        
        if (!canJumpMoreThanLimit)
        {
            if (violation2Text != null)
            {
                violation2Text.text = brokeJumpLimit ? "Violation!" : jumpCount + "/" + maxJumps;
                violation2Text.color = brokeJumpLimit ? Color.red : Color.green;
            }
        }
        
        if (!canEnterForbiddenZone)
        {
            if (violation3Text != null)
            {
                violation3Text.text = brokeForbiddenZone ? "Violation!" : "OK";
                violation3Text.color = brokeForbiddenZone ? Color.red : Color.green;
            }
        }
    }

    public void ResetRules()
    {
        brokeLeftMovement = false;
        brokeKilling = false;
        brokeStanding = false;
        brokeJumpLimit = false;
        brokeForbiddenZone = false;
        standingTimer = 0f;
        jumpCount = 0;
        hasStartedMoving = false;
        isKillingPlayer = false;
        
        // Resetuj UI
        if (violation1Text != null)
        {
            violation1Text.text = "OK";
            violation1Text.color = Color.green;
        }
        if (violation2Text != null)
        {
            violation2Text.text = "OK";
            violation2Text.color = Color.green;
        }
        if (violation3Text != null)
        {
            violation3Text.text = "OK";
            violation3Text.color = Color.green;
        }
    }
}