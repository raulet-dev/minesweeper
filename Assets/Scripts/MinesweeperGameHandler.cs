using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MinesweeperGameHandler : MonoBehaviour {

    [SerializeField] private GridPrefabVisual gridPrefabVisual;
    [SerializeField] private TMPro.TextMeshPro timerText;

    private Map map;
    private float timer;
    private bool isGameActive;

    private void Start() {
        map = new Map();
        gridPrefabVisual.Setup(map.GetGrid());
        
        isGameActive = true;

        map.OnEntireMapRevealed += Map_OnEntireMapRevealed;
    }

    private void Map_OnEntireMapRevealed(object sender, System.EventArgs e) {
        isGameActive = false;
        int timeScore = Mathf.FloorToInt(timer);
        UI_Blocker.Show_Static();
        UI_InputWindow.Show_Static("Player Name", "", "ABCDEFGHIJKLMNOPQRSTUVXYWZ", 3, () => { 
            // Cancel
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }, (string playerName) => {
            // Ok
            HighscoreTable.Instance.Show();
            HighscoreTable.Instance.AddHighscoreEntry(timeScore, playerName);
            UI_PlayAgain.Instance.Show();
        });
    }

    private void Update() {
        Vector3 worldPosition = UtilsClass.GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0)) {
            if (isGameActive) {
                MapGridObject.Type gridType = map.RevealGridPosition(worldPosition);
                if (gridType == MapGridObject.Type.Mine) {
                    // Revealed a Mine, Game Over!
                    isGameActive = false;
                    map.RevealEntireMap();
                    UI_Blocker.Show_Static();
                    UI_GameOver.Instance.Show();
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            if (isGameActive) {
                map.FlagGridPosition(worldPosition);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.T)) {
            gridPrefabVisual.SetRevealEntireMap(true);
        }
        if (Input.GetKeyUp(KeyCode.T)) {
            gridPrefabVisual.SetRevealEntireMap(false);
        }

        HandleTimer();
    }

    private void HandleTimer() {
        if (isGameActive) {
            timer += Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timer).ToString();
        }
    }

}
