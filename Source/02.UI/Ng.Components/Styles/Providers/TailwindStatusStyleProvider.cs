using System.Diagnostics;
using Ng.Domain.Enums;

namespace Ng.Components.Styles.Providers
{
    public class TailwindStatusStyleProvider : IPlayerStatusStyleProvider
    {
        public string GetClass(PlayerStatus status) => status switch
        {
            PlayerStatus.Active => "bg-emerald-500 text-white",
            PlayerStatus.Injured => "bg-red-500 text-white",
            PlayerStatus.Resting => "bg-gray-500 text-white",
            PlayerStatus.Suspended => "bg-amber-500 text-white",
            PlayerStatus.Free => "bg-violet-500 text-white",
            PlayerStatus.Transferred => "bg-blue-500 text-white",
            PlayerStatus.Retired => "bg-gray-700 text-white",
            _ => GetDefaultClass(status)
        };

        public string GetInlineStyle(PlayerStatus status) => status switch
        {
            PlayerStatus.Active => "background-color: #10B981; color: white;",
            PlayerStatus.Injured => "background-color: #EF4444; color: white;",
            PlayerStatus.Resting => "background-color: #6B7280; color: white;",
            PlayerStatus.Suspended => "background-color: #F59E0B; color: white;",
            PlayerStatus.Free => "background-color: #8B5CF6; color: white;",
            PlayerStatus.Transferred => "background-color: #3B82F6; color: white;",
            PlayerStatus.Retired => "background-color: #374151; color: white;",
            _ => GetDefaultStyle(status)
        };

        private static string GetDefaultClass(PlayerStatus status)
        {
            Debug.Assert(false, $"Undefined player status: {status}");
            return "bg-gray-200 text-black";
        }

        private static string GetDefaultStyle(PlayerStatus status)
        {
            Debug.Assert(false, $"Undefined player status: {status}");
            return "background-color: #E5E7EB; color: black;";
        }
    }
}
