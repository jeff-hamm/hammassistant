//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:04.1740771-08:00
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
public partial record TodoUpdateItemParameters
{
    ///<summary>The current name of the to-do item. eg: Submit income tax return</summary>
    [JsonPropertyName("item")]
    public string? Item { get; init; }

    ///<summary>The new name for the to-do item eg: Something else</summary>
    [JsonPropertyName("rename")]
    public string? Rename { get; init; }

    ///<summary>A status or confirmation of the to-do item. eg: needs_action</summary>
    [JsonPropertyName("status")]
    public object? Status { get; init; }

    ///<summary>The date the to-do item is expected to be completed. eg: 2023-11-17</summary>
    [JsonPropertyName("due_date")]
    public DateOnly? DueDate { get; init; }

    ///<summary>The date and time the to-do item is expected to be completed. eg: 2023-11-17 13:30:00</summary>
    [JsonPropertyName("due_datetime")]
    public DateTime? DueDatetime { get; init; }

    ///<summary>A more complete description of the to-do item than provided by the item name. eg: A more complete description of the to-do item than that provided by the summary.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}