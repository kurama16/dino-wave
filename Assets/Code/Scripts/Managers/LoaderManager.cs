using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    public void LoadScene(int indexScene)
    {
        SceneManager.LoadScene(indexScene);
    }
}
