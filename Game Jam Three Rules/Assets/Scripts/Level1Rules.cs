using UnityEngine;

public class Level1Rules : MonoBehaviour
{
    void Start()
    {
        // Pravila za Level 1
        if (RuleManager.instance != null)
        {
            // Level 1: 
            RuleManager.instance.canMoveLeft = false;          // NE SME levo
            RuleManager.instance.canKillEnemies = true;        // SME da ubija neprijatelje
            RuleManager.instance.canStand = false;             // NE SME da stane
            RuleManager.instance.canJumpMoreThanLimit = true;  // SME da skače bez limita
            RuleManager.instance.canEnterForbiddenZone = false;// NE SME u crvene zone
            
            RuleManager.instance.standingTimeLimit = 0.5f; 
        }
        else
        {
            Debug.LogError("RuleManager instance not found! Make sure RuleManager is in the scene.");
        }
    }
}