using System.Threading.Tasks;

namespace Metrics.Utils
{
    internal static class TaskEx
    {
        public static readonly Task CompletedTask = System.Threading.Tasks.TaskEx.FromResult(0);
    }
}