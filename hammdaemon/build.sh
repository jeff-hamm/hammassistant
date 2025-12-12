OUT_DIR="${1:-${0%/*}/../netdaemon5}"
dotnet publish "${0%/*}/src/Hammassistant/Hammassistant.csproj" -c Release -o "$OUT_DIR"
. "${0%/*}/restart.sh"