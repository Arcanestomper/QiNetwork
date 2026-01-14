using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text;

namespace QiNetwork
{
    public class Qi_Network
    {
        public Node?[] Nodes { get; set; } = new Node[10];

        public List<Connection?> Connections { get; set; } = new List<Connection?>();


    }

    public class Node (int Id, string Name, Point coordinates)
    {
        [Required]
        public int _Id { get; set; } = Id;

        public string? _Name { get; set; } = Name;
        public Point Position { get; set; } = coordinates;

        public Qi Generated_Qi { get; set; } = new Qi();

        public Qi Current_Qi { get; set; } = new Qi();

        public class Qi
        {
            public Dictionary<string, double> Qi_Type { get; set; } = new Dictionary<string, double>();
        }

    }

    public class Connection (int Start, int End)
    {
        public int Node_Id_Start { get; set; } = Start;
        public int Node_Id_End { get; set; } = End;
    }

    public class Qi_Vector(int X, int Y)
    {
        public int _X { get; set; } = X;
        public int _Y { get; set; } = Y;
    }

}
