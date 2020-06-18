using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Cards : MonoBehaviourPun
{
    // Start is called before the first frame update
    
    void Start()
    {
        // hashtable for UI
        // PhotonHashtable LegendCard = new PhotonHashtable();
        // LegendCard.Add("C1", "LegendCardC1");
        // LegendCard.Add("C2", "LegendCardC2");
        // LegendCard.Add("G", "LegendCardG");
        // LegendCard.Add("N", "LegendCardN");
        // LegendCard.Add("Runestone", "LegendCardRunestone");
        // PhotonNetwork.SetCustomProperties
        PhotonHashtable EventCard = new PhotonHashtable();
        PhotonHashtable GoldenEventCard = new PhotonHashtable();
    }

    // C1 & C2 get triggered together? Legend Track only has C

    [PunRPC]
    public void EventCard22()
    {
        //the well on space 45 is removed from the game
        //get all the wells
        List<GameObject> wells = new List<GameObject>(GameObject.FindGameObjectsWithTag("Well"));
        //loop thru them wells to check for the one w position 45
        for (int i = 0; i < wells.Count; i++) {
            GameObject myWell = wells[i];
            if (myWell.GetComponent<Wells>().pos == 45) { //how to properly call the well position attribute??
                 Debug.Log("Found well");
                 myWell.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);
            }
        }

        Debug.Log("event card 22 is called");
    }

    [PunRPC]
    public void EventCard15()
    {
        //the well on space 35 is removed from the game
        //get all the wells
        List<GameObject> wells = new List<GameObject>(GameObject.FindGameObjectsWithTag("Well"));
        //loop thru them wells to check for the one w position 35
        for (int i = 0; i < wells.Count; i++) {
            GameObject myWell = wells[i];
            if (myWell.GetComponent<Wells>().pos == 35) {
                myWell.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);
            }
        }
        Debug.Log("event card 15 is called");
    }

    [PunRPC]
    public void EventCardNumUnknown()
    {
        //the wizard and archer each immediately get 3 wp points
        int i = Array.IndexOf((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"], "Wizard");
        if (i!=-1) {
            int currPlayerWP = (int)PhotonNetwork.PlayerList[i].CustomProperties["wp"];
            int newWP = currPlayerWP + 3;
            PhotonHashtable p = new PhotonHashtable();
            p.Add("wp", newWP);
            // update properties of the current player
            PhotonNetwork.PlayerList[i].SetCustomProperties(p);
            string s = (string) PhotonNetwork.PlayerList[i].CustomProperties["Type"];
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
            
        }

        int j = Array.IndexOf((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"], "Archer");
        if (j!=-1) {
            int currPlayerWP2 = (int)PhotonNetwork.PlayerList[j].CustomProperties["wp"];
            int newWP2 = currPlayerWP2 + 3;
            PhotonHashtable p2 = new PhotonHashtable();
            p2.Add("wp", newWP2);
            // update properties of the current player
            PhotonNetwork.PlayerList[j].SetCustomProperties(p2);
            string s2 = (string) PhotonNetwork.PlayerList[j].CustomProperties["Type"];
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s2);
            Debug.Log("event card unknown is called");
        }
    }

    [PunRPC]
    public void EventCard14()
    {
        //the dwarf and the warrior each immediately get 3 wp points

        int i = Array.IndexOf((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"], "Dwarf");
        if (i!=-1) {
            int currPlayerWP = (int)PhotonNetwork.PlayerList[i].CustomProperties["wp"];
            int newWP = currPlayerWP + 3;
            PhotonHashtable p = new PhotonHashtable();
            p.Add("wp", newWP);
            // update properties of the current player
            PhotonNetwork.PlayerList[i].SetCustomProperties(p);
            string s = (string) PhotonNetwork.PlayerList[i].CustomProperties["Type"];
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
        }   
        int j = Array.IndexOf((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"], "Warrior");
        if (j!=-1) {
    
            int currPlayerWP2 = (int)PhotonNetwork.PlayerList[j].CustomProperties["wp"];
            int newWP2 = currPlayerWP2 + 3;
            PhotonHashtable p2 = new PhotonHashtable();
            p2.Add("wp", newWP2);
            // update properties of the current player
            PhotonNetwork.PlayerList[j].SetCustomProperties(p2);
            string s2 = (string) PhotonNetwork.PlayerList[j].CustomProperties["Type"];
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s2);
            Debug.Log("event card 14 is called");
        }
    }

    [PunRPC]
    public void EventCard17()
    {
        //each hero with more than 12 wp points reduces his point total to 12
        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
        //for (int i = 0; i < ((string[])PhotonNetwork.PlayerList).Length; i++) {
            // if hero has more than 12 wp
            if ((int)PhotonNetwork.PlayerList[i].CustomProperties["wp"] > 12) {
                PhotonHashtable p = new PhotonHashtable();
                p.Add("wp", 12);
                string s = (string) PhotonNetwork.PlayerList[i].CustomProperties["Type"];
                // update properties of the current player
                PhotonNetwork.PlayerList[i].SetCustomProperties(p);
                StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
            }
        }
        Debug.Log("event card 17 is called");
    }

    [PunRPC]
    public void DontDestroy()
    {
        // DontDestroyOnLoad(this.gameObject);
    }
    [PunRPC]
    public void DisplayCard()
    {
        StartCoroutine(Display());
    }

    private IEnumerator Display()
    {
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
}