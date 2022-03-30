/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Routing;

public class CoreNavigationManager : NavigationManager, IBlazrNavigationManager, IDisposable
{
    private NavigationManager _baseNavigationManager;

    public CoreNavigationManager(NavigationManager? baseNavigationManager)
    {
        _baseNavigationManager = baseNavigationManager!;
        base.Initialize(_baseNavigationManager!.BaseUri, _baseNavigationManager.Uri);
        _baseNavigationManager.LocationChanged += OnBaseLocationChanged;
    }

    protected override void EnsureInitialized()
        => base.Initialize(_baseNavigationManager.BaseUri, _baseNavigationManager.Uri);

    private void OnBaseLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        this.Uri = e.Location;
        // Trigger the Location Changed event for all listeners
        this.NotifyLocationChanged(e.IsNavigationIntercepted);
    }

    public void Dispose()
        => _baseNavigationManager.LocationChanged -= OnBaseLocationChanged;
}

