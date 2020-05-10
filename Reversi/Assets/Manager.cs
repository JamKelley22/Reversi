using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public TextMeshProUGUI difficultyText;
    public GameObject WinCanvas;
    public TextMeshProUGUI winTextElement;

    private void Start()
    {
        UpdateDifficultyText(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void UpdateDifficultyText(float val)
    {
        string stringVal = "";
        switch (val)
        {
            case 2:
                stringVal = "Very Easy";
                break;
            case 3:
                stringVal = "Easy";
                break;
            case 4:
                stringVal = "Moderate";
                break;
            case 5:
                stringVal = "Medium";
                break;
            case 6:
                stringVal = "Hard";
                break;
            default:
                stringVal = "You broke it";
                break;
        }
        difficultyText.text = "Difficulty: " + stringVal;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnWin(string winText)
    {
        WinCanvas.SetActive(true);
        winTextElement.text = winText;
    }
}
