using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform menuPanel;
    [SerializeField]
    private Button menuButton;

    public void ShowMenuPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), menuPanel.GetComponent<RectTransform>());
        menuButton.interactable = false;
        GameManager.instance.PauseGame();
    }

    public void ShowWinPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        menuButton.interactable = false;
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public void HideMenuPanel()
    {
        FadeOut(overlayPanel.GetComponent<CanvasGroup>(), menuPanel.GetComponent<RectTransform>());
        menuButton.interactable = true;
        GameManager.instance.ResumeGame();
    }

    private void FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 500), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true).OnComplete(() =>
        {
            overlayPanel.gameObject.SetActive(false);
            menuPanel.gameObject.SetActive(false);
        });
    }
}
