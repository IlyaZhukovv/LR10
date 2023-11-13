using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер матрицы(неориентированной): ");
            int size = Convert.ToInt32(Console.ReadLine());

            int[,] adjacencyMatrix = GenerateAdjacencyMatrix(size);

            Console.WriteLine("Матрица смежности для неориентированного графа:");
            PrintMatrix(adjacencyMatrix);

            Console.Write("Введите вершину, с которой начать обход: ");
            int startVertex = Convert.ToInt32(Console.ReadLine());

            int[] distances = CalculateDistances(adjacencyMatrix, startVertex);

            Console.WriteLine("Расстояния от исходной вершины:");
            for (int i = 0; i < distances.Length; i++)
            {
                if (distances[i] > 0)
                {
                    Console.WriteLine("Исходная -> " + i + ": " + distances[i]);
                }
                else
                {
                    Console.WriteLine("Исходная -> " + i + ": ");
                }
            }
        }

        //метод генерирует случайную матрицу смежности
        private static int[,] GenerateAdjacencyMatrix(int size)
        {
            Random r = new Random();

            //Создается двумерный массив matrix размером size x size, представляющий матрицу смежности для графа.
            //Все элементы массива инициализируются значением 0.
            int[,] matrix = new int[size, size];

            //Запускается первый цикл for, который итерируется по каждой строке матрицы (вершине).
            for (int i = 0; i < size; i++)
            {
                //Внутри первого цикла запускается второй цикл for, который итерируется по каждому столбцу матрицы, начиная с индекса, следующего за текущим (i+1). Это делается для того, чтобы избежать повторения и дублирования ребер.
                for (int j = i+1; j < size; j++)
                {
                    //Внутри второго цикла проверяется, если текущая строка (i) не равна текущему столбцу (j), то выполняются следующие действия
                    if (i != j)
                    {
                        //Генерируется случайное весовое значение ребра от 1 до 10 с помощью метода r.Next(1, 10).
                        matrix[i, j] = r.Next(1,10);
                        matrix[j, i] = matrix[i, j]; 
                    }
                }
            }
            return matrix;
        }
        //выводит матрицу на экран
        static void PrintMatrix(int[,] matrix)
        {
            int size = matrix.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        //алгоритм поиска кратчайших расстояний в взвешенном неориентированном графе. 
        private static int[] CalculateDistances(int[,] adjacencyMatrix, int startVertex)
        {
            int size = adjacencyMatrix.GetLength(0);
            int[] distances = new int[size];

            //Затем инициализируется массив `distances` размером `size`, в котором будет храниться результат - кратчайшие
            //расстояния от `startVertex` до остальных вершин. Изначально все элементы массива устанавливаются в -1.
            for (int i = 0; i < size; i++)
            {
                distances[i] = -1;
            }
            
            //Далее, расстояние от `startVertex` до самого себя устанавливается равным 0 в массиве `distances`, а `startVertex` добавляется в очередь `queue` с помощью метода `Enqueue`.
            distances[startVertex] = 0;

            Queue<int> queue = new Queue<int>();    
            queue.Enqueue(startVertex);

            //Пока очередь `queue` не станет пустой
            while (queue.Count > 0)
            {
                //извлекаем текущую вершину из очереди с помощью метода `Dequeue`. 
                int currentVertex = queue.Dequeue();

                for (int i = 0; i < size; i++)
                {
                    //Если между текущей вершиной `currentVertex` и вершиной `i` существует ребро (значение в `adjacencyMatrix[currentVertex, i]` не равно 0)
                    //и расстояние до вершины `i` еще не было установлено (значение в `distances[i]` равно -1), то вершина `i` добавляется в очередь `queue`
                    //и устанавливается кратчайшее расстояние до нее, равное сумме расстояния до текущей вершины `currentVertex` и веса ребра между ними `adjacencyMatrix[currentVertex, i]`.
                    if (adjacencyMatrix[currentVertex, i] != 0 && distances[i] == -1)
                    {
                        queue.Enqueue(i);
                        distances[i] = distances[currentVertex] + adjacencyMatrix[currentVertex, i];
                    }
                }
            }
            return distances;
        }
    }
}
