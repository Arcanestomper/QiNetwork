// See https://aka.ms/new-console-template for more information
using QiNetwork;
using System.Drawing;
using System.Numerics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
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

        Console.WriteLine(ChosenOne.Nodes[0]);
    }
}