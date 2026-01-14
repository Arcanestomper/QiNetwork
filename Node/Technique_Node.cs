using QiNetwork.Common;

namespace QiNetwork.Node
{
    public class Technique_Node : BaseNode
    {
        public override void FinishCycle()
        {
            CurrentQi = _nextCycleQi * 0.9d;
            foreach (var type in QiTypeCollections.ElementalTypes)
            {
                CurrentQi[type] = CurrentQi[type] - 10d;
                CurrentQi[type] = Math.Clamp(CurrentQi[type], 0d, double.MaxValue);
            }
            foreach (var type in QiTypeCollections.OtherTypes)
            {
                CurrentQi[type] = CurrentQi[type] - 10d;
                CurrentQi[type] = Math.Clamp(CurrentQi[type], -1d, 1d);
            }
            _nextCycleQi = new();
        }
    }
}
