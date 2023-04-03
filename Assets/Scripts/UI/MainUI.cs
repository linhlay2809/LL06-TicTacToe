using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        public void SelectLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}