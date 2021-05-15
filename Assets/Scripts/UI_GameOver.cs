using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeMonkey.Utils;

public class UI_GameOver : MonoBehaviour {

    public static UI_GameOver Instance { get; private set; }

    private void Awake() {
        Instance = this;
        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () => SceneManager.LoadScene(0);
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
