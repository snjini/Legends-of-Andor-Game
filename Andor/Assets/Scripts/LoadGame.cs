using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using UnityEngine.UI;


public class LoadGame : MonoBehaviourPun
{
    [SerializeField]
    private int NbPlayers;
    private GameObject mainMenu;
    private bool isContinuedGame;

    void Start()
    {
        mainMenu = GameObject.Find("MainMenu");

    }

    public void SetContinuedGame() {
        this.isContinuedGame = true;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("GameScene");
        foreach (string s in (string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])
        {
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

        }
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateGameStats", RpcTarget.AllBuffered);
    }

    public void Load(string charName)
    {
        StartCoroutine(RoomMade(charName));
    }

    IEnumerator RoomMade(string charName)
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom != null);
        Debug.Log("Loading Scene");
        if (CanCreatePlayer(charName))
        {
            CreatePlayer(charName);
            PhotonHashtable h = new PhotonHashtable();
            //set number of players allowed
            if (PhotonNetwork.CurrentRoom.CustomProperties["GameSize"] == null)
            {
                h.Add("GameSize", NbPlayers);
                if(!isContinuedGame) InstantiateCanvas();
                else FastForwardGame();
            }
            //set first player to join as first player to get a turn
            if (PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"] == null)
            {
                h.Add("CurrentPlayerIndex", 0);
                InstantiateMonsters();
                InstantiateWells();
                InstantiateFarmers();
                InstantiateNarrator();
                InstantiateRunestones();
                h.Add("NarratorPosition", 0);
                h.Add("MonstersAtCastle", 0);
                h.Add("FarmersAtCastle", 0);
                h.Add("HerbAtCastle", false);
                h.Add("SkralAtCastleDefeated", true);   //change once skral is implemented
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(h);
            SceneManager.LoadScene("GameScene");
            StartCoroutine(StatsPanelSetUp());
        }

    }

    IEnumerator StatsPanelSetUp()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject stats = GameObject.Find("StatsPanel");
            Debug.Log("stats is null" + (stats == null));
            stats.GetPhotonView().RPC("DontDestroy", RpcTarget.AllBuffered);
            Debug.Log("BBBB");
        }
    }

    private bool CanCreatePlayer(string charName)
    {
        string[] p = (string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"];
        int count = p == null ? 0 : p.Length;
        Debug.Log("Max number of players: " + NbPlayers);
        Debug.Log("Current number of players: " + count);
        string[] heroes = (string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"];
        if (count < NbPlayers && (heroes == null || (!Array.Exists(heroes, element => element == charName) && !Array.Exists(heroes, element => element == charName + "(Clone)"))))
        {
            Debug.Log("can join");
            return true;
        }
        else
        {
            Debug.Log("cannot join");
            PhotonNetwork.LeaveRoom();
            // mainMenu.SetActive(true);
            return false;
        }
    }

    private void CreatePlayer(string charName)
    {
        GameObject obj;
        obj = (GameObject)PhotonNetwork.Instantiate(charName, getPosChar(charName), Quaternion.identity, 0);
        obj.name = charName;
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("GameScene"));
        PhotonView view = obj.GetPhotonView();
        view.RPC("DontDestroy", RpcTarget.AllBuffered);
        PhotonHashtable h = new PhotonHashtable();
        h.Add("Type", charName);
        h.Add("Hours", 0);
        h.Add("DayIsEnded", false);
        h.Add("sp", 1);
        h.Add("wp", 7);
        h.Add("farmer", 0);
        h.Add("gold", 0);
        h.Add("deposit",0);
        h.Add("wineskin",0);
        h.Add("shield",0);
        h.Add("falcon",0);
        h.Add("bow",0);
        h.Add("telescope",0);
        h.Add("helm",0);
        h.Add("brew", 0);
        h.Add("herb", false);
        h.Add("yellowStone",0);
        h.Add("blueStone",0);
        h.Add("greenStone",0);
        PhotonNetwork.SetPlayerCustomProperties(h);
        PhotonNetwork.SetPlayerCustomProperties(h);
        this.addPlayerToList(charName);
        Hero myHero = obj.GetComponent<Hero>();
        GameBoardRegion.setHero(myHero);

        GameObject o2 = (GameObject)PhotonNetwork.Instantiate(charName + "TimeTracker", new Vector3(-1.3f, 12.55f, 15), Quaternion.identity, 0);
        o2.name = charName + "TimeTracker";
        SceneManager.MoveGameObjectToScene(o2, SceneManager.GetSceneByName("GameScene"));
        PhotonView v2 = o2.GetPhotonView();
        v2.RPC("DontDestroy", RpcTarget.AllBuffered);
        myHero.timeTrackToken = o2.GetComponent<SpriteRenderer>();
    }
    private void addPlayerToList(string charName)
    {
        object o = PhotonNetwork.CurrentRoom.CustomProperties["Players"];
        string[] players;
        if (o == null)
        {
            players = new string[1];
            players[0] = charName;
        }
        else
        {
            players = (string[])o;
            List<string> list = new List<string>(players);
            list.Add(charName);
            players = list.ToArray();
        }
        PhotonHashtable h = new PhotonHashtable();
        h.Add("Players", players);
        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
    }

    private Vector3 getPosChar(string charName)
    {
        if (charName == "Dwarf")
        {
            return new Vector3(-15.4f, 10.4f, 15);
        }
        else if (charName == "Wizard")
        {
            return new Vector3(-13.1f, -5.3f, 15);
        }
        else if (charName == "Warrior")
        {
            return new Vector3(-10, 2.1f, 15);
        }
        else
        { //archer
            return new Vector3(-19, -6.2f, 15);
        }
    }

    public void setNbPlayers(int i)
    {
        NbPlayers = i;
    }

    public void InstantiateMonsters()
    {
        // call legend cards - need to add to dictionary when cards are flipped
        // why are we not using the getCoordinates method to get vector??
        Dictionary<int, Vector3> gorPositions = new Dictionary<int, Vector3>() {
            {8, new Vector3(-10.5f, 9.5f, 15)},
            {20, new Vector3(-18, -.25f, 15)},
            {21, new Vector3(-20, -.25f, 15)},
            {26, new Vector3(-20, -12.5f, 15)},
            {48, new Vector3(-1.5f, 5.5f, 15)},
        };

        GameObject m;
        foreach (KeyValuePair<int, Vector3> gor in gorPositions)
        {
            // check to ignore those that have been already instantiated
            // condition for new monsters from legend/ event cards can be instantiated 
            m = (GameObject)PhotonNetwork.InstantiateSceneObject("Gor", gor.Value, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(m, SceneManager.GetSceneByName("GameScene"));
            PhotonView v3 = m.GetPhotonView();
            v3.RPC("DontDestroy", RpcTarget.AllBuffered);
            v3.RPC("SetPosition", RpcTarget.AllBuffered, gor.Key);
        }
        //Skral
        m = (GameObject)PhotonNetwork.InstantiateSceneObject("Skral", new Vector3(-15.5f, -.5f, 15), Quaternion.identity);
        SceneManager.MoveGameObjectToScene(m, SceneManager.GetSceneByName("GameScene"));
        PhotonView v4 = m.GetPhotonView();
        v4.RPC("DontDestroy", RpcTarget.AllBuffered);
        v4.RPC("SetPosition", RpcTarget.AllBuffered, 19);

    }

    public void InstantiateWells()
    {
        GameObject w;
        PhotonView view;
        w = (GameObject)PhotonNetwork.InstantiateSceneObject("Well", new Vector3(-19.7f, 4.1f, 15), Quaternion.identity);
        view = w.GetPhotonView();
        view.RPC("setPosOfWells", RpcTarget.AllBuffered, (int)5);
        SceneManager.MoveGameObjectToScene(w, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);
        view.RPC("RefillWell", RpcTarget.AllBuffered);


        w = (GameObject)PhotonNetwork.InstantiateSceneObject("Well", new Vector3(-15, -6.9f, 15), Quaternion.identity);
        view = w.GetPhotonView();
        view.RPC("setPosOfWells", RpcTarget.AllBuffered, (int)35);
        SceneManager.MoveGameObjectToScene(w, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);
        view.RPC("RefillWell", RpcTarget.AllBuffered);

        w = (GameObject)PhotonNetwork.InstantiateSceneObject("Well", new Vector3(9, -0.2f, 15), Quaternion.identity);
        view = w.GetPhotonView();
        view.RPC("setPosOfWells", RpcTarget.AllBuffered, (int)45);
        SceneManager.MoveGameObjectToScene(w, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);
        view.RPC("RefillWell", RpcTarget.AllBuffered);

        w = (GameObject)PhotonNetwork.InstantiateSceneObject("Well", new Vector3(4.4f, 10.5f, 15), Quaternion.identity);
        view = w.GetPhotonView();
        view.RPC("setPosOfWells", RpcTarget.AllBuffered, (int)55);
        SceneManager.MoveGameObjectToScene(w, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);
        view.RPC("RefillWell", RpcTarget.AllBuffered);
    }

    public void InstantiateFarmers()
    {
        Dictionary<int, Vector3> farmerPositions = new Dictionary<int, Vector3>() {
            {24, new Vector3(-20, -3, 15)},
         };

        GameObject farmer;
        foreach (KeyValuePair<int, Vector3> f in farmerPositions)
        {
            // check to ignore those that have been already instantiated
            // condition for new monsters from legend/ event cards can be instantiated 
            farmer = (GameObject)PhotonNetwork.InstantiateSceneObject("FarmerToken", f.Value, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(farmer, SceneManager.GetSceneByName("GameScene"));
            PhotonView view = farmer.GetPhotonView();
            view.RPC("SetPosOfFarmer", RpcTarget.AllBuffered, f.Key);
            view.RPC("DontDestroy", RpcTarget.AllBuffered);
        }
        // GameObject farmer = (GameObject)PhotonNetwork.InstantiateSceneObject("FarmerToken", new Vector3(-20, -3, 15), Quaternion.identity);
        // PhotonView view = farmer.GetPhotonView();
        // view.RPC("SetPosOfFarmer", RpcTarget.AllBuffered, (int)24);
        // SceneManager.MoveGameObjectToScene(farmer, SceneManager.GetSceneByName("GameScene"));
        // view.RPC("DontDestroy", RpcTarget.AllBuffered);
    }



    public void InstantiateNarrator()
    {
        GameObject narr = (GameObject)PhotonNetwork.InstantiateSceneObject("Narrator", new Vector3(19.5f, -12.5f, 15), Quaternion.identity);
        PhotonView view = narr.GetPhotonView();
        view.RPC("SetPosOfNarrator", RpcTarget.AllBuffered, 0);
        SceneManager.MoveGameObjectToScene(narr, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);

        //instantiate Narrator Stars
        GameObject c = (GameObject)PhotonNetwork.InstantiateSceneObject("Star", new Vector3(19.5f, -8.7f, 15), Quaternion.identity);
        GameObject g = (GameObject)PhotonNetwork.InstantiateSceneObject("Star", new Vector3(19.5f, -0.9f, 15), Quaternion.identity);
        GameObject n = (GameObject)PhotonNetwork.InstantiateSceneObject("Star", new Vector3(19.5f, 12.6f, 15), Quaternion.identity);
        PhotonView cView = c.GetPhotonView();
        PhotonView gView = g.GetPhotonView();
        PhotonView nView = n.GetPhotonView();
        cView.RPC("SetPosOfStar", RpcTarget.AllBuffered, 2);
        SceneManager.MoveGameObjectToScene(c, SceneManager.GetSceneByName("GameScene"));
        cView.RPC("DontDestroy", RpcTarget.AllBuffered);
        gView.RPC("SetPosOfStar", RpcTarget.AllBuffered, 6);
        SceneManager.MoveGameObjectToScene(g, SceneManager.GetSceneByName("GameScene"));
        gView.RPC("DontDestroy", RpcTarget.AllBuffered);
        nView.RPC("SetPosOfStar", RpcTarget.AllBuffered, 13);
        SceneManager.MoveGameObjectToScene(n, SceneManager.GetSceneByName("GameScene"));
        nView.RPC("DontDestroy", RpcTarget.AllBuffered);

    }

    public void InstantiateCanvas()
    {
        GameObject n = (GameObject)PhotonNetwork.InstantiateSceneObject("CollectiveDecisionCanvas", new Vector3(0, 0, 15), Quaternion.identity);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Button ok = GameObject.Find("OKGoldButton").GetComponent<Button>();
            ok.onClick.AddListener(() => CollectiveDecisionController.OKButtonClicked());
            Button dwarfYes = GameObject.Find("AddGoldDwarfButton").GetComponent<Button>();
            dwarfYes.onClick.AddListener(() => CollectiveDecisionController.UpdateDwarfCount(1));
            Button dwarfNo = GameObject.Find("SubtractGoldDwarfButton").GetComponent<Button>();
            dwarfNo.onClick.AddListener(() => CollectiveDecisionController.UpdateDwarfCount(-1));
            Button wizardYes = GameObject.Find("AddGoldWizardButton").GetComponent<Button>();
            wizardYes.onClick.AddListener(() => CollectiveDecisionController.UpdateWizardCount(1));
            Button wizardNo = GameObject.Find("SubtractGoldWizardButton").GetComponent<Button>();
            wizardNo.onClick.AddListener(() => CollectiveDecisionController.UpdateWizardCount(-1));
            Button warriorYes = GameObject.Find("AddGoldWarriorButton").GetComponent<Button>();
            warriorYes.onClick.AddListener(() => CollectiveDecisionController.UpdateWarriorCount(1));
            Button warriorNo = GameObject.Find("SubtractGoldWarriorButton").GetComponent<Button>();
            warriorNo.onClick.AddListener(() => CollectiveDecisionController.UpdateWarriorCount(-1));
            Button archerYes = GameObject.Find("AddGoldArcherButton").GetComponent<Button>();
            archerYes.onClick.AddListener(() => CollectiveDecisionController.UpdateArcherCount(1));
            Button archerNo = GameObject.Find("SubtractGoldArcherButton").GetComponent<Button>();
            archerNo.onClick.AddListener(() => CollectiveDecisionController.UpdateArcherCount(-1));
        }
        PhotonView view = n.GetPhotonView();
        SceneManager.MoveGameObjectToScene(n, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);
    }

    public void InstantiateRunestones() {
        Vector3[] yellowRuneStoneCoords= new Vector3[2]; 
        int[] yellowRuneStonePos = new int[2];
        Vector3[] blueRuneStoneCoords = new Vector3[2]; 
        int[] blueRuneStonePos= new int[2];
        Vector3[] greenRuneStoneCoords= new Vector3[2]; 
        int[] greenRuneStonePos=  new int[2];
        Debug.Log("AAAA");
        for (int i=0; i<2; i++ ){
            int randomNum = UnityEngine.Random.Range(0, 6);
            int randomNum2 = UnityEngine.Random.Range(0, 6);
            int pos = randomNum*10 + randomNum2;
            Debug.Log(pos);
            Vector3 coords = GameBoardRegion.regionCoords[pos];
            yellowRuneStoneCoords[i] = coords;
            yellowRuneStonePos[i] = pos;

            randomNum = UnityEngine.Random.Range(0, 6);
            randomNum2 = UnityEngine.Random.Range(0, 6);
            pos = randomNum*10 + randomNum2;
            Vector3 coords2 = GameBoardRegion.regionCoords[pos];
            blueRuneStoneCoords[i] = coords2; 
            blueRuneStonePos[i] = pos;

            randomNum = UnityEngine.Random.Range(0, 6);
            randomNum2 = UnityEngine.Random.Range(0, 6);
            pos = randomNum*10 + randomNum2;
            Vector3 coords3 = GameBoardRegion.regionCoords[pos];
            greenRuneStoneCoords[i] = coords3; 
            greenRuneStonePos[i] = pos;
        }
        GameObject runestoneYellow1 = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneYellow", yellowRuneStoneCoords[0], Quaternion.identity);
        GameObject runestoneYellow2 = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneYellow", yellowRuneStoneCoords[1], Quaternion.identity);
        GameObject runestoneBlue1 = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneBlue", blueRuneStoneCoords[0], Quaternion.identity);
        GameObject runestoneBlue2 = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneBlue", blueRuneStoneCoords[1], Quaternion.identity);
        GameObject runestoneGreen1 = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneGreen", greenRuneStoneCoords[0], Quaternion.identity);
        GameObject runestoneGreen2 = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneGreen", greenRuneStoneCoords[1], Quaternion.identity);

        PhotonView view = runestoneYellow1.GetPhotonView();
        view.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, yellowRuneStonePos[0]);
        SceneManager.MoveGameObjectToScene(runestoneYellow1, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);

        view = runestoneYellow2.GetPhotonView();
        view.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, yellowRuneStonePos[1]);
        SceneManager.MoveGameObjectToScene(runestoneYellow2, SceneManager.GetSceneByName("GameScene"));
        view.RPC("DontDestroy", RpcTarget.AllBuffered);

        PhotonView view2 = runestoneBlue1.GetPhotonView();
        view2.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, blueRuneStonePos[0]);
        SceneManager.MoveGameObjectToScene(runestoneBlue1, SceneManager.GetSceneByName("GameScene"));
        view2.RPC("DontDestroy", RpcTarget.AllBuffered);

        view2 = runestoneBlue2.GetPhotonView();
        view2.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, blueRuneStonePos[1]);
        SceneManager.MoveGameObjectToScene(runestoneBlue2, SceneManager.GetSceneByName("GameScene"));
        view2.RPC("DontDestroy", RpcTarget.AllBuffered);

        PhotonView view3 = runestoneGreen1.GetPhotonView();
        view3.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, greenRuneStonePos[0]);
        SceneManager.MoveGameObjectToScene(runestoneGreen1, SceneManager.GetSceneByName("GameScene"));
        view3.RPC("DontDestroy", RpcTarget.AllBuffered);

        view3 = runestoneGreen2.GetPhotonView();
        view3.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, greenRuneStonePos[1]);
        SceneManager.MoveGameObjectToScene(runestoneGreen2, SceneManager.GetSceneByName("GameScene"));
        view3.RPC("DontDestroy", RpcTarget.AllBuffered);

    }

    public void FastForwardGame() {
        Debug.Log("fast-forwarding...");
    }
}
