using System;
using System.Linq;
using SimpleCMSDBDLL;

namespace SimpleCmsDbClassDLL
{
    class RmsCalculator : ITrendCalculator
    {
        public string title { get; set; }
        public int? start, end; // property들을 읽어옴
                               //생성자
        public RmsCalculator(string title, int? start, int? end)
        {
            this.title = title;
            this.start = start;
            this.end = end;
        }
        
        //Get Rms with Option
        public TrendData GetTrend(WaveData wave, float[] spectrum)
        {
            double square = 0;
            float mean, root = 0;

            for (int i = start.Value; i < end; i++)
            {
                square += (wave.Floats[i] * wave.Floats[i]);
            }
            mean = (float)square / wave.data.Length;
            root = (float)Math.Sqrt(mean);
           

            TrendData trendData = new TrendData();
            trendData.Value = root;
            trendData.Time = wave.time;
            return trendData;
        }
    }
}
