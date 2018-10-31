using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFade : MonoBehaviour {

    public Animator[] anims;

    public GameObject MainMenu;
    public GameObject GameUI;
    public GameObject Credits;
   
    public List<GameObject> Levels;

    private bool result;
    private AnimationClip[] clips;
    private float time = 0f;
    private GameObject menu;
    private int currentLevel;
    private bool finalLevel;
    private GameObject levelToLoad;

    public enum FadeState {MenuToCredits, MenuToGame, RestartLevel, NextLevel}


    private void Start()
    {
        currentLevel = 0;
        finalLevel = false;
        menu = GameObject.FindGameObjectWithTag("Menu");
        clips = anims[1].runtimeAnimatorController.animationClips;
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
                NextLevel(result);
                break;
            case FadeState.RestartLevel:
                NextLevel(result);
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
            StartCoroutine("FadeNextLevelCoroutine");
            if (!finalLevel) //Checks if its the last level
            {
                currentLevel += 1;
                if (currentLevel == Levels.Count - 1)
                {
                    finalLevel = true;
                }
                levelToLoad = Levels[currentLevel + 1];
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

    IEnumerator FadeMenuToCreditsCoroutine()// Menu to Credits
    {
        int levelno = currentLevel += 1;
        string dis = string.Format("Level Disappear 0{0}", levelno);
        anims[0].Play("Fade In");
        anims[1].Play(dis);
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //Do other stuff here
        GameUI.SetActive(false);
        Credits.SetActive(true);
    }

    IEnumerator FadeMenuToGameCoroutine()// Menu to Game
    {
        
        anims[0].Play("Fade In");
        anims[1].Play("Level Disappear 01");
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //TODO MAIN MENU FADE
        MainMenu.SetActive(false);
        GameUI.SetActive(true);

        anims[0].Play("Fade Out");
        anims[1].Play("Level Reappear 01");
    }

    IEnumerator FadeNextLevelCoroutine()// Level to Level
    {
        int levelno = currentLevel += 1;
        string dis = string.Format("Level Disappear 0{0}", levelno);
        string re = string.Format("Level Reappear 0{0}", levelno + 1);
        anims[0].Play("Fade In");
        anims[1].Play(dis);
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish

        Levels[currentLevel].SetActive(false); //Disable lest level

        anims[0].Play("Fade Out");
        anims[1].Play(re);
        levelToLoad.SetActive(true); //Enable next level

    }

    IEnumerator FadeRestartLevelCoroutine()// Restart Level
    {
        int levelno = currentLevel += 1;
        string dis = string.Format("Level Disappear 0{0}", levelno);
        string re = string.Format("Level Reappear 0{0}", levelno);
        anims[0].Play("Fade In");
        anims[1].Play(dis);
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //TODO: RESET THE CURRENT SCENE
        anims[0].Play("Fade Out");
        anims[1].Play(re);
    }
}
