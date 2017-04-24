using NetCoreStack.Contracts;
using System;

namespace Hisar.Component.Guideline.Models
{
    public class GuidelineEntityBson : EntityIdentityBson
    {
        public string Name { get; set; }
        public int Age { get; set; } 
        public DateTime BirthDate { get; set; }
    }
}