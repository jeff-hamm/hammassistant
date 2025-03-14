//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:03.9160076-08:00
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
public partial record UpdateAttributes
{
    [JsonPropertyName("auto_update")]
    public bool? AutoUpdate { get; init; }

    [JsonPropertyName("display_precision")]
    public double? DisplayPrecision { get; init; }

    [JsonPropertyName("installed_version")]
    public string? InstalledVersion { get; init; }

    [JsonPropertyName("in_progress")]
    public bool? InProgress { get; init; }

    [JsonPropertyName("latest_version")]
    public string? LatestVersion { get; init; }

    [JsonPropertyName("release_summary")]
    public string? ReleaseSummary { get; init; }

    [JsonPropertyName("release_url")]
    public string? ReleaseUrl { get; init; }

    [JsonPropertyName("skipped_version")]
    public object? SkippedVersion { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("update_percentage")]
    public object? UpdatePercentage { get; init; }

    [JsonPropertyName("entity_picture")]
    public string? EntityPicture { get; init; }

    [JsonPropertyName("friendly_name")]
    public string? FriendlyName { get; init; }

    [JsonPropertyName("supported_features")]
    public double? SupportedFeatures { get; init; }

    [JsonPropertyName("device_class")]
    public string? DeviceClass { get; init; }
}