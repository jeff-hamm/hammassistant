import logging
from os import path, walk

from homeassistant.components.frontend import add_extra_js_url
from homeassistant.components.http import StaticPathConfig
from homeassistant.components.http.view import HomeAssistantView
from homeassistant.config_entries import ConfigEntry
from homeassistant.core import HomeAssistant
from homeassistant.util import dt as dt_util

LOGGER = logging.getLogger(__name__)

NAME = "Emojii Symbols"
DOMAIN = "emojii_symbols"
VERSION = "0.1.1"

ICONS_URL = f"/{DOMAIN}"
LOADER_URL = f"/{DOMAIN}/emojii_symbols.js"
LOADER_URL_VERSIONED = f"{LOADER_URL}?v={VERSION}"
ICONS_PATH = f"custom_components/{DOMAIN}/data"
LOADER_PATH = f"custom_components/{DOMAIN}/emojii_symbols.js"


class ListingView(HomeAssistantView):
    requires_auth = False

    def __init__(self, url, iconset_path, hass, iconset_prefix):
        self.url = url
        self.name = f"api:{DOMAIN}:{iconset_prefix}"
        self.iconset_path = iconset_path
        self.hass = hass
        self._cache = None
        self._cache_time = None
        self._cache_duration = 300
        LOGGER.debug(
            "ListingView initialised with URL: %s, iconset_path: %s, iconset_prefix: %s",
            self.url,
            self.iconset_path,
            iconset_prefix,
        )

    async def get(self, request):
        current_time = dt_util.utcnow()
        if (
            self._cache is not None
            and self._cache_time is not None
            and (current_time - self._cache_time).total_seconds() < self._cache_duration
        ):
            return self.json(self._cache)

        try:
            icons_list = await self.hass.async_add_executor_job(
                self.get_icons_list, self.iconset_path
            )
            self._cache = icons_list
            self._cache_time = current_time
            return self.json(icons_list)
        except Exception as err:  # noqa: BLE001
            LOGGER.error("Error fetching icons list for %s: %s", self.iconset_path, err)
            return self.json([])

    def get_icons_list(self, iconset_path):
        icons = []
        try:
            if not path.exists(iconset_path):
                LOGGER.warning("Iconset path does not exist: %s", iconset_path)
                return icons

            for dirpath, _dirnames, filenames in walk(iconset_path):
                for fn in filenames:
                    if fn.endswith(".svg"):
                        icons.append({"name": fn[:-4]})
        except Exception as err:  # noqa: BLE001
            LOGGER.error("Error building icons list for %s: %s", iconset_path, err)

        return icons


async def async_setup(hass: HomeAssistant, config):
    LOGGER.info("Setting up %s with version %s", NAME, VERSION)

    try:
        await hass.http.async_register_static_paths(
            [StaticPathConfig(LOADER_URL, hass.config.path(LOADER_PATH), True)]
        )
        # Add a version query string to help bust aggressive browser caching.
        add_extra_js_url(hass, LOADER_URL_VERSIONED)

        iconset_prefixes = ["emo"]
        registered_count = 0

        for iconset_prefix in iconset_prefixes:
            icons_url = f"{ICONS_URL}/{iconset_prefix}"
            icons_path = hass.config.path(f"{ICONS_PATH}/{iconset_prefix}")
            icons_list_url = f"{icons_url}/icons.json"

            if not path.exists(icons_path):
                LOGGER.warning("Icon path does not exist, skipping: %s", icons_path)
                continue

            await hass.http.async_register_static_paths(
                [StaticPathConfig(icons_url, icons_path, True)]
            )
            hass.http.register_view(
                ListingView(icons_list_url, icons_path, hass, iconset_prefix)
            )
            registered_count += 1

        if registered_count == 0:
            LOGGER.error("No icon sets were registered - check if icon data exists")
            return False

        LOGGER.info(
            "%s setup complete. Registered %s/%s icon sets.",
            NAME,
            registered_count,
            len(iconset_prefixes),
        )
        return True

    except Exception as err:  # noqa: BLE001
        LOGGER.error("Error during %s setup: %s", NAME, err)
        return False


async def async_setup_entry(hass: HomeAssistant, entry: ConfigEntry):
    LOGGER.info("Setting up entry for %s: %s", NAME, entry)
    return True


async def async_remove_entry(hass: HomeAssistant, entry: ConfigEntry):
    LOGGER.info("Removing entry for %s: %s", NAME, entry)
    return True


async def async_migrate_entry(hass: HomeAssistant, entry: ConfigEntry) -> bool:
    if entry.version == 1:
        entry.version = 2
        hass.config_entries.async_update_entry(entry, title=NAME)
        LOGGER.info("Migrated %s config entry to version 2.", NAME)
    return True
