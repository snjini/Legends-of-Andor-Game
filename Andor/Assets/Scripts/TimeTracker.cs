using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TimeTracker : MonoBehaviourPun{
    [PunRPC]
    public void ResetTokens() {
        this.transform.position = new Vector3(-1.3f, 12.55f, 15); 
    }

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject);
    }
}