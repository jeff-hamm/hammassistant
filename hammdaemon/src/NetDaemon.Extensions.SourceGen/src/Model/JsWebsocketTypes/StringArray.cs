using System.Text.Json.Serialization;
using Dunet;

namespace NetDaemon.Extensions.SourceGen.Model.JsWebsocket;

[Union]
[JsonConverter(typeof(StringArrayConverter))]
public partial record StringArray
{
    public partial record Single(string Value);

    public partial record Multiple(string[] Values);

}