using UnityEngine;

public class Level4Rules : MonoBehaviour
{
    void Start()
    {
        // Postavi pravila za Level 4
        if (RuleManager.instance != null)
        {
            // Level 4: 
            RuleManager.instance.canMoveLeft = false;           
            RuleManager.instance.canKillEnemies = true;        
            RuleManager.instance.canStand = false;             
            RuleManager.instance.canJumpMoreThanLimit = true; 
            RuleManager.instance.canEnterForbiddenZone = false;
            
            
            RuleManager.instance.standingTimeLimit = 0.5f; 
        }
        else
        {
            Debug.LogError("RuleManager instance not found! Make sure RuleManager is in the scene.");
        }
    }
}