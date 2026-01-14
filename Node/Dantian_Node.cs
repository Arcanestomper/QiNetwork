using QiNetwork.Common;

namespace QiNetwork.Node
{
    public class Dantian_Node : BaseNode
    {
        protected override QiVector<double> _getQiForFlowCalculation() =>
            base._getQiForFlowCalculation() + new QiVector<double>(defaultValueElemental: 10, defaultValueOther: 0);
    }
}
