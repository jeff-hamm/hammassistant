//------------------------------------------------------------------------------
// <auto-generated>
// Generated using NetDaemon CodeGenerator nd-codegen v1.0.0.0
//   At: 2025-02-14T20:58:03.9267062-08:00
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
public partial class Services : IServices
{
    private readonly IHaContext _haContext;
    public Services(IHaContext haContext)
    {
        _haContext = haContext;
    }

    public AlarmControlPanelServices AlarmControlPanel => new(_haContext);
    public AutomationServices Automation => new(_haContext);
    public BackupServices Backup => new(_haContext);
    public ButtonServices Button => new(_haContext);
    public CameraServices Camera => new(_haContext);
    public CastServices Cast => new(_haContext);
    public ClimateServices Climate => new(_haContext);
    public CloudServices Cloud => new(_haContext);
    public CloudflareServices Cloudflare => new(_haContext);
    public ConversationServices Conversation => new(_haContext);
    public CounterServices Counter => new(_haContext);
    public CoverServices Cover => new(_haContext);
    public FanServices Fan => new(_haContext);
    public FfmpegServices Ffmpeg => new(_haContext);
    public FluxLedServices FluxLed => new(_haContext);
    public FrontendServices Frontend => new(_haContext);
    public GroupServices Group => new(_haContext);
    public HassioServices Hassio => new(_haContext);
    public HomeassistantServices Homeassistant => new(_haContext);
    public HumidifierServices Humidifier => new(_haContext);
    public InputBooleanServices InputBoolean => new(_haContext);
    public InputButtonServices InputButton => new(_haContext);
    public InputDatetimeServices InputDatetime => new(_haContext);
    public InputNumberServices InputNumber => new(_haContext);
    public InputSelectServices InputSelect => new(_haContext);
    public InputTextServices InputText => new(_haContext);
    public LightServices Light => new(_haContext);
    public LockServices Lock => new(_haContext);
    public LogbookServices Logbook => new(_haContext);
    public LoggerServices Logger => new(_haContext);
    public MediaPlayerServices MediaPlayer => new(_haContext);
    public MqttServices Mqtt => new(_haContext);
    public NotifyServices Notify => new(_haContext);
    public NumberServices Number => new(_haContext);
    public PersistentNotificationServices PersistentNotification => new(_haContext);
    public PersonServices Person => new(_haContext);
    public PythonScriptServices PythonScript => new(_haContext);
    public RecorderServices Recorder => new(_haContext);
    public SceneServices Scene => new(_haContext);
    public ScheduleServices Schedule => new(_haContext);
    public ScriptServices Script => new(_haContext);
    public SelectServices Select => new(_haContext);
    public ShoppingListServices ShoppingList => new(_haContext);
    public SirenServices Siren => new(_haContext);
    public SwitchServices Switch => new(_haContext);
    public SystemLogServices SystemLog => new(_haContext);
    public TextServices Text => new(_haContext);
    public TimerServices Timer => new(_haContext);
    public TodoServices Todo => new(_haContext);
    public TtsServices Tts => new(_haContext);
    public UpdateServices Update => new(_haContext);
    public VacuumServices Vacuum => new(_haContext);
    public ValveServices Valve => new(_haContext);
    public WeatherServices Weather => new(_haContext);
    public ZoneServices Zone => new(_haContext);
}