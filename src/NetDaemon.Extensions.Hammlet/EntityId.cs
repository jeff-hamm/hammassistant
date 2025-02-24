using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vogen;

namespace NetDaemon.Extensions.Hammlet;


[ValueObject<string>(parsableForStrings:ParsableForStrings.GenerateMethods,
    fromPrimitiveCasting: CastOperator.Implicit,
    toPrimitiveCasting: CastOperator.Implicit)]
[StructLayout(LayoutKind.Auto)]
public partial struct EntityId
{

    private Match? _match;
    private Match Match => _match ??= EntityIdRegex().Match(this.Value);
    public string Entity => Match.Groups["entity"].Value;
    public string? Domain => Match.Groups["domain"] is {Success:true, Value:var v} ? v :null;

    public bool HasDomain => !string.IsNullOrEmpty(Domain);

    public static readonly string[] NumericDomains = ["input_number", "number", "proximity"];
    public static readonly string[] MixedDomains = ["sensor"];

    [GeneratedRegex(@"^((?<domain>[^\.\s]+)\.)?(?<entity>[^\s]+)$")]
    public static partial Regex EntityIdRegex();
    private static string Normalize(string input) => input.ToLowerInvariant();
    private static Validation Validate(string input) => EntityIdRegex().IsMatch(input)  ? Validation.Ok : Validation.Invalid("Invalid entityId");

    public EntityId InDomain(string newDomain)
    {
        return newDomain.Equals(Domain, StringComparison.InvariantCultureIgnoreCase) ? this : EntityId.From(newDomain + "." + Entity);
    }

}