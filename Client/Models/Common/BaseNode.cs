using System.Numerics;

namespace QiNetwork.Common
{
    public abstract class BaseNode : IEquatable<BaseNode>
    {
        public int Id { get; set; } = -1;
        public Vector2 Position { get; set; } = new();
        public QiVector<double> CurrentQi { get; set; } = new();
        protected QiVector<double> _nextCycleQi { get; set; } = new();

        protected virtual QiVector<double> _getQiForFlowCalculation() => CurrentQi;
        protected virtual QiVector<double> _addtoNextCycleQi(QiVector<double> toAdd) => _nextCycleQi += toAdd;

        /// <summary>
        /// Calculates the flows for a cycle.
        /// </summary>
        public virtual void CalculateFlows(IEnumerable<BaseConnection> connections, IEnumerable<BaseNode> nodes)
        {
            Dictionary<BaseNode, QiVector<double>> gradients = [];
            var localConcentration = _getQiForFlowCalculation();

            foreach (var connection in connections)
            {
                // we'll ignore directions for now to ease the calculations for the first iteration of the program
                var otherNode = connection.NodeIdStart == this.Id
                    ? nodes.FirstOrDefault(f => f.Id == connection.NodeIdEnd)
                    : nodes.FirstOrDefault(f => f.Id == connection.NodeIdStart);

                if (otherNode is null)
                {
                    throw new ArgumentException("Connection was passed without target node. Could not calculate flows.");
                }

                var gradient = connection.GetGradient(
                    new Tuple<int, QiVector<double>>(this.Id, localConcentration),
                    new Tuple<int, QiVector<double>>(otherNode.Id, otherNode._getQiForFlowCalculation()));

                // negative values will be nulled for now, though in the future, they should push qi in the opposite direction
                foreach (var type in QiTypeCollections.ElementalTypes)
                {
                    gradient[type] = Math.Clamp(gradient[type], 0d, double.MaxValue);
                }
                foreach (var type in QiTypeCollections.OtherTypes)
                {
                    gradient[type] = Math.Clamp(gradient[type], -1d, 1d);
                }
                gradients[otherNode] = gradient;
            }

            var totalGradients = gradients.Values.Aggregate((a, b) => a + b);
            if (gradients.Count == 1) { totalGradients = totalGradients / totalGradients; }
            var remainder = localConcentration;
            foreach (var gradient in gradients)
            {
                var finalFlow = localConcentration * gradient.Value / totalGradients;
                if (this.GetType().Name == "Dantian_Node")
                {
                    finalFlow = finalFlow / 5;
                }
                remainder -= finalFlow;
                gradient.Key._addtoNextCycleQi(finalFlow);
            }
            this._addtoNextCycleQi(remainder);
        }

        public virtual void FinishCycle()
        {
            CurrentQi = _nextCycleQi * 0.9d;
            foreach (var type in QiTypeCollections.ElementalTypes)
            {
                CurrentQi[type] = Math.Clamp(CurrentQi[type], 0d, double.MaxValue);
            }
            foreach (var type in QiTypeCollections.OtherTypes)
            {
                CurrentQi[type] = Math.Clamp(CurrentQi[type], -1d, 1d);
            }
            _nextCycleQi = new();
        }

        #region IEquatable
        public override bool Equals(object? obj) => this.Equals(obj as BaseNode);

        public bool Equals(BaseNode? other) => this.Id == other?.Id;

        public override int GetHashCode() => this.Id.GetHashCode();
        #endregion

        public override string ToString() => $"{{ {this.GetType().Name}: {CurrentQi} }}";
    }
}
