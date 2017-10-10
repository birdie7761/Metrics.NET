using System.Collections.Generic;
using System.Linq;
using HdrHistogram;

namespace Metrics.Sampling
{
    internal sealed class HdrSnapshot : Snapshot
    {
        private readonly AbstractHistogram histogram;

        public HdrSnapshot(AbstractHistogram histogram, long minValue, string minUserValue, long maxValue, string maxUserValue)
        {
            this.histogram = histogram;
            this.Min = minValue;
            this.MinUserValue = minUserValue;
            this.Max = maxValue;
            this.MaxUserValue = maxUserValue;
        }

        public IEnumerable<long> Values
        {
            get { return this.histogram.RecordedValues().Select(v => v.getValueIteratedTo()); }
        }

        public double GetValue(double quantile)
        {
            return this.histogram.getValueAtPercentile(quantile * 100);
        }

        public long Min { get; private set; }
        public string MinUserValue { get; private set; }
        public long Max { get; private set; }
        public string MaxUserValue { get; private set; }

        public long Count { get { return this.histogram.getTotalCount();}}
        public double Mean {get{return this.histogram.getMean();}}
        public double StdDev {get{return this.histogram.getStdDeviation();}}

        public double Median {get{return this.histogram.getValueAtPercentile(50);}}
        public double Percentile75 {get{return this.histogram.getValueAtPercentile(75);}}
        public double Percentile95 {get{return this.histogram.getValueAtPercentile(95);}}
        public double Percentile98 {get{return this.histogram.getValueAtPercentile(98);}}
        public double Percentile99 {get{return this.histogram.getValueAtPercentile(99);}}
        public double Percentile999 {get{return this.histogram.getValueAtPercentile(99.9);}}
        
        public int Size {get{return this.histogram.getEstimatedFootprintInBytes();}}
    }
}