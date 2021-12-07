/// ============================================================
/// Original Code Adam Stevenson - https://github.com/SL-AdamStevenson
/// Modified By: Shaun Curtis, Cold Elm Coders
/// License:  MIT
/// Mods: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================


namespace Blazr.Routing
{
    public class BlazrNavigationManager : Microsoft.AspNetCore.Components.NavigationManager
    {
        private Microsoft.AspNetCore.Components.NavigationManager _UnderlyingNavigationManager;
        private bool _isBlindNavigation = false;

        public event EventHandler<NavigationData>? BeforeLocationChange;

        public BlazrNavigationManager(Microsoft.AspNetCore.Components.NavigationManager? underlyingNavigationManager)
        {
            _UnderlyingNavigationManager = underlyingNavigationManager!;

            base.Initialize(underlyingNavigationManager!.BaseUri, underlyingNavigationManager.Uri);

            _UnderlyingNavigationManager.LocationChanged += OnUnderlyingNavigationManagerLocationChanged;
        }

        protected override void EnsureInitialized()
        {
            base.Initialize(_UnderlyingNavigationManager.BaseUri, _UnderlyingNavigationManager.Uri);
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            // Call the underlying navigation manager.
            _UnderlyingNavigationManager.NavigateTo(uri, forceLoad);
        }

        private NavigationData NotifyBeforeLocationChange(LocationChangedEventArgs e)
        {
            var navigation = new NavigationData()
            {
                CurrentLocation = this.Uri,
                NewLocation = e.Location,
                IsNavigationIntercepted = e.IsNavigationIntercepted,
                IsCanceled = false
            };

            BeforeLocationChange?.Invoke(this, navigation);

            return navigation;
        }

        private void OnUnderlyingNavigationManagerLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            var navigation = NotifyBeforeLocationChange(e);

            // Check our blind navigation flag.  If we are blind navigating
            // we just set the flag back to false and exit
            if (_isBlindNavigation)
            {
                _isBlindNavigation = false;

                return;
            }

            // Navigation is cancelled
            // we set the flag so we don't create a loop with the dummy run
            if (navigation.IsCanceled)
            {
                // prevents a NavigateTo loop
                _isBlindNavigation = true;

                // Puts the link back - else it will change, but the page will not navigate.
                _UnderlyingNavigationManager.NavigateTo(this.Uri, false);

                return;
            }

            // Normal Navigation path

            // NOTE: We set the Uri before calling notify location changed, as it will use this uri property in its args.
            this.Uri = e.Location;

            // Trigger the Location Changed event for all listeners including the Router
            this.NotifyLocationChanged(e.IsNavigationIntercepted);

            // Belt and braces to ensure false 
            _isBlindNavigation = false;
        }
    }
}
