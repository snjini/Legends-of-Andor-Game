using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using TMPro;
using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using System.Threading.Tasks;
using System.Collections.Generic;
    
public class Dice : MonoBehaviour {
    private Sprite[] diceSides;
    private Image img;
    private Fight currentFight;
    private int nbDice;
    private int highestRoll;
    private int highestMonsterRoll;
    private bool endTurnEarlyButtonClicked;
    private Button endTurnEarlyButton;
    private bool flipDiceButtonClicked;
    private Button flipDiceButton;
    public static bool helmUsed;
    public static bool brewUsed;
    public static bool runestonesUsed;
    public static List<int> HeroRoundRolls;
    public static string monsterType;
    private int monsterRangeNum;
    private int heroRangeNum;

    private void Start () {
        img = GetComponent<Image>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        currentFight = GameObject.Find("FightController").GetComponent<Fight>();
        HeroRoundRolls = new List<int>();

        //Power Buttons
        Button endTurnEarlyButton = GameObject.Find("EndTurnEarlyButton").GetComponent<Button>();
        endTurnEarlyButton.onClick.AddListener(() => EndTurnEarly());
        if (!((string)PhotonNetwork.LocalPlayer.CustomProperties["Type"]).Equals("Archer")) {
            endTurnEarlyButton.gameObject.SetActive(false);
        }

        Button flipDiceButton = GameObject.Find("FlipDiceButton").GetComponent<Button>();
        flipDiceButton.onClick.AddListener(() => FlipDice());
        if (!((string)PhotonNetwork.LocalPlayer.CustomProperties["Type"]).Equals("Wizard")) {
            flipDiceButton.gameObject.SetActive(false);
        }


    }
    void OnMouseDown()
    {   
        if (nbDice > 0) {
            StartCoroutine("RollTheDice");
        } 
    }
    public IEnumerator RollTheDice()
    { 
        int heroRangeNum = 6;
        if (runestonesUsed) {
            heroRangeNum = 10;
            runestonesUsed = false;
        }
        int randomDiceSide = 0;
        int finalSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(0, heroRangeNum);
            img.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
        if (helmUsed) {
            //add together == rolls
            finalSide = randomDiceSide + 1;
            int total = finalSide;
            if (HeroRoundRolls.Count > 0) {
                foreach (int i in HeroRoundRolls) {
                    if (i == finalSide) {
                        total += finalSide;
                    }
                }
            }
            HeroRoundRolls.Add(finalSide);

            if (total > highestRoll) {
                highestRoll = total;
                currentFight.UpdateBestScore(highestRoll);
            }
         
        } else {
            finalSide = randomDiceSide + 1;
            if (finalSide>highestRoll) {
                highestRoll = finalSide;
                currentFight.UpdateBestScore(highestRoll);
            }
        }

        nbDice --;
        heroRangeNum = 6;
        Fight.useHelmButton.gameObject.SetActive(false);
        GameObject.Find("DiceRollsLeft").GetComponent<TextMeshProUGUI>().text = "Dice Left to Roll:    " + nbDice;

        //Options of Powers for wizard
        string s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        Task usePowerOption = this.UsePowers(s, finalSide);

    }

    private async Task UsePowers(string s, int finalSide) {
        await Task.Delay(3000);
        if (s.Equals("Archer") && nbDice > 0 && endTurnEarlyButtonClicked) {
            endTurnEarlyButtonClicked = false;
            currentFight.EndTurn();
        } else {
            if (s.Equals("Wizard") && flipDiceButtonClicked) {
                //WIZARD POWER
                flipDiceButtonClicked = false;
                finalSide = 6 - finalSide + 1;
                img.sprite = diceSides[finalSide - 1];
                if (finalSide>highestRoll) {
                    highestRoll = finalSide;
                    currentFight.UpdateBestScore(highestRoll);
                }
            }
            //BREW OPTION
            if (brewUsed && !helmUsed) {
                finalSide = finalSide * 2;
                if (finalSide>highestRoll) {
                    highestRoll = finalSide;
                    currentFight.UpdateBestScore(highestRoll);
                }
                brewUsed = false;
            }

            if (nbDice==0) {
                helmUsed = false;
                brewUsed = false;
                currentFight.EndTurn();
                Debug.Log("turn over");
            }
        }
    }

    private void EndTurnEarly() {
        if (((string)PhotonNetwork.LocalPlayer.CustomProperties["Type"]).Equals("Archer")) {
            endTurnEarlyButtonClicked = true;  
            nbDice = 0;
            GameObject.Find("DiceRollsLeft").GetComponent<TextMeshProUGUI>().text = "Dice Left to Roll:    " + nbDice;
     
        } 
    }

    private void FlipDice() {
        if (((string)PhotonNetwork.LocalPlayer.CustomProperties["Type"]).Equals("Wizard")) {
            flipDiceButtonClicked = true;
            //flip dice
            flipDiceButton.gameObject.SetActive(false);       
        } 
    }

    public IEnumerator AutoRoll()
    {
        int monsterRangeNum = 6;
        if (currentFight.monsterType.Equals("Wardrak")) {
            monsterRangeNum = 10;
        }
        HeroRoundRolls = new List<int>();
        this.highestMonsterRoll = 0;
        int randomDiceSide = 0;
        int finalSide = 0;
        int firstRoll=0;
        while(nbDice > 0) {
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i <= 20; i++) {
                randomDiceSide = UnityEngine.Random.Range(0, monsterRangeNum);
                img.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }
            finalSide = randomDiceSide + 1;
            if(nbDice==2) firstRoll = finalSide;
            if (finalSide > highestMonsterRoll) {
                highestMonsterRoll = finalSide;
                currentFight.UpdateMonsterBestScore(highestMonsterRoll);
            }
            if (nbDice==1 && firstRoll==finalSide && 2*firstRoll > highestMonsterRoll) {
                highestMonsterRoll = 2*firstRoll;
                currentFight.UpdateMonsterBestScore(highestMonsterRoll);
            }
            nbDice --;
            //GameObject.Find("DiceRollsLeft").GetComponent<TextMeshProUGUI>().text = "Dice Left to Roll:    " + nbDice;
        }
        yield return new WaitForSeconds(3.0f);
        firstRoll = 0;
        currentFight.EndMonsterTurn();
        Debug.Log("monster turn over");
    }
    public void SetDice(int d) {
        this.nbDice = d;
    }

    public void Reset() {
        this.highestMonsterRoll = 0;
        this.highestRoll = 0;
    }
}