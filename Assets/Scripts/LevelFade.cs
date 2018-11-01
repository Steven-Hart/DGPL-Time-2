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
    

    private bool result;
    private AnimationClip[] clips;
    private float time = 0f;
    private GameObject menu;
    private int currentLevel;
    private bool finalLevel;
    private GameObject levelToLoad;
    private bool fromGame; //Loading credits from game or menu

    public enum FadeState {MenuToCredits, MenuToGame, RestartLevel, NextLevel}


    private void Start()
    {
        currentLevel = 0;
        finalLevel = false;
        menu = GameObject.FindGameObjectWithTag("Menu");
        clips = anims[0].runtimeAnimatorController.animationClips;
        for (int j = 0; j < clips.Length; j++)
        {
            if (clips[j].name == "Level Disappear 01")
            {
                time = clips[j].length;
            }
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
                currentLevel += 1;


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
        Levels[currentLevel].SetActive(true);
        UI[1].Play("In Game UI Fade In");
        GameUI.SetActive(true);


    }

    IEnumerator FadeNextLevelCoroutine()// Level to Level
    {
        int levelno = currentLevel + 1;
        string dis = string.Format("Level Disappear 0{0}", levelno);
        string re = string.Format("Level Reappear 0{0}", levelno + 1);
        UI[0].Play("Fade In");
        anims[currentLevel].Play(dis);
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish

        Levels[currentLevel - 1].SetActive(false); //Disable last level

        UI[0].Play("Fade Out");
        anims[currentLevel + 1].Play(re);
        levelToLoad.SetActive(true); //Enable next level
        foreach (MoveCounter m in UI[1].GetComponentsInChildren<MoveCounter>())
        {
            m.ResetMove();
        }

    }

    IEnumerator FadeRestartLevelCoroutine()// Restart Level
    {
        int levelno = currentLevel + 1;
        string dis = string.Format("Level Disappear 0{0}", levelno);
        string re = string.Format("Level Reappear 0{0}", levelno);
        UI[0].Play("Fade In");
        anims[1].Play(dis);
        Levels[currentLevel].SetActive(false);
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //TODO: RESET THE CURRENT SCENE
        UI[0].Play("Fade Out");
        anims[1].Play(re);
        Levels[currentLevel].SetActive(true);
    }
}
