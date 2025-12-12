using System.Text.Json;
using System.Text.Json.Serialization;
using Dunet;
using NetDaemon.Extensions.SourceGen.src.Model.UtilityTypes;

namespace NetDaemon.Extensions.SourceGen.Model;

[Union]
[JsonConverter(typeof(JsonAnyJsonConverter))]
public partial record JsonAny
{
    public object? ToObject() => WrappedValue;
    protected virtual Object? WrappedValue { get;}

    public partial record Null() {
        protected override Object? WrappedValue => null;
    }

    public partial record Int(int Value) {
        protected override Object? WrappedValue => Value;
    }

    public partial record Double(double Value){
        protected override Object? WrappedValue => Value;
    }

    public partial record String(string Value){
        protected override Object? WrappedValue => Value;
    }

    public partial record Bool(bool Value){
        protected override Object? WrappedValue => Value;
    }

    public partial record DateTime(System.DateTime Value){
        protected override Object? WrappedValue => Value;
    }
    public partial record JsonArray(IReadOnlyCollection<JsonAny> Value, Type? ValueType) {
        protected override Object? WrappedValue => Value;
    }
    public partial record JsonObject(Dictionary<string, JsonAny> Value, Type? ValueType)  {
        protected override Object? WrappedValue => Value;
    }

    public partial record Element(JsonElement Value)    {
        protected override Object? WrappedValue => Value;
    }
}