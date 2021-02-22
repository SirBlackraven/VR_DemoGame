using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MechUIManager : MonoBehaviour
{
    /// <summary>
    /// These lists hold all the UI elements that would show a playe's score
    /// Example: The mech dash display and the in-game scoreboard (TODO)
    /// </summary>
    public List<GameObject> PlayerOne_ScoreReadouts = new List<GameObject>();
    public List<GameObject> PlayerTwo_ScoreReadouts = new List<GameObject>();
    public List<GameObject> PlayerThree_ScoreReadouts = new List<GameObject>();
    public List<GameObject> PlayerFour_ScoreReadouts = new List<GameObject>();

    //mech health bar
    public List<ProgressBar> playerHealthDisplay = new List<ProgressBar>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeHealthBars();
    }

    public void InitializeHealthBars()
    {
        foreach (ProgressBar bar in playerHealthDisplay)
        {
            bar.BarValue = 100.0f;
        }
    }

    public void UpdateHealthBar(int playerNumber, float amount)
    {
        playerHealthDisplay[playerNumber].BarValue = amount;
    }

    /// <summary>
    /// Adds to the specified player's score. This will update all UI's that display the score
    /// Note: Player number is zero-based, of course; Player 1 = 0, ect ;)
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="amount"></param>
    public void AddScore(int playerNumber, int amount)
    {
        //DebugManagerScript.Instance.AddMessage("Echo from Mech UI- score message: Player:" + playerNumber.ToString() + ", amount:" + amount.ToString());

        switch (playerNumber)
        {
            case 0:
                foreach (GameObject g in PlayerOne_ScoreReadouts)
                {
                    try
                    {
                        var text = g.GetComponent<Text>();
                        text.text = amount.ToString();
                    }
                    catch (System.Exception ex)
                    {
                        DebugManagerScript.Instance.AddMessage("Something went wrong updating the scores");
                    }

                }
                break;
            case 1:
                foreach (GameObject g in PlayerTwo_ScoreReadouts)
                {
                    try
                    {
                        var text = g.GetComponent<Text>();
                        text.text = amount.ToString();
                    }
                    catch (System.Exception ex)
                    {
                        DebugManagerScript.Instance.AddMessage("Something went wrong updating the scores");
                    }

                }
                break;
            case 2:
                foreach (GameObject g in PlayerThree_ScoreReadouts)
                {
                    try
                    {
                        var text = g.GetComponent<Text>();
                        text.text = amount.ToString();
                    }
                    catch (System.Exception ex)
                    {
                        DebugManagerScript.Instance.AddMessage("Something went wrong updating the scores");
                    }

                }
                break;
            case 3:
                foreach (GameObject g in PlayerFour_ScoreReadouts)
                {
                    try
                    {
                        var text = g.GetComponent<Text>();
                        text.text = amount.ToString();
                    }
                    catch (System.Exception ex)
                    {
                        DebugManagerScript.Instance.AddMessage("Something went wrong updating the scores");
                    }

                }
                break;
        }
    }
}
