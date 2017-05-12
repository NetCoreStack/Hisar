using NetCoreStack.Data.Contracts;
using System;

namespace Hisar.Component.Guideline.Models
{
    public class GuidelineEntity : EntityIdentityBson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public DateTime BirthDate { get; set; }
    }
}