# Blazr.Demo.Routing

This repo contains the code that demonstrates the features of the `BlazrRouter` and `BlazrNavigationManager` in the Blazor.Routing library.  It's principle purpose is to provide  functionality to turn routing/navigation on and off.  The principle application of this functionality is in edit forms to stop navigation when the form state is dirty.

[The Detail](./detail.md).

It's a culmination of many iterations of my own work and code with a contributions from elsewhere including a NavigationManager implementation by Adam Stevenson documented in his [Github Repo here](https://github.com/SL-AdamStevenson).  There's a significant amount of Microsoft AspnetCore repository "lifted" code to rebuild the Blazor Router.

The Repo is structured using my [Blazor Clean Design Template](https://github.com/ShaunCurtis/Blazr.Demo).  It's Net6.0,  can be run in either Web Assembly or Server modes - it's the same code base.  Just set the appropriate Startup project.

Go to the "Edit Form Demo" page to see the form/page locking concepts in action.  Click on the button to toggle the form from dirty to clean and then attempt to navigate away from the page by clicking links, using the back button, F5 or clicking on favourites.  There's also some cheeky feedback messages to show the events.

## How Navigation Manager Works

In both Web Assembly and Server all in-browser navigation events are captured by the Blazor JS code.  However, they surface very differently in the NetCore libraries.

In Web Assembly the JS navigation event code calls the `NotifyLocationChanged` JsInterop registered method.  This gets the current instance of `WebAssemblyNavigationManager` and calls `SetLocation`.  Note this gets the instance of `WebAssemblyNavigationManager` directly through it's `Instance` static method, not through the DI container.

```csharp
public static class JSInteropMethods
{
    [JSInvokable(nameof(NotifyLocationChanged))]
    public static void NotifyLocationChanged(string uri, bool isInterceptedLink)
    {
        WebAssemblyNavigationManager.Instance.SetLocation(uri, isInterceptedLink);
    }
}
```

In Server the calls get transferred over the SignalR connection and are handled by the `ComponentHub`

```csharp
public async ValueTask OnLocationChanged(string uri, bool intercepted)
{
    var circuitHost = await GetActiveCircuitAsync();
    if (circuitHost == null)
        return;

    _ = circuitHost.OnLocationChangedAsync(uri, intercepted);
}
```

This calls the `CircuitHost` method:

```csharp
public async Task OnLocationChangedAsync(string uri, bool intercepted)
{
    await Renderer.Dispatcher.InvokeAsync(() =>
    {
        var navigationManager = (RemoteNavigationManager)Services.GetRequiredService<NavigationManager>();
        navigationManager.NotifyLocationChanged(uri, intercepted);
    });
 }
//lots of code missing to only show the relevant lines
 ```

This time it gets the registered `NavigationManager` service and casts it as a `RemoteNavigationManager` object before calling the `NotifyLocationChanged` method.

## Running the Demo

The demo can be run in either Web Assembly or Server modes.

Set *Blazr.Demo.Routing.Server.Web* as the startup project to run the Server version.

Set *Blazr.Demo.Routing.WASM.Web* as the startup project to run the Web Assembly version.



## Implementation the Solution in a Project

1. Add Blazr.Routing to the solution, and add project references where required. 

2. Change out the `Router` in `App.razor`

```csharp
@namespace Blazr.UI
<BlazrRouter AppAssembly="@typeof(App).Assembly">
  ......
</BlazrRouter>
```

3. Add the new Navigation Manager service

```csharp
services.AddBlazrNavigationManager();
```

4. Add the JS reference to the SPA startup page -  __Hosts.html, __Layout.html or index.html .

```csharp
    <script src="_content/Blazr.Routing/site.js"></script>
```

5. Review `EditForm.razor` for an implementation demo of how to control the `BlazrNavigationManager` lock state.

6. Add the `PageLocker` component to the page.

```csharp
@page "/EditForm"
@namespace Blazr.UI

<PageLocker />
//.....
```
