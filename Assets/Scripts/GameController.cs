using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();

    
    public Sprite backgroundImage;

    public TextMeshProUGUI countText;

    public Sprite[] cats;

    public List<Sprite> catsList = new List<Sprite>();

    private bool firstGuess;
    private bool secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex;
    private int secondGuessIndex;

    private string firstGuessCat;
    private string secondGuessCat;

    private void Start()
    {
        GetButtons();
        AddListeners();
        AddCatCards();
        Shuffle(catsList);
        gameGuesses = catsList.Count / 2;
    }
    private void Update()
    {
        countText.text = countGuesses.ToString();
    }
    public void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            buttons.Add(objects[i].GetComponent<Button>());
            buttons[i].image.sprite = backgroundImage;
        }
    }
    private void AddCatCards()
    {
        int looper = buttons.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (looper / 2 == index)
            {
                index = 0;
            }
            catsList.Add(cats[index]);
            index++;
        }
    }
    private void AddListeners() 
    { 
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() =>  PickACard());
        }
    }

   
    public void PickACard()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        if (!firstGuess)
        {
            firstGuess = true;

            firstGuessIndex = int.Parse(name);

            firstGuessCat = catsList[firstGuessIndex].name;

            buttons[firstGuessIndex].image.sprite = catsList[firstGuessIndex];
        }
        else if(!secondGuess) 
        {
            secondGuess = true;

            secondGuessIndex = int.Parse(name);

            secondGuessCat = catsList[secondGuessIndex].name;

            buttons[secondGuessIndex].image.sprite = catsList[secondGuessIndex];

            countGuesses++;

            StartCoroutine(CheckCatsMatch());
    
        }

    }
    IEnumerator CheckCatsMatch()
    {
        yield return new WaitForSeconds(0.75f);

        if(firstGuessCat == secondGuessCat) 
        {
            yield return new WaitForSeconds(.3f);

            buttons[firstGuessIndex].interactable = false;
            buttons[secondGuessIndex].interactable=false;

            CheckIfGameIsFinished();

        }
        else
        {
            buttons[firstGuessIndex].image.sprite = backgroundImage;
            buttons[secondGuessIndex].image.sprite = backgroundImage;
        }
        yield return new WaitForSeconds(.3f);

        firstGuess = false;
        secondGuess= false;
    }
    private void CheckIfGameIsFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            StartCoroutine(UnlockNewLevel());

            if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
            {
                PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
                PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
                PlayerPrefs.Save();
            }
        }
    }

    IEnumerator UnlockNewLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp; 
        }
    }

}
