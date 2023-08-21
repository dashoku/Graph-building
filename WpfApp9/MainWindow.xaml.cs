    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    namespace WpfApp9
    {
        public partial class MainWindow : Window
        {
            public class Castle
            {
                private int N; // количество вершин
                public List<int>[] adj; // список 

                public Castle(int n)
                {
                    N = n;
                    adj = new List<int>[n];
                    for (int i = 0; i < n; i++)
                    {
                        adj[i] = new List<int>();
                    }
                }

                public void AddEdge(int u, int v)
                {
                    if (u != v)
                    {
                        adj[u].Add(v);
                        adj[v].Add(u);
                    }
                }

                public void Draw(Canvas canvas, List<int> path)
                {
                    canvas.Children.Clear();

                    int canvasWidth = (int)canvas.ActualWidth;
                    int canvasHeight = (int)canvas.ActualHeight;
                    int vertexRadius = Math.Min(canvasWidth, canvasHeight) / (4 * N);

                // рисуем ребра/линии
                foreach (int u in Enumerable.Range(0, N))
                {
                    foreach (int v in adj[u])
                    {
                        int ux = (int)((u % Math.Sqrt(N) + 0.5) * canvasWidth / Math.Sqrt(N));
                        int uy = (int)((u / Math.Sqrt(N) + 0.5) * canvasHeight / Math.Sqrt(N));
                        int vx = (int)((v % Math.Sqrt(N) + 0.5) * canvasWidth / Math.Sqrt(N));
                        int vy = (int)((v / Math.Sqrt(N) + 0.5) * canvasHeight / Math.Sqrt(N));

                        Line line = new Line
                        {
                            X1 = ux,
                            Y1 = uy,
                            X2 = vx,
                            Y2 = vy,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2
                        };
                        Canvas.SetZIndex(line, 0);
                        canvas.Children.Add(line);
                    }
                }

                // рисуем вершины/круги
                for (int u = 0; u < N; u++)
                {
                    Brush brush = path.Contains(u) ? Brushes.Red : Brushes.Blue;
                    int x = (int)((u % Math.Sqrt(N) + 0.5) * canvasWidth / Math.Sqrt(N)) - vertexRadius;
                    int y = (int)((u / Math.Sqrt(N) + 0.5) * canvasHeight / Math.Sqrt(N)) - vertexRadius;
                    int d = 2 * vertexRadius;
                    Ellipse ellipse = new Ellipse
                    {
                        Width = d,
                        Height = d,
                        Fill = brush,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                    Canvas.SetLeft(ellipse, x);
                    Canvas.SetTop(ellipse, y);
                    Canvas.SetZIndex(ellipse, 1);
                    canvas.Children.Add(ellipse);

                    TextBlock textBlock = new TextBlock
                    {
                        Text = (u + 1).ToString(),
                        FontSize = 12,
                        Foreground = Brushes.Black
                    };
                    Canvas.SetLeft(textBlock, x + d / 2 - 8);
                    Canvas.SetTop(textBlock, y + d / 2 - 8);
                    Canvas.SetZIndex(textBlock, 2);
                    canvas.Children.Add(textBlock);
                }
                }

            
                public void DFS(int s, bool[] visited, List<int> path, List<List<int>> paths)
                {
                    visited[s] = true; //Помечает текущий узел как посещенный
                    path.Add(s);//Добавляет текущий узел в путь
                    if (path.Count == N)
                    {
                        paths.Add(new List<int>(path));
                    }
                    else
                    {
                        foreach (int u in adj[s])
                        {
                            if (!visited[u])
                            {
                                DFS(u, visited, path, paths);
                            }
                        }
                        visited[s] = false;//Помечает текущий узел как не посещенный
                    path.Remove(s);//Удаляет текущий узел из пути
                    }
                }
                public List<List<int>> FindHamiltonianPaths()
                {
                    List<List<int>> paths = new List<List<int>>();
                    bool[] visited = new bool[N];
                    List<int> path = new List<int>();
                    DFS(0, visited, path, paths);
                    return paths;
                }
            }

            private Castle graph;

            public MainWindow()
            {
                InitializeComponent();
            }

            private void button_Click(object sender, RoutedEventArgs e)
            {
                int n;

                if (Int32.TryParse(textBox.Text, out n))
                {
                    graph = new Castle(n);
                    if (n > 1)
                    {
                        Random random = new Random();
                        int start = random.Next(n); // случайная начальная комната
                        for (int u = 0; u < n; u++)
                        {
                            for (int v = u + 1; v < n; v++)
                            {
                                graph.AddEdge(u, v);
                            }
                        }
                        // Находим все гамильтоновы пути в графе
                        List<List<int>> paths = graph.FindHamiltonianPaths();
                        if (paths.Count > 0)
                        {
                            graph.Draw(canvas, paths[0]);// Рисуем первый гамильтонов путь
                        }
                        else
                        {
                            graph.Draw(canvas, new List<int>());// Если гамильтоновы пути не найдены, рисуем на холсте пустой путь
                        }   
                    }
                    else
                    {
                        graph.Draw(canvas, new List<int> { 0 });// Если n = 1, рисуем на холсте одну комнату
                }
                }
                else
                {
                    MessageBox.Show("Please enter a valid integer value for n.");
                }
            }
        }
    }