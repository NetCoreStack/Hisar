using Hisar.Component.Guideline.Models;
using NetCoreStack.Mvc;

namespace Hisar.Component.Guideline
{
    public class AlbumViewModelComposer : ModelComposerBase<AlbumViewModel>
    {
        private readonly SampleService _someDependency;
        public AlbumViewModelComposer(SampleService someDependency)
        {
            _someDependency = someDependency;
        }

        public override void Invoke(AlbumViewModel model)
        {
            _someDependency.Invoke();
            model.Price = 2.00M;
        }
    }

    public class AnotherComposer : ModelComposerBase<GenreViewModel>
    {
        private readonly SampleService _someDependency;
        public AnotherComposer(SampleService someDependency)
        {
            _someDependency = someDependency;
        }

        public override void Invoke(GenreViewModel model)
        {
            _someDependency.Invoke();
            model.Genre = "Some Text";
        }
    }
}
