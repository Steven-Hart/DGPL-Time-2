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
    private int currentLevel;
    private bool finalLevel;
    private GameObject levelToLoad;
    private bool fromGame; //Loading credits from game or menu
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
    }

	void Update()
	{
		if (Input.GetAxis("Restart") > 0)
		{
			// Restart level here also needs to check if its on a level or main menu/credits
		}
	}

    public void ChooseFade(FadeState _state)
    {

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
                if (currentLevel + 1 == Levels.Count - 1)
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
        //First Level
        StartCoroutine("FadeMenuToGameCoroutine");
    }

    public void GameCredits()
    {
        StartCoroutine("FadeMenuToCreditsCoroutine");
    }
    public void GameCreditsToMenu()
    {
        StartCoroutine("FadeCreditsToMenuCoroutine");
    }

    IEnumerator FadeMenuToCreditsCoroutine()// Menu to Credits
    {
        float _time;
        _time = 0.5f;
        if (fromGame)
        {
            int levelno = currentLevel + 1;
            string dis = string.Format("Level Disappear 0{0}", levelno);
            UI[0].Play("Fade In");
            anims[currentLevel].Play(dis);
            _time = time + 2f;
        }
        
        yield return new WaitForSeconds(_time); //Wait for clip to finish
        //Do other stuff here
        MainMenu.SetActive(false);
        GameUI.SetActive(false);
        Credits.SetActive(true);
    }

    IEnumerator FadeCreditsToMenuCoroutine()// Menu to Credits
    {
        yield return new WaitForSeconds(0.5f); //Wait for clip to finish
        //Do other stuff here
        MainMenu.SetActive(true);
        GameUI.SetActive(false);
        Credits.SetActive(false);
    }

    IEnumerator FadeMenuToGameCoroutine()// Menu to Game
    {


        //Wait for clip to finish
        //TODO MAIN MENU FADE
        UI[0].Play("Fade In");
        yield return new WaitForSeconds(1f);
        MainMenu.SetActive(false);
        UI[0].Play("Fade Out");
		UI[1].Play("In Game UI Fade In");
		GameUI.SetActive(true);
        Levels[currentLevel].SetActive(true);


    }

    IEnumerator FadeNextLevelCoroutine()// Level to Level
    {
        
        //int levelno = currentLevel + 1;
        //string dis = string.Format("Level Disappear 0{0}", levelno);
        //string re = string.Format("Level Reappear 0{0}", levelno + 1);
        UI[0].Play("Fade In");
        //anims[currentLevel].Play(dis);
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish

        Levels[currentLevel].SetActive(false); //Disable last level

        UI[0].Play("Fade Out");
		//if(currentLevel + 1 < anims.Length)
        //	anims[currentLevel + 1].Play(re);
		//if (currentLevel + 1 < Levels.Count)
		Levels[currentLevel + 1].GetComponentInChildren<Player>().lifeLine = lifecontrol; // Links new player to life controller
		levelToLoad.SetActive(true); //Enable next level
		lifecontrol.ResetCanvas();
        currentLevel += 1;
    }

    IEnumerator FadeRestartLevelCoroutine()// Restart Level
    {
        int levelno = currentLevel + 1;
        string dis = string.Format("Level Disappear 0{0}", levelno);
        string re = string.Format("Level Reappear 0{0}", levelno);
        anims[currentLevel].Play(dis);
        Levels[currentLevel].SetActive(false);
        UI[0].Play("Fade In");

        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish

        Vector3 levelpos = Levels[currentLevel].transform.position;
        Destroy(Levels[currentLevel]); // Destroys old level

        GameObject newLevel = Instantiate(LevelPrefabs[currentLevel]); // Let new level be born
        Levels[currentLevel] = newLevel;
        anims[currentLevel] = Levels[currentLevel].GetComponent<Animator>();
		Levels[currentLevel].GetComponentInChildren<Player>().lifeLine = lifecontrol; // Links new player to life controller

        UI[0].Play("Fade Out");
        anims[currentLevel].Play(re);
        Levels[currentLevel].SetActive(true);
		lifecontrol.ResetCanvas();
        
    }
}