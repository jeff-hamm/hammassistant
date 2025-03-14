//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:03.8267991-08:00
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
public partial class UpdateEntities : IEntityDomain<UpdateEntity>
{
    private readonly IHaContext _haContext;
    public UpdateEntities(IHaContext haContext)
    {
        _haContext = haContext;
    }

    /// <summary>Enumerates all update entities currently registered (at runtime) in Home Assistant as UpdateEntity</summary>
    public IEnumerable<UpdateEntity> EnumerateAll() => _haContext.GetAllEntities().Where(e => e.EntityId.StartsWith("update.")).Select(e => new UpdateEntity(e));
    public UpdateEntity Entity(string entityId)
    {
        return (UpdateEntity)_haContext.Entity(entityId);
    }

    ///<summary>Advanced SSH &amp; Web Terminal Update</summary>
    public UpdateEntity AdvancedSshWebTerminalUpdate => new(_haContext, "update.advanced_ssh_web_terminal_update");
    ///<summary>Bubble Card update</summary>
    public UpdateEntity BubbleCardUpdate => new(_haContext, "update.bubble_card_update");
    ///<summary>Clock Weather Card update</summary>
    public UpdateEntity ClockWeatherCardUpdate => new(_haContext, "update.clock_weather_card_update");
    ///<summary>Filebrowser Update</summary>
    public UpdateEntity FilebrowserUpdate => new(_haContext, "update.filebrowser_update");
    ///<summary>Get HACS Update</summary>
    public UpdateEntity GetHacsUpdate => new(_haContext, "update.get_hacs_update");
    ///<summary>HACS update</summary>
    public UpdateEntity HacsUpdate => new(_haContext, "update.hacs_update");
    ///<summary>Home Assistant Core Update</summary>
    public UpdateEntity HomeAssistantCoreUpdate => new(_haContext, "update.home_assistant_core_update");
    ///<summary>Home Assistant Operating System Update</summary>
    public UpdateEntity HomeAssistantOperatingSystemUpdate => new(_haContext, "update.home_assistant_operating_system_update");
    ///<summary>Home Assistant Supervisor Update</summary>
    public UpdateEntity HomeAssistantSupervisorUpdate => new(_haContext, "update.home_assistant_supervisor_update");
    ///<summary>iOS Themes - Dark Mode and Light Mode update</summary>
    public UpdateEntity IosThemesDarkModeAndLightModeUpdate => new(_haContext, "update.ios_themes_dark_mode_and_light_mode_update");
    ///<summary>layout-card update</summary>
    public UpdateEntity LayoutCardUpdate => new(_haContext, "update.layout_card_update");
    ///<summary>Light Entity Card update</summary>
    public UpdateEntity LightEntityCardUpdate => new(_haContext, "update.light_entity_card_update");
    ///<summary>Lightener update</summary>
    public UpdateEntity LightenerUpdate => new(_haContext, "update.lightener_update");
    ///<summary>Local Tuya update</summary>
    public UpdateEntity LocalTuyaUpdate => new(_haContext, "update.local_tuya_update");
    ///<summary>Matter Server Update</summary>
    public UpdateEntity MatterServerUpdate => new(_haContext, "update.matter_server_update");
    ///<summary>Mikaela Dimmer Firmware</summary>
    public UpdateEntity MikaelaDimmerFirmware => new(_haContext, "update.mikaela_dimmer_firmware");
    ///<summary>Mini Media Player update</summary>
    public UpdateEntity MiniMediaPlayerUpdate => new(_haContext, "update.mini_media_player_update");
    ///<summary>Mosquitto broker Update</summary>
    public UpdateEntity MosquittoBrokerUpdate => new(_haContext, "update.mosquitto_broker_update");
    ///<summary>Mushroom update</summary>
    public UpdateEntity MushroomUpdate => new(_haContext, "update.mushroom_update");
    ///<summary>NetDaemon V5 (.NET 9) Update</summary>
    public UpdateEntity NetdaemonV5Net9Update => new(_haContext, "update.netdaemon_v5_net_9_update");
    ///<summary>Samba share Update</summary>
    public UpdateEntity SambaShareUpdate => new(_haContext, "update.samba_share_update");
    ///<summary>Studio Code Server Update</summary>
    public UpdateEntity StudioCodeServerUpdate => new(_haContext, "update.studio_code_server_update");
    ///<summary>Z-Wave JS UI Update</summary>
    public UpdateEntity ZWaveJsUiUpdate => new(_haContext, "update.z_wave_js_ui_update");
    public UpdateIds Ids => new();
}