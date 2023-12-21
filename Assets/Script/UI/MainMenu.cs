using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;
    [SerializeField]
    private Transform guideLine;
    [SerializeField]
    private Transform bg;


    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);

        gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        gameLogo.GetComponent<CanvasGroup>().DOFade(1, 2f).SetUpdate(true);
        gameLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 350), 2f, false).SetEase(Ease.OutQuint).SetUpdate(true).OnComplete(() =>
        {
            gameLogo.DOScale(.95f, .5f).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        });
        bg.DOShakePosition(10f, 100f, 0, 0, false, true).SetEase(Ease.InSine).SetLoops(-1);
    }

    public void ShowTutorPanel()
    {
        tutorPanel.gameObject.SetActive(true);
        guideLine.gameObject.SetActive(true);
        FadeIn(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>());

    }

    public void HideTutorPanel()
    {
        StartCoroutine(FadeOut(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>()));

    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 1700, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 1700), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        guideLine.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(false);

    }

}
