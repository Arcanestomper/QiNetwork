namespace QiNetwork.Common
{
    public abstract class BaseConnection : IEquatable<BaseConnection>
    {
        public int NodeIdStart { get; set; } = -1;
        public int NodeIdEnd { get; set; } = -1;

        public BaseConnection() { }

        public BaseConnection(int node_Id_start, int node_Id_End)
        {
            NodeIdEnd = node_Id_start;
            NodeIdStart = node_Id_End;
        }

        public virtual QiVector<double> GetGradient(Tuple<int, QiVector<double>> requester, Tuple<int, QiVector<double>> otherNode)
        {
            var sum = requester.Item2 + otherNode.Item2;
            return requester.Item2 / sum - otherNode.Item2 / sum;
        }

        #region IEquatable
        public override bool Equals(object? obj) => this.Equals(obj as BaseConnection);

        public bool Equals(BaseConnection? other) =>
            (this.NodeIdStart == other?.NodeIdStart
            && this.NodeIdEnd == other.NodeIdEnd)
            ||
            (this.NodeIdStart == other?.NodeIdEnd
            && this.NodeIdEnd == other.NodeIdStart);

        public override int GetHashCode() => HashCode.Combine(NodeIdStart, NodeIdEnd);
        #endregion

        public override string ToString() => $"{{ {this.GetType().Name}: {{ {NodeIdStart}, {NodeIdEnd} }} }}";
    }
}
