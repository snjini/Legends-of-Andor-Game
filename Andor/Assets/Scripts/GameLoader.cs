using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class GameLoader : MonoBehaviour {

    private string gameName;
    private string[] data;

    public void TriggerGameLoad() {
        Dropdown d = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        string s = d.options[d.value].text;
        this.gameName = s;       
    }

    public void LoadGame(string charName) {
        Debug.Log("loading in saved game...");
        string path = System.IO.Directory.GetCurrentDirectory();
        data = System.IO.File.ReadAllLines(path  +gameName+".txt");
        StartCoroutine(RoomMade(charName));
    }

    IEnumerator RoomMade(string charName)
    {
        yield return new WaitUntil(() => PhotonNetwork.CurrentRoom != null);
        Debug.Log("Loading Scene");
        CreatePlayer(charName);
        PhotonHashtable h = new PhotonHashtable();
        //set number of players allowed
        if (PhotonNetwork.CurrentRoom.CustomProperties["GameSize"] == null)
        {
            h.Add("GameSize", 3);
        }
        //set first player to join as first player to get a turn
        if (PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"] == null)
        {
            h.Add("CurrentPlayerIndex", int.Parse(data[0]));
            InstantiateMonsters();
            InstantiateWells();
            InstantiateFarmers();
            InstantiateNarrator();
            InstantiateRunestones();
            InstantiateGold();
            InstantiateBrew();
            InstantiateHerb();
            h.Add("NarratorPosition", int.Parse(data[1]));
            h.Add("MonstersAtCastle", int.Parse(data[2]));
            h.Add("FarmersAtCastle", int.Parse(data[3]));
            h.Add("HerbAtCastle", false);
            h.Add("SkralAtCastleDefeated", true);   //change once skral is implemented
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        SceneManager.LoadScene("GameScene");
        StartCoroutine(StatsPanelSetUp());
    }

    IEnumerator StatsPanelSetUp()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "GameScene");
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject stats = GameObject.Find("StatsPanel");
            Debug.Log("stats is null" + (stats == null));
            stats.GetPhotonView().RPC("DontDestroy", RpcTarget.AllBuffered);
        }
    }

    private void CreatePlayer(string charName)
    {
        string str = "";
        foreach(string s in data) {
            if(s.StartsWith(charName)) {
                str = s;
                break;
            }
        }
        char[] separator = {'\t'};
        string[] arr = str.Split(separator);
        GameObject obj;
        Vector3 coords = GameBoardRegion.regionCoords[int.Parse(arr[1])];
        obj = (GameObject)PhotonNetwork.Instantiate(charName, coords, Quaternion.identity, 0);
        obj.name = charName;
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("GameScene"));
        PhotonView view = obj.GetPhotonView();
        view.RPC("DontDestroy", RpcTarget.AllBuffered);
        PhotonHashtable h = new PhotonHashtable();
        h.Add("Type", charName);
        h.Add("pos", int.Parse(arr[1]));
        h.Add("Hours", int.Parse(arr[2]));
        h.Add("DayIsEnded", false);
        h.Add("sp", int.Parse(arr[4]));
        h.Add("wp", int.Parse(arr[3]));
        h.Add("farmer", int.Parse(arr[5]));
        h.Add("gold", int.Parse(arr[6]));
        h.Add("deposit",0);
        h.Add("wineskin", int.Parse(arr[8]));
        h.Add("shield",int.Parse(arr[9]));
        h.Add("falcon",int.Parse(arr[10]));
        h.Add("bow",int.Parse(arr[12]));
        h.Add("telescope",int.Parse(arr[11]));
        h.Add("helm",int.Parse(arr[13]));
        h.Add("brew", int.Parse(arr[14]));
        h.Add("herb", bool.Parse(arr[7]));
        h.Add("yellowStone",int.Parse(arr[15]));
        h.Add("blueStone",int.Parse(arr[16]));
        h.Add("greenStone",int.Parse(arr[17]));
        PhotonNetwork.SetPlayerCustomProperties(h);
        this.addPlayerToList(charName);
        Hero myHero = obj.GetComponent<Hero>();
        GameBoardRegion.setHero(myHero);

        float shift = 0;
        int hours = int.Parse(arr[2]);
        if (hours > 7) {  //account for uneven space between hour 7 and 8
            shift += 0.55f;
        }
        shift += 1.75f * hours;
        GameObject o2 = (GameObject)PhotonNetwork.Instantiate(charName + "TimeTracker", new Vector3(-1.3f+shift, 12.55f, 15), Quaternion.identity, 0);
        o2.name = charName + "TimeTracker";
        SceneManager.MoveGameObjectToScene(o2, SceneManager.GetSceneByName("GameScene"));
        PhotonView v2 = o2.GetPhotonView();
        v2.RPC("DontDestroy", RpcTarget.AllBuffered);
        myHero.timeTrackToken = o2.GetComponent<SpriteRenderer>();
        int hrs = (int)PhotonNetwork.LocalPlayer.CustomProperties["Hours"];
        Debug.Log("newHours: " + hrs);
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
    
    public void InstantiateMonsters()
    {
        foreach(string s in data) {
            if(s.StartsWith("Gor")) {
                char[] separator = {'\t'}; 
                int pos = int.Parse((s.Split(separator))[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                GameObject mon = (GameObject)PhotonNetwork.InstantiateSceneObject("Gor", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(mon, SceneManager.GetSceneByName("GameScene"));
                PhotonView v3 = mon.GetPhotonView();
                v3.RPC("DontDestroy", RpcTarget.AllBuffered);
                v3.RPC("SetPosition", RpcTarget.AllBuffered, pos);
            }
        }

        foreach(string s in data) {
            if(s.StartsWith("Skral")) {
                char[] separator = {'\t'}; 
                int pos = int.Parse((s.Split(separator))[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                GameObject mon = (GameObject)PhotonNetwork.InstantiateSceneObject("Skral", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(mon, SceneManager.GetSceneByName("GameScene"));
                PhotonView v3 = mon.GetPhotonView();
                v3.RPC("DontDestroy", RpcTarget.AllBuffered);
                v3.RPC("SetPosition", RpcTarget.AllBuffered, pos);
            }
        }

        foreach(string s in data) {
            if(s.StartsWith("Wardrak")) {
                char[] separator = {'\t'}; 
                int pos = int.Parse((s.Split(separator))[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                GameObject mon = (GameObject)PhotonNetwork.InstantiateSceneObject("Wardrak", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(mon, SceneManager.GetSceneByName("GameScene"));
                PhotonView v3 = mon.GetPhotonView();
                v3.RPC("DontDestroy", RpcTarget.AllBuffered);
                v3.RPC("SetPosition", RpcTarget.AllBuffered, pos);
            }
        }
    }

    public void InstantiateWells()
    {
        GameObject w;
        PhotonView view;
        foreach(string s in data) {
            if(s.StartsWith("well")) {
                Debug.Log("found well");
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                w = (GameObject)PhotonNetwork.InstantiateSceneObject("Well", vect, Quaternion.identity);
                view = w.GetPhotonView();
                view.RPC("setPosOfWells", RpcTarget.AllBuffered, pos);
                bool b = bool.Parse(strings[2]);
                SceneManager.MoveGameObjectToScene(w, SceneManager.GetSceneByName("GameScene"));
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
                if(!b) {
                    view.RPC("SetWellEmpty", RpcTarget.AllBuffered);
                }
                else {
                    view.RPC("SetWellFull", RpcTarget.AllBuffered);
                }
            }
        }
    }

    public void InstantiateFarmers()
    {
        GameObject f;
        PhotonView view;
        foreach(string s in data) {
            if(s.StartsWith("farmer")) {
                Debug.Log("found farmer");
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                f = (GameObject)PhotonNetwork.InstantiateSceneObject("FarmerToken", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(f, SceneManager.GetSceneByName("GameScene"));
                view = f.GetPhotonView();
                view.RPC("SetPosOfFarmer", RpcTarget.AllBuffered, pos);
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
            }
        }
    }



    public void InstantiateNarrator()
    {
        int narratorPos = int.Parse(data[1]);
        Debug.Log("narrator pos: " + narratorPos );
        GameObject narr = (GameObject)PhotonNetwork.InstantiateSceneObject("Narrator", new Vector3(19.5f, -12.5f+1.93f*narratorPos, 15), Quaternion.identity);
        PhotonView view = narr.GetPhotonView();
        view.RPC("SetNarratorSimple", RpcTarget.AllBuffered, narratorPos);
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

    public void InstantiateRunestones() {

        GameObject r;
        PhotonView view;
        foreach(string s in data) {
            if(s.StartsWith("yellow")) {
                Debug.Log("found yellow stone");
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                r = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneYellow", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(r, SceneManager.GetSceneByName("GameScene"));
                view = r.GetPhotonView();
                view.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, pos);
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
            }
        }
        foreach(string s in data) {
            if(s.StartsWith("green")) {
                Debug.Log("found yellow stone");
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                r = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneGreen", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(r, SceneManager.GetSceneByName("GameScene"));
                view = r.GetPhotonView();
                view.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, pos);
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
            }
        }
        foreach(string s in data) {
            if(s.StartsWith("blue")) {
                Debug.Log("found blue stone");
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                r = (GameObject)PhotonNetwork.InstantiateSceneObject("RuneStoneBlue", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(r, SceneManager.GetSceneByName("GameScene"));
                view = r.GetPhotonView();
                view.RPC("SetPosOfRuneStone", RpcTarget.AllBuffered, pos);
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
            }
        }
    }
    public void InstantiateGold() {
        GameObject gold;
        PhotonView view;
        foreach(string s in data) {
            if(s.StartsWith("gold")) {
                Debug.Log("found gold");
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                gold = (GameObject)PhotonNetwork.InstantiateSceneObject("Gold", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(gold, SceneManager.GetSceneByName("GameScene"));
                view = gold.GetPhotonView();
                view.RPC("SetPosOfGoldToken", RpcTarget.AllBuffered, pos);
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
            }
        }
    }

    public void InstantiateBrew() {
        GameObject brew;
        PhotonView view;
        foreach(string s in data) {
            if(s.StartsWith("witch")) {
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                brew = (GameObject)PhotonNetwork.InstantiateSceneObject("WitchBrew", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(brew, SceneManager.GetSceneByName("GameScene"));
                view = brew.GetPhotonView();
                view.RPC("SetPosOfBrew", RpcTarget.AllBuffered, pos);
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
            }
        }
    }

    public void InstantiateHerb() {
        GameObject herb;
        PhotonView view;
        foreach(string s in data) {
            if(s.StartsWith("herb")) {
                char[] separator = {'\t'}; 
                string[] strings = s.Split(separator);
                int pos = int.Parse(strings[1]);
                Vector3 vect = GameBoardRegion.regionCoords[pos];
                herb = (GameObject)PhotonNetwork.InstantiateSceneObject("Herb", vect, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(herb, SceneManager.GetSceneByName("GameScene"));
                view = herb.GetPhotonView();
                view.RPC("SetPosOfHerb", RpcTarget.AllBuffered, pos);
                view.RPC("DontDestroy", RpcTarget.AllBuffered);
            }
        }
    }
}