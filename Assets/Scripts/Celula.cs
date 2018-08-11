using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Animacao
{
    Normal = 0,
    Selecionada = 1,
    Alternativa = 2
}

public class Celula : MonoBehaviour {

    #region Variáveis Públicas
    public int Linha { get; set; }
    public int Coluna { get; set; }
    public int Numero { get; set; }
    #endregion

    #region Variáveis Privadas
    private Animator _animator;
    private int _animacao = (int)Animacao.Normal;
    private bool _selecionada = false;
    private bool _alternativa = false;
    #endregion

    void Start () {
        _animator = GetComponent<Animator>();
	}
	
	void Update () {
        _animator.SetInteger("_animacao", _animacao);

        CircleCollider2D[] _colliders = GetComponents<CircleCollider2D>();

        foreach (CircleCollider2D _collider in _colliders)
        {
            _collider.enabled = _selecionada;
        }
	}

    public void Selecionar()
    {
        _selecionada = !_selecionada;
        _animacao = (_selecionada) ? (int)Animacao.Selecionada : (int)Animacao.Normal;
    }

    public void Alternativa()
    {
        _alternativa = !_alternativa;
        _animacao = (_alternativa) ? (int)Animacao.Alternativa : (int)Animacao.Normal;
    }

    /*private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Celula"))
        {
            _alternativa = true;
            _animacao = (int)Animacao.Alternativa;
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        _alternativa = false;
        _animacao = (int)Animacao.Normal;
    }*/
}
