using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    private const float TIME_UNTIL_PROMPT = 5f;
    private const float CARD_FADE_SPEED = 2f;

    [SerializeField] private GameObject[] cards;
    [SerializeField] private CanvasGroup continuePromptCG;
    [SerializeField] private string nextScene;

    private int currentCard = 0;

    private Coroutine co_switchingCards = null;
    private Coroutine co_fadingContinuePrompt = null;
    private bool switchingCards => co_switchingCards != null;
    private bool fadingContinuePrompt => co_fadingContinuePrompt != null;

    private bool isHoveringSkip;

    private SceneController sceneController => SceneController.Instance;
    private FadeController fadeController => FadeController.Instance;

    private void Start()
    {
        continuePromptCG.alpha = 0;
        FadeInContinuePrompt();
    }

    private void Update()
    {
        if (Input.anyKeyDown && SceneController.Instance.SwitchingScenes == false && isHoveringSkip == false)
        {
            NextCard();
        }
    }

    public void NextCard()
    {
        if (switchingCards)
            return;

        co_switchingCards = StartCoroutine(SwitchingCardsProcess());
    }

    private IEnumerator SwitchingCardsProcess()
    {
        //don't fade out cards as scene is already switching 
        if (SceneController.Instance.SwitchingScenes)
            yield break;

        //this was the last card so go to game scene
        if (currentCard >= cards.Length - 1)
        {
            sceneController.SwitchScene(nextScene);
            yield break;
        }
            
        //not the last card so fade to show next card
        fadeController.FadeIn(CARD_FADE_SPEED); 

        while(fadeController.FadingIn)
            yield return null;

        cards[currentCard].SetActive(false); //hide current card
        continuePromptCG.alpha = 0; //hide continue prompt
        currentCard++;
        cards[currentCard].SetActive(true); //show next card

        fadeController.FadeOut(CARD_FADE_SPEED);
        
        while (fadeController.FadingOut)
            yield return null;

        FadeInContinuePrompt();
        co_switchingCards = null;
    }

    #region CONTINUE PROMPT
    private void FadeInContinuePrompt()
    {
        if (fadingContinuePrompt)
            StopCoroutine(co_fadingContinuePrompt);

        co_fadingContinuePrompt = StartCoroutine(FadeInContinuePromptProcess());
    }

    private IEnumerator FadeInContinuePromptProcess()
    {
        yield return new WaitForSeconds(TIME_UNTIL_PROMPT);

        float targetAlpha = 1;

        while (continuePromptCG.alpha < targetAlpha)
        {
            continuePromptCG.alpha = Mathf.MoveTowards(continuePromptCG.alpha, targetAlpha, CARD_FADE_SPEED * Time.deltaTime);
            yield return null;
        }

        co_fadingContinuePrompt = null;
    }
    #endregion

    public void SetHoveringSkip(bool hovering) => isHoveringSkip = hovering;
}
