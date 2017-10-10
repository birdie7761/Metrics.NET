using System;
using System.Collections.Generic;
using System.Linq;

namespace Metrics.Sampling
{
    public sealed class UniformSnapshot : Snapshot
    {
        private readonly long[] values;

        public UniformSnapshot(long count, IEnumerable<long> values, bool valuesAreSorted = false, string minUserValue = null, string maxUserValue = null)
        {
            this.Count = count;
            this.values = values.ToArray();
            if (!valuesAreSorted)
            {
                Array.Sort(this.values);
            }
            this.MinUserValue = minUserValue;
            this.MaxUserValue = maxUserValue;
        }

        public long Count { get;private set; }

        public int Size {get{return this.values.Length;}}

        public long Max {get{return this.values.LastOrDefault();}}
        public long Min {get{return this.values.FirstOrDefault();}}

        public string MaxUserValue { get;private set; }
        public string MinUserValue { get;private set; }

        public double Mean {get{return Size == 0 ? 0.0 : this.values.Average();}}

        public double StdDev
        {
            get
            {
                if (this.Size <= 1)
                {
                    return 0;
                }

                var avg = this.values.Average();
                var sum = this.values.Sum(d => Math.Pow(d - avg, 2));

                return Math.Sqrt((sum) / (this.values.Length - 1));
            }
        }

        public double Median {get{return GetValue(0.5d);}}
        public double Percentile75 {get{return GetValue(0.75d);}}
        public double Percentile95 {get{return GetValue(0.95d);}}
        public double Percentile98 {get{return GetValue(0.98d);}}
        public double Percentile99 {get{return GetValue(0.99d);}}
        public double Percentile999 {get{return GetValue(0.999d);}}

        public IEnumerable<long> Values {get{return this.values;}}

        public double GetValue(double quantile)
        {
            if (quantile < 0.0 || quantile > 1.0 || double.IsNaN(quantile))
            {
                throw new ArgumentException(string.Concat(quantile.ToString()," is not in [0..1]"));
            }

            if (this.Size == 0)
            {
                return 0;
            }

            var pos = quantile * (this.values.Length + 1);
            var index = (int)pos;

            if (index < 1)
            {
                return this.values[0];
            }

            if (index >= this.values.Length)
            {
                return this.values[this.values.Length - 1];
            }

            double lower = this.values[index - 1];
            double upper = this.values[index];

            return lower + (pos - Math.Floor(pos)) * (upper - lower);
        }
    }
}
