using UnityEngine;
using TMPro;
using MilkShake;
using System.Collections;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static ScoreManager Instance;

    // UI elements
    public GameObject WinScreen, WrongText, Cam;
    public float score, total_Score;
    public TextMeshProUGUI scoreText;
    public ShakePreset ShakePreset;

    // Initialize game state and UI
    void Start()
    {
        Instance = this;
        scoreText.text = $"{score}/{total_Score}";
        WinScreen.SetActive(false);
    }

    // Add points to the score
    public void AddPoint(float value)
    {
        score += value;
        Debug.Log($"New score: {score}");
        scoreText.text = $"{score}/{total_Score}";
        AudioManager.Instance.PlaySFX("Score");
        CheckWin();
    }

    // Check if the score equals the total score
    void CheckWin()
    {
        if (score == total_Score)
        {
            StartCoroutine(Win(0f));
        }
        // else if (score == total_Score - 1)
        // {
        //     StartCoroutine(PreWin(0f));
        // }
    }

    // Handle winning the game
    IEnumerator Win(float seconds)
    {
        AudioManager.Instance.SetSFXAllowOverlap(true);
        // AudioManager.Instance.SetSFXLooping(false);
        // AudioManager.Instance.PlaySFX("Bonus");
        // Cam.transform.DOMove(new Vector3(0f, 0f, 0f), 1.5f).SetEase(Ease.Linear).OnComplete(() =>
        //    {
        //    });
        yield return new WaitForSeconds(seconds);
        AudioManager.Instance.SetSFXAllowOverlap(false);
        AudioManager.Instance.SetSFXLooping(false);
        Time.timeScale = 0f;
        VideoManager.Instance.PlayVideo("Win");
        Debug.Log("Win!");
        AudioManager.Instance.bgmSource.Stop();
        AudioManager.Instance.sfxSource.Stop();
        AudioManager.Instance.PlaySFX("Win");
        WinScreen.SetActive(true);
    }

    // Handle Pre-Winning the game
    IEnumerator PreWin(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    // Handle wrong answer
    public void Wrong(Vector3 newPosition)
    {
        AudioManager.Instance.PlaySFX("Wrong");
        GameObject Text = Instantiate(WrongText, newPosition, Quaternion.identity);
        Text.transform.DOScale(0f, 0.5f).SetEase(Ease.InExpo).SetDelay(0.5f).OnComplete(() => Destroy(Text));
        Shaker.ShakeAll(ShakePreset);
    }
}