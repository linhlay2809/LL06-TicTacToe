using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Move move;

    private GameController _gameController;
    public Action<int, int> OnClick;


    private void Start()
    {
        _gameController = GameController.Instance;
    }

    public Move Move()
    {
        return this.move;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_gameController.currentState == GameState.Player)
        {
            OnClick?.Invoke(move.Row, move.Col);
        }
    }

    // Update X sprite or O sprite to Box UI
    public void UpdateBoxUI(GameState state)
    {
        if (state == GameState.Player)
        {
            image.sprite = _gameController.xSprite;
            image.color = _gameController.xColor;
        }
        else
        {
            image.sprite = _gameController.oSprite;
            image.color = _gameController.oColor;
        }
    }
}