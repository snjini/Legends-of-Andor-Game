using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class WellController : MonoBehaviourPun {
    [PunRPC]
    public void ResetPopUpListeners() {
        GameBoardRegion.yesButton.onClick.RemoveAllListeners();
        GameBoardRegion.yesButton.onClick.AddListener(() => GameBoardRegion.setActiveFalse(GameBoardRegion.wellPopup, 4));
    }
}