using UnityEngine;

public class Level3Rules : MonoBehaviour
{
    void Start()
    {
        // Pravila za Level 3
        if (RuleManager.instance != null)
        {
            // Level 3: Ne sme u crvene zone, ne sme da skače, ne sme da stane
            RuleManager.instance.canMoveLeft = true;           // SME levo
            RuleManager.instance.canKillEnemies = true;        // SME da ubija neprijatelje
            RuleManager.instance.canStand = false;             // NE SME da stane
            RuleManager.instance.canJumpMoreThanLimit = false; // NE SME da skače (0 skokova)
            RuleManager.instance.canEnterForbiddenZone = false;// NE SME u crvene zone
            
            RuleManager.instance.maxJumps = 30; // NULA skokova - čim skoči jednom, prekršava pravilo
            RuleManager.instance.standingTimeLimit = 0f; // 1 sekunda limit za stajanje
        }
        else
        {
            Debug.LogError("RuleManager instance not found! Make sure RuleManager is in the scene.");
        }
    }
}