using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.Guideline.ViewComponents
{
    [ViewComponent]
    public class WithAttribute
    {
        public string Invoke() => "Hello";
    }

    public class DerivedWithAttribute : WithAttribute
    {
    }

    [ViewComponent(Name = "AttributeName")]
    public class WithAttributeAndName
    {
        public string Invoke() => "Hello";
    }

    public class WithoutSuffix : ViewComponent
    {
        public string Invoke() => "Hello";
    }

    [NonViewComponent]
    public class NonViewComponentAttributeViewComponent
    {
    }
}
