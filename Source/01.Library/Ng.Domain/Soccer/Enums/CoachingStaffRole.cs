using System.ComponentModel.DataAnnotations;

namespace Ng.Domain.Soccer.Enums;

public enum CoachingStaffRole
{
    [Display(Name = "Head Coach / Manager")]
    HeadCoach,

    [Display(Name = "Assistant Coach / First Team Coach")]
    AssistantCoach,

    [Display(Name = "Fitness Coach / Strength Coach")]
    FitnessCoach,

    [Display(Name = "Goalkeeper Coach")]
    GoalkeeperCoach,

    [Display(Name = "Tactical Analyst / Video Analyst")]
    TacticalAnalyst,

    [Display(Name = "Trainer / Athletic Trainer")]
    Trainer,

    [Display(Name = "Technical Coach")]
    TechnicalCoach
}
