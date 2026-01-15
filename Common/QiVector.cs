using System.Collections;
using System.Numerics;

namespace QiNetwork.Common
{
    /// <summary>
    /// Generic class that provides the ability to store arbitrary values for each qi type and defines simple arithmetic on it for ease of processing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QiVector<T> : IEnumerable<Tuple<QiType, T>> where T :
        IAdditionOperators<T, T, T>,
        ISubtractionOperators<T, T, T>,
        IMultiplyOperators<T, T, T>,
        IMultiplyOperators<T, double, T>,
        IDivisionOperators<T, T, T>,
        IDivisionOperators<T, double, T>
    {
        private readonly Dictionary<QiType, T> _values = [];

        public QiVector() { }

        public QiVector(T defaultValue)
        {
            foreach (var type in QiTypeCollections.AllTypes)
            {
                _values[type] = defaultValue;
            }
        }

        public QiVector(T defaultValueElemental, T defaultValueOther)
        {
            foreach (var type in QiTypeCollections.ElementalTypes)
            {
                _values[type] = defaultValueElemental;
            }
            foreach (var type in QiTypeCollections.OtherTypes)
            {
                _values[type] = defaultValueOther;
            }
        }

        #region Operators
        public static QiVector<T> operator +(QiVector<T> left, QiVector<T> right)
        {
            var newQi = new QiVector<T>();
            T? totalElementalLeft = default;
            T? totalElementalRight = default;
            foreach (var elementalType in QiTypeCollections.ElementalTypes)
            {
                newQi[elementalType] = left[elementalType] + right[elementalType];

                if (totalElementalLeft == null)
                {
                    totalElementalLeft = left[elementalType];
                }
                else
                {
                    totalElementalLeft += left[elementalType];
                }

                if (totalElementalRight == null)
                {
                    totalElementalRight = right[elementalType];
                }
                else
                {
                    totalElementalRight += right[elementalType];
                }
            }

            var totalElemental = totalElementalLeft + totalElementalRight;
            if (totalElemental == null || totalElemental.Equals(default(T)))
            {
                foreach (var otherType in QiTypeCollections.OtherTypes)
                {
                    newQi[otherType] = default;
                }
            }
            else
            {
                foreach (var otherType in QiTypeCollections.OtherTypes)
                {
                    newQi[otherType] =
                        left[otherType] * (totalElementalLeft / totalElemental)
                        + right[otherType] * (totalElementalRight / totalElemental);
                }
            }

            return newQi;
        }

        public static QiVector<T> operator -(QiVector<T> left, QiVector<T> right)
        {
            var newQi = new QiVector<T>();
            T? totalElementalLeft = default;
            T? totalElementalRight = default;
            foreach (var elementalType in QiTypeCollections.ElementalTypes)
            {
                newQi[elementalType] = left[elementalType] - right[elementalType];

                if (totalElementalLeft == null)
                {
                    totalElementalLeft = left[elementalType];
                }
                else
                {
                    totalElementalLeft += left[elementalType];
                }

                if (totalElementalRight == null)
                {
                    totalElementalRight = right[elementalType];
                }
                else
                {
                    totalElementalRight += right[elementalType];
                }
            }

            var totalElemental = totalElementalLeft - totalElementalRight;
            if (totalElemental == null || totalElemental.Equals(default(T)))
            {
                foreach (var otherType in QiTypeCollections.OtherTypes)
                {
                    newQi[otherType] = default;
                }
            }
            else
            {
                foreach (var otherType in QiTypeCollections.OtherTypes)
                {
                    newQi[otherType] =
                        left[otherType] * (totalElementalLeft / totalElemental)
                        - right[otherType] * (totalElementalRight / totalElemental);
                }
            }

            return newQi;
        }

        public static QiVector<T> operator *(int left, QiVector<T> right) => right * left;
        public static QiVector<T> operator *(QiVector<T> left, int right) => left * Convert.ToDouble(right);
        public static QiVector<T> operator *(float left, QiVector<T> right) => right * left;
        public static QiVector<T> operator *(QiVector<T> left, float right) => left * Convert.ToDouble(right);
        public static QiVector<T> operator *(decimal left, QiVector<T> right) => right * left;
        public static QiVector<T> operator *(QiVector<T> left, decimal right) => left * Convert.ToDouble(right);
        public static QiVector<T> operator *(double left, QiVector<T> right) => right * left;
        public static QiVector<T> operator *(QiVector<T> left, double right)
        {
            var newQi = new QiVector<T>();
            foreach (var type in QiTypeCollections.AllTypes)
            {
                newQi[type] = left[type] * right;
            }
            return newQi;
        }

        public static QiVector<T> operator *(QiVector<T> left, QiVector<T> right)
        {
            var newQi = new QiVector<T>();
            foreach (var type in QiTypeCollections.AllTypes)
            {
                newQi[type] = left[type] * right[type];
            }
            return newQi;
        }

        public static QiVector<T> operator /(QiVector<T> left, int right) => left / Convert.ToDouble(right);
        public static QiVector<T> operator /(QiVector<T> left, float right) => left / Convert.ToDouble(right);
        public static QiVector<T> operator /(QiVector<T> left, decimal right) => left / Convert.ToDouble(right);
        public static QiVector<T> operator /(QiVector<T> left, double right)
        {
            var newQi = new QiVector<T>();
            foreach (var type in QiTypeCollections.AllTypes)
            {
                newQi[type] = left[type] / right;
            }
            return newQi;
        }

        public static QiVector<T> operator /(QiVector<T> left, QiVector<T> right)
        {
            var newQi = new QiVector<T>();
            foreach (var type in QiTypeCollections.AllTypes)
            {
                if (right[type] == null || right[type].Equals(default) || Convert.ToDouble(right[type]) == 0d)
                {
                    newQi[type] = default;
                }
                else
                {
                    newQi[type] = left[type] / right[type];
                }
            }
            return newQi;
        }
        #endregion

        #region Indexer
        public T this[QiType index]
        {
            get => _values.GetValueOrDefault(index);
            set => _values[index] = value;
        }
        #endregion

        #region IEnumerable<>
        public IEnumerator<Tuple<QiType, T>> GetEnumerator()
        {
            foreach (var type in QiTypeCollections.AllTypes)
            {
                yield return new Tuple<QiType, T>(type, _values.GetValueOrDefault(type));
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        public override string ToString() =>
            $"{{ {string.Join(", ", this.Select(f => $"{f.Item1}: {f.Item2}"))} }}";
    }

    public static class QiVectorExtensions
    {
        public static QiVector<double> Round(this QiVector<double> input, int significantDigits)
        {
            var newQi = new QiVector<double>();
            foreach (var type in QiTypeCollections.AllTypes)
            {
                newQi[type] = Math.Round(input[type], significantDigits);
            }
            return newQi;
        }

        public static QiVector<double> ClampElemental(this QiVector<double> input, double min, double max)
        {
            var newQi = new QiVector<double>();
            foreach (var type in QiTypeCollections.ElementalTypes)
            {
                newQi[type] = Math.Clamp(input[type], min, max);
            }
            foreach (var type in QiTypeCollections.OtherTypes)
            {
                newQi[type] = input[type];
            }
            return newQi;
        }

        public static QiVector<double> ClampOther(this QiVector<double> input, double min, double max)
        {
            var newQi = new QiVector<double>();
            foreach (var type in QiTypeCollections.ElementalTypes)
            {
                newQi[type] = input[type];
            }
            foreach (var type in QiTypeCollections.OtherTypes)
            {
                newQi[type] = Math.Clamp(input[type], min, max);
            }
            return newQi;
        }
    }
}
