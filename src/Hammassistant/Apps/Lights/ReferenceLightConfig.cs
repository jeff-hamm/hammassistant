namespace Hammlet.Apps.Lights;

public class ReferenceLightConfig
{
    internal Guid Id { get; } = Guid.NewGuid();
    public string EntityId { get; set; } = "light.reference_light";
    public string TracksReferenceLabel { get; set; } = "watch_reference_light";
    public string[] TrackedEntities { get; set; } = ["light.color_bulbs","light.bedroom_fan_light","light.hammlet_bathroom"
        ,"light.hammlet_pantry","light.kitchen_ceiling","light.kitchen_light2","light.liminal_ceiling","light.living_fan","light.plant_lamp",
    "light.laser_lamp"];

    public bool CreateReferenceLight { get; set; } = true;
}
