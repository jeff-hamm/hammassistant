//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.2179806-08:00
//
// *** Make sure the version of the codegen tool and your nugets NetDaemon.* have the same version.***
// You can use following command to keep it up to date with the latest version:
//   dotnet tool update NetDaemon.HassModel.CodeGen
//
// To update this file with latest entities run this command in your project directory:
//   dotnet tool run nd-codegen
//
// In the template projects we provided a convenience powershell script that will update
// the codegen and nugets to latest versions update_all_dependencies.ps1.
//
// For more information: https://netdaemon.xyz/docs/user/hass_model/hass_model_codegen
// For more information about NetDaemon: https://netdaemon.xyz/
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetDaemon.HassModel;
using NetDaemon.HassModel.Entities;
using NetDaemon.HassModel.Entities.Core;

namespace Hammlet.NetDaemon.Models;
public static class WeatherEntityExtensionMethods
{
    ///<summary>Get weather forecasts.</summary>
    public static Task<JsonElement?> GetForecastsAsync(this IWeatherEntityCore target, WeatherGetForecastsParameters data)
    {
        return target.CallServiceWithResponseAsync("get_forecasts", data);
    }

    ///<summary>Get weather forecasts.</summary>
    ///<param name="target">The IWeatherEntityCore to call this service for</param>
    ///<param name="type">Forecast type: daily, hourly or twice daily.</param>
    public static Task<JsonElement?> GetForecastsAsync(this IWeatherEntityCore target, object @type)
    {
        return target.CallServiceWithResponseAsync("get_forecasts", new WeatherGetForecastsParameters { Type = @type });
    }

    ///<summary>Get weather forecasts.</summary>
    public static void GetForecasts(this IWeatherEntityCore target, WeatherGetForecastsParameters data)
    {
        target.CallService("get_forecasts", data);
    }

    ///<summary>Get weather forecasts.</summary>
    public static void GetForecasts(this IEnumerable<IWeatherEntityCore> target, WeatherGetForecastsParameters data)
    {
        target.CallService("get_forecasts", data);
    }

    ///<summary>Get weather forecasts.</summary>
    ///<param name="target">The IWeatherEntityCore to call this service for</param>
    ///<param name="type">Forecast type: daily, hourly or twice daily.</param>
    public static void GetForecasts(this IWeatherEntityCore target, object @type)
    {
        target.CallService("get_forecasts", new WeatherGetForecastsParameters { Type = @type });
    }

    ///<summary>Get weather forecasts.</summary>
    ///<param name="target">The IEnumerable&lt;IWeatherEntityCore&gt; to call this service for</param>
    ///<param name="type">Forecast type: daily, hourly or twice daily.</param>
    public static void GetForecasts(this IEnumerable<IWeatherEntityCore> target, object @type)
    {
        target.CallService("get_forecasts", new WeatherGetForecastsParameters { Type = @type });
    }
}