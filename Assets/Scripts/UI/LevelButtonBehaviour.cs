using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonBehaviour : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void Start()
    {
        SaveData data = SaveManagerBehaviour.Load(sceneName);
        Text text = GetComponentInChildren<Text>();

        if (text == null) return;
        if ( data != null)
        {
            text.text = "Nivel: " + data.GetLevelName() + "(" + data.GetScore() + ")";
        }
        else
        {
            text.text = "Nivel: " + sceneName;
        }

    }

    public void OnClick()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
