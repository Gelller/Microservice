using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Responses;

namespace MetricsManager
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // добавлять сопоставления в таком стиле нужно для всех объектов 
            CreateMap<CpuMetrics, CpuMetricsDto>();
            CreateMap<DotNetMetrics, DotNetMetricsDto>();
            CreateMap<HddMetrics, HddMetricsDto>();
            CreateMap<NetworkMetrics, NetworkMetricsDto>();
            CreateMap<RamMetrics, RamMetricsDto>();
            CreateMap<AgentInfo, AgentInfoDto>();
        }
    }

}
