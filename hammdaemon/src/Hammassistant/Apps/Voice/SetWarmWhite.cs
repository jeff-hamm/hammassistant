using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Hammlet.NetDaemon.Extensions;
using Hammlet.NetDaemon.Models;
using HassModel;
using NetDaemon.Extensions.Tts;
using NetDaemon.HassModel.Entities;

namespace Hammlet.Apps.Voice;

public class SentenceTrigger
{
    public string id { get; set; }
    public string idx { get; set; }
    public object alias { get; set; }
    public string platform { get; set; }
    public string sentence { get; set; }
    public Details details { get; set; }
    public Slots slots { get; set; } = new Slots();
    public string device_id { get; set; }
    public User_Input user_input { get; set; }
}

public class Details
{
    public Area? area { get; set; }
}

public class Area
{
    public string name { get; set; }
    public string text { get; set; }
    public string value { get; set; }
}

public class Slots : Dictionary<string, object>
{
    public string? area { get; set; }
    public string? name { get; set; }
}

public class User_Input
{
    public string text { get; set; }
    public Context context { get; set; }
    public string conversation_id { get; set; }
    public string device_id { get; set; }
    public string language { get; set; }
    public string agent_id { get; set; }
    public object extra_system_prompt { get; set; }
}

public class Context
{
    public string id { get; set; }
    public object parent_id { get; set; }
    public object user_id { get; set; }
}
#if HAMMLET
[NetDaemonApp]
internal class SetWarmWhite
{
    public SetWarmWhite(IHaContext ha, ITriggerManager triggerManager, 
        TtsEntities ttsEntities, IScheduler scheduler,
        IHaRegistry areaNavigator, ITextToSpeechService tts, MediaPlayerEntities mediaPlayers,MusicAssistantServices musicAssistant,
        ILogger<SetWarmWhite> logger)
    {
        triggerManager.RegisterTrigger<SentenceTrigger>(new
        {
            platform = "conversation",
            command = new[] { "[set|change] lights [in [the] {area}] to warm white" }
        }).Subscribe(t =>
        {
            logger.LogInformation("{data}", t);
            if(t?.slots?.area is { } a)
            {
                foreach(var light in areaNavigator.GetArea(a.ToSnakeCase())?
                            .Entities.OfType<LightEntity>() ?? [])
                {
                    if(light.IsOn())
                        light.MakeWarm();
                }
            }
        });

        var context = new SpeechContext(ttsEntities.Piper, mediaPlayers.HeyNabu, triggerManager, 
            musicAssistant,mediaPlayers, scheduler);

        
        context
            .RegisterSentence("Do you like {name} butt")
            .Then(t =>
            {
                t.Say(m => $"Yes, {m["name"]} has a great butt!");
                scheduler.Schedule(TimeSpan.FromMilliseconds(1000), _ =>
                {
                    t.Play("ytmusic://track/Q0pM8XeEz7o", TimeSpan.FromSeconds(30));
                });
            });
    }
}

public record RegisteredSpeechContext(IObservable<SentenceTrigger?> Trigger, SpeechContext Context);
public class MusicAssistantServices
{
    public void PlayMedia(ServiceTarget target, string url, string mediaType)
    {
        throw new NotImplementedException();
    }
}
public record SpeechContext(TtsEntity Tts, MediaPlayerEntity Player, 
    ITriggerManager TriggerManager, MusicAssistantServices MusicAssistant, MediaPlayerEntities Players, IScheduler scheduler);

public record SpeechTrigger(SentenceTrigger Sentence, SpeechContext Context);
public static class SentencExtensions
{
    public static RegisteredSpeechContext RegisterSentence(this SpeechContext @this, params string[] command)
    {
        return 
            new RegisteredSpeechContext(
            @this.TriggerManager.RegisterTrigger<SentenceTrigger>(new
        {
            platform = "conversation",
            command
        }),@this);
    }
    
    public static void Then(this RegisteredSpeechContext @this,Action<SpeechTrigger> action) =>
        @this.Trigger.Subscribe(t =>
        {
            action(new SpeechTrigger(t, @this.Context));
        });
    public static SpeechTrigger Say(this SpeechTrigger @this,Func<Dictionary<string,object>,string> message)
    {
        @this.Context.Tts.Speak(@this.Context.Player.EntityId, message(@this.Sentence.slots));
        return @this;
    }
    public static SpeechTrigger Play(this SpeechTrigger @this,string url, TimeSpan? duration=null)
    {

        @this.Context.MusicAssistant.PlayMedia(
            ServiceTarget.FromEntity(@this.Context.Players.HeyNabuMediaPlayer.EntityId),
            url,mediaType:"track");
        if (duration != null)
        {
            @this.Context.scheduler.Schedule(duration.Value,
                () => @this.Context.Players.HeyNabuMediaPlayer.MediaStop());
        }
        return @this;
    }

    public static SpeechTrigger Say(this SpeechTrigger @this,string message)
    {
        @this.Context.Tts.Speak(@this.Context.Player.EntityId, message);
        return @this;
    }

    public static void ThenSay(this RegisteredSpeechContext @this,string message) =>
        @this.Trigger.Subscribe(t =>
        {
            @this.Context.Tts.Speak(@this.Context.Player.EntityId, message);
        });
    public static void ThenSay(this RegisteredSpeechContext @this,Func<Dictionary<string,object>?,string> message) =>
        @this.Trigger.Subscribe(t =>
        {
            @this.Context.Tts.Speak(@this.Context.Player.EntityId, message(t.slots));
        });
}
/*alias: Sentence - Area light to Warm White
   description: ""
   triggers:
     - trigger: conversation
       command:
         - "[set|change] lights [in [the] {area}] to warm white"
   conditions: []
   actions:
     - variables:
         area: >-
           {{ area_name(trigger.device_id) if trigger.slots.area is undefined else
           trigger.slots.area }}
     - action: light.turn_on
       data:
         kelvin: 2800
       target:
         entity_id: |
           {{ expand(area_entities(area))
           | selectattr('entity_id', 'match', 'light.')
           | selectattr('state', 'eq', 'on')
           | map(attribute='entity_id')
           | list }}
   mode: single
}
/*
#endif