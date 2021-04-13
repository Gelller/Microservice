using MetricsManager.Responses;
using MetricsManager.Requests;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        AllRamMetricsApiResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request);

        AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request);

        AllDotNetMetricsApiResponse GetAllDotNetMetrics(GetAllDotNetMetrisApiRequest request);

        AllCpuMetricsApiResponse GetAllCpuMetrics(GetAllCpuMetricsApiRequest request);

        AllNetworkMetricsApiResponse GetAllNetworkMetrics(GetAllNetworkMetricsApiRequest request);
    }
}
