using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class QuitStoreButton: MonoBehaviour
{
 
  void Start() {
    Button b = transform.GetComponent<Button>();
    b.onClick.AddListener(delegate() { this.quitStore(); });
  }
  
  void quitStore() {
    LoadGame loading = new LoadGame();
    loading.LoadGameScene();
  }

}
