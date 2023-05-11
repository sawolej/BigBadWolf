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
        private Wolf wolf;
        private Dictionary<ModelVisual3D, string> modelWolfName = new Dictionary<ModelVisual3D, string>();
        public MainWindow()
        {
            InitializeComponent();
            wolf = new Wolf(new Point3D(0, 0, 0), this.Dispatcher);
            List<Rabbit> rabbits = Rabbit.InitializeRabits();
            // Create a dictionary to associate each rabbit with its ModelVisual3D
            

            Debug.WriteLine("Planets initialized.");

            viewport3D.Children.Add(CreateWolfModel());
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
            Thread wolfThread = new Thread(() => WolfThreadStart(wolf, rabbits));
            wolfThread.Start();
        }

        private void RabbitThreadStart(Rabbit rabbit)
        {
            while (true)
            {
                rabbit.Running(rabbit);
                //Thread.Sleep(1000); // Wait for 2 seconds before updating the position again
            }
        }
        private void WolfThreadStart(Wolf wolf, List<Rabbit> rabbits)
        {
            while (true)
            {
                wolf.RunWolf(rabbits);
                Thread.Sleep(1000); // Wait for 2 seconds before updating the position again
            }
        }

        // Start the simulation loop
        private void MoveRabbit(Dictionary<ModelVisual3D, Rabbit> rabbitModels)
        {
            ModelVisual3D wolfModel = null;

            

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
                                Debug.WriteLine($"Found a rabbit model");
                                transform.OffsetX = currentRabbit.Position.X;
                                transform.OffsetY = currentRabbit.Position.Y;
                                transform.OffsetZ = currentRabbit.Position.Z;
                            }
                           
                            /*if (modelWolfName.TryGetValue(model, out string modelName))
                            {
                                Console.WriteLine("OMG i have wolf");
                                if (model != null && model.Content is GeometryModel3D geometry)
                                {
                                    if (geometry.Transform is TranslateTransform3D transform)
                                    {
                                        transform.OffsetX = wolf.Position.X;
                                        transform.OffsetY = wolf.Position.Y;
                                        transform.OffsetZ = wolf.Position.Z;
                                    }
                                }
                            }*/
                        }
                        else if (modelWolfName.TryGetValue(model, out string modelName))
                        {
                            Debug.WriteLine($"Found a model with the name: {modelName}");
                            Debug.WriteLine("wolfModel: " + wolfModel);
                            Debug.WriteLine("wolfModel.Content: " + model.Content);
                            if (model != null && model.Content is Model3DGroup geometry)
                            {
                                Debug.WriteLine($"WOLF MOVIN");
                                Debug.WriteLine("geometry.Transform " + geometry.Transform);
                                if (geometry.Transform is MatrixTransform3D transform)
                                {
                                    Debug.WriteLine($"WOLF MOVIN");

                                    Matrix3D matrix = transform.Matrix;
                                    matrix.OffsetX = wolf.Position.X;
                                    matrix.OffsetY = wolf.Position.Y;
                                    matrix.OffsetZ = wolf.Position.Z;

                                    MatrixTransform3D newTransform = new MatrixTransform3D(matrix);
                                    geometry.Transform = newTransform;
                                }
                            }
                        }
                    }
                });

                Thread.Sleep(100); // Wait for 2 seconds before updating the UI again
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

        private ModelVisual3D CreateWolfModel()
        {
            var loader = new ObjReader();
            var model3D = loader.Read("C:\\Users\\HP\\Desktop\\studia\\rok3\\s2\\ASP\\wolf\\wolf\\models\\Wolf.obj");

            double scaleFactor = 0.02; // Adjust the scale factor as needed
            ScaleTransform3D scaleTransform = new ScaleTransform3D(scaleFactor, scaleFactor, scaleFactor);

            // Create a TranslateTransform3D to position the model
            TranslateTransform3D positionTransform = new TranslateTransform3D(0, 0, 0);

            // Combine the scale and position transforms using a Transform3DGroup
            Transform3DGroup transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(positionTransform);

            var wolfModel = new ModelVisual3D
            {
                Content = model3D,
                Transform = transformGroup
            };
            modelWolfName[wolfModel] = "wolfModel";
            
            return wolfModel;
        }
    }
}
