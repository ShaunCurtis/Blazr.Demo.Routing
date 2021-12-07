/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.Routing
{
    public class NavigationData
    {
        public bool IsCanceled { get; set; }

        public string NewLocation { get; set; } = string.Empty;

        public string? CurrentLocation { get; set; } = String.Empty;

        public bool IsNavigationIntercepted { get; set; }
    }
}
