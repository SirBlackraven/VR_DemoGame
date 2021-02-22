#define CHECKDEATH

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerData
{
    public int PlayerNumber = 0;
    public int PlayerScore = 0;
    public bool IsActive = false;
    public bool IsDead = false;
    public bool IsInvulnerable = false; //test mode or power up (maybe)
    public float Health = 100.0f;
}

public class GameStateManagerScript : MonoBehaviour
{
    //singleton
    public static GameStateManagerScript Instance;

     public MovementController moveController;

    //Suporting managers
    public AvatarSelectionManager avatarManager;
    public MechUIManager mechUi;
    public EnemyManagerScript enemyManager;
    public MechGameAudioManagerScript audioManager;
        
    public List<PlayerData> players = new List<PlayerData>();    //Player data collection

    //general game state data
    public int TotalNumberPlayers = 1;
    public int CurrentLevel = 1;
    public bool GameActive = false;

    public GameObject BridgeToMech;                 //special ref to remove the mech bridge when mech activated (it gets in the way if not) 
    public GameObject PlayerMechRepresentation;     //remove the placeholder static mech when mech activated    TODO: move this and above to centralized manager
    public GameObject EnterMechButton;              //master button to enter mech and start game
       

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //setup data holders
        PlayerData Player1 = new PlayerData();
        PlayerData Player2 = new PlayerData();
        PlayerData Player3 = new PlayerData();
        PlayerData Player4 = new PlayerData();

        players.Add(Player1);
        players.Add(Player2);
        players.Add(Player3);
        players.Add(Player4);

        //there must be at least 1 player for the game to begin
        Player1.IsActive = true;
    }   

    public void StartGame()
    {

        if(GameActive)
        {
            return; //can happen on a retrigger of the hangar door tripwire which starts the enemy spawner
        }

        DebugManagerScript.Instance.AddMessage(">>GAME STARTED<<");
        GameActive = true;        

        try
        {
            //the healthbar prefabs defaut to empty. Set them to 100%        
            mechUi.InitializeHealthBars();

            //Start background music
            MechGameAudioManagerScript.Instance.PlayMusic();
        }
        catch(System.Exception ex)
        {
            DebugManagerScript.Instance.AddMessage("Error:" + ex.Message);
        }
    }

    public void PauseGame()
    {

        //TODO- maybe, maybe not. Its an MU game

    }


    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    public void SeekerKilled()
    {
        //leaving this here even though its a pass-thru. We will need to figure out which player to call the Sound effect on in MU play
        audioManager.PlaySeekerDeathSound();
    }

    /// <summary>
    /// Reduces a player's health
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="amount"></param>
    public void ApplyDamage(int playerNumber, int amount)
    {
        if (players[playerNumber].IsInvulnerable)
        {           
            return;
        }

        players[playerNumber].Health -= amount;

        if(players[playerNumber].Health <= 0)
        {
            
            players[playerNumber].IsDead = true;            

            //turn off mech style movement, reset throttle
            moveController.ResetRigState();

            //swap avatars
           // avatarManager.LeaveMech();
        }
        else
        {
            if(players[playerNumber].Health > 100)
            {
                players[playerNumber].Health = 100;
            }

            mechUi.UpdateHealthBar(playerNumber, players[playerNumber].Health);
        }

        //preprocessor to disable resetting the game. used for testing 
#if CHECKDEATH

        //enumerate all players in MU game, then check them
        int checkCount = 0;
        List<PlayerData> playersToCheck = new List<PlayerData>();
        foreach(PlayerData p in this.players)
        {
            if(p.IsActive)
            {
                playersToCheck.Add(p);
                checkCount++;
            }
        }

        //assume all are dead then check if this is true
        bool gameOver = true;
        foreach(PlayerData checkPlayer in playersToCheck)
        {
            if(checkPlayer.IsDead == false)
            {
                gameOver = false;
            }
        }

        if(gameOver)
        {
            DebugManagerScript.Instance.AddMessage("Resetting game") ;
            ResetGame();
        }   
#endif
    }

    //add score to indicated player
    public void AddScore(int playerNumber, int amount)
    {
        players[playerNumber].PlayerScore += amount;

        mechUi.AddScore(playerNumber, players[playerNumber].PlayerScore);
    }

    /// <summary>
    /// Nain function to return the game to its initial state
    /// </summary>
    public void ResetGame()
    {
        foreach(PlayerData p in this.players)
        {
            p.PlayerScore = 0;

            if(p.IsActive)
            {
                p.IsDead = false;
            }

            p.Health = 100;
        }

        //reset player avatar and state
        avatarManager.ResetPlayer(0);

        //reset Enemy manager
        enemyManager.EnemiesSpawned = 0;

        //end active state
        GameActive = false;

        //re-enable the bridge and boarding mech
        BridgeToMech.SetActive(true);
        PlayerMechRepresentation.SetActive(true);
        EnterMechButton.SetActive(true);

        //turn off mech walking sound
        MechGameAudioManagerScript.Instance.StopMechWalking();
        
        //play player killed SFX. This is done at the end so it will
        //play where the player can hear it
        MechGameAudioManagerScript.Instance.PlayMechDestroyed();
    }
}

