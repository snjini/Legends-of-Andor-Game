using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class RuneStone : MonoBehaviourPun
{

    public int pos;

    [PunRPC]
    public void DontDestroy()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    [PunRPC]
    public void SetPosOfRuneStone(int position)
    {
        // hero needs to roll dice: red die = first digit, hero die = second digit
        this.pos = position;
    }

    [PunRPC]
    public int SetPosOfRuneStoneCard(int numRolled)
    {
        // one player rolls dice to determine where the runestone card will be triggered on the Legend track
        //  if num == 1, then set pos = 1 (B)
        if (numRolled == 1)
        {
            return 1;
        }
        //  if num == 2, then set pos = 3 (D)
        else if (numRolled == 2)
        {
            return 3;
        }
        //  if num == 3, then set pos = 4 (E)
        else if (numRolled == 3)
        {
            return 4;
        }
        //  if num == 4 || num == 5, then set pos = 5 (F)
        else if (numRolled == 4 || numRolled == 5)
        {
            return 5;
        }
        //  if num == 6, then set pos = 7 (H)
        else if (numRolled == 6)
        {
            return 7;
        } 
        return -1;
    }

    [PunRPC]
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}