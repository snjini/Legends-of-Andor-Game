using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System; 
using System.Collections.Generic;
using System.Collections; 
using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

using UnityEngine.SceneManagement;

public class Fight: MonoBehaviourPun {

    public string monsterType;
    private int monsterWP;
    private int monsterSP;
    private int heroBattleValue;
    private int heroBestScore;
    private int monsterBestScore;
    private int region;
    public List<string> fighters = new List<string>();
    private int round = 0;
    private int[] diceToRoll;
    private bool turnInProgress;
    private Dice heroDice;
    private Dice monsterDice;
    private GameObject flipDiceButton;
    private GameObject roundOverPanel;
    private GameObject heroWinnerPanel;
    private GameObject monsterWinnerPanel;
    private GameObject outOfHoursPanel;
    public List<int> hoursUsed = new List<int>();
    public int numToInvite;
    public int numDecided;
    private Button useShieldButton;
    public static Button useHelmButton;
    private Button useBrewButton;
    private Button useRunestonesButton;
    private int oldWp;

    private GameObject monstToken;

    

    [PunRPC]
    public void AddToFight(string s) {
        this.fighters.Add(s);
        this.hoursUsed.Add(0);
        numDecided++;
    }

    public void InitializeFight(GameObject token, string type, int region, string inviter) {
        Debug.Log("initializing fight");
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"]-1;
        if(i<0) i = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]).Length-1;
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        string myHero = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        if (myHero == s) {
            this.monsterType = type;
            Dice.monsterType = type;
            this.region = region;
            this.monstToken = token;
            Vector2Int stats = GetMonsterStats(type);
            this.monsterWP = stats[0];
            this.monsterSP = stats[1];
            this.gameObject.name = "FightController";
            turnInProgress = true;
        } 
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("FightScene"));  
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(FightSceneLoaded(inviter));
    
    }

    IEnumerator LoadInTheFight(GameObject token, string type, int region, string inviter) {
        Debug.Log("Waiting for the players to decide");
        yield return new WaitUntil(() => numDecided == numToInvite);
        Debug.Log("LoadInTheFight");
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"]-1;
        if(i<0) i = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]).Length-1;
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        string myHero = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        this.monsterType = type;
        Dice.monsterType = type;
        this.region = region;
        Vector2Int stats = GetMonsterStats(type);
        this.monsterWP = stats[0];
        this.monsterSP = stats[1];
        this.monstToken = token;
        this.gameObject.name = "FightController";
        turnInProgress = true;
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("FightScene"));  
        DontDestroyOnLoad(this.gameObject);
        this.gameObject.GetPhotonView().RPC("LoadTheLevel", RpcTarget.MasterClient);
        StartCoroutine(FightSceneLoaded(inviter));

    }

    [PunRPC]
    public void LoadTheLevel() {
        PhotonNetwork.LoadLevel(3);
    }


    public void InviteFighters(GameObject monstToken, string type, int region, string inviter) {
        List<string> players = new List<string>() {"Archer", "Dwarf", "Warrior", "Wizard"};
        players.Remove(inviter);
        Debug.Log("inviter: " + inviter);
        foreach (string p in players) {
            int index = Array.IndexOf(((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]), p);
            if (index != -1 && p != inviter) {
                int heroReg = (int)PhotonNetwork.PlayerList[index].CustomProperties["pos"];
                int[] adjRegions = Graph.getAdjRegions(region);
                if (heroReg == region) {
                    Debug.Log("inviting " + p);
                    numToInvite ++;
                    InviteToFight(p);
                } else if (p == "Archer" && Array.Exists(adjRegions, r => r == heroReg)) {
                    Debug.Log("inviting " + p);
                    numToInvite ++;
                    InviteToFight(p);
                }
            }
        }
        Debug.Log("numToInvite = " + numToInvite);
       StartCoroutine(LoadInTheFight(monstToken, type, region, inviter));
    }

    public void InviteToFight(string s) {
        Debug.Log("Inviting hero to fight: " + s);
        GameObject joinFightCanvas = GameBoardRegion.joinFightPopup;
        Button joinConfirm = GameBoardRegion.joinYesButton;
        GameObject hero = GameObject.Find(s);
        if (hero == null) hero = GameObject.Find(s + "(Clone)");
        PhotonView heroView = hero.GetPhotonView();
        int index = Array.IndexOf((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"], s);
        Debug.Log("setting up the join fight popup");
        //Player target = heroView.Owner;
        //PhotonPlayer target = PhotonPlayer.Get(targetID);
        GameBoardRegion.joinYesButton.onClick.AddListener(() => this.gameObject.GetPhotonView().RPC("AddToFight", RpcTarget.AllBuffered, (string)s));
        GameBoardRegion.joinNoButton.onClick.AddListener(() => joinFightCanvas.GetPhotonView().RPC("SetActive", PhotonNetwork.PlayerList[index], (bool)false));
        joinFightCanvas.GetPhotonView().RPC("SetActive", PhotonNetwork.PlayerList[index], (bool)true);
    }

    private Vector2Int GetMonsterStats(string type) {
        Debug.Log("Type: " + type);
        if (type=="Gor") {
            return new Vector2Int(4,2);
        }
        else if (type=="Skral") {
            return new Vector2Int(6,6);
        }
        else if (type=="Troll") {
            return new Vector2Int(7,10);
        }
        else {
            return new Vector2Int(12,14);
        }
    }

    public IEnumerator FightSceneLoaded(string s) {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "FightScene");
        Debug.Log("In FightSceneLoaded!!!");
        Sprite newSprite = Resources.Load<Sprite>(s+"Icon");
        GameObject.Find("CurrentPlayerPic").GetComponent<SpriteRenderer>().sprite = newSprite;
        this.heroDice = GameObject.Find("HeroDice").GetComponent<Dice>();
        this.monsterDice = GameObject.Find("MonsterDice").GetComponent<Dice>();
        this.flipDiceButton = GameObject.Find("FlipDiceButton");
        Debug.Log("fight scene loaded");
        this.roundOverPanel = GameObject.Find("RoundOverPanel");
        this.heroWinnerPanel = GameObject.Find("HeroWinnerPanel");
        this.monsterWinnerPanel = GameObject.Find("MonsterWinnerPanel");
        this.outOfHoursPanel = GameObject.Find("OutOfHoursPanel");
        Button yes = GameObject.Find("PlayAnotherButton").GetComponent<Button>();
        Button no = GameObject.Find("ExitFightButton").GetComponent<Button>();
        Button ok1 = GameObject.Find("OKButton1").GetComponent<Button>();
        Button ok2 = GameObject.Find("OKButton2").GetComponent<Button>();
        //Button ok3 = GameObject.Find("OKButton3").GetComponent<Button>();
        Button wpPrize = GameObject.Find("WpPrizeButton").GetComponent<Button>();
        Button goldPrize = GameObject.Find("GoldPrizeButton").GetComponent<Button>();

        LoadGame l = GameObject.Find("LoadGameFromFight").GetComponent<LoadGame>();

        wpPrize.onClick.AddListener(() => WinWillpower());
        goldPrize.onClick.AddListener(() => WinGold());

        this.useShieldButton = GameObject.Find("UseShieldButton").GetComponent<Button>();
        this.useShieldButton.onClick.AddListener(() => UseShield());

        useHelmButton = GameObject.Find("UseHelmButton").GetComponent<Button>();
        useHelmButton.onClick.AddListener(() => UseHelm());

        this.useBrewButton = GameObject.Find("UseBrewButton").GetComponent<Button>();
        this.useBrewButton.onClick.AddListener(() => UseBrew());

        this.useRunestonesButton = GameObject.Find("UseRunestonesButton").GetComponent<Button>();
        this.useRunestonesButton.onClick.AddListener(() => UseRunestones());

        no.onClick.AddListener(() => GameObject.Find(s).GetComponent<Hero>().UpdateTimeTrack(this.hoursUsed[0]));
        no.onClick.AddListener(() => this.DestroyFightController());
        no.onClick.AddListener(() => l.LoadGameScene());
        ok1.onClick.AddListener(() => GameObject.Find(s).GetComponent<Hero>().UpdateTimeTrack(this.hoursUsed[0]));
        ok1.onClick.AddListener(() => GameObject.Find(s).GetComponent<Hero>().endTurn());
        ok1.onClick.AddListener(() => l.LoadGameScene());
        ok1.onClick.AddListener(() => this.DestroyFightController());
        ok2.onClick.AddListener(() => GameObject.Find(s).GetComponent<Hero>().UpdateTimeTrack(this.hoursUsed[0]));
        ok2.onClick.AddListener(() => this.DestroyFightController());
        ok2.onClick.AddListener(() => l.LoadGameScene());
        wpPrize.onClick.AddListener(() => GameObject.Find(s).GetComponent<Hero>().UpdateTimeTrack(this.hoursUsed[0]));
        wpPrize.onClick.AddListener(() => this.DestroyFightController());
        goldPrize.onClick.AddListener(() => GameObject.Find(s).GetComponent<Hero>().UpdateTimeTrack(this.hoursUsed[0]));
        goldPrize.onClick.AddListener(() => this.DestroyFightController());
        
        yes.onClick.AddListener(() => this.PlayRound());
        this.roundOverPanel.SetActive(false);
        this.heroWinnerPanel.SetActive(false);
        this.monsterWinnerPanel.SetActive(false);
        this.outOfHoursPanel.SetActive(false);
        this.useShieldButton.gameObject.SetActive(false);
        useHelmButton.gameObject.SetActive(false);
        this.useBrewButton.gameObject.SetActive(false);
        this.useRunestonesButton.gameObject.SetActive(false);

        PlayRound();
    }

    public void PlayRound() {
        turnInProgress = true;
        hoursUsed[0]++;
        heroDice.Reset();
        monsterDice.Reset();
        this.heroBestScore = 0;
        this.monsterBestScore = 0;
        this.roundOverPanel.SetActive(false);
        if (this.fighters[0].Equals("Wizard")) {
            this.flipDiceButton.SetActive(true);
        }
        //HELM OPTION
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["helm"] > 0 && !this.fighters[0].Equals("Wizard") && !this.fighters[0].Equals("Archer")) {
            useHelmButton.gameObject.SetActive(true);
        }

        //BREW OPTION
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["brew"] > 0) {
            this.useBrewButton.gameObject.SetActive(true);
        }

        //RUNESTONES OPTION
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["greenStone"] > 0 && (int)PhotonNetwork.LocalPlayer.CustomProperties["yellowStone"] > 0 && (int)PhotonNetwork.LocalPlayer.CustomProperties["blueStone"] > 0) {
            this.useRunestonesButton.gameObject.SetActive(true);
        }

        GameObject.Find("CurrentPlayerPic").GetComponent<SpriteRenderer>().sortingLayerName = "FightImage";
        //update round counter GUI
        this.round++;
        GameObject.Find("RoundCounter").GetComponent<TextMeshProUGUI>().text = "ROUND " + this.round + "\n ------------";
        //update monster stats GUI
        GameObject.Find("MonsterSP").GetComponent<TextMeshProUGUI>().text = "Monster Strength:     " + this.monsterSP;
        //update battle value GUI
        int[] dice = GetNbDice();
        GameObject.Find("BattleValueText").GetComponent<TextMeshProUGUI>().text = "Battle Value:   " + this.heroBattleValue;
        GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Best Score:    " + this.heroBestScore;
        GameObject.Find("MonsterBestScore").GetComponent<TextMeshProUGUI>().text = "Best Score:               " + this.monsterBestScore;
        GameObject.Find("DiceRollsLeft").GetComponent<TextMeshProUGUI>().text = "Dice Left to Roll:    " + dice[0];

        //for(int i=0; i<fighters.Count; i++) {
            heroDice.SetDice(dice[0]);
            StartCoroutine(WaitForEndOfTurn());
        //}
        //update dice to roll for curr player - in new method??
    }

    IEnumerator WaitForEndOfTurn() {
        Debug.Log("In Wait for End Of Turn");
        yield return new WaitUntil(() => turnInProgress==false);
        turnInProgress=true;
        Debug.Log("Switching to Monster Turn");
        if (monsterWP <= 6) {
            monsterDice.SetDice(2);
        }
        else {
            monsterDice.SetDice(3);
        }
        StartCoroutine(monsterDice.AutoRoll());
    }

    private int[] GetNbDice() {
        Debug.Log("length:" + fighters.Count);
        this.diceToRoll = new int[fighters.Count];
        this.heroBattleValue = 0;
        for(int i = 0; i<fighters.Count; i++) {
            //hoursUsed[i] += 1;
            int playerWP = (int)PhotonNetwork.LocalPlayer.CustomProperties["wp"]; //change for multiplayer fight
            this.heroBattleValue += (int)PhotonNetwork.LocalPlayer.CustomProperties["sp"]; //change for multiplayer fight
            string playerType = fighters[i];
            if (playerType=="Dwarf") {
                this.diceToRoll[i] = playerWP/7+ 1;
            } else if (playerType=="Wizard") {
                this.diceToRoll[i] = 1;
            } else if (playerType=="Archer") {
                this.diceToRoll[i] = playerWP/7+ 3;
            } else if (playerType=="Warrior") {
                this.diceToRoll[i] = playerWP/7+ 2;
            }
        }
        Debug.Log("dice to roll:" + diceToRoll[0]);
        return this.diceToRoll;
    }

    public void EndTurn() {
        Debug.Log("Ending Turn");
        this.turnInProgress = false;
    }

    public void EndMonsterTurn() {
        this.turnInProgress = false;
        if (this.monsterBestScore > this.heroBestScore) {
            Debug.Log("Monster wins");
        }
        else if (this.heroBestScore > this.monsterBestScore) {
            Debug.Log("Hero wins");
        }
        else {
            Debug.Log("tie");
        }
        EndOfRound(this.heroBestScore - this.monsterBestScore);
    }

    private void EndOfRound(int outcome) {
        this.turnInProgress = true;
        int wp = (int)PhotonNetwork.LocalPlayer.CustomProperties["wp"];
        this.oldWp = wp;
        Debug.Log("Outcome: " + outcome);
        int hours = (int)PhotonNetwork.LocalPlayer.CustomProperties["Hours"];
        if (outcome > 0)  { //hero wins
            this.monsterWP -= outcome;
            if(monsterWP<=0) {
                heroWinnerPanel.SetActive(true);
                //delete monster from the board
                /*GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                GameObject g = Array.Find(monsters, m => m.GetComponent<Monster>().pos == region);
                if (g == null) Debug.Log("couldn't find monster to remove from board.");
                else {
                    g.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);
                }*/
                monstToken.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);

                //move narrator
                GameObject narr = GameObject.Find("Narrator");
                if (narr == null) narr = GameObject.Find("Narrator(Clone)");
                narr.GetPhotonView().RPC("AdvanceNarrator", RpcTarget.AllBuffered);
            }
            else {
                if (10-hours-this.hoursUsed[0] <= 0) {
                    outOfHoursPanel.SetActive(true);
                }  else {
                    roundOverPanel.SetActive(true);
                    GameObject.Find("RoundWinnerText").GetComponent<TextMeshProUGUI>().text = "Hero won the round!!";
                    GameObject.Find("WPtext").GetComponent<TextMeshProUGUI>().text = ""+(int)(wp);
                    GameObject.Find("MonsterWPtext").GetComponent<TextMeshProUGUI>().text = ""+(int)(this.monsterWP);
                    hours = (int)PhotonNetwork.LocalPlayer.CustomProperties["Hours"];
                    GameObject.Find("HoursText").GetComponent<TextMeshProUGUI>().text = ""+(int)(10-hours-this.hoursUsed[0]);
                }
            }

        }
        else if (outcome < 0) { //monster wins

            //SHIELD OPTION
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["shield"] > 0) {
                useShieldButton.gameObject.SetActive(true);
            }

            PhotonHashtable h = new PhotonHashtable();
            if (wp+outcome <= 0) {  //you lost
                h.Add("wp", 1);
                PhotonNetwork.LocalPlayer.SetCustomProperties(h);
                this.monsterWinnerPanel.SetActive(true);
            }
            else {
                if (10-hours-this.hoursUsed[0] <= 0) {
                    outOfHoursPanel.SetActive(true);
                } else {
                    roundOverPanel.SetActive(true);
                    GameObject.Find("WPtext").GetComponent<TextMeshProUGUI>().text = ""+(int)(wp+outcome);
                    GameObject.Find("MonsterWPtext").GetComponent<TextMeshProUGUI>().text = ""+(int)(this.monsterWP);
                    GameObject.Find("RoundWinnerText").GetComponent<TextMeshProUGUI>().text = "Monster won the round!!";
                    GameObject.Find("HoursText").GetComponent<TextMeshProUGUI>().text = ""+(int)(10-hours-this.hoursUsed[0]);
                    h.Add("wp", wp+outcome);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(h);
                }
        
            }
        } else { //tie
            if (10-hours-this.hoursUsed[0] <= 0) {
                    outOfHoursPanel.SetActive(true);
            }  else {
                roundOverPanel.SetActive(true);
                GameObject.Find("WPtext").GetComponent<TextMeshProUGUI>().text = ""+(int)(wp);
                GameObject.Find("MonsterWPtext").GetComponent<TextMeshProUGUI>().text = ""+(int)(this.monsterWP);
                GameObject.Find("RoundWinnerText").GetComponent<TextMeshProUGUI>().text = "It's a tie!!";
                GameObject.Find("HoursText").GetComponent<TextMeshProUGUI>().text = ""+(int)(10-hours-this.hoursUsed[0]);
            }
        }
        GameObject.Find("CurrentPlayerPic").GetComponent<SpriteRenderer>().sortingLayerName = "Fight";
    }

    private void UseShield() {
        Debug.Log("Using Shield");
        this.useShieldButton.gameObject.SetActive(false);
        // update char stats
        int oldShield = (int)PhotonNetwork.LocalPlayer.CustomProperties["shield"];
        PhotonHashtable p = new PhotonHashtable();
        p.Add("wp", oldWp);
        p.Add("shield", oldShield - 1);
        string s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        PhotonNetwork.LocalPlayer.SetCustomProperties(p);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

    }

    private void UseHelm() {
        Debug.Log("Using Helm");
        useHelmButton.gameObject.SetActive(false);
        useBrewButton.gameObject.SetActive(false);
        // update char stats
        int oldHelm = (int)PhotonNetwork.LocalPlayer.CustomProperties["helm"];
        PhotonHashtable p = new PhotonHashtable();
        p.Add("helm", oldHelm - 1);
        string s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        PhotonNetwork.LocalPlayer.SetCustomProperties(p);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
        Dice.helmUsed = true;
        
    }

    private void UseBrew() {
        if (!Dice.helmUsed) {
            Debug.Log("Using Brew");
            this.useBrewButton.gameObject.SetActive(false);
            useHelmButton.gameObject.SetActive(false);
            // update char stats
            int oldBrew = (int)PhotonNetwork.LocalPlayer.CustomProperties["brew"];
            PhotonHashtable p = new PhotonHashtable();
            p.Add("brew", oldBrew - 1);
            string s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
            PhotonNetwork.LocalPlayer.SetCustomProperties(p);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
            Dice.brewUsed = true;
        }
    }

    private void UseRunestones() {
        this.useRunestonesButton.gameObject.SetActive(false);
        int oldYellow = (int)PhotonNetwork.LocalPlayer.CustomProperties["yellowStone"];
        int oldGreen = (int)PhotonNetwork.LocalPlayer.CustomProperties["greenStone"];
        int oldBlue = (int)PhotonNetwork.LocalPlayer.CustomProperties["blueStone"];
        PhotonHashtable p = new PhotonHashtable();
        p.Add("yellowStone", oldYellow - 1);
        p.Add("greenStone", oldGreen - 1);
        p.Add("blueStone", oldBlue - 1);
        string s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        PhotonNetwork.LocalPlayer.SetCustomProperties(p);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
        Dice.runestonesUsed = true;
    }

    public void DestroyFightController() {
        Destroy(this.gameObject);
        //SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("GameScene"));
        //DontDestroyOnLoad(this.gameObject);
    }

    public void UpdateBestScore(int highestRoll) {
        this.heroBestScore = highestRoll + this.heroBattleValue;
        GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Best Score:   " + this.heroBestScore;
    }

    public void UpdateMonsterBestScore(int highestRoll) {
        this.monsterBestScore = highestRoll + this.monsterSP;
        GameObject.Find("MonsterBestScore").GetComponent<TextMeshProUGUI>().text = "Best Score:               " + this.monsterBestScore;
    }

    public void WinWillpower() {
        int wp = (int)PhotonNetwork.LocalPlayer.CustomProperties["wp"];
        Debug.Log("Old WP: " + wp);
        if (monsterType=="Gor") {
            wp += 2;
        }
        else if (monsterType=="Skral") {
            wp += 4;
        }
        else {
            wp += 6;
        }
        PhotonHashtable h = new PhotonHashtable();
        h.Add("wp", wp);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        Debug.Log("New WP: " + wp);
        LoadGame loading = new LoadGame();
        loading.LoadGameScene();
    }

    public void WinGold() {
        int gold = (int)PhotonNetwork.LocalPlayer.CustomProperties["gold"];
        Debug.Log("Old gold: " + gold);
        if (monsterType=="Gor") {
            gold += 2;
        }
        else if (monsterType=="Skral") {
            gold += 4;
        }
        else {
            gold += 6;
        }
        PhotonHashtable h = new PhotonHashtable();
        h.Add("gold", gold);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        Debug.Log("New gold: " + gold);
        LoadGame loading = new LoadGame();
        loading.LoadGameScene();
    }
}