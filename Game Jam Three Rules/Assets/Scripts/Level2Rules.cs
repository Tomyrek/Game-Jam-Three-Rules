using UnityEngine;

public class Level2Rules : MonoBehaviour
{
    void Start()
    {
        // Postavi pravila za Level 2
        if (RuleManager.instance != null)
        {
            // Level 2: Ne sme više od 3 skokova, ne sme da stane kada krene, ne sme u crvene zone
            RuleManager.instance.canMoveLeft = true;           // SME levo
            RuleManager.instance.canKillEnemies = true;        // SME da ubija neprijatelje
            RuleManager.instance.canStand = false;             // NE SME da stane kada krene
            RuleManager.instance.canJumpMoreThanLimit = false; // NE SME više od 3 skoka
            RuleManager.instance.canEnterForbiddenZone = false;// NE SME u crvene zone
            
            RuleManager.instance.maxJumps = 3;
            RuleManager.instance.standingTimeLimit = 0.5f; // Vrlo kratak limit - čim stane prekršava pravilo
        }
        else
        {
            Debug.LogError("RuleManager instance not found! Make sure RuleManager is in the scene.");
        }
    }
}