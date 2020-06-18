using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;

using UnityEngine.SceneManagement;


public class LobbyController : MonoBehaviourPunCallbacks {
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject cancelButton;
    [SerializeField]
    private int RoomSize;
    private string mode;
    public GameObject mainMenu;

    void Start() {
        this.mainMenu = GameObject.Find("MainMenu");
    }
    public override void OnConnectedToMaster() {
        PhotonNetwork.AutomaticallySyncScene = true;
        startButton.SetActive(true);
    }

    public void setMode(string m) {
        this.mode = m;
    }

    public void QuickStart() {
        if (mode=="create") {
            QuickStartCreate();
        } else if (mode=="join") {
            QuickStartJoin();
        }
    }

    public void QuickStartCreate() {
        startButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public void QuickStartJoin() {
        startButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public void QuickCancel() {
        cancelButton.SetActive(false);
        startButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
        this.mainMenu.gameObject.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("Join Failed, trying again");
        Debug.Log(message);
        CreateRoom();
    }

    void CreateRoom() {
        Debug.Log("Creating Room");
        int rand = Random.Range(0,10000);
        RoomOptions roomOptions = new RoomOptions() {
            IsVisible = true, IsOpen = true, MaxPlayers = 4, PlayerTtl = -1
        };
        PhotonNetwork.CreateRoom("Room" + rand, roomOptions);
        base.OnEnable();
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Create room failed, trying again.");
        CreateRoom();
    }

     public override void OnEnable() {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable() {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom() {
        Debug.Log("Joined Room");
        StartGame();
    }

    public override void OnCreatedRoom() {
        Debug.Log("Created Room");
        StartGame();
    }


    private void StartGame() {
        if (PhotonNetwork.IsMasterClient) {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(1);
        }

    }

    public override void OnLeftRoom() {
        PhotonNetwork.LoadLevel(0);
    }

    public void ReJoinRoom(string roomName) {
        LoadBalancingClient client = new LoadBalancingClient();
        client.OpRejoinRoom(roomName);
    }
    
}