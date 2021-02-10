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
    public static GameStateManagerScript Instance;

    //UI Elements to be removed
    /*public List<GameObject> PlayerOne_ScoreReadouts = new List<GameObject>();
    public List<GameObject> PlayerTwo_ScoreReadouts = new List<GameObject>();
    public List<GameObject> PlayerThree_ScoreReadouts = new List<GameObject>();
    public List<GameObject> PlayerFour_ScoreReadouts = new List<GameObject>();

    public MovementController PlayerOne_MovementController; //needed to reset positions and movement mode once they are killed
    public MovementController PlayerTwo_MovementController; //needed to reset positions and movement mode once they are killed
    public MovementController PlayerThree_MovementController; //needed to reset positions and movement mode once they are killed
    public MovementController PlayerFour_MovementController; //needed to reset positions and movement mode once they are killed*/

    //public List<ProgressBar> playerHealthDisplay = new List<ProgressBar>();
    //end ui

    public MovementController moveController;

    //public GameObject playerController;

    //Suporting managers
    public AvatarSelectionManager avatarManager;
    public MechUIManager mechUi;
    public EnemyManagerScript enemyManager;
    public MechGameAudioManagerScript audioManager;

    //Player data collection
    public List<PlayerData> players = new List<PlayerData>();    

    //general game state data
    public int TotalNumberPlayers = 1;
    public int CurrentLevel = 1;
    public bool GameActive = false;

    public GameObject BridgeToMech;
    public GameObject PlayerMechRepresentation;
    public GameObject EnterMechButton;

    

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
            return; //can happen on a retrigger of the hangar door 
        }

        DebugManagerScript.Instance.AddMessage(">>GAME STARTED<<");
        GameActive = true;

        /*foreach(ProgressBar bar in playerHealthDisplay)
        {
            bar.BarValue = 100.0f;
        }*/
        
        mechUi.InitializeHealthBars();

        DebugManagerScript.Instance.AddMessage("playing music:");

        try
        {
            DebugManagerScript.Instance.AddMessage("Try music:");
            MechGameAudioManagerScript.Instance.PlayMusic();
            DebugManagerScript.Instance.AddMessage("After Try music:");
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

    // Update is called once per frame
    //Saving in case needed later
    /*void Update()
    {
        if(!GameActive)
        {
            return;
        }

        //Possibly do other stuff here
    }*/

    public void SeekerKilled()
    {
        //leaving this here even though its a pass-thru. We might need to figure out which player to call the Sound effect on
        audioManager.PlaySeekerDeathSound();


    }

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
            avatarManager.LeaveMech();
        }
        else
        {
            if(players[playerNumber].Health > 100)
            {
                players[playerNumber].Health = 100;
            }
            //playerHealthDisplay[playerNumber].BarValue = players[playerNumber].Health;
            mechUi.UpdateHealthBar(playerNumber, players[playerNumber].Health);
        }

#if CHECKDEATH
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

        DebugManagerScript.Instance.AddMessage("Player check count:" + checkCount.ToString());

        bool gameOver = true;
        foreach(PlayerData checkPlayer in playersToCheck)
        {
            if(checkPlayer.IsDead == false)
            {
                gameOver = false;
                DebugManagerScript.Instance.AddMessage("game over set to false");
            }
        }

        if(gameOver)
        {
            DebugManagerScript.Instance.AddMessage("Resetting game") ;
            ResetGame();
        }   
#endif
    }


    public void AddScore(int playerNumber, int amount)
    {
        //DebugManagerScript.Instance.AddMessage("score message: Player:" + playerNumber.ToString() + ", amount:" + amount.ToString());

        players[playerNumber].PlayerScore += amount;

        mechUi.AddScore(playerNumber, players[playerNumber].PlayerScore);
    }


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

        //return players to start somehow
        //avatarManager.LeaveMech();

        //need to rethink this;
        //playerController.transform.position = new Vector3(26.78f, 1.4f, -49.5f);
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
        MechGameAudioManagerScript.Instance.StopMechWalking();
        //TODO: kill player effect
        MechGameAudioManagerScript.Instance.PlayMechDestroyed();
    }
}

