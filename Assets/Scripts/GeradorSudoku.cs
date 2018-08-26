using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorSudoku : MonoBehaviour {

    #region Variáveis Públicas
    public readonly int TotalLinhas = 9;
    public readonly int TotalColunas = 9;
    public GameObject[,] ListaCelulas { get; set; }
    public int[,] Sudoku;
    [SerializeField] GameObject Celulas;
    [SerializeField] GameObject Celula;
    #endregion

    #region Variáveis Privadas
    private int[] _numeros;
    #endregion


    [SerializeField]
    int[,] TESTE = {{6,2,1,7,5,4,8,9,3}
                    ,{4,8,9,3,6,2,1,7,5}
                    ,{3,7,5,1,8,9,4,6,2}
                    ,{1,6,2,4,7,5,3,8,9}
                    ,{9,3,7,2,1,8,5,4,6}
                    ,{8,5,4,6,9,3,7,2,1}
                    ,{2,1,6,5,4,7,9,3,8}
                    ,{7,9,3,8,2,1,6,5,4}
                    ,{5,4,8,9,3,6,2,1,7}};



    void Start () {
        GerarSudoku();
    }

    void Update()
    {
        if (ValidarSudoku())
        {
            Debug.Log("Parabéns!!");
        }
        else
        {
            Debug.Log("Ainda não...");
        }
    }

    /// <summary>
    /// Executa as chamada aos métodos para inicialização e geração do jogo
    /// </summary>
    public void GerarSudoku()
    {
        // * Inicializa os arrays
        Sudoku = new int[TotalLinhas, TotalColunas];
        ListaCelulas = new GameObject[TotalLinhas, TotalColunas];
        _numeros = new int[TotalLinhas];

        Sudoku = TESTE;

        MapearTabuleiro();
        /*IniciarNumeros();
        IniciarSudoku();
        PopularSudoku();*/
        PreencherNumeroCelula();
        ImprimirSudoku();
    }

    /// <summary>
    /// Mapeia o tabuleiro do jogo com as imagens
    /// </summary>
    void MapearTabuleiro()
    {
        // * Recupera a largura do sprite
        float _larguraCelula = Celula.GetComponent<SpriteRenderer>().bounds.size.x;
        // * Recupera a altura do sprite
        float _alturaCelula = Celula.GetComponent<SpriteRenderer>().bounds.size.y;
        // * Define o ponto de spawn inicial no eixo X
        float _pontoSpawnX = Celulas.transform.position.x - (TotalColunas / 2) * _larguraCelula;
        float _pontoSpawnXInicial = _pontoSpawnX;
        // * Define o ponto de spawn inicial no eixo Y
        float _pontoSpawnY = Celulas.transform.position.y + (TotalLinhas / 2) * _alturaCelula;

        for (int _linha = 0; _linha < TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
            {
                // * Instância o GameObject Celula (Prefab)
                GameObject _celula = Instantiate(Celula) as GameObject;
                // * Adiciona ao GameObject pai Celulas
                _celula.transform.parent = Celulas.transform;
                // * Define o posicionamento dentro do objeto pai de acordo com as coordenadas
                _celula.transform.position = new Vector2(_pontoSpawnX, _pontoSpawnY);
                // * Renomeia o GameObject
                _celula.transform.name = "Celula_" + _linha.ToString() + "_" + _coluna.ToString();
                // * Recupera o componente script para definição dos números da linha e coluna da célula
                _celula.GetComponent<Celula>().Linha = _linha;
                _celula.GetComponent<Celula>().Coluna = _coluna;

                // * Adiciona o GameObject a um array para utlização posterior
                ListaCelulas[_linha, _coluna] = _celula;

                _pontoSpawnX += _larguraCelula;
            }
            _pontoSpawnY -= _alturaCelula;
            _pontoSpawnX = _pontoSpawnXInicial;
        }
    }

    /// <summary>
    /// Inicializar todas as posições do array com zero
    /// </summary>
    void IniciarSudoku()
    {
        for (int _linha = 0; _linha < TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
            {
                Sudoku[_linha, _coluna] = 0;
            }
        }        
    }

    /// <summary>
    /// Inicializar o array de números utilizados no jogo
    /// </summary>
    void IniciarNumeros()
    {
        for (int _numero = 0; _numero < TotalLinhas; _numero++)
        {
            _numeros[_numero] = _numero + 1;
        }
    }

    /// <summary>
    /// Realiza a definição do jogo
    /// </summary>
    void PopularSudoku()
    {
        int _definidos = 0;
        int _totalCelulas = TotalLinhas * TotalColunas;

        while (_definidos < _totalCelulas)
        {
            for (int _linha = 0; _linha < TotalLinhas; _linha++)
            {
                Randomizar(_numeros);

                for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
                {
                    if (DefinirNumeroCelula(_linha, _coluna))
                        _definidos++;
                }
            }

            if (_definidos != _totalCelulas)
            {
                _definidos = 0;
                IniciarSudoku();
            }
        }

        Randomizar(Sudoku);
    }

    /// <summary>
    /// Realiza a validação do sudoku
    /// </summary>
    /// <returns>Verdadeiro se o jogo estiver correto e falso caso contrário</returns>
    bool ValidarSudoku()
    {
        for (int _linha = 0; _linha < TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
            {
                if (ValidarLinha(Sudoku[_linha, _coluna], _linha, _coluna) || ValidarColuna(Sudoku[_linha, _coluna], _coluna, _linha) || ValidarQuadrante(Sudoku[_linha, _coluna], _linha, _coluna))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Define um determinado número aleatório para a célula
    /// </summary>
    /// <param name="_linha">Número da linha da célula</param>
    /// <param name="_coluna">Número da coluna da célula</param>
    bool DefinirNumeroCelula(int _linha, int _coluna)
    {
        for (int _indice = 0; _indice < _numeros.Length; _indice++)
        {
            if (!VerificarLinha(_numeros[_indice], _linha) && !VerificarColuna(_numeros[_indice], _coluna) && !VerificarQuadrante(_numeros[_indice], _linha, _coluna))
            {
                Sudoku[_linha, _coluna] = _numeros[_indice];
                return true;
            }
        }

        return false;
    }    

    /// <summary>
    /// Preenche cada célula com seu número correspondente após randomizado o tabuleiro
    /// </summary>
    void PreencherNumeroCelula()
    {
        for (int _linha = 0; _linha < TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
            {
                ListaCelulas[_linha, _coluna].GetComponent<Celula>().Numero = Sudoku[_linha, _coluna];
            }
        }
    }

    /// <summary>
    /// Verifica se o número já existe em determinada linha
    /// </summary>
    /// <param name="_numero">Número a ser pesquisado</param>
    /// <param name="_linha">Linha onde o número deve ser pesquisado</param>
    /// <returns>Retorna verdadeiro caso o número seja encontrado e falso caso contrário</returns>
    bool VerificarLinha(int _numero, int _linha)
    {
        for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
        {
            if (Sudoku[_linha, _coluna] == _numero)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Verifica se o número já existe em determinada coluna
    /// </summary>
    /// <param name="_numero">Número a ser pesquisado</param>
    /// <param name="_coluna">Coluna onde o número deve ser pesquisado</param>
    /// <returns>Retorna verdadeiro caso o número seja encontrado e falso caso contrário</returns>
    bool VerificarColuna(int _numero, int _coluna)
    {
        for (int _linha = 0; _linha < TotalLinhas; _linha++)
        {
            if (Sudoku[_linha, _coluna] == _numero)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Verifica se o número já existe em determinado quadrante
    /// </summary>
    /// <param name="_numero">Número a ser pesquisado</param>
    /// <param name="_linha">Linha onde a célula do quadrante está localizada</param>
    /// <param name="_coluna">Coluna onde a célula do quadrante está localizada</param>
    /// <returns></returns>
    bool VerificarQuadrante(int _numero, int _linha, int _coluna)
    {
        int _linhaMinimo = (int) Mathf.Floor(_linha / 3) * 3;
        int _linhaMaximo = _linhaMinimo + 2;
        int _colunaMinimo = (int) Mathf.Floor(_coluna / 3) * 3;
        int _colunaMaximo = _colunaMinimo + 2;

        for (int _l = _linhaMinimo; _l <= _linhaMaximo; _l++)
        {
            for (int _c = _colunaMinimo; _c <= _colunaMaximo; _c++)
            {
                if (Sudoku[_l, _c] == _numero)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Valida se o número já existe em determinada linha
    /// </summary>
    /// <param name="_numero">Número a ser pesquisado</param>
    /// <param name="_linha">Linha onde o número deve ser pesquisado</param>
    /// <param name="_coluna">Coluna onde a célula do quadrante está localizada</param>
    /// <returns>Retorna verdadeiro se o número seja encontrado e falso caso contrário</returns>
    bool ValidarLinha(int _numero, int _linha, int _coluna)
    {
        for (int _c = 0; _c < TotalColunas; _c++)
        {
            if (_c != _coluna)
            {
                if (Sudoku[_linha, _c] == _numero)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Valida se o número já existe em determinada coluna
    /// </summary>
    /// <param name="_numero">Número a ser pesquisado</param>
    /// <param name="_coluna">Coluna onde o número deve ser pesquisado</param>
    /// <param name="_linha">Linha onde a célula do quadrante está localizada</param>
    /// <returns>Retorna verdadeiro se o número seja encontrado e falso caso contrário</returns>
    bool ValidarColuna(int _numero, int _coluna, int _linha)
    {
        for (int _l = 0; _l < TotalLinhas; _l++)
        {
            if (_l != _linha)
            {
                if (Sudoku[_l, _coluna] == _numero)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Valida se o número já existe em determinado quadrante
    /// </summary>
    /// <param name="_numero">Número a ser pesquisado</param>
    /// <param name="_linha">Linha onde a célula do quadrante está localizada</param>
    /// <param name="_coluna">Coluna onde a célula do quadrante está localizada</param>
    /// <returns>Retorna verdadeiro se o número for encontrado e falso caso contrário</returns>
    bool ValidarQuadrante(int _numero, int _linha, int _coluna)
    {
        int _linhaMinimo = (int)Mathf.Floor(_linha / 3) * 3;
        int _linhaMaximo = _linhaMinimo + 2;
        int _colunaMinimo = (int)Mathf.Floor(_coluna / 3) * 3;
        int _colunaMaximo = _colunaMinimo + 2;

        for (int _l = _linhaMinimo; _l <= _linhaMaximo; _l++)
        {
            for (int _c = _colunaMinimo; _c <= _colunaMaximo; _c++)
            {
                if (_l != _linha && _c != _coluna)
                {
                    if (Sudoku[_l, _c] == _numero)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Realiza a exibição dos números definidos para o jogo
    /// </summary>
    public void ImprimirSudoku()
    {
        string sud = "";
        for (int _linha = 0; _linha < TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
            {
                if (_coluna != 0) sud += ";";
                sud += Sudoku[_linha, _coluna];
            }
            sud += "\n";
        }
        print(sud);
    }

    /// <summary>
    /// Randomiza um array
    /// </summary>
    /// <typeparam name="T">Tipo do array</typeparam>
    /// <param name="_array">Array a ter os valores randomizados</param>
    public void Randomizar<T>(IList<T> _array)
    {
        System.Random _random = new System.Random();

        for (int _x = _array.Count; _x > 1;)
        {
            int _y = _random.Next(_x);
            --_x;
            T temp = _array[_x];
            _array[_x] = _array[_y];
            _array[_y] = temp;
        }
    }

    /// <summary>
    /// Ramdomiza um array bidimensional
    /// </summary>
    /// <typeparam name="T">Tipo do array</typeparam>
    /// <param name="_array">Array a ter os valores randomizados</param>
    public static void Randomizar<T>(T[,] _array)
    {
        System.Random _random = new System.Random();
        int _tamanhoLinha = _array.GetLength(1);

        for (int _x = _array.Length - 1; _x > 0; _x--)
        {
            int _x0 = _x / _tamanhoLinha;
            int _x1 = _x % _tamanhoLinha;

            int _y = _random.Next(_x + 1);
            int _y0 = _y / _tamanhoLinha;
            int _y1 = _y % _tamanhoLinha;

            T temp = _array[_x0, _x1];
            _array[_x0, _x1] = _array[_y0, _y1];
            _array[_y0, _y1] = temp;
        }
    }



}
