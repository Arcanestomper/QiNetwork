using QiNetwork.Common;
using QiNetwork.Connection;
using QiNetwork.Node;
using System;
using System.Linq;
using System.Net;

namespace QiNetwork
{
    public class Network
    {
        public List<BaseNode> Nodes { get; set; } = [];

        public List<BaseConnection> Connections { get; set; } = [];

        public void SimulateCycle()
        {
            foreach (var node in Nodes)
            {
                var relatedConnections = Connections.Where(f => f.NodeIdStart == node.Id || f.NodeIdEnd == node.Id).ToArray();
                var relatedNodes = Nodes.Where(f => f.Id != node.Id && relatedConnections.Any(g => g.NodeIdStart == f.Id || g.NodeIdEnd == f.Id)).ToArray();

                node.CalculateFlows(relatedConnections, relatedNodes);
            }

            foreach (var node in Nodes)
            {
                node.FinishCycle();
            }
        }
        public void InitializeNetwork()
        {
            Nodes.Add(new Dantian_Node() { Id = 1 });
        }

        public void AddNode(string type, int start_id)
        {
            var new_id = Nodes.Count + 1 ;
            switch (type) 
            {
                case "Dantian":
                    Nodes.Add(new Dantian_Node() { Id = new_id });
                    break;
                case "Basic":
                    Nodes.Add(new Basic_Node() { Id = new_id });
                    break;
                case "Technique":
                    Nodes.Add(new Technique_Node() { Id = new_id });
                    break;
                default:
                    Nodes.Add(new Basic_Node() { Id = new_id });
                    break;
            }
            
            Connections.Add(new Basic_Connection() { NodeIdStart = start_id, NodeIdEnd = new_id });
        }
    }
}