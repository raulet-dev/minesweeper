using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;

public class UI_PlayAgain : MonoBehaviour {

    public static UI_PlayAgain Instance { get; private set; }

    private void Awake() {
        Instance = this;
        transform.Find("playAgainBtn").GetComponent<Button_UI>().ClickFunc = () => SceneManager.LoadScene(0);
        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

}
