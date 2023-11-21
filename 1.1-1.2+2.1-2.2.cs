using System;
using System.Collections.Generic;
using System.Security.Policy;

class Program
{
    internal class example2
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер матрицы: ");
            int size2 = Convert.ToInt32(Console.ReadLine());
            int[,] adjacencyMatrix = GenerateAdjacencyMatrix2(size2);
            Console.WriteLine("Матрица смежности для неориентированного графа:");
            PrintMatrix2(adjacencyMatrix);
            int[,] distances2 = DistanceSearch(adjacencyMatrix);
            Console.WriteLine("Минимальные расстояния между вершинами:");
            PrintDistances2(distances2);

            Console.WriteLine();

            FindMaxDistance(distances2);

            //массив, в котором хранятся эксцентриситеты каждой вершины графа.
            int[] eccentricities = new int[size2];

            //переменная, в которой будет храниться текущее значение диаметра графа.
            int diameter = 0;

            //переменная, в которой будет храниться текущее значение радиуса графа.
            int radius = int.MaxValue;

            //список, в который будут добавляться вершины с максимальным эксцентриситетом (периферийные точки).
            List<int> peripheralPoints = new List<int>();

            //список, в который будут добавляться вершины с минимальным эксцентриситетом (центральные точки).
            List<int> centralPoints = new List<int>();


            for (int vertex = 0; vertex < size2; vertex++)
            {
                int maxDistance = 0;
                int maxVertex = -1;
                for (int i = 0; i < size2; i++)
                {
                    if (i != vertex && distances2[vertex, i] > maxDistance)
                    {
                       //Вычисляется максимальное расстояние maxDistance от текущей вершины до других вершин графа, исключая саму вершину.
                       maxDistance = distances2[vertex, i];

                       //Сохраняется индекс вершины maxVertex, до которой достигается максимальное расстояние.
                        maxVertex = i;
                    }
                }
                //Значение maxDistance присваивается соответствующему элементу массива eccentricities.
                eccentricities[vertex] = maxDistance;

                //Если maxDistance больше текущего значения diameter, то diameter обновляется.
                if (maxDistance > diameter)
                    diameter = maxDistance;

                //Если maxDistance меньше текущего значения radius, то radius обновляется.
                if (maxDistance < radius)
                    radius = maxDistance;
            }

            for (int vertex = 0; vertex < size2; vertex++)
            {
                //Если эксцентриситет текущей вершины равен diameter, то добавляется индекс вершины + 1 в список peripheralPoints.
                if (eccentricities[vertex] == diameter)
                    peripheralPoints.Add(vertex + 1);

                //Если эксцентриситет текущей вершины равен radius, то добавляется индекс вершины + 1 в список centralPoints.
                if (eccentricities[vertex] == radius)
                    centralPoints.Add(vertex + 1);
            }

            Console.WriteLine("Диаметр графа: " + diameter);
            Console.WriteLine("Радиус графа: " + radius);
            Console.WriteLine("Периферийные точки: " + string.Join(", ", peripheralPoints));
            Console.WriteLine("Центральные точки: " + string.Join(", ", centralPoints));
        }

        //Генерация неориентированной матрицы смежности (взвешенной)
        private static int[,] GenerateAdjacencyMatrix2(int size)
        {
            Random r = new Random();
            int[,] matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i != j)
                    {
                        matrix[i, j] = r.Next(0, 10);
                        matrix[j, i] = matrix[i, j];
                    }
                }
            }
            return matrix;
        }

        //Вывод матрицы на экран
        static void PrintMatrix2(int[,] matrix)
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
        }
        //Метод, осуществляющий поиск расстояний в графе
        static int[,] DistanceSearch(int[,] adjacencyMatrix)
        {
            //размерность матрицы, равная количеству вершин в графе.
            int size = adjacencyMatrix.GetLength(0);

            //двумерный массив, в котором будет храниться информация о расстояниях между вершинами.
            int[,] distances = new int[size, size];

            //Сначала инициализируется массив расстояний distances. 
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    distances[i, j] = -1;
                }
            }

            for (int vertex = 0; vertex < size; vertex++)
            {
                //Создается массив visited, в котором все элементы инициализируются значением false.
                bool[] visited = new bool[size];

                //Создается очередь queue, куда добавляется текущая вершина.
                Queue<int> queue = new Queue<int>();

                //Текущей вершине присваивается расстояние 0.
                distances[vertex, vertex] = 0;

                //Помечается текущая вершина как посещенная.
                visited[vertex] = true;
                queue.Enqueue(vertex);

                //Пока очередь не пуста, происходит обход графа в ширину.
                while (queue.Count > 0)
                {
                    //Извлекается вершина из очереди.
                    int currentVertex = queue.Dequeue();

                    //Для каждой вершины, смежной с текущей, и которая еще не была посещена, вычисляется расстояние до нее и добавляется в очередь и массив расстояний distances.
                    for (int i = 0; i < size; i++)
                    {
                        if (adjacencyMatrix[currentVertex, i] > 0 && !visited[i])
                        {
                            distances[vertex, i] = distances[vertex, currentVertex] + adjacencyMatrix[currentVertex, i];
                            visited[i] = true;
                            queue.Enqueue(i);
                        }
                    }
                }

                int maxDistance = 0;
                int maxVertex = -1;

                // После обхода графа для каждой вершины вычисляется максимальное расстояние maxDistance и соответствующая вершина maxVertex.
                for (int i = 0; i < size; i++)
                {
                    if (i != vertex && distances[vertex, i] > maxDistance)
                    {
                        maxDistance = distances[vertex, i];
                        maxVertex = i;
                    }
                }
            }

            return distances;
        }

        static void PrintDistances2(int[,] distances)
        {
            //получает размер массива с помощью метода GetLength(0)
            int size = distances.GetLength(0);

            //затем использует вложенные циклы for для перебора всех элементов массива.
            //Если значение элемента больше 0, оно выводится на экран, в противном случае выводится 0.
            //После каждой строки матрицы происходит переход на новую строку.
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (distances[i, j] > 0)
                    {
                        Console.Write(" " + distances[i, j]);
                    }
                    else
                    {
                        Console.Write(" " + "0");
                    }
                }
                Console.WriteLine();
            }
        }
        //Метод находит максимальное расстояние от каждой вершины до других вершин в графе. 
        private static void FindMaxDistance(int[,] distances)
        {
            //Сначала определяется размер матрицы distances.
            int size = distances.GetLength(0);

            //Затем происходит цикл по всем вершинам графа.
            for (int vertex = 0; vertex < size; vertex++)
            {
                //Для каждой вершины инициализируется переменная maxDistance, которая
                //будет содержать максимальное расстояние от текущей вершины до других вершин.
                int maxDistance = 0;

                //Затем происходит вложенный цикл, в котором происходит перебор всех элементов в строке матрицы distances для текущей вершины.
                for (int i = 0; i < size; i++)
                {
                    //Если значение элемента больше текущего maxDistance, то maxDistance обновляется.
                    if (distances[vertex, i] > maxDistance)
                    {
                        maxDistance = distances[vertex, i];
                    }
                }
                //После завершения внутреннего цикла для каждой вершины находится максимальное расстояние до других вершин и выводится в консоль.
                Console.WriteLine("Эксцентриситет " + (vertex + 1) + ": " + maxDistance);
            }
        }
    }
}
