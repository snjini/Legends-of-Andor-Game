using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
public class Wells : MonoBehaviourPun
{
    public int pos;
    public bool wellStatus;

    public GameObject wellFull;

    public GameObject wellEmpty;
    // Start is called before the first frame update
    void Start()
    {
        //this.wellStatus = true;
       // wellEmpty.gameObject.SetActive(false);
    }

    [PunRPC]
    public void SetWellEmpty(){
        this.wellStatus = false;
        this.wellFull.gameObject.SetActive(false);
        this.wellEmpty.gameObject.SetActive(true);
    }
    [PunRPC]
    public void SetWellFull(){
        this.wellStatus = true;
        this.wellFull.gameObject.SetActive(true);
        this.wellEmpty.gameObject.SetActive(false);
    }

    [PunRPC]
    public void DrinkWell(int i)
    {
        // drink from the wells only if it's full 
        if (this.wellStatus)
        {
            //int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
            int index = i - 1;
            if (index < 0)
            {
                index = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"] - 1;
            }
            string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[index];
            Debug.Log(s + "  " + index);
            // get current player index
            int currPlayerWP = (int)PhotonNetwork.PlayerList[index].CustomProperties["wp"];
            int newWP;
            if (s.Equals("Warrior"))
            {
                newWP = currPlayerWP + 5;
            }
            else
            {
                newWP = currPlayerWP + 3;
            }
            PhotonHashtable p = new PhotonHashtable();
            p.Add("wp", newWP);
            // update properties of the current player
            PhotonNetwork.PlayerList[index].SetCustomProperties(p);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
            this.wellStatus = false;
            this.wellFull.gameObject.SetActive(false);
            this.wellEmpty.gameObject.SetActive(true);
        }
    }
    [PunRPC]
    public void RefillWell()
    {
        this.wellStatus = true;
        this.wellFull.gameObject.SetActive(true);
        this.wellEmpty.gameObject.SetActive(false);
    }
    [PunRPC]
    public void setPosOfWells(int position)
    {
        Debug.Log(position);
        this.pos = position;
    }
    [PunRPC]
    public void DontDestroy()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    [PunRPC]
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}