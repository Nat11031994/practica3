using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace hidato
{
    class Program
    {
        struct Posicion
        {
            public int fil, col;
        };
        struct Casilla
        {
            public int num;   // número de la casilla (-1: no jugable, 0: vacía)
            public bool fija; // Indica si la casilla es fija (true) o editable (false)
        };
        struct Tablero
        {
            public Casilla[,] cas; // matriz de casillas
            public Posicion cursor;     // posición actual del cursor
        };


        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Posicion posicion;
            Tablero tablero = new Tablero
            {
                cas = new Casilla[8, 8],
                cursor = new Posicion { col = 0, fil = 0 },
            };
            Casilla casilla;


            int[,] mat = {
                { 0, 33, 35,  0,  0, -1, -1, -1},
                { 0,  0, 24, 22,  0, -1, -1, -1},
                { 0,  0,  0, 21,  0,  0, -1, -1},
                { 0, 26,  0, 13, 40, 11, -1, -1},
                {27,  0,  0,  0,  9,  0,  1, -1},
                {-1, -1,  0,  0, 18,  0,  0, -1},
                {-1, -1, -1, -1,  0,  7,  0,  0},
                {-1, -1, -1, -1, -1, -1,  5,  0}
            };
            Inicializa(mat, tablero);
            Render(tablero);
            LeeInput();
        }



        static char LeeInput()
        {
            char d = ' ';

            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey(true).Key.ToString();
                switch (tecla)
                {

                    /* INPUTS ELEMENTALES PARA EL JUEGO BÁSICO */

                    // movimiento del cursor 	
                    case "LeftArrow": d = 'l'; break;
                    case "UpArrow": d = 'u'; break;
                    case "RightArrow": d = 'r'; break;
                    case "DownArrow": d = 'd'; break;

                    // comprobar tablero 
                    case "c": case "C": d = 'c'; break;

                    // terminar juego
                    case "Escape": case "q": case "Q": d = 'q'; break;

                    // ver posibles valores en posicion	
                    default:
                        if (tecla.Length == 2 && tecla[0] == 'D' && tecla[1] >= '0' && tecla[1] <= '9')
                            d = tecla[1];
                        break;
                }
            }
            return d;
        }

        static void Inicializa(int[,] mat, Tablero tab)
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    int numero = mat[i, j];
                    if (numero == -1)
                    {
                        tab.cas[i, j] = new Casilla { num = -1, fija = false };
                    }
                    else if (numero == 0)
                    {
                        tab.cas[i, j] = new Casilla { num = 0, fija = false };
                    }
                    else if (numero > 0)
                    {
                        tab.cas[i, j] = new Casilla { num = mat[i, j], fija = true };
                    }

                }

            }
        }

        static void Render(Tablero tab)
        {
            Console.Clear(); // Limpia la consola antes de imprimir el tablero

            for (int i = 0; i < tab.cas.GetLength(0); i++)
            {
                for (int j = 0; j < tab.cas.GetLength(1); j++)
                {
             
                    if (tab.cas[i, j].num == -1)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = tab.cas[i, j].fija ? ConsoleColor.Black : ConsoleColor.Green;

                      
                        if (tab.cursor.fil == i && tab.cursor.col == j)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                        }

                     
                        if (tab.cas[i, j].num == 0)
                        {
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.Write(tab.cas[i, j].num.ToString().PadLeft(2)); // Alineación
                        }
                    }

                   
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }


        static void ActualizaEstado(char c, ref Tablero tab)
        {
            int nuevaFil = tab.cursor.fil, nuevaCol = tab.cursor.col;

            switch (c)
            {
                case 'l': nuevaCol--; break;
                case 'r': nuevaCol++; break;
                case 'u': nuevaFil--; break;
                case 'd': nuevaFil++; break;
                case 'q': Environment.Exit(0); break; 
            }

            if (nuevaFil >= 0 && nuevaFil < tab.cas.GetLength(0) && nuevaCol >= 0 && nuevaCol < tab.cas.GetLength(1) && tab.cas[nuevaFil, nuevaCol].num != -1)
            {
                tab.cursor.fil = nuevaFil;
                tab.cursor.col = nuevaCol;
            }
        }

        static Posicion Busca1(Tablero tab)
        {
            for (int i = 0; i < tab.cas.GetLength(0); i++)
            {
                for (int j = 0; j < tab.cas.GetLength(1); j++)
                {
                    if (tab.cas[i, j].num == 1)
                    {
                        return new Posicion { fil = i, col = j };
                    }
                }
            }
            return new Posicion { fil = -1, col = -1 }; // No encontrado (no debería ocurrir)
        }


    }
}