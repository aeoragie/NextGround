using Ng.Domain.Common;
using Ng.Domain.Soccer.Aggregates;
using Ng.Domain.Soccer.Enums;

namespace Ng.Domain.Soccer.Entities;

/// <summary>
/// 코칭 스태프 멤버 엔티티
/// </summary>
public class CoachingStaffMember : Entity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();

    public CoachingStaffRole Role { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public string? Nationality { get; private set; }

    public Guid TeamId { get; private set; }
    public Team Team { get; private set; } = null!;

    public DateTime? ContractStartDate { get; private set; }
    public DateTime? ContractEndDate { get; private set; }
    public string? PhotoUrl { get; private set; }
    public bool IsActive { get; private set; } = true;

    private CoachingStaffMember() { }

    public CoachingStaffMember(
        string firstName,
        string lastName,
        CoachingStaffRole role,
        Guid teamId)
    {
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        TeamId = teamId;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string? nationality, DateTime? dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        Nationality = nationality;
        DateOfBirth = dateOfBirth;
    }

    public void ChangeRole(CoachingStaffRole role) => Role = role;

    public void UpdateContract(DateTime startDate, DateTime endDate)
    {
        ContractStartDate = startDate;
        ContractEndDate = endDate;
    }

    public void SetPhotoUrl(string photoUrl) => PhotoUrl = photoUrl;
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
