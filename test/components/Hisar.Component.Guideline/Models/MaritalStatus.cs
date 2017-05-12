using System.ComponentModel.DataAnnotations;

namespace Hisar.Component.Guideline.Models
{
    public enum MaritalStatus
    {
        Single = 0,
        [Display(Name = "Married and Happy")]
        Married,
        Separated,
        Divorced,
        Widowed
    }
}
