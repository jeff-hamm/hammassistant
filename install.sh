wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
apt-get update && apt-get install -y dotnet-sdk-9.0
git submodule update --init --recursive 
chmod +x functions.sh
nd_src="${0%/*}"
nd_config_path=${1:-$(read -p "Config Path: " config_path && echo "$config_path")}
if [[ ! "$nd_config_path" =~ ^/ ]]; then
    nd_config_path="$nd_src/Environments/$nd_config_path"
fi
if [ ! -z $ZSH_CUSTOM ]; then
    echo "export nd_src=$nd_src" > "$ZSH_CUSTOM/nd_functions.zsh"
    echo "export nd_config_path=$nd_config_path" >> "$ZSH_CUSTOM/nd_functions.zsh"
    echo "export NetDaemon__ApplicationConfigurationFolder=$nd_config_path" >> "$ZSH_CUSTOM/nd_functions.zsh"
    cat "$nd_src/nd_functions.sh" >> "$ZSH_CUSTOM/nd_functions.zsh"
else
    echo "export nd_src=$nd_src" >> "~/.bashrc"
    echo "export nd_config_path=$nd_config_path" >> "~/.bashrc"
    echo "export NetDaemon__ApplicationConfigurationFolder=$nd_config_path" >> "~/.bashrc"
    echo "source ~/nd_functions.sh" >> "~/.bashrc"
    cp "$nd_src/nd_functions.sh" "~/nd_functions.sh"
fi