using System.ComponentModel;

namespace SchoolsNearMe.Models
{
    public enum TypeOfEstablishment
    {
        [Description("Nursery")] Nursery,
        [Description("Primary")] Primary,
        [Description("Secondary")] Secondary,
        [Description("Not Applicable")] NotApplicable,
        [Description("Sixteen plus")] SixteenPlus
    }
}