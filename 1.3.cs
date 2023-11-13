using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер матрицы(ориентированной): ");
            int size = Convert.ToInt32(Console.ReadLine());

            int[][] graph = GenerateWeightedGraph(size); 
            Console.WriteLine("Матрица смежности для ориентированного графа:");
            PrintMatrix(graph);
            Console.WriteLine();
            Console.Write("Введите вершину, с которой начать обход: ");
            int startVertex = Convert.ToInt32(Console.ReadLine());
            int[] distances = FindDistances(graph, startVertex); 
            Console.WriteLine("Расстояния от исходной вершины:");

            //С помощью цикла for выводятся расстояния от заданной вершины до каждой вершины графа. Выводится номер вершины и ее расстояние от заданной вершины.
            for (int i = 0; i < distances.Length; i++)
            {
                Console.WriteLine("Исходная -> " + i + ": " + distances[i]);
            }
        }
        //Данный код генерирует случайный взвешенный граф и возвращает его в виде матрицы смежности.
        static int[][] GenerateWeightedGraph(int vertices)
        {
            Random random = new Random();

            //Создается двумерный массив graph размером vertices x vertices.
            int[][] graph = new int[vertices][];

            //Запускается цикл for, который итерируется по каждой вершине графа.
            for (int i = 0; i < vertices; i++)
            {
                //Внутри первого цикла создается одномерный массив размером vertices,
                //который является строкой матрицы смежности для текущей вершины.
                //Таким образом, каждая вершина имеет свою строку в матрице смежности.
                graph[i] = new int[vertices];

                //Запускается второй цикл for, который итерируется по каждому столбцу матрицы (вершине).
                for (int j = 0; j < vertices; j++)
                {
                    //Внутри второго цикла проверяется, если текущая вершина (i) равна текущему столбцу (j), то весовое значение устанавливается равным 0. Это означает, что между вершиной и самой собой нет ребра.
                    if (i == j)
                        graph[i][j] = 0; // Нет петли

                    //Иначе, генерируется случайное весовое значение от 1 до 10 с помощью метода random.Next(1, 10), и это значение присваивается весовому значению ребра между вершинами i и j.
                    else
                        graph[i][j] = random.Next(1, 10); // Случайное весовое значение от 1 до 10
                }
            }
            return graph;
        }
        //вывод матрицы на экран
        static void PrintMatrix(int[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        //метод реализует алгоритм поиска расстояний от заданной вершины до всех остальных вершин графа.
        static int[] FindDistances(int[][] graph, int startVertex)
        {
            //Создаются переменные vertices для хранения количества вершин графа
            int vertices = graph.Length;

            //distances для хранения расстояний от стартовой вершины до остальных вершин
            int[] distances = new int[vertices];

            //visited для отслеживания посещенных вершин.
            bool[] visited = new bool[vertices];

            //С помощью цикла for инициализируются значения в массивах distances и visited.
            //Для всех вершин значения расстояний устанавливаются равными -1 (что означает недостижимость),
            //а значения в массиве visited устанавливаются равными false (вершины не посещены).
            for (int i = 0; i < vertices; i++)
            {
                distances[i] = -1;
                visited[i] = false;
            }

            //Значение расстояния от стартовой вершины до самой себя устанавливается равным 0, а сама вершина помечается как посещенная.
            distances[startVertex] = 0;
            visited[startVertex] = true;

            //Создается очередь queue (тип Queue<int>), в которую помещается стартовая вершина с помощью метода Enqueue.
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(startVertex);

            //запускается цикл while, который выполняется, пока очередь не пуста.
            while (queue.Count > 0)
            {
                //Внутри цикла извлекается вершина из начала очереди с помощью метода Dequeue и сохраняется в переменной currentVertex.
                int currentVertex = queue.Dequeue();

                //С помощью цикла for проверяются все вершины графа
                for (int i = 0; i < vertices; i++)
                {
                    //Если существует ребро от currentVertex до вершины i и вершина i не была посещена ранее, то вершина i добавляется в очередь,
                    //помечается как посещенная, а значение расстояния до нее равно сумме расстояния до currentVertex и веса ребра между этими вершинами.
                    if (graph[currentVertex][i] > 0 && !visited[i])
                    {
                        queue.Enqueue(i);
                        visited[i] = true;
                        distances[i] = distances[currentVertex] + graph[currentVertex][i];
                    }
                }
            }
            return distances;
        }
    }
}