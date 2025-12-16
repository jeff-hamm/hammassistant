#!/usr/bin/env python3
import json
import os
import sys
import time
import urllib.error
import urllib.request
from pathlib import Path


def load_env_from_secret(env_path: Path) -> None:
    if not env_path.exists():
        return
    for line in env_path.read_text(encoding="utf-8", errors="replace").splitlines():
        line = line.strip()
        if not line or line.startswith("#") or "=" not in line:
            continue
        key, value = line.split("=", 1)
        key = key.strip()
        value = value.strip().strip('"').strip("'")
        os.environ.setdefault(key, value)


def pick_base_url(urls: list[str], token: str) -> str:
    headers = {"Authorization": f"Bearer {token}"}
    for base in urls:
        req = urllib.request.Request(f"{base}/api/", headers=headers)
        try:
            with urllib.request.urlopen(req, timeout=5) as r:
                if r.status == 200:
                    return base
        except Exception:
            continue
    raise RuntimeError(f"Failed to connect to any Home Assistant URL: {urls}")


def api_get(base_url: str, token: str, path: str) -> dict:
    req = urllib.request.Request(
        f"{base_url}{path}",
        headers={"Authorization": f"Bearer {token}", "Content-Type": "application/json"},
    )
    with urllib.request.urlopen(req, timeout=10) as r:
        return json.loads(r.read().decode("utf-8"))


def api_post(base_url: str, token: str, path: str, payload: dict) -> int:
    body = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(
        f"{base_url}{path}",
        method="POST",
        headers={"Authorization": f"Bearer {token}", "Content-Type": "application/json"},
        data=body,
    )
    with urllib.request.urlopen(req, timeout=10) as r:
        # HA returns 200 and a JSON list
        _ = r.read()
        return r.status


def api_post_text(base_url: str, token: str, path: str, payload: dict) -> str:
    body = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(
        f"{base_url}{path}",
        method="POST",
        headers={"Authorization": f"Bearer {token}", "Content-Type": "application/json"},
        data=body,
    )
    with urllib.request.urlopen(req, timeout=10) as r:
        return r.read().decode("utf-8")


def summarize_state(ent: dict) -> dict:
    attrs = ent.get("attributes", {})
    return {
        "state": ent.get("state"),
        "brightness": attrs.get("brightness"),
        "color_mode": attrs.get("color_mode"),
        "last_changed": ent.get("last_changed"),
    }


def summarize_script(ent: dict) -> dict:
    attrs = ent.get("attributes", {})
    return {"state": ent.get("state"), "last_triggered": attrs.get("last_triggered")}


def main() -> int:
    script_dir = Path(__file__).resolve().parent
    load_env_from_secret(script_dir / "secret.env")

    urls_raw = os.environ.get("HOME_ASSISTANT_URLS", "")
    token = os.environ.get("HOME_ASSISTANT_TOKEN", "")

    urls = [u.strip().rstrip("/") for u in urls_raw.replace("\r", "").split(",") if u.strip()]
    token = token.replace("\r", "").strip()

    if not urls or not token:
        print("Missing HOME_ASSISTANT_URLS or HOME_ASSISTANT_TOKEN", file=sys.stderr)
        return 2

    label_id = os.environ.get("LABEL_ID", "turn_off_sleep")
    lights_entity_id = os.environ.get("LIGHTS_ENTITY_ID", "").strip()
    entity_id = os.environ.get("MONITOR_ENTITY", "light.tentacle_lamp")
    transition_seconds = int(os.environ.get("TRANSITION_SECONDS", "10"))

    base_url = pick_base_url(urls, token)
    print(f"Using {base_url}")

    # Show what HA thinks this label contains (helps validate label_id usage)
    try:
        rendered = api_post_text(
            base_url,
            token,
            "/api/template",
            {"template": f"{{{{ label_entities('{label_id}') }}}}"},
        )
        print(f"Label entities for {label_id!r}:", rendered.strip())
    except Exception as e:
        print(f"(Could not query /api/template for label_entities: {e})")

    before = api_get(base_url, token, f"/api/states/{entity_id}")
    print("Before", summarize_state(before))

    fader_before = api_get(base_url, token, "/api/states/script.target_lights_fader")
    inner_before = api_get(base_url, token, "/api/states/script.ashley_s_light_fader")
    print("Script before", {"script.target_lights_fader": summarize_script(fader_before), "script.ashley_s_light_fader": summarize_script(inner_before)})

    lights_target = {"entity_id": lights_entity_id} if lights_entity_id else {"label_id": label_id}

    payload = {
        "lights": lights_target,
        "lampBrightnessScale": "zeroToTwoFiftyFive",
        "transitionTime": {"hours": 0, "minutes": 0, "seconds": transition_seconds},
        "easingTypeInput": "auto",
        "endBrightnessPercent": 0,
        "endBrightnessEntityScale": "zeroToOneHundred",
        "shouldStopIfTheLampIsTurnedOffDuringTheFade": True,
        "shouldResetTheStopEntityToOffAtStart": False,
        "shouldInvertTheValueOfTheStopEntity": False,
        "shouldTryToUseNativeLampTransitionsToo": True,
        "isDebugMode": True,
    }

    status = api_post(base_url, token, "/api/services/script/target_lights_fader", payload)
    print(
        "Called script.target_lights_fader",
        status,
        {"lights": lights_target, "transition_seconds": transition_seconds},
    )

    fader_after = api_get(base_url, token, "/api/states/script.target_lights_fader")
    inner_after = api_get(base_url, token, "/api/states/script.ashley_s_light_fader")
    print("Script after", {"script.target_lights_fader": summarize_script(fader_after), "script.ashley_s_light_fader": summarize_script(inner_after)})

    for i in range(transition_seconds + 2):
        time.sleep(1)
        ent = api_get(base_url, token, f"/api/states/{entity_id}")
        print(f"t+{i+1:02d}s", summarize_state(ent))

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
