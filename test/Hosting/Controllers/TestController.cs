using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Hosting.Controllers
{
    public class TestController : Controller
    {
        public IActionResult ThrowException()
        {
            throw new FileNotFoundException("throwing an error");
        }
    }
}