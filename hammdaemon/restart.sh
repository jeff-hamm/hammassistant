#!/usr/bin/env bashio

addon='c6a2317c_netdaemon5'
bashio::addon.stop $addon
bashio::addon.start $addon
