using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Estado
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


    [SerializeField] int __teste;
    #endregion

    #region Variáveis Privadas
    private Animator _animator;
    private int _estado = (int)Estado.Normal;
    private bool _selecionada = false;
    private bool _alternativa = false;
    #endregion

    void Start () {
        _animator = GetComponent<Animator>();
	}
	
	void Update () {
        // * Define a animação de estado para a célula
        _animator.SetInteger("_estado", _estado);

        __teste = Numero;
	}

    /// <summary>
    /// Define o estado da célula como selecionada
    /// </summary>
    public void Selecionada()
    {
        _selecionada = true;
        _alternativa = false;
        _estado = (int)Estado.Selecionada;
    }

    /// <summary>
    /// Define o estado da célula como alternativa
    /// </summary>
    public void Alternativa()
    {
        _alternativa = true;
        _selecionada = false;
        _estado = (int)Estado.Alternativa;
    }

    /// <summary>
    /// Define o estado da célula como normal
    /// </summary>
    public void Normal()
    {
        _alternativa = false;
        _selecionada = false;
        _estado = (int)Estado.Normal;
    }
}
