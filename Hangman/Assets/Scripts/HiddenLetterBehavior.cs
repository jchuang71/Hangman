using TMPro;
using UnityEngine;

public class HiddenLetterBehavior : MonoBehaviour
{
    public int indexInWord;
    public char hiddenLetter;
    private TMP_Text text;
    public bool isGuessed;

    void Start()
    {
        // Initialize the hidden letter
        text = GetComponent<TMP_Text>();
        text.text = "_";
        isGuessed = false;
    }

    public void CheckLetter(char guessedLetter)
    {
        if(guessedLetter == hiddenLetter)
        {
            text.text = hiddenLetter.ToString();
            isGuessed = true;
        }
        else
        {
            text.text = "_";
        }
    }
}
