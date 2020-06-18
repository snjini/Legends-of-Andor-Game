using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;


public class Narrator : MonoBehaviourPun
{
    private int location;
    private int runestoneCardPos;

    [PunRPC]
    public void DontDestroy()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [PunRPC]
    public void SetNarratorSimple(int position)
    {
        this.location = position;
    }

    [PunRPC]
    public void SetPosOfNarrator(int position)
    {
        //0 = A, 13 = N
        this.location = position;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonHashtable h = new PhotonHashtable();
            h.Add("NarratorPosition", position);
            PhotonNetwork.CurrentRoom.SetCustomProperties(h);
            // || position == runestoneCardPos
            if (position == 2 || position == 6 || position == 13)
            {

                GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
                GameObject foundStar = Array.Find(stars, f => f.GetComponent<StarController>().location == position);
                if (foundStar != null)
                {
                    PhotonView starView = foundStar.GetPhotonView();
                    starView.RPC("Destroy", RpcTarget.AllBuffered);
                }

                //Flip legend cards depending on narrator position

                // Legend card C
                if (position == 2)
                {
                    this.gameObject.GetPhotonView().RPC("LegendCardC", RpcTarget.MasterClient);
                    Debug.Log("Legend Card C flipped");
                }
                // Legend card G
                if (position == 6)
                {
                        
                    this.gameObject.GetPhotonView().RPC("LegendCardG", RpcTarget.MasterClient);
                    Debug.Log("Legend Card G flipped");
                }
                
                // Legend card Runestone
                if (position == 1)
                {
                    //Cards.LegendCardRunestone();
                    //Debug.Log("Runestone Legend Card flipped");
                }

                // Debug.Log("legend card flipped");
                //FINISH
            }
        }
    }

    [PunRPC]
    public void AdvanceNarrator()
    {
        Debug.Log("Narrator old pos:" + this.location);
        this.SetPosOfNarrator(this.location + 1);
        Debug.Log("Narrator new pos:" + this.location);
        Vector3 shift = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        shift.y += 1.93f;
        this.gameObject.transform.position = shift;

    }

    [PunRPC]
    public void LegendCardC()
    {

        // hero rolls dice then add 50 to the number rolled & add skral on tower
        // 6 wp + [# heros X 10 - 10] sp
        // farmer on space 28
        Vector3 farmer28Coords = GameBoardRegion.getCoordinates(28);
        GameObject farmer = (GameObject)PhotonNetwork.InstantiateSceneObject("FarmerToken", farmer28Coords, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(farmer, SceneManager.GetSceneByName("GameScene"));
        PhotonView view = farmer.GetPhotonView();
        view.RPC("SetPosOfFarmer", RpcTarget.AllBuffered, 28);
        view.RPC("DontDestroy", RpcTarget.AllBuffered);

        // add gros on space 27& 31
        Vector3 gor27Coords = GameBoardRegion.getCoordinates(27);
        Vector3 gor31Coords = GameBoardRegion.getCoordinates(31);
        Dictionary<int, Vector3> gorPositions = new Dictionary<int, Vector3>() {
            {27, gor27Coords},
            {31, gor31Coords}
        };

        GameObject m;
        foreach (KeyValuePair<int, Vector3> gor in gorPositions)
        {
            m = (GameObject)PhotonNetwork.InstantiateSceneObject("Gor", gor.Value, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(m, SceneManager.GetSceneByName("GameScene"));
            PhotonView v3 = m.GetPhotonView();
            v3.RPC("DontDestroy", RpcTarget.AllBuffered);
            v3.RPC("SetPosition", RpcTarget.AllBuffered, gor.Key);
        }

        // InstantiateMonsters();

        // add skrals on space 29 --> already there??? 
        Vector3 skrallVector = GameBoardRegion.getCoordinates(29);
        m = (GameObject)PhotonNetwork.InstantiateSceneObject("Skral", skrallVector, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(m, SceneManager.GetSceneByName("GameScene"));
        PhotonView v4 = m.GetPhotonView();
        v4.RPC("DontDestroy", RpcTarget.AllBuffered);
        v4.RPC("SetPosition", RpcTarget.AllBuffered, 29);
        // add prince on space 27 (prince is +4 sp and can move up to 4 spaces in an hour)
        Vector3 princeVector = GameBoardRegion.getCoordinates(27);
        // MAKE SURE THAT THE NAME OF THE TOKEN IS CORRECT IN RESOURCE 
        m = (GameObject)PhotonNetwork.InstantiateSceneObject("Prince", princeVector, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(m, SceneManager.GetSceneByName("GameScene"));
        v4 = m.GetPhotonView();
        v4.RPC("DontDestroy", RpcTarget.AllBuffered);
        v4.RPC("SetPosOfPrince", RpcTarget.AllBuffered, 27);
    }
    // [PunRPC]
    // public void LegendCardC2()
    // {
    // }
    [PunRPC]
    public void LegendCardG()
    {
        // Prince Thorald leaves the game 
        GameObject found = GameObject.Find("Prince");
        if (found == null) found = GameObject.Find("Prince(Clone)");
        found.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);     
        // add wardarks on space 26 and 27 (move twice at the end of the day)
        Vector3 w26 = GameBoardRegion.getCoordinates(26);
        Vector3 w27 = GameBoardRegion.getCoordinates(27);
        Dictionary<int, Vector3> wPositions = new Dictionary<int, Vector3>() {
            {26, w26},
            {27, w27}
        };
        GameObject m;

        foreach (KeyValuePair<int, Vector3> w in wPositions)
        {
            m = (GameObject)PhotonNetwork.InstantiateSceneObject("Wardrak", w.Value, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(m, SceneManager.GetSceneByName("GameScene"));
            PhotonView v3 = m.GetPhotonView();
            v3.RPC("DontDestroy", RpcTarget.AllBuffered);
            v3.RPC("SetPosition", RpcTarget.AllBuffered, w.Key);
        }
    }
}