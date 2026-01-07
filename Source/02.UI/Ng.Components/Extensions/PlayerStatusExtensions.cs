using Ng.Domain.Enums;

namespace Ng.Components.Extensions
{
    public static class PlayerStatusExtensions
    {
        public static string GetStatusStyle(this PlayerStatus status) => status switch
        {
            PlayerStatus.Active => "background-color: #10B981; color: white;",
            PlayerStatus.Injured => "background-color: #EF4444; color: white;",
            PlayerStatus.Resting => "background-color: #6B7280; color: white;",
            PlayerStatus.Suspended => "background-color: #F59E0B; color: white;",
            PlayerStatus.Free => "background-color: #8B5CF6; color: white;",
            PlayerStatus.Transferred => "background-color: #3B82F6; color: white;",
            PlayerStatus.Retired => "background-color: #374151; color: white;",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Undefined player status value.")
        };

        public static string GetStatusClass(this PlayerStatus status) => status switch
        {
            PlayerStatus.Active => "bg-emerald-500 text-white",
            PlayerStatus.Injured => "bg-red-500 text-white",
            PlayerStatus.Free => "bg-violet-500 text-white",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Undefined player status value.")
        };
    }
}
