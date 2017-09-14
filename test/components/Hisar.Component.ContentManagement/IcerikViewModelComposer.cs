using NetCoreStack.Mvc;

namespace Hisar.Component.ContentManagement
{
    public class IcerikViewModelComposer : ModelComposerBase<ContentObjectViewModel>
    {
        public override void Invoke(ContentObjectViewModel model)
        {
            if (model == null)
                return;
        }
    }
}
