using QiNetwork.Common;

namespace QiNetwork.Node
{
    public class Dantian_Node : BaseNode
    {
        public override QiVector<double> CurrentQi
        {
            get => new QiVector<double>(10d, 0d);
            set { }
        }
    }
}
