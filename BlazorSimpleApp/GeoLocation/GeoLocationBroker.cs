using BlazorSimpleApp.Models;
using Microsoft.JSInterop;

namespace BlazorSimpleApp.GeoLocation;

public class GeoLocationBroker : IGeoLocationBroker
{
    private readonly IJSRuntime jsRuntime;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;
    private readonly DotNetObjectReference<GeoLocationBroker> dotNetObjectReference;

    public GeoLocationBroker(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;

        moduleTask = new(() => this.jsRuntime!.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./geoLocationJsInterop.js")
            .AsTask());

        dotNetObjectReference = DotNetObjectReference.Create(this);
    }

    public async ValueTask RequestGeoLocationAsync(bool enableHighAccuracy, int maximumAgeInMilliseconds)
    {
        var module = await moduleTask.Value;
        var dotNetObjectReference = this.dotNetObjectReference;

        await module.InvokeVoidAsync(identifier: "getCurrentPosition",
            dotNetObjectReference,
            enableHighAccuracy,
            maximumAgeInMilliseconds);
    }

    public async ValueTask RequestGeoLocationAsync()
    {
        await RequestGeoLocationAsync(enableHighAccuracy: true, maximumAgeInMilliseconds: 0);
    }

    public event Func<Coordinate, ValueTask> CoordinatesChanged = default!;

    public event Func<GeolocationPositionError, ValueTask> OnGeolocationPositionError = default!;

    [JSInvokable]
    public async Task OnSuccessAsync(Coordinate coordinates)
    {
        await CoordinatesChanged.Invoke(coordinates);
    }

    [JSInvokable]
    public async Task OnErrorAsync(GeolocationPositionError error)
    {
        await OnGeolocationPositionError.Invoke(error);
    }
}