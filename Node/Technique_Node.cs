using QiNetwork.Common;

namespace QiNetwork.Node
{
    public class Technique_Node : BaseNode
    {
        public override void FinishCycle()
        {
            _previousCycleQi = CurrentQi;
            CurrentQi = (_nextCycleQi * 0.9d - new QiVector<double>(10d, 0)).Round(Globals.QI_SIGNIFICANT_DIGITS);

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
    }
}
