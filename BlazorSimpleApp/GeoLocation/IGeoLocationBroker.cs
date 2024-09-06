using BlazorSimpleApp.Models;

namespace BlazorSimpleApp.GeoLocation;

public interface IGeoLocationBroker
{
    ValueTask RequestGeoLocationAsync(bool enableHighAccuracy, int maximumAgeInMilliseconds);

    ValueTask RequestGeoLocationAsync();

    event Func<Coordinate, ValueTask> CoordinatesChanged;

    event Func<GeolocationPositionError, ValueTask> OnGeolocationPositionError;
}