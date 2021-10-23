using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameState : MonoBehaviour
{
    private bool isPlayerDead = false;
    private bool isEnemyDead = false;
    private Transform targetEnemy;
    private Transform targetPlayer;
    private MainMenu _menu;
    const string water = "Water";
    const string letter = "Letter3";
    private bool hasWater = false;
    private bool hasLetter = false;
    private bool isInventoryFull = false;
    private bool isHoldingShift = false;
    [SerializeField]
    private bool wasLetterPickedUp = false;
    [SerializeField]
    private bool wasWaterPickedUp = false;
    private bool isDoorOpen = false;
    public bool isPlayerSafe = false;
    private bool canOpenDoor = true;

    void Start()
    {
        targetEnemy = GameObject.Find("Enemy").GetComponent<Transform>();
        _menu = gameObject.GetComponent<MainMenu>();        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _menu.StopGame();
        }
    }    
    public void PlayerCaught()
    {        
        isPlayerDead = true;
    }
    public void KilledEnemy() => isEnemyDead = true;
    public void UpdateEnemyPosition(Transform enemy) => targetEnemy = enemy;
    public void UpdatePlayerPosition(Transform player) => targetPlayer = player;
    public bool IsPlayerDead() => isPlayerDead;
    public bool IsEnemyDead() => isEnemyDead;
    public bool IsInventoryFull() => isInventoryFull;
    public Transform TargetEnemy() => targetEnemy;
    public Transform TargetPlayer() => targetPlayer;
    public bool HoldsShift(bool holds) => isHoldingShift = holds;
    public bool IsHoldingShift() => isHoldingShift;
    public void PlayerFinishedDeathSequence()
    {        
        _menu.EndGame();
    }
    public void EnemyFinishedDeathSequence()
    {
        _menu.EndGameWon();
    }
    public void UpdateInventory(string objectName)
    {
        if (objectName == water)
            hasWater = true;
        if (objectName == letter)
            hasLetter = true;

        if (hasWater == true)
            isInventoryFull = true;
    }
    public void ObjectWasPickedup(string objectName)
    {       
        if (objectName == letter)
            wasLetterPickedUp = true;
        if (objectName == water)
        {
            wasLetterPickedUp = false;
            wasWaterPickedUp = true;
        }
    }
    public void PlayerSafe(bool isSafe)
    {
        isPlayerSafe = isSafe & !isDoorOpen;
    }
    public bool IsPlayerSafe() => isPlayerSafe;
    public void ChangeDoorState(bool open) => isDoorOpen = open;
    public bool WasLetterPickedUp() => wasLetterPickedUp;
    public bool WasWaterPickedUp() => wasWaterPickedUp;
    public void BlockOpeningDoor(bool canOpen) => canOpenDoor = canOpen;
    public bool CanOpenDoor() => canOpenDoor;
}
