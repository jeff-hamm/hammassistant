//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:03.8980678-08:00
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
[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
public enum SelectOptions
{
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("disabled")]
    Disabled,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("last")]
    Last,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("none")]
    None,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("open")]
    Open,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("paired only")]
    Pairedonly,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("pos")]
    Pos,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("power_off")]
    PowerOff,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("power_on")]
    PowerOn,
    [System.Text.Json.Serialization.JsonStringEnumMemberNameAttribute("relay")]
    Relay
}