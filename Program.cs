// See https://aka.ms/new-console-template for more information
using QiNetwork;
using QiNetwork.Node;

internal class Program
{
    private static void Main(string[] args)
    {
        var network = new Network();
        /*
        {
            Nodes =
            [
                new Dantian_Node(){ Id = 1, },
                new Basic_Node(){ Id = 2 },
                new Basic_Node(){ Id = 3 },
                new Basic_Node(){ Id = 4 },
                new Basic_Node(){ Id = 5 },
                new Technique_Node(){ Id = 6 },
            ],
            Connections =
            [
                new Basic_Connection(){ NodeIdStart = 1, NodeIdEnd = 2 },
                new Basic_Connection(){ NodeIdStart = 2, NodeIdEnd = 3 },
                new Basic_Connection(){ NodeIdStart = 3, NodeIdEnd = 4 },
                new Basic_Connection(){ NodeIdStart = 4, NodeIdEnd = 5 },
                new Basic_Connection(){ NodeIdStart = 5, NodeIdEnd = 6 },
            ]
        };
        */
        network.InitializeNetwork();
        network.AddNode<Basic_Node>(1);
        network.AddNode<Basic_Node>(2);
        network.AddNode<Basic_Node>(3);
        network.AddNode<Basic_Node>(4);
        network.AddNode<Technique_Node>(5);

        /*
        for (int i = 0; i < 10; i++)
        {
            network.AddNode<Basic_Node>(i + 6);
        }
        */


        for (int i = 0; i < 1000; i++)
        {
            network.SimulateCycle();
        }

        Console.WriteLine("End Simulation");

        /*Console.WriteLine("Hello, World!");
        Point current_location = new Point();
        current_location.X = 0;
        current_location.Y = 0;

        Qi_Network ChosenOne = new Qi_Network();

        //Initialize Dantian
        ChosenOne.Nodes[0] = new Node(0, "Dantian", current_location);
        ChosenOne.Nodes[0].Generated_Qi.Qi_Type.Add("Pure", 10);

        //Create Meridian
        current_location.X = 1;
        current_location.Y = 1;
        ChosenOne.Nodes[1] = new Node(1, "Meridian", current_location);
        ChosenOne.Nodes[1].Generated_Qi.Qi_Type.Add("Pure", 10);
        ChosenOne.Connections.Add(new Connection(0, 1));

        //Cycle_Qi
        var v1 = new Qi_Vector(ChosenOne.Nodes[1].Position.X - ChosenOne.Nodes[0].Position.X, 
               ChosenOne.Nodes[1].Position.Y - ChosenOne.Nodes[0].Position.Y);
        Console.WriteLine(v1);

        Console.WriteLine(ChosenOne.Nodes[0]);*/
    }
}