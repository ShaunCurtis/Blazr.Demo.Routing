/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.NavigationManager.UI
{
    public partial class EditForm : ComponentBase
    {
        private bool _dirty;

        [Inject] private BlazrNavigationManager? _navigationManager { get; set; }

        [Inject] private IJSRuntime? _js { get; set; }

        private string BtnCss => _dirty ? "btn-danger" : "btn-success";

        private string BtnText => _dirty ? "Set Form To Clean" : "Set Form To Dirty";

        protected override void OnInitialized()
        {
            _navigationManager!.BeforeLocationChange += NavigationManager_BeforeLocationChange;
            base.OnInitialized();
        }

        private void SwitchDirtyState()
        {
            _dirty = !_dirty;
            SetPageExitCheck(_dirty);
        }

        private void NavigationManager_BeforeLocationChange(object? sender, NavigationData e)
            => e.IsCanceled = _dirty;

        private void SetPageExitCheck(bool action)
            => _js!.InvokeAsync<bool>("blazr_setEditorExitCheck", action);

    }
}
