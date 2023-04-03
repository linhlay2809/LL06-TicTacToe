using UnityEngine;

namespace Gameplay
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;
        public static GameController Instance => _instance;

        public GameState currentState;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public Sprite xSprite;
        public Sprite oSprite;
        public Color xColor;
        public Color oColor;

        [field: SerializeField] public bool EndGame { get; private set; }
        [SerializeField] private LineRenderer lineRenderer;
        
        public void ShowLine(Transform pos0, Transform pos1, Transform pos2)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0,pos0.position);
            lineRenderer.SetPosition(1,pos1.position);
            lineRenderer.SetPosition(2,pos2.position);
        }
        public void SetEndGame(bool endGame)
        {
            this.EndGame = endGame;
        }

        public void SwitchState()
        {
            currentState = currentState == GameState.Player ? GameState.Opponent : GameState.Player;
        }
    }
}