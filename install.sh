wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
apt-get update && apt-get install -y dotnet-sdk-9.0
git submodule update --init --recursive 
chmod +x nd_functions.sh
export nd_src="${1:-0%/*}"
shift
nd_config_path=${1:-$(read -p "Config Path: " config_path && echo "$config_path")}
if [[ ! "$nd_config_path" =~ ^/ ]]; then
    nd_config_path="$nd_src/Environments/$nd_config_path"
fi
export nd_config_path=$nd_config_path
if [ -z $ZSH_CUSTOM ]; then
    echo "source ~/nd_functions.sh" >> "~/.bashrc"
fi
source "{0%/*}/nd_functions.sh"
update