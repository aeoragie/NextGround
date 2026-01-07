using System.Diagnostics;
using Ng.Domain.Enums;

namespace Ng.Components.Styles.Providers
{
    public class BootstrapStatusStyleProvider : IPlayerStatusStyleProvider
    {
        public string GetClass(PlayerStatus status) => status switch
        {
            PlayerStatus.Active => "badge bg-success text-white",
            PlayerStatus.Injured => "badge bg-danger text-white",
            PlayerStatus.Resting => "badge bg-secondary text-white",
            PlayerStatus.Suspended => "badge bg-warning text-dark",
            PlayerStatus.Free => "badge bg-primary text-white",
            PlayerStatus.Transferred => "badge bg-info text-white",
            PlayerStatus.Retired => "badge bg-dark text-white",
            _ => GetDefaultClass(status)
        };

        public string GetInlineStyle(PlayerStatus status) => status switch
        {
            PlayerStatus.Active => "background-color: #198754; color: white;",
            PlayerStatus.Injured => "background-color: #DC3545; color: white;",
            PlayerStatus.Resting => "background-color: #6C757D; color: white;",
            PlayerStatus.Suspended => "background-color: #FFC107; color: black;",
            PlayerStatus.Free => "background-color: #0D6EFD; color: white;",
            PlayerStatus.Transferred => "background-color: #0DCAF0; color: white;",
            PlayerStatus.Retired => "background-color: #212529; color: white;",
            _ => GetDefaultStyle(status)
        };

        private static string GetDefaultClass(PlayerStatus status)
        {
            Debug.Assert(false, $"Undefined player status: {status}");
            return "badge bg-light text-dark";
        }

        private static string GetDefaultStyle(PlayerStatus status)
        {
            Debug.Assert(false, $"Undefined player status: {status}");
            return "background-color: #F8F9FA; color: black;";
        }
    }
}
