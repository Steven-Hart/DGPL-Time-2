using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFade : MonoBehaviour {

    public Animator[] anims;

    public GameObject Menu;
    public GameObject Credits;
   
    public List<GameObject> Levels;

    private bool result;
    private AnimationClip[] clips;
    private float time = 0f;
    private GameObject menu;
    private GameObject currentLevel;
    private GameObject nextLevel;

    public enum FadeState {MenuToCredits, MenuToGame, RestartLevel, NextLevel}


    private void Start()
    {
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
                NextLevel(result, nextLevel);
                break;
            case FadeState.RestartLevel:
                NextLevel(result, currentLevel);
                break;
        }
    }
    
    public void NextLevel(bool result, GameObject nextLevel)
    {

        //Did they Win
        if (result)
        {
            //Next Level
            //Set Next Level
            StartCoroutine("FadeNextLevelCoroutine");
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

        anims[0].Play("Fade In");
        anims[1].Play("Level Disappear 01");
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //Do other stuff here

        Credits.SetActive(true);
    }

    IEnumerator FadeMenuToGameCoroutine()// Menu to Game
    {

        anims[0].Play("Fade In");
        anims[1].Play("Level Disappear 01");
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //Do other stuff here
        anims[0].Play("Fade Out");
        anims[1].Play("Level Reappear 01");
    }

    IEnumerator FadeNextLevelCoroutine(int currentLevel, int NextLevel)// Level to Level
    {

        anims[0].Play("Fade In");
        anims[1].Play("Level Disappear 01");
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //Do other stuff here
        anims[0].Play("Fade Out");
        anims[1].Play("Level Reappear 01");
    }

    IEnumerator FadeRestartLevelCoroutine()// Restart Level
    {

        anims[0].Play("Fade In");
        anims[1].Play("Level Disappear 01");
        yield return new WaitForSeconds(time + 2f); //Wait for clip to finish
        //Do other stuff here
        anims[0].Play("Fade Out");
        anims[1].Play("Level Reappear 01");
    }
}
