using System.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;

        public static UIManager Instance => _instance;

        [SerializeField] private ResultPnl resultPnl;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        // Show the result at the end of the game
        public async void ShowResult(ResultType type)
        {
            await Task.Delay(1000);

            resultPnl.gameObject.SetActive(true);

            resultPnl.ShowResult(type);
        }

        
    }
}