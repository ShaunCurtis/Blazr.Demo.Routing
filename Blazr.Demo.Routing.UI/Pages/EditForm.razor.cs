/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.NavigationManager.UI;

public partial class EditForm : ComponentBase, IDisposable
{
    private bool _dirty;
    private string message = string.Empty;

    [Inject] private IBlazrNavigationManager? _navigationManager { get; set; }
    private IBlazrNavigationManager NavigationManager => _navigationManager!;

    private BlazrNavigationManager? BlazrNavManager => _navigationManager is BlazrNavigationManager ? NavigationManager as BlazrNavigationManager : null;

    private string BtnCss => _dirty ? "btn-danger" : "btn-success";

    private string DivCss => _dirty ? "border-danger" : "border-success";

    private string BtnText => _dirty ? "Form Edited" : "Form Clean";

    protected override void OnInitialized()
    {
        if (this.BlazrNavManager is not null)
        {
            this.BlazrNavManager.BrowserExitAttempted += BrowserExitAttempted;
            this.BlazrNavManager.NavigationEventBlocked += NavigationAttempted;
        }
    }

    private void BrowserExitAttempted(object? sender, EventArgs e)
    {
        this.message += "You can't terminate me that easily!<br />";
        this.InvokeAsync(this.StateHasChanged);
    }

    private void NavigationAttempted(object? sender, BlazrNavigationEventArgs e)
    {
        this.message += $"Naughty! You can't go to {e.Uri} just now.<br />";
        this.InvokeAsync(this.StateHasChanged);
    }

    private void SwitchDirtyState()
    {
        _dirty = !_dirty;
        this.message = string.Empty;
        if (BlazrNavManager is not null)
            BlazrNavManager.SetLockState(_dirty);
    }

    public void Dispose()
    {
        if (this.BlazrNavManager is not null)
        {
            this.BlazrNavManager.BrowserExitAttempted -= BrowserExitAttempted;
            this.BlazrNavManager.NavigationEventBlocked -= NavigationAttempted;
        }
    }
}

