config_root=${config_root:-/config}
nd_name=${nd_name:-"netdaemon5"}
nd_slug=${nd_slug:-"c6a2317c_$nd_name"}
nd_path=${nd_path:-"$config_root/$nd_name"}
nd_src=${nd_src:-$config_root/hammassistant}
#nd_config_path=${nd_config_path}
function logs() {
    ha host logs --identifier addon_$nd_slug $@
}

function restart() {
    logs -f &
    background_pid=$!
    bashio $nd_src/restart.sh
    sleep 10
    kill $background_pid
}

function publish() {
    dotnet publish "$nd_src/src/Hammassistant/Hammassistant.csproj" -c Release -o "$nd_path"
}