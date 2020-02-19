using System;
using SimpleCmsDbClassDLL;

namespace SimpleCMSDBDLL
{
    partial class Channel
    {
    }

    partial class WaveData
    {
        public float[] Floats;

        public void ToFloatArray()
        {
            Floats = new float[data.ToArray().Length / 2];
            for(int i=0;i<data.ToArray().Length; i+=2)
            {
                Int16 val = BitConverter.ToInt16(data.ToArray(), i);
                Floats[i/2] = val;
            }
            
        }
    }

    partial class TrendData
    {
    }

    partial class TrendConfig
    {
        public virtual TrendData CalTrend(WaveData wave, Spectrum spectrum)
        {
            return new TrendData();
        }
    }

    partial class PeakConfig : TrendConfig
    {
        public override TrendData CalTrend(WaveData wave, Spectrum spectrum)
        {
            PeakCalculator peak = new PeakCalculator(name, option);
            var trendData = peak.GetTrend(wave, spectrum.fft);
            trendData.trendConfig_Id = base.Id;
            return trendData;
        }
    }

    partial class RmsConfig : TrendConfig
    {
        public override TrendData CalTrend(WaveData wave, Spectrum spectrum)
        {
            RmsCalculator peak = new RmsCalculator(name, start, end);
            var trendData = peak.GetTrend(wave, spectrum.fft);
            trendData.trendConfig_Id = base.Id;
            return trendData;
        }
    }
}