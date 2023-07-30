using FleetSharp;
using FleetSharp.Types;

namespace HodlCoin.Client
{
    public static class Helpers
    {
        public static async Task<List<NodeMempoolTransaction?>> GetTXesFromMempool(NodeInterface node, List<string> txIds)
        {
            var taskList = new List<Task<NodeMempoolTransaction?>>();

            foreach (var txId in txIds)
            {
                taskList.Add(node.GetTXFromMempool(txId));
            }

            var result = await Task.WhenAll(taskList.ToList()).ConfigureAwait(false);
            return result.ToList();
        }
    }
}
