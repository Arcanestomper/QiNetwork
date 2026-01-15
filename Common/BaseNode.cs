using System.Numerics;

namespace QiNetwork.Common
{
    public abstract class BaseNode : IEquatable<BaseNode>
    {
        public int Id { get; set; } = -1;
        public Vector2 Position { get; set; } = new();
        protected QiVector<double> _previousCycleQi { get; set; } = new();
        public virtual QiVector<double> CurrentQi { get; set; } = new();
        protected QiVector<double> _nextCycleQi { get; set; } = new();

        protected virtual QiVector<double> _getLocalPressure() => CurrentQi;
        protected virtual QiVector<double> _addtoNextCycleQi(QiVector<double> toAdd) => _nextCycleQi += toAdd;

        /// <summary>
        /// Calculates the flows for a cycle.
        /// </summary>
        public virtual void CalculateFlows(IEnumerable<BaseConnection> connections, IEnumerable<BaseNode> nodes)
        {
            Dictionary<BaseNode, QiVector<double>> desiredFlows = [];
            var localPressure = _getLocalPressure();

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

                var effectivePressure = connection.GetEffectivePressure(
                    new Tuple<int, QiVector<double>>(this.Id, localPressure),
                    new Tuple<int, QiVector<double>>(otherNode.Id, otherNode._getLocalPressure()));

                var desiredFlow = 0.5d * localPressure * (localPressure - effectivePressure ) / (localPressure + effectivePressure);

                desiredFlows[otherNode] = desiredFlow.ClampElemental(0, double.MaxValue).ClampOther(-1, 1);
            }

            var totalFlow = desiredFlows.Values.Aggregate((a, b) => a + b);
            foreach(var type in QiTypeCollections.ElementalTypes)
            {
                if (totalFlow[type] > CurrentQi[type] * 0.5d)
                {
                    foreach(var kvp in desiredFlows)
                    {
                        kvp.Value[type] = 0.5d * CurrentQi[type] * kvp.Value[type] / totalFlow[type];
                    }
                }
            }

            var remainder = CurrentQi;
            foreach(var kvp in desiredFlows)
            {
                var finalFlow = kvp.Value;
                remainder -= finalFlow;
                kvp.Key._addtoNextCycleQi(finalFlow);
            }
            this._addtoNextCycleQi(remainder);
        }

        public virtual void FinishCycle()
        {
            _previousCycleQi = CurrentQi;
            CurrentQi = (_nextCycleQi * 0.9d).Round(Globals.QI_SIGNIFICANT_DIGITS);
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
