using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Repository
{
    public class StringToUriConverter : ITypeConverter<string, Uri>
    {
        public Uri Convert(string source, Uri destination, ResolutionContext context)
        {
            Uri.TryCreate(source, UriKind.Absolute, out destination);
            return destination;
        }
    }
}
