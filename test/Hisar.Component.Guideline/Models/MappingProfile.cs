using AutoMapper;

namespace Hisar.Component.Guideline.Models
{
    /// <summary>
    /// Third party dependency sample
    /// Assign the <see cref="IMapper"/> in the constructor for dependency injection
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GuidelineEntityBson, GuidelineViewModel>();
        }
    }
}
