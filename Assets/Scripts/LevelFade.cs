using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFade : MonoBehaviour {

    public Animator[] anims;

    public GameObject MainMenu;
    public GameObject GameUI;
    public GameObject Credits;

    public List<Animator> UI;
    public List<GameObject> Levels;
    public List<GameObject> LevelPrefabs;
    

    private bool result;
    private AnimationClip[] clips;
    private float time = 0f;
    private GameObject menu;
    public int currentLevel;
    private bool finalLevel;
    private GameObject levelToLoad;
    private bool fromGame; //Loading credits from game or menu
	private bool fadeInProgress = false;
	private LifeController lifecontrol;

    public enum FadeState {MenuToCredits, MenuToGame, RestartLevel, NextLevel}


	void Start()
    {
        currentLevel = 0;
        finalLevel = false;
        menu = GameObject.FindGameObjectWithTag("Menu");
        clips = anims[0].runtimeAnimatorController.animationClips;
		lifecontrol = GetComponent<LifeController>();
        for (int j = 0; j < clips.Length; j++)
        {
            if (clips[j].name == "Level Disappear 01")
            {
                time = clips[j].length;
            }
		}
		MainMenu.GetComponent<Animator>().Play("Fade In");
    }

	void Update()
	{
		if (fadeInProgress)
			return;
		if (Input.GetAxis("Restart") > 0)
		{
			// Restart level here also needs to check if its on a level or main menu/credits
			ChooseFade(FadeState.RestartLevel);
		}
	}

    public void ChooseFade(FadeState _state)
    {
		if (fadeInProgress)
			return;
        switch (_state)
        {
            case FadeState.MenuToCredits:
                GameCredits();
                break;
            case FadeState.MenuToGame:
                StartGame();
                break;
            case FadeState.NextLevel:
                NextLevel(true);
                break;
            case FadeState.RestartLevel:
                NextLevel(false);
                break;
        }
    }
    
    public void NextLevel(bool result)
    {

        //Did they Win
        if (result)
        {
            //Next Level
            //Set Next Level
            if (!finalLevel) //Checks if its the last level
            {
                if (currentLevel + 2== Levels.Count )
                {
                    finalLevel = true;
                }
                levelToLoad = Levels[currentLevel + 1];
                StartCoroutine("FadeNextLevelCoroutine");


            }
            else //Goes to credits if last level was won
            {
                ChooseFade(FadeState.MenuToCredits);
            }
        }
        else
        {
            //Reset
            StartCoroutine("FadeRestartLevelCoroutine");
        }
    }
    public void StartGame()
	{
		if (fadeInProgress)
			return;
        //First Level
        StartCoroutine("FadeMenuToGameCoroutine");
    }

    public void GameCredits()
	{
		if (fadeInProgress)
			return;
        StartCoroutine("FadeMenuToCreditsCoroutine");
    }
    public void GameCreditsToMenu()
	{
		if (fadeInProgress)
			return;
        StartCoroutine("FadeCreditsToMenuCoroutine");
    }

	IEnumerator FadeCreditsToMenuFinal()
	{
		fadeInProgress = true;
		MainMenu.SetActive(true);
		MainMenu.GetComponent<Animator>().Play("Fade In");
		yield return new WaitForSeconds(1f); //Wait for clip to finish
											 //Do other stuff here
		GameUI.SetActive(false);
		Credits.SetActive(false);
		fadeInProgress = false;
	}

    IEnumerator FadeMenuToCreditsCoroutine()// Menu to Credits
    {
		fadeInProgress = true;
        float _time;
        _time = 1f;
		MainMenu.GetComponent<Animator>().Play("Fade Out");
        if (fromGame)
        {
            UI[0].Play("Fade In");
            anims[currentLevel].Play("Level Disappear");
            _time = time + 1f;
        }
        
        yield return new WaitForSeconds(_time); //Wait for clip to finish
        //Do other stuff here
        MainMenu.SetActive(false);
        GameUI.SetActive(false);
        Credits.SetActive(true);
		Credits.GetComponent<Animator>().Play("Fade Out");
		fadeInProgress = false;
    }

    IEnumerator FadeCreditsToMenuCoroutine()// Menu to Credits
    {
		fadeInProgress = true;
		MainMenu.SetActive(true);
		MainMenu.GetComponent<Animator>().Play("Fade In");
        yield return new WaitForSeconds(1f); //Wait for clip to finish
        //Do other stuff here
        GameUI.SetActive(false);
        Credits.SetActive(false);
		fadeInProgress =false;
    }

    IEnumerator FadeMenuToGameCoroutine()// Menu to Game
    {
		fadeInProgress = true;
        //Wait for clip to finish
        //TODO MAIN MENU FADE
		MainMenu.GetComponent<Animator>().Play("Fade Out");
        UI[0].Play("Fade In");
        yield return new WaitForSeconds(0.9f); // Main menu fade in looks better with 0.9 instead of 1
        MainMenu.SetActive(false);
        UI[0].Play("Fade Out");
		GameUI.SetActive(true);
		UI[1].Play("In Game UI Fade In"); // UI 1 Game HUD
		Levels[currentLevel].SetActive(true);
		fadeInProgress = false;
    }

    IEnumerator FadeNextLevelCoroutine()// Level to Level
	{
		fadeInProgress = true;
        UI[0].Play("Fade In");
        anims[currentLevel].Play("Level Disappear");
		UI[1].Play("In Game UI Fade Out");
        yield return new WaitForSeconds(time + 1f); //Wait for clip to finish

        Levels[currentLevel].SetActive(false); //Disable last level

        UI[0].Play("Fade Out");
		UI[1].Play("In Game UI Fade In");
		//if(currentLevel + 1 < anims.Length)
		//	anims[currentLevel + 1].Play(re);
		//if (currentLevel + 1 < Levels.Count)

		currentLevel ++;
		Levels[currentLevel].GetComponentInChildren<Player>().lifeLine = lifecontrol; // Links new player to life controller
		lifecontrol._lives = Levels[currentLevel].GetComponent<LifeStore>().Lifelines;
		levelToLoad.SetActive(true); //Enable next level
		lifecontrol.ResetCanvas();
		anims[currentLevel].Play("Level Reappear");
		fadeInProgress = false;
    }

    IEnumerator FadeRestartLevelCoroutine()// Restart Level
	{
		fadeInProgress = true;
        anims[currentLevel].Play("Level Disappear");
        UI[0].Play("Fade In"); // UI 0 background colour overlay
		UI[1].Play("In Game UI Fade Out");
        yield return new WaitForSeconds(time + 1f); //Wait for clip to finish
		Levels[currentLevel].SetActive(false);
        //Vector3 levelpos = Levels[currentLevel].transform.position;
        Destroy(Levels[currentLevel]); // Destroys old level
		UI[1].Play("In Game UI Fade In");
        GameObject newLevel = Instantiate(LevelPrefabs[currentLevel]); // Let new level be born
        Levels[currentLevel] = newLevel;
		anims[currentLevel] = Levels[currentLevel].GetComponent<Animator>();
		lifecontrol._lives = Levels[currentLevel].GetComponent<LifeStore>().Lifelines;
		Levels[currentLevel].GetComponentInChildren<Player>().lifeLine = lifecontrol; // Links new player to life controller

        UI[0].Play("Fade Out");
        Levels[currentLevel].SetActive(true);
		anims[currentLevel].Play("Level Reappear");
		lifecontrol.ResetCanvas();
		fadeInProgress = false;
    }
}