﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorSudoku : MonoBehaviour {

    [SerializeField] int TotalLinhas = 9;
    [SerializeField] int TotalColunas = 9;
    private int[,] _sudoku;
    private int[] _numeros;


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



    // Use this for initialization
    void Start () {
        GerarSudoku();
	}

    public void GerarSudoku()
    {
        _sudoku = new int[TotalLinhas, TotalColunas];
        _numeros = new int[TotalLinhas];

        //_sudoku = TESTE;

        IniciarNumeros();
        IniciarSudoku();
        PopularSudoku();
        ImprimirSudoku();
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
                _sudoku[_linha, _coluna] = 0;
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
                    if (PreencherCelula(_linha, _coluna))
                        _definidos++;
                }
            }

            if (_definidos != _totalCelulas)
            {
                _definidos = 0;
                IniciarSudoku();
            }
        }

        Randomizar(_sudoku);
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
                if (ValidarLinha(_sudoku[_linha, _coluna], _linha, _coluna) || ValidarColuna(_sudoku[_linha, _coluna], _coluna, _linha) || ValidarQuadrante(_sudoku[_linha, _coluna], _linha, _coluna))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Preencher as células com números aleatórios para formação do jogo 
    /// </summary>
    /// <param name="_linha">Número da linha da célula</param>
    /// <param name="_coluna">Número da coluna da célula</param>
    bool PreencherCelula(int _linha, int _coluna)
    {
        //System.Random _random = new System.Random();
        //_numeros = _numeros.OrderBy(x => _random.Next()).ToArray();

        for (int _indice = 0; _indice < _numeros.Length; _indice++)
        {
            if (!VerificarLinha(_numeros[_indice], _linha) && !VerificarColuna(_numeros[_indice], _coluna) && !VerificarQuadrante(_numeros[_indice], _linha, _coluna))
            {
                _sudoku[_linha, _coluna] = _numeros[_indice];
                return true;
            }
        }

        return false;
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
            if (_sudoku[_linha, _coluna] == _numero)
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
            if (_sudoku[_linha, _coluna] == _numero)
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
                if (_sudoku[_l, _c] == _numero)
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
                if (_sudoku[_linha, _c] == _numero)
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
                if (_sudoku[_l, _coluna] == _numero)
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
                    if (_sudoku[_l, _c] == _numero)
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
    void ImprimirSudoku()
    {
        string sud = "";
        for (int _linha = 0; _linha < TotalLinhas; _linha++)
        {
            for (int _coluna = 0; _coluna < TotalColunas; _coluna++)
            {
                if (_coluna != 0) sud += ";";
                sud += _sudoku[_linha, _coluna];
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
