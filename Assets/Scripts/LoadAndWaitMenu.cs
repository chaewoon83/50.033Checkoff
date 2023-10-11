using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAndWaitMenu : MonoBehaviour
{
    public CanvasGroup c;

    void Start()
    {

    }

    public void ButtonTrigger()
    {
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        for (float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f)
        {
            c.alpha = alpha;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        // once done, go to next scene
        SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
    }

    public void ReturnToMain()
    {
        // TODO
        Debug.Log("Return to main menu");
    }
}
