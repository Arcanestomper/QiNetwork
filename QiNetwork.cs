using QiNetwork.Common;
using System;
using System.Linq;

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
    }
}