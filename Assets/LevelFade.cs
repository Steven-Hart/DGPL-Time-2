using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFade : MonoBehaviour {

    public Animator[] anims;


    private bool result;
    private AnimationClip[] clips;
    private float time = 0f;
    private GameObject menu;

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
                NextLevel(result, nextLevel);
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
        anims[0].Play("Fade Out");
        anims[1].Play("Level Reappear 01");
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

    IEnumerator FadeNextLevelCoroutine()// Level to Level
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
