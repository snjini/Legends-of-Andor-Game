using UnityEngine;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class Hero : MonoBehaviourPun {
    public int pos;
    public string type;
    public SpriteRenderer token;

    public SpriteRenderer timeTrackToken;

    void Start() {

        if (gameObject.name.Contains("(Clone)")) {
            gameObject.name = gameObject.name.Replace("(Clone)", "");
        }
        //Debug.Log(gameObject.name);
        string name = gameObject.name;
        PhotonHashtable h = new PhotonHashtable();
        if (name=="Dwarf") {
            h.Add("pos", 7);
            pos = 7;
            type = "Dwarf";
            h.Add("Type", "Dwarf");
        }
        else if (name=="Warrior") {
            h.Add("pos", 14);
            pos = 14;
            h.Add("Type", "Warrior");
            type = "Warrior";
        }
        else if (name=="Archer") {
            h.Add("pos", 25);
            pos = 25;
            h.Add("Type", "Archer");
            type = "Archer";
        } 
        else if (name=="Wizard") {
            h.Add("pos", 34);
            pos = 34;
            h.Add("Type", "Wizard");
            type = "Wizard";
        }
        token = GetComponent<SpriteRenderer>();
    }

    //check if hero is permitted to move
    public bool canMove(int newRegion) {
        //check if it is hero's turn
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        string[] arr = (string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"];
        string s = arr[i];
        //if not, don't move
        if (this.gameObject.name != s) {
            Debug.Log("Not your turn!");
            return false;
        }
        //check if hero still has hours left in the day
        int hours = (int)PhotonNetwork.LocalPlayer.CustomProperties["Hours"];
        int dist = Graph.BFS(pos, newRegion);
        if (hours + dist > 7 && hours + dist <= 10) {
            PhotonHashtable h = new PhotonHashtable();
            int willpower = (int)PhotonNetwork.LocalPlayer.CustomProperties["wp"];
            if(willpower - 2 < 0) {
                Debug.Log("not enough willpower");
                return false;
            }
            else {
                h.Add("wp", willpower-(2*(10-hours-1)));
                PhotonNetwork.LocalPlayer.SetCustomProperties(h);
                return true;
            }
        }

        if (hours+dist>10) {
            Debug.Log("Too far, not enough hours");
            return false;
        }
        return true;
    }
    
    public void endTurn() {
        if (EndOfDayController.IsEndOfDay()) {
            EndOfDayController.ExecuteEndOfDay();
        }
        else {
            int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
            string[] arr = (string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"];
            PhotonHashtable h = new PhotonHashtable();
            bool b = false;
            while(!b) {
                string s = arr[(i+1) % arr.Length];
                GameObject nextHero = GameObject.Find(s);
                if (nextHero == null) nextHero = GameObject.Find(s+"(Clone)");
                bool dayIsEnded = (bool)nextHero.GetPhotonView().Owner.CustomProperties["DayIsEnded"];
                if (dayIsEnded) {
                    i++;
                    continue;
                } else {
                    h.Add("CurrentPlayerIndex", (i+1) % arr.Length);
                    b = true;
                    GameObject.Find("TurnPopup").GetPhotonView().RPC("Enable", PhotonNetwork.PlayerList[(i+1) % arr.Length]);                
                    GameObject.Find("TurnPopup").GetPhotonView().RPC("Disable", PhotonNetwork.LocalPlayer);                

                }
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        }
    }

    public void updatePos(Vector3 newPos) {
        token.transform.position = newPos;
    }

    public int getPos()
    {
        return this.pos;
    }

    public void updateRegion(int newRegion) {
        int dist = Graph.BFS(pos, newRegion);
        //change pos for other clients
        PhotonView view = this.gameObject.GetPhotonView();
        view.RPC("UpdatePosVar", RpcTarget.AllBuffered, (int)newRegion);
        UpdateTimeTrack(dist);
    }

    [PunRPC]
    public void UpdatePosVar(int newRegion) {
        this.pos = newRegion;
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        PhotonHashtable h = new PhotonHashtable();
        h.Add("pos", newRegion);
        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
    }
    public void UpdateTimeTrack(int dist) {
        Debug.Log("updating timetrack");
        int hours = (int)PhotonNetwork.LocalPlayer.CustomProperties["Hours"];
        //move time track token
        Vector3 shift = new Vector3(timeTrackToken.transform.position.x, timeTrackToken.transform.position.y, timeTrackToken.transform.position.z);
        if (hours <= 7 && hours + dist > 7) {  //account for uneven space between hour 7 and 8
            shift.x += 0.55f;
        }
        shift.x += 1.75f * dist;
        timeTrackToken.transform.position = shift;
        PhotonHashtable h = new PhotonHashtable();
        h.Add("Hours", hours + dist);
        PhotonNetwork.SetPlayerCustomProperties(h);
        Debug.Log("Hours: "+hours);
    }

    public void PickupFarmer() {
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        //int index = i-1;
        //if (index < 0) {
        //    index = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"] -1;
        //}
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        
        int currFarmers = (int)PhotonNetwork.PlayerList[i].CustomProperties["farmer"];
        PhotonHashtable h = new PhotonHashtable();
        h.Add("farmer", currFarmers+1);
        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
        Debug.Log("in pickup farmer");
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

    }

    public void PickupGold(int amount){
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        //int index = i-1;
        //if (index < 0) {
        //    index = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"] -1;
        //}
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        
        int gold = (int)PhotonNetwork.PlayerList[i].CustomProperties["gold"];
        int newGoldAmount = gold+amount;
        PhotonHashtable p = new PhotonHashtable();
        p.Add("gold", newGoldAmount);
        PhotonNetwork.PlayerList[i].SetCustomProperties(p);
        Debug.Log(newGoldAmount);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

    }

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject);
    }

    [PunRPC]
    public void AddFarmerToCastle() {
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        //int index = i-1;
        //if (index < 0) {
        //    index = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"] -1;
        //}
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        //FIX FOR MULTIPLE FARMERS - just no UI or a number?
        if (PhotonNetwork.IsMasterClient) {
            GameObject farmer = (GameObject)PhotonNetwork.InstantiateSceneObject("FarmerToken", new Vector3(-17.5f, 9, 15), Quaternion.identity);
            PhotonView view = farmer.GetPhotonView();
            view.RPC("SetPosOfFarmer", RpcTarget.AllBuffered, (int)0);
            view.RPC("DontDestroy", RpcTarget.AllBuffered);
        }
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateGameStats", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void DropGoldToken(int location) {
        if (PhotonNetwork.IsMasterClient) {
            GameObject gold = (GameObject)PhotonNetwork.InstantiateSceneObject("Gold", GameBoardRegion.getCoordinates(location), Quaternion.identity);
            PhotonView view = gold.GetPhotonView();
            view.RPC("SetPosOfGoldToken", RpcTarget.AllBuffered, (int)location);
            view.RPC("DontDestroy", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void PlaceHerbToken(int location) {
        if (PhotonNetwork.IsMasterClient) {
            GameObject herb = (GameObject)PhotonNetwork.InstantiateSceneObject("Herb", GameBoardRegion.getCoordinates(location), Quaternion.identity);
            PhotonView view = herb.GetPhotonView();
            view.RPC("SetPosOfHerb", RpcTarget.AllBuffered, (int)location);
            view.RPC("DontDestroy", RpcTarget.AllBuffered);
        }
    }


    [PunRPC]
    public void PlaceWitchToken(int location) {
        if (PhotonNetwork.IsMasterClient) {
            GameObject brew = (GameObject)PhotonNetwork.InstantiateSceneObject("WitchBrew", GameBoardRegion.getCoordinates(location), Quaternion.identity);
            PhotonView view = brew.GetPhotonView();
            view.RPC("SetPosOfBrew", RpcTarget.AllBuffered, (int)location);
            view.RPC("DontDestroy", RpcTarget.AllBuffered);
        }
    }


}

