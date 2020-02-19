using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMSDBDLL
{
    public class DBController
    {
        const string connectionString = @"Server=.;database=SimpleCMSDB;uid=sa;password=rootroot;";

        public static TrendConfig[] GetAllTrendType()
        {
            using (var db = new SimpleCmsDBClassDataContext(connectionString))
            {
                var trends = db.TrendConfig.Select(w => w).ToArray();
                return trends;
                
            }
        }

        public static TrendData[] GetTrendDatas(string name, DateTime startTime, DateTime endTime)
        {
            using (var db = new SimpleCmsDBClassDataContext(connectionString))
            {
                var trendData = from trend in db.TrendData
                                from config in db.TrendConfig
                                where config.Id == trend.trendConfig_Id
                                where config.name == name
                                where trend.Time >= startTime && trend.Time <= endTime
                                select trend;
                return trendData.ToArray();
            }
        }

    }
}