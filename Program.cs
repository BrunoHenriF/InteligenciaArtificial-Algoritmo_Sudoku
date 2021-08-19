using System;
using System.Collections.Generic;
using System.IO;

namespace Sudoku_IA
{
    class Sudoku_IA
    {


        //função que lê o arquivo txt e retorna um vetor de strings onde cada elemento é uma linha do txt
        public static string[] Le_arquivo()
        {
            string[] sudokus = File.ReadAllLines("top50.txt"); //caso queira mudar o arquivo lido, deverá mudar o parâmetro
            return sudokus;
        }

        //função que transforma uma string em um vetor de char explícito (C# voltando às raizes de C hehe)
        public static char[] Transforma_em_vetor(string minha_string)
        {
            char[] vetor_char = new char[minha_string.Length];

            for (int i = 0; i < minha_string.Length; i++)
            {
                vetor_char[i] = minha_string[i];
            }
            return vetor_char;
        }

        //função que, ao receber a posição de uma célula no vetor, descobre a linha que a mesma se encontra
        public static int Descobre_linha(int posicao_celula)
        {
            double n = posicao_celula / 9;
            int linha = (int)n;
            return linha;
            //pego a posição da célula, divido por 10 e a parte inteira será minha linha
        }

        //função que,ao receber a posição de uma célula no vetor, descobre a coluna que a mesma se encontra
        public static int Descobre_coluna(int posicao_celula)
        {
            int coluna = posicao_celula % 9;
            return coluna;
            //pego a posição da célula, divido por 9 e o resto será minha coluna
        }

        //função que checa se determinado número se encontra na coluna de uma célula
        public static bool Checa_coluna(char numero, int posicao_celula, char[] sudoku)
        {
            int coluna = Descobre_coluna(posicao_celula);
            bool flag = true;
            for (int i = coluna; i < sudoku.Length; i += 9)
            {
                if (sudoku[i] != '.' && sudoku[i] == numero)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        //função que checa se determinado número se encontra na linha de uma célula
        public static bool Checa_linha(char numero, int posicao_celula, char[] sudoku)
        {
            int coluna = Descobre_coluna(posicao_celula);
            bool flag = true;
            int n = posicao_celula - coluna;
            for (int i = n; i < n + 9; i++)
            {
                if (sudoku[i] != '.' && sudoku[i] == numero)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        public static int Descobre_quadrante(int posicao_celula)
        {
            int coluna = Descobre_coluna(posicao_celula);
            int linha = Descobre_linha(posicao_celula);

            int x = coluna / 3;
            int y = linha / 3;

            int x_quad = (int)x;
            int y_quad = (int)y;
            //dividindo por 3 as coordenadas linha e coluna da célula, obtém-se um valor com parte inteira variando de 1 a 3 

            int posicao_quadrante = y_quad * 27 + x_quad * 3; /*usando os valores obtidos acima e fazendo essa manipulação algébrica,
            chega-se na posição do primeiro elemento do quadrante que contém a célula passada por parâmetro*/

            return posicao_quadrante;
        }


        //função que checa se determinado número se encontra no quadrante de uma célula
        public static bool Checa_quadrante(char numero, int posicao_celula, char[] sudoku)
        {

            int posicao_quadrante = Descobre_quadrante(posicao_celula);
            bool resultado = Percorre_quadrante(numero, posicao_quadrante, sudoku);
            return resultado;
        }

        //função que percorre um quadrante de uma determinada célula
        public static bool Percorre_quadrante(char numero, int posicao_quadrante, char[] sudoku)
        {
            int posicao = posicao_quadrante; //o valor de posicao_quadrante é obtido pela função Checa_quadrante, mas é passado por parâmetro
            bool flag = true;

            for (int i = 0; i < 3; i++)
            {
                for (int j = posicao; j < posicao + 3; j++)
                {
                    if (sudoku[j] != '.' && sudoku[j] == numero)
                    {
                        flag = false;
                        break;
                    }
                }
                posicao += 9;
            }
            return flag;
        }

        //função booleana que retorna se o um número pode ou não ser inserido em determinada posição
        public static bool Numero_valido(char numero, int posicao_celula, char[] sudoku)
        {
            return Checa_linha(numero, posicao_celula, sudoku) && Checa_coluna(numero, posicao_celula, sudoku) && Checa_quadrante(numero, posicao_celula, sudoku);
        }

        //função que copia um vetor elemento por elemento
        public static char[] Copia_vetor(char[] vetor, char[] vetor_copia)
        {
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor_copia[i] = vetor[i];
            }
            return vetor_copia;
        }

        /* função que, recebendo um vetor sudoku já preenchido por um estado anterior - pela função Insere_estado() -, 
        retorna os possíveis números a serem colocados em determinada posicao_celula */
        public static List<char> Possiveis_numeros(char[] sudoku, int posicao_celula)
        {
            List<char> possiveis_numeros = new List<char>();
            char possivel_numero;

            for (int j = 1; j <= 9; j++)
            {
                possivel_numero = j.ToString()[0];
                if (Numero_valido(possivel_numero, posicao_celula, sudoku))//caso o número seja válido na posição da célula, ele é adicionado à lista
                {
                    possiveis_numeros.Add(possivel_numero);
                }
            }
            return possiveis_numeros;
        }

        //função que insere os números de um determinado estado no vetor sudoku_copia e retorna o sudoku_copia
        public static char[] Insere_estado(char[] sudoku_original, string estado)
        {
            char[] sudoku_copia = new char[sudoku_original.Length];
            sudoku_copia = Copia_vetor(sudoku_original, sudoku_copia);

            int contador = 0, coordenada = 0;

            while (contador < estado.Length)//o contador serve para contar quantos números já foram inseridos nas células em branco
            {
                if (sudoku_copia[coordenada] == '.')
                {
                    sudoku_copia[coordenada] = estado[contador];
                    contador++;
                }
                coordenada++;
            }
            return sudoku_copia;
        }

        //Percorre o vetor e retorna a coordenada do primeiro espaço vazio ou -1 caso não haja espaços vazios
        public static int Percorre_sudoku(char[] sudoku)
        {
            for (int i = 0; i < sudoku.Length; i++)
            {
                if (sudoku[i] == '.')
                    return i;
            }
            return -1;
        }


        //função que enfileira os novos estados obtidos pela concatenação do estado atual com os números que podem ser inseridos na célula
        public static Queue<string> Enfileira(Queue<string> fila_estados, List<char> possiveis_numeros, string estado)
        {
            foreach (var numero in possiveis_numeros)
            {
                fila_estados.Enqueue(estado + numero.ToString());//estados seguintes vão para o final da fila
            }
            return fila_estados;
        }

        //função que empilha os novos estados obtidos pela concatenação do estado atual com os números que podem ser inseridos na célula
        public static Stack<string> Empilha(Stack<string> pilha_estados, List<char> possiveis_numeros, string estado)
        {
            foreach (var numero in possiveis_numeros)
            {
                pilha_estados.Push(estado + numero.ToString());//estados seguintes vão para o topo da pilha
            }
            return pilha_estados;
        }


        //função DFS iterativa
        public static void Busca_em_produndidade(char[] sudoku_original, string estado, Stack<string> pilha_estados) //a pilha de estados é declarada no main
        {
            int coordenada_vazia = -2; //inicializa a variável em -2 pois é um número inacessível no vetor e diferente de -1 
            char[] sudoku_copia = new char[sudoku_original.Length]; //cria-se uma cópia do sudoku para que não se manipule o original
            sudoku_copia = Copia_vetor(sudoku_original, sudoku_copia);

            List<char> numeros_possiveis;//declara a lista que irá guardar os possíveis números de cada iteração

            while (coordenada_vazia != -1) //quando a coordenada vazia for -1, significa que não há nenhuma coordenada em que a célula é vazia
            {
                if (estado != null)//o estado é null na primeira iteração da função, por isso jamais poderia ser usado em Insere_estado()
                    sudoku_copia = Insere_estado(sudoku_original, estado);

                coordenada_vazia = Percorre_sudoku(sudoku_copia);

                if (coordenada_vazia == -1)
                {
                    Console.WriteLine(sudoku_copia);//imprime o vetor sudoku_copia que é a solução para meu sudoku
                    break;
                }
                numeros_possiveis = Possiveis_numeros(sudoku_copia, coordenada_vazia);

                if (numeros_possiveis.Count != 0) //caso haja possíveis números, eles serão adicionados à minha pilha de estados
                {
                    Empilha(pilha_estados, numeros_possiveis, estado);
                    estado = pilha_estados.Pop();
                    continue;
                }
                else //se não houver, o estado atual é descartado e o próximo da pilha é chamado e usado na próxima iteração
                {
                    estado = pilha_estados.Pop();
                    continue;
                }
            }
        }


        //função BFS iterativa, a lógica é a mesma da DFS, mas ao invés de usar uma pilha, usa uma fila
        public static void Busca_em_largura(char[] sudoku_original, string estado, Queue<string> fila_estados)
        {
            int coordenada_vazia = -2;
            char[] sudoku_copia = new char[sudoku_original.Length];
            sudoku_copia = Copia_vetor(sudoku_original, sudoku_copia);

            List<char> numeros_possiveis;//declara a lista que irá guardar os possíveis números de cada iteração

            while (coordenada_vazia != -1)
            {
                if (estado != null)//na primeira chamada, o estado é null, portanto não se pode inserir um estado null no sudoku
                    sudoku_copia = Insere_estado(sudoku_original, estado);

                coordenada_vazia = Percorre_sudoku(sudoku_copia);

                if (coordenada_vazia == -1)//caso a coordenada vazia seja -1, significa que o sudoku está finalizado
                {
                    Console.WriteLine(sudoku_copia);
                    break;
                }
                numeros_possiveis = Possiveis_numeros(sudoku_copia, coordenada_vazia);

                if (numeros_possiveis.Count != 0) //caso haja números possíveis, são adicionados na fila de estados
                {
                    Enfileira(fila_estados, numeros_possiveis, estado);
                    estado = fila_estados.Dequeue();
                    continue;
                }
                else //caso contrário, não há adição e reatribui ao estado o próximo valor da fila
                {
                    estado = fila_estados.Dequeue();
                    continue;
                }
            }
        }

/*
        //função que determina os domínios para cada variável e retorna um dicionário contendo as variáveis e seus respectivos domínios
        public static Dictionary<int, List<char>> Dominios(char[] sudoku)
        {
            List<char> valores;
            Dictionary<int, List<char>> variaveis_dominio = new Dictionary<int, List<char>>();

            for (int i = 0; i < sudoku.Length; i++)
            {
                if (sudoku[i] == '.')//checa todos os espaços em branco, que serão as variáveis
                {
                    valores = Possiveis_numeros(sudoku, i);//checa os valores possíveis naquele espaço
                    variaveis_dominio.Add(i, valores);//adiciona na lista de domínios de cada variável
                }
            }
            return variaveis_dominio;
        }


        //função que, para cada variável, verifica suas restrições e devolve um dicionário contendo as variáveis e suas listas de restrições
        public static Dictionary<int, List<(int, int)>> Restricoes(char[] sudoku, Dictionary<int, List<char>> variaveis_dominio)
        {
            List<(int, int)> lista_restricoes = new List<(int, int)>();
            Dictionary<int, List<(int, int)>> variaveis_restricoes = new Dictionary<int, List<(int, int)>>();

            foreach (var variavel in variaveis_dominio.Keys)
            {
                int linha = Descobre_linha(variavel);
                int coluna = Descobre_coluna(variavel);
                int quadrante = Descobre_quadrante(variavel);//descobre-se a linha, coluna e quadrante da variável

                //os três laços são utilizados para verificar todas as variáveis que estão na mesma linha, coluna e quadrante da variável

                int n = linha - coluna;
                for (int a = n; a < n + 9; a++)
                {
                    if (sudoku[a] == '.' && a != variavel)
                    {
                        (int, int) t = (variavel, a);//armazena a variável que pode ter restrição em uma tupla
                        lista_restricoes.Add(t);//adiciona a tupla em uma lista de tuplas
                    }
                }

                for (int b = coluna; b < sudoku.Length; b += 9)
                {
                    if (sudoku[b] == '.' && b != variavel)
                    {
                        (int, int) t = (variavel, b);//armazena a variável que pode ter restrição em uma tupla
                        lista_restricoes.Add(t);//adiciona a tupla em uma lista de tuplas
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = quadrante; j < quadrante + 3; j++)
                    {
                        if (sudoku[j] == '.' && j != variavel)
                        {
                            (int, int) t = (variavel, j);//armazena a variável que pode ter restrição em uma tupla
                            lista_restricoes.Add(t);//adiciona a tupla em uma lista de tuplas
                        }
                    }
                    quadrante += 9;
                }
                variaveis_restricoes.Add(variavel, lista_restricoes);
            }

            return variaveis_restricoes;
        }

        //função que verifica a variável com maior número restrições
        public static int Maior_grau(Dictionary<int, List<(int, int)>> variaveis_restricao)
        {
            int maior, atual;
            int var_maior_grau = 0;//aloca-se uma variável qualquer inicilamente como de maior grau

            maior = variaveis_restricao[var_maior_grau].Count;

            foreach (var variavel in variaveis_restricao.Keys)
            {
                atual = variaveis_restricao[variavel].Count;//verifica-se o número de restrições de uma variável
                if (atual > maior)
                {
                    maior = atual;
                    var_maior_grau = variavel;//caso o número de restrições seja maior do que a variável anterior, atribui-se um novo valor
                }                               
            }
            return var_maior_grau;//retorna a variávle de maior grau
        }


        /*
        public static void AC3((Dictionary<int, List<char>>, List<(int, int)>) csp)
        {
            Queue<(int, int)> fila = new Queue<(int, int)>();

            foreach (var item in csp.Item2)
            {
                fila.Enqueue(item);
            }

            while (fila.Count != 0)
            {
                (int i, int j) arco = fila.Dequeue();
               // bool revisou = Revisa(arco, csp);
               // if (revisou)
                {
                    continue;
                }
            }
            return;
        }

        public static bool Revisa((int i, int j) arco, (Dictionary<int, List<char>>, List<(int, int)>) csp)
        {
            bool flag = false;
            foreach (var valor1 in csp.Item1[arco.i])
            {
                foreach (var valor2 in arco.j)
                {
                    if (valor1 == valor2)
                    {
                        //retiro o valor do domínio da variavel arco.i
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        } 



        public static void Backtracking(char[] sudoku_original, string estado)
        {
            int coordenada_vazia = -2; //inicializa a variável em -2 pois é um número inacessível no vetor e diferente de -1 
            char[] sudoku_estado = new char[sudoku_original.Length]; //cria-se uma cópia do sudoku para que não se manipule o original
            sudoku_estado = Copia_vetor(sudoku_original, sudoku_estado);

            var variaveis_dominio = Dominios(sudoku_original);
            var variaveis_restricao = Restricoes(sudoku_original, variaveis_dominio);

            int celula;

            while (coordenada_vazia != -1)
            {

                celula = Maior_grau(variaveis_restricao);

                foreach (var valor in variaveis_dominio[celula])
                {
                    sudoku_estado[celula] = variaveis_dominio[celula][0];

                }

                variaveis_dominio = Dominios(sudoku_estado);
                variaveis_restricao = Restricoes(sudoku_estado, variaveis_dominio);

            }

        }
         */


        static void Main(string[] args)
        {
            string[] jogos;
            jogos = Le_arquivo();//leio as linhas do arquivo .txt e salvo no vetor jogos
                                               //caso queira mudar o arquivo, deverá mudar na função Le_arquivo()

            Queue<string> fila_estado = new Queue<string>();  //inicialização da fila usada no BFS
            Stack<string> pilha_estado = new Stack<string>(); //inicialização da pilha usada no DFS

            Console.WriteLine("Escolha o tipo de busca: ");
            Console.WriteLine("[1] - BFS, [2] - DFS");

            string resposta = Console.ReadLine();
            Console.WriteLine();

            switch (resposta)
            {
                case "1"://chama o busca em largura

                    
                    foreach (var item in jogos)
                    {
                        char[] sudoku = Transforma_em_vetor(item);
                        Busca_em_largura(sudoku, null, fila_estado);
                    }
                    break;

                case "2"://chama o busca em profundidade

                   
                    foreach (var item in jogos)
                    {
                        char[] sudoku = Transforma_em_vetor(item);
                        Busca_em_produndidade(sudoku, null, pilha_estado);
                    }
                    break;

                default:
                    Console.WriteLine("Resposta inválida");
                    break;
            }
            Console.ReadLine();
        }
    }
}


