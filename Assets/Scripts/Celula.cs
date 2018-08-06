using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celula : MonoBehaviour {

    #region Variáveis Públicas
    public int Linha { get; set; }
    public int Coluna { get; set; }
    public int Numero { get; set; }
    #endregion

    #region Variáveis Privadas
    private Animator _animator;
    #endregion

    void Start () {
        _animator = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
