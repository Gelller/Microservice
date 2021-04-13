using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace MetricsManager.DAL.Repository
{
    public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value)
            => DateTimeOffset.FromUnixTimeSeconds((Int64)value);

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
            => parameter.Value = value;
    }
}
