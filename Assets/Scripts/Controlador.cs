using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{
    #region Variáveis Privadas
    private int _totalCelulasSelecionadas = 0;
    private GameObject[] _celulas;
    private GeradorSudoku _geradorSudoku;
    #endregion

    // Use this for initialization
    void Start()
    {
        _geradorSudoku = FindObjectOfType(typeof(GeradorSudoku)) as GeradorSudoku;
        _celulas = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (_totalCelulasSelecionadas < 2)
        {
            SelecionarCelula();
        }
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
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
#endif
#if UNITY_ANDROID
        //_hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y), Vector2.zero, 0);
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            }            
        }
#endif

        if (_hit)
        {
            if (_hit.collider.CompareTag("Celula"))
            {                
                Debug.Log("Selecionada célula " + _hit.transform.name);
                _hit.collider.gameObject.SendMessage("Selecionar");
                _celulas[_totalCelulasSelecionadas] = _hit.collider.gameObject;
                StartCoroutine(DesabilitarCelulas(_hit.collider.gameObject));
                _totalCelulasSelecionadas++;
            }
        }
    }

    IEnumerator DesabilitarCelulas(GameObject _celula)
    {
        for (int _linha = 0; _linha < _geradorSudoku.TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < _geradorSudoku.TotalColunas; _coluna++)
            {
                Celula _scriptCelula = _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<Celula>();
                /** TODO *
                 Deixar habilitadas somente as 4 células nas direções norte, sul, leste e oeste da célula selecionada
                 */
                if (
                    (_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha - 1 && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna) ||
                    (_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha + 1 && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna) ||
                    (_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna - 1) ||
                    (_scriptCelula.Linha == _celula.GetComponent<Celula>().Linha && _scriptCelula.Coluna == _celula.GetComponent<Celula>().Coluna + 1)
                    ) {
                    _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    _scriptCelula.Alternativa();
                }
                else
                {
                    _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }

                
                //print(_geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<Celula>().Linha +" - "+ _geradorSudoku.ListaCelulas[_linha, _coluna].gameObject.GetComponent<Celula>().Coluna);
            }
        }

        yield return null; 
    }

    IEnumerator ExecutarTroca()
    {
        for (int _indice = 0; _indice < _celulas.Length; _indice++)
        {
            _celulas[_indice].SendMessage("Selecionar");
        }
        _totalCelulasSelecionadas = 0;
        yield return new WaitForSeconds(1f);
    }
}
