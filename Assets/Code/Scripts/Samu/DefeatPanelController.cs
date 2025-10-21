using UnityEngine;

public class DefeatPanelController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private CanvasGroup defeatPanel;
    [SerializeField] private float fadeDuration = 5f;

    private void Awake()
    {
        if(player == null)
        {
            Debug.Log("no hay player");
        }
    }
    private void OnEnable()
    {
        if (player != null) 
            player.OnPlayerDie += ShowDefeatPanel;
    }

    private void OnDisable()
    {
        if (player != null) 
            player.OnPlayerDie -= ShowDefeatPanel;
    }

    public void ShowDefeatPanel()
    {
        defeatPanel.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    System.Collections.IEnumerator FadeIn()
    {
        defeatPanel.interactable = false;
        defeatPanel.blocksRaycasts = false;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            defeatPanel.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        defeatPanel.interactable = true;
        defeatPanel.blocksRaycasts = true;
        defeatPanel.alpha = 1;
    }
}
