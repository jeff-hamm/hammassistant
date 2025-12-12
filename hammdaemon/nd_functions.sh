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
    export background_pid=$!
    trap "trap - SIGINT SIGTERM EXIT && kill $background_pid" SIGINT SIGTERM EXIT
    bashio $nd_src/restart.sh
    sleep 10
    kill $background_pid
    export background_pid=
}

function publish() {
    rm -rf "$nd_path/*"
    dotnet publish "$nd_src/src/Hammassistant/Hammassistant.csproj" -c Release -o "$nd_path" -p:NetDaemon__ApplicationConfigurationFolder=$NetDaemon__ApplicationConfigurationFolder -tl:off $@
    if [ $? -ne 0 ]; then
        return
    fi
    restart
}

function update() {
    pushd $nd_src
    git pull --recurse-submodules
    if [ $? -ne 0 ]; then
        popd
        return
    fi
    if [ ! -z $ZSH_CUSTOM ]; then
        target_file=$ZSH_CUSTOM/nd_functions.zsh
    else
        target_file=~/nd_functions.zsh
    fi
    echo "export nd_src=$nd_src" > "$target_file"
    echo "export nd_config_path=$nd_config_path" >> "$target_file"
    echo "export NetDaemon__ApplicationConfigurationFolder=$nd_config_path" >> "$target_file"
    cat "$nd_src/nd_functions.sh" >> "$target_file"
    source "$target_file"
    popd
    publish $@
}