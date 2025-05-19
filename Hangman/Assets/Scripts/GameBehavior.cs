using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    public List<GameObject> bodyparts = new List<GameObject>();
    public Button restart;
    public TMP_Text usedLetters;
    public TMP_Text gameState;
    public List<char> usedChars = new List<char>(); // List of used characters
    public GameObject hiddenLetterPrefab;
    public List<HiddenLetterBehavior> hiddenLetters = new List<HiddenLetterBehavior>();
    public Transform canvas;
    public string[] wordList;
    public string currentWord;
    public int lives;
    public int maxLives;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                char[] letter = key.ToString().ToLower().ToCharArray();
                if (Input.GetKey(key) && char.IsLetter(letter[0]) && letter.Length == 1 && lives > 0 && !usedChars.Contains(letter[0]))
                {
                    CheckKey(key);
                }
            }
        }
        
    }

    public string SelectWordFromList()
    {
        int randomIndex = Random.Range(0, wordList.Length);
        return wordList[randomIndex];
    }

    public void Initialize()
    {
        currentWord = SelectWordFromList();
        gameState.text = "Lives: " + lives;
        for (int i = 0; i < currentWord.Length; i++)
        {
            GameObject hiddenLetter = Instantiate(hiddenLetterPrefab, canvas);
            hiddenLetter.transform.position += new Vector3(600 + (i * 50), 700, 0); // Adjust position as needed
            hiddenLetter.GetComponent<HiddenLetterBehavior>().indexInWord = i;
            hiddenLetters.Add(hiddenLetter.GetComponent<HiddenLetterBehavior>());
            hiddenLetters[i].hiddenLetter = currentWord[i];
        }
    }

    public void CheckKey(KeyCode _key)
    {
        usedChars.Add(_key.ToString().ToLower()[0]);
        usedLetters.text += "\n" + _key.ToString().ToLower();
        if (currentWord.Contains(_key.ToString().ToLower()))
        {
            // Correct Guess
            for (int i = 0; i < usedChars.Count; i++)
            {
                for (int j = 0; j < hiddenLetters.Count; j++)
                {
                    if (!hiddenLetters[j].isGuessed)
                    {
                        hiddenLetters[j].CheckLetter(usedChars[i]);
                    }
                }
            }
            if(hiddenLetters.All(h => h.isGuessed))
            {
                gameState.text = "You Win!";
                lives = 0;
            }
        }
        else
        {
            // Incorrect guess
            lives -= 1;
            bodyparts[lives].SetActive(true);
            gameState.text = "Lives: " + lives;
            if (lives <= 0)
            {
                gameState.text = "Game Over";
                restart.gameObject.SetActive(true);
            }
        }
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
