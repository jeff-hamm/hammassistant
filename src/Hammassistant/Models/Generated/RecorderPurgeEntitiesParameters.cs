//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.1311211-08:00
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
public partial record RecorderPurgeEntitiesParameters
{
    ///<summary>List of entities for which the data is to be removed from the recorder database.</summary>
    [JsonPropertyName("entity_id")]
    public IEnumerable<string>? EntityId { get; init; }

    ///<summary>List of domains for which the data needs to be removed from the recorder database. eg: sun</summary>
    [JsonPropertyName("domains")]
    public object? Domains { get; init; }

    ///<summary>List of glob patterns used to select the entities for which the data is to be removed from the recorder database. eg: domain*.object_id*</summary>
    [JsonPropertyName("entity_globs")]
    public object? EntityGlobs { get; init; }

    ///<summary>Number of days to keep the data for rows matching the filter. Starting today, counting backward. A value of `7` means that everything older than a week will be purged. The default of 0 days will remove all matching rows immediately.</summary>
    [JsonPropertyName("keep_days")]
    public double? KeepDays { get; init; }
}