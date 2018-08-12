using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    #region Variáveis Privadas
    private int _totalCelulasSelecionadas = 0;
    private GameObject[] _celulasSelecionadas;
    private GeradorSudoku _geradorSudoku;
    #endregion

    // Use this for initialization
    void Start()
    {
        _geradorSudoku = FindObjectOfType(typeof(GeradorSudoku)) as GeradorSudoku;
        _celulasSelecionadas = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        // * Se o total de células selecionadas for menor que 2, então permite selecionar uma outra célula
        if (_totalCelulasSelecionadas < 2)
        {
            SelecionarCelula();
        }
        // * Se o total de células selecionadas for igual a 2, então executa a troca de valores entre elas
        else
        {
            StartCoroutine(ExecutarTroca());
        }
    }

    void SelecionarCelula()
    {
        RaycastHit2D _hit = new RaycastHit2D();

#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            // * Recupera a posição do mouse após o clique
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
#endif
#if UNITY_ANDROID
        //_hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y), Vector2.zero, 0);
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // * Recupera a posição do toque
                _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            }            
        }
#endif

        if (_hit)
        {
            if (_hit.collider.CompareTag("Celula"))
            {                
                Debug.Log("Selecionada célula " + _hit.transform.name);
                // * Recupera o componente script da célula para defini-la como selecionada
                _hit.collider.gameObject.GetComponent<Celula>().Selecionada();
                
                // * Atribui a célula ao array de células selecionadas
                _celulasSelecionadas[_totalCelulasSelecionadas] = _hit.collider.gameObject;

                // * Incrementa o total de células selecionadas
                _totalCelulasSelecionadas++;

                // * Se o total de células selecionadas é menor que 2, então executa corotina para desabilitar as células que não podem ser escolhidas
                if (_totalCelulasSelecionadas < 2)
                    StartCoroutine(DesabilitarCelulas(_hit.collider.gameObject));
            }
        }
    }

    /// <summary>
    /// Corotina para desabilitar as células que não poderão ser selecionadas após a marcação de uma célula durante a jogada.
    /// Serão mantidas habilitadas as células nas posições norte, sul, leste e oeste em relação àquela selecionada, marcando-as como alternativas.
    /// </summary>
    /// <param name="_celula">GameObject da célula selecionada</param>
    /// <returns>Nulo</returns>
    IEnumerator DesabilitarCelulas(GameObject _celula)
    {
        for (int _linha = 0; _linha < _geradorSudoku.TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < _geradorSudoku.TotalColunas; _coluna++)
            {
                // * Recupera o collider da célula
                BoxCollider2D _boxCollider2D = _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<BoxCollider2D>();
                // * Recupera o componente script da célula
                Celula _scriptCelula = _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<Celula>();
                
                // * Verifica se a célula percorrida no laço está na posição norte, sul, leste ou oeste em relação à celula selecionada, em caso positivo
                // * habilita o collider da célula e altera seu estado para "Alternativa"
                if ((_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha - 1 && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna) ||
                    (_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha + 1 && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna) ||
                    (_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna - 1) ||
                    (_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna + 1)) {
                    _boxCollider2D.enabled = true;
                    _scriptCelula.Alternativa();
                }
                // * Caso contrário desabilita o collider da célula percorrida pelo laço
                else
                {
                    _boxCollider2D.enabled = false;
                }
            }
        }

        yield return null; 
    }

    /// <summary>
    /// Corotina para habilitar todas as células e definir o estado como "Normal" após uma jogada.
    /// </summary>
    /// <returns>Nulo</returns>
    IEnumerator HabilitarCelulas()
    {
        for (int _linha = 0; _linha < _geradorSudoku.TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < _geradorSudoku.TotalColunas; _coluna++)
            {
                // * Habilita o collider da célula
                _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<BoxCollider2D>().enabled = true;
                // * Define o estado da célula como "Normal"
                _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<Celula>().Normal();
            }
        }

        yield return null;
    }

    /// <summary>
    /// Corotina para executar a troca dos números entre as células selecionadas após uma jogada
    /// </summary>
    /// <returns>Espera de 1 segundo</returns>
    IEnumerator ExecutarTroca()
    {
        /*for (int _indice = 0; _indice < _celulas.Length; _indice++)
        {
            _celulasSelecionadas[_indice].gameObject.GetComponent<Celula>().Normal();
        }*/

        // * Executa corotina para habilitar todas as células
        StartCoroutine(HabilitarCelulas());

        // * Define o total de células selecionadas como zero
        _totalCelulasSelecionadas = 0;

        yield return new WaitForSeconds(1f);
    }
}
