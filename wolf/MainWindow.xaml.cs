using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using System.Threading;

namespace wolf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Rabbit> rabbits = Rabbit.InitializeRabits();
            // Create a dictionary to associate each rabbit with its ModelVisual3D
            

            Debug.WriteLine("Planets initialized.");


            foreach (Rabbit rabbit in rabbits)
            {
                Debug.WriteLine($"Added {rabbit.Name} to the viewport.");
                ModelVisual3D rabbitModel = CreateRabbitModel(rabbit);
                viewport3D.Children.Add(rabbitModel);
                Globals.rabbitModels.Add(rabbitModel, rabbit);

                Thread w = new Thread(new ThreadStart(() =>
                {
                    Console.WriteLine("create new thread");
                    RabbitThreadStart(rabbit);
                }));
                w.Start();
            }

            Thread moveRabbitThread = new Thread(() => MoveRabbit(Globals.rabbitModels));
            moveRabbitThread.Start();
        }

        private void RabbitThreadStart(Rabbit rabbit)
        {
            while (true)
            {
                rabbit.Running(rabbit);
                Thread.Sleep(1000); // Wait for 2 seconds before updating the position again
            }
        }

        // Start the simulation loop
        private void MoveRabbit(Dictionary<ModelVisual3D, Rabbit> rabbitModels)
        {
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    // Update the UI with the new rabbit position
                    foreach (ModelVisual3D model in viewport3D.Children)
                    {
                        if (rabbitModels.TryGetValue(model, out Rabbit currentRabbit))
                        {
                            if (model.Content is GeometryModel3D geometry && geometry.Transform is TranslateTransform3D transform)
                            {
                                transform.OffsetX = currentRabbit.Position.X;
                                transform.OffsetY = currentRabbit.Position.Y;
                                transform.OffsetZ = currentRabbit.Position.Z;
                            }
                        }
                    }
                });

                Thread.Sleep(1); // Wait for 2 seconds before updating the UI again
            }
        }
        

        private ModelVisual3D CreateRabbitModel(Rabbit rabbit)
        {
            // Create a sphere geometry for the planet
            MeshBuilder meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(rabbit.Position, 1); // Change 0.5 to the desired planet size

            // Create the planet material (color, texture, etc.)
            DiffuseMaterial planetMaterial = new DiffuseMaterial(new SolidColorBrush(rabbit.Color));

            // Create the transform for the planet
            TranslateTransform3D planetTransform = new TranslateTransform3D(rabbit.Position.X, rabbit.Position.Y, rabbit.Position.Z);

            // Create and return the planet 3D model
            return new ModelVisual3D
            {
                Content = new GeometryModel3D(meshBuilder.ToMesh(), planetMaterial)
                {
                    Transform = planetTransform
                }
            };
        }
    }
}
