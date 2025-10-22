using System.ComponentModel;

namespace AlquilaFacilPlatform.Locals.Domain.Model.ValueObjects;

public enum EALocalCategoryTypes
{
    [Description("Casa de playa")]
    BeachHouse,

    [Description("Casa de campo")]
    LandscapeHouse,

    [Description("Casa urbana")]
    CityHouse,

    [Description("Salón elegante")]
    ElegantRoom
}
