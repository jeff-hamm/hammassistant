#!/usr/bin/env python3

from __future__ import annotations

import argparse
import json
import pathlib
import shutil
import tarfile
import tempfile
import urllib.parse
import urllib.request


COMPONENT_DIR = pathlib.Path(__file__).resolve().parents[1]
DEFAULT_MAP_PATH = COMPONENT_DIR / "tools" / "svgmoji_icons.json"
DEST_DIR = COMPONENT_DIR / "data" / "emo"

NPM_REGISTRY_URL = "https://registry.npmjs.org"
SVGMOJI_GITHUB_TARBALL = "https://codeload.github.com/svgmoji/svgmoji/tar.gz/{ref}"

ALIASES_PATH = DEST_DIR / "aliases.json"


def download(url: str, dest: pathlib.Path) -> None:
    dest.parent.mkdir(parents=True, exist_ok=True)
    req = urllib.request.Request(
        url,
        headers={
            "User-Agent": "emojii_symbols-sync/1.0",
        },
    )
    with urllib.request.urlopen(req, timeout=60) as resp:
        data = resp.read()
    dest.write_bytes(data)


def fetch_json(url: str) -> dict:
    req = urllib.request.Request(
        url,
        headers={
            "User-Agent": "emojii_symbols-sync/1.0",
            "Accept": "application/json",
        },
    )
    with urllib.request.urlopen(req, timeout=60) as resp:
        return json.loads(resp.read().decode("utf-8"))


def npm_tarball_url(package: str, version: str) -> str:
    encoded = urllib.parse.quote(package, safe="@/")
    meta = fetch_json(f"{NPM_REGISTRY_URL}/{encoded}")

    if version == "latest":
        resolved = meta.get("dist-tags", {}).get("latest")
    else:
        resolved = version

    versions = meta.get("versions", {})
    info = versions.get(resolved)
    if not info:
        raise SystemExit(f"Version not found on npm: {package}@{resolved}")

    tarball = info.get("dist", {}).get("tarball")
    if not tarball:
        raise SystemExit(f"No tarball URL in npm metadata for: {package}@{resolved}")

    return tarball


def github_tarball_url(ref: str) -> str:
    return SVGMOJI_GITHUB_TARBALL.format(ref=urllib.parse.quote(ref))


def extract_tgz(tgz_path: pathlib.Path, extract_dir: pathlib.Path) -> pathlib.Path:
    extract_dir.mkdir(parents=True, exist_ok=True)
    with tarfile.open(tgz_path, mode="r:gz") as tf:
        tf.extractall(extract_dir)  # noqa: S202

    # GitHub tarballs contain a single top-level directory.
    top_level_dirs = [p for p in extract_dir.iterdir() if p.is_dir()]
    if len(top_level_dirs) != 1:
        raise SystemExit(
            f"Unexpected tarball layout: expected 1 top-level dir, found {len(top_level_dirs)}"
        )
    return top_level_dirs[0]


def apply_aliases(*, icon_map: dict, source_dir: pathlib.Path) -> None:
    """Create friendly-name aliases like grapes.svg -> 1F347.svg.

    This is optional, but keeps nice names alongside codepoint filenames.
    """

    for name, entry in icon_map.items():
        code = entry.get("code")
        if not code:
            continue

        src = source_dir / f"{code}.svg"
        if not src.exists():
            continue

        dest = DEST_DIR / f"{name}.svg"
        shutil.copyfile(src, dest)


def slugify(value: str) -> str:
    value = (value or "").strip().lower()
    if not value:
        return ""

    out = []
    prev_us = False
    for ch in value:
        if "a" <= ch <= "z" or "0" <= ch <= "9":
            out.append(ch)
            prev_us = False
        else:
            if not prev_us:
                out.append("_")
                prev_us = True

    slug = "".join(out).strip("_")
    while "__" in slug:
        slug = slug.replace("__", "_")
    return slug


def add_alias(aliases: dict[str, str], alias: str, code: str) -> None:
    if not alias:
        return

    existing = aliases.get(alias)
    if existing is None:
        aliases[alias] = code
        return
    if existing == code:
        return

    # Collision: keep existing, add a deterministic disambiguated alias.
    disambiguated = f"{alias}__{code.lower()}"
    if disambiguated not in aliases:
        aliases[disambiguated] = code


def generate_alias_map(svgmoji_root: pathlib.Path) -> int:
    """Generate aliases.json mapping friendly names/shortcodes -> codepoint filename.

    Uses svgmoji's emoji metadata under packages/svgmoji/*.json and only emits
    aliases for codepoints that exist in DEST_DIR.
    """

    meta_dir = svgmoji_root / "packages" / "svgmoji"
    sources = [
        meta_dir / "emoji.json",
        meta_dir / "emoji-github.json",
        meta_dir / "emoji-slack.json",
        meta_dir / "emoji-discord.json",
    ]

    aliases: dict[str, str] = {}
    for src in sources:
        if not src.exists():
            continue

        data = json.loads(src.read_text(encoding="utf-8"))
        if not isinstance(data, list):
            continue

        for entry in data:
            if not isinstance(entry, dict):
                continue

            code = (entry.get("hexcode") or "").upper()
            if not code:
                continue

            if not (DEST_DIR / f"{code}.svg").exists():
                continue

            add_alias(aliases, slugify(entry.get("annotation") or ""), code)
            for sc in entry.get("shortcodes") or []:
                add_alias(aliases, slugify(sc), code)

    # Write deterministically
    ALIASES_PATH.write_text(
        json.dumps(dict(sorted(aliases.items())), indent=2, ensure_ascii=False) + "\n",
        encoding="utf-8",
    )

    return len(aliases)


def sync_all_svgs(
    *,
    source: str,
    ref: str,
    emoji_set: str,
    clean: bool,
    generate_aliases: bool,
) -> None:
    DEST_DIR.mkdir(parents=True, exist_ok=True)

    if clean:
        for p in DEST_DIR.glob("*.svg"):
            p.unlink(missing_ok=True)

    if source == "github":
        tarball_url = github_tarball_url(ref)
        print(f"Fetching GitHub tarball: {tarball_url}")
    else:
        raise SystemExit(f"Unsupported source for full sync: {source}")

    with tempfile.TemporaryDirectory(prefix="svgmoji_") as tmp:
        tmp_path = pathlib.Path(tmp)
        tgz_path = tmp_path / "pkg.tgz"
        download(tarball_url, tgz_path)

        extract_dir = tmp_path / "extract"
        root = extract_tgz(tgz_path, extract_dir)

        # Raw SVGs live in the monorepo at packages/svgmoji__{set}/svg
        set_dir = {
            "twemoji": "svgmoji__twemoji",
            "noto": "svgmoji__noto",
        }.get(emoji_set)
        if not set_dir:
            raise SystemExit(f"Unsupported emoji set: {emoji_set}")

        svg_dir = root / "packages" / set_dir / "svg"
        if not svg_dir.exists():
            raise SystemExit(
                f"Expected SVG directory not found in GitHub tarball: {svg_dir}"
            )

        svgs = sorted(svg_dir.glob("*.svg"))
        if not svgs:
            raise SystemExit(f"No SVGs found in: {svg_dir}")

        print(f"Copying {len(svgs)} SVGs into {DEST_DIR}")
        for src in svgs:
            # Keep upstream filenames (codepoints) to avoid collisions.
            shutil.copyfile(src, DEST_DIR / src.name)

        # Optional: also create friendly-name aliases (e.g. grapes.svg) from our map file.
        if DEFAULT_MAP_PATH.exists():
            icon_map = json.loads(DEFAULT_MAP_PATH.read_text(encoding="utf-8"))
            apply_aliases(icon_map=icon_map, source_dir=DEST_DIR)

        if generate_aliases:
            count = generate_alias_map(root)
            print(f"Wrote {count} aliases to {ALIASES_PATH}")


def sync_selected_svgs(*, version: str, map_path: pathlib.Path) -> None:
    if not map_path.exists():
        raise SystemExit(f"Map file not found: {map_path}")

    icon_map = json.loads(map_path.read_text(encoding="utf-8"))
    DEST_DIR.mkdir(parents=True, exist_ok=True)

    for name, entry in icon_map.items():
        emoji_set = entry.get("set", "twemoji")
        code = entry.get("code")
        if not code:
            raise SystemExit(f"Missing 'code' for icon '{name}'")

        url = (
            f"https://cdn.jsdelivr.net/npm/@svgmoji/{emoji_set}@{version}/svg/{code}.svg"
        )
        dest = DEST_DIR / f"{name}.svg"

        print(f"Downloading {name}: {url} -> {dest}")
        download(url, dest)


def main() -> int:
    parser = argparse.ArgumentParser(
        description=(
            "Sync svgmoji emoji SVGs into the emojii_symbols HA iconset. "
            "Use --mode all to fetch the full set (recommended for 'all of them')."
        ),
    )
    parser.add_argument(
        "--mode",
        choices=["all", "selected"],
        default="all",
        help="Sync mode: all (extract full package) or selected (map-based) (default: all)",
    )
    parser.add_argument(
        "--source",
        choices=["github"],
        default="github",
        help="Source for --mode all (default: github)",
    )
    parser.add_argument(
        "--ref",
        default="main",
        help="svgmoji Git ref for --mode all (default: main)",
    )
    parser.add_argument(
        "--emoji-set",
        choices=["twemoji", "noto"],
        default="twemoji",
        help="Emoji SVG set to sync in --mode all (default: twemoji)",
    )
    parser.add_argument(
        "--version",
        default="latest",
        help="svgmoji npm version tag to use, e.g. latest or 2.0.0 (default: latest)",
    )
    parser.add_argument(
        "--package",
        default="@svgmoji/twemoji",
        help="npm package to sync in --mode all (default: @svgmoji/twemoji)",
    )
    parser.add_argument(
        "--clean",
        action="store_true",
        help="Delete existing SVGs in the destination before syncing",
    )
    parser.add_argument(
        "--no-aliases",
        action="store_true",
        help="Disable generation of aliases.json in --mode all",
    )
    parser.add_argument(
        "--map",
        dest="map_path",
        default=str(DEFAULT_MAP_PATH),
        help="Path to a JSON map of icon name -> {set, code} (used in --mode selected)",
    )
    args = parser.parse_args()

    if args.mode == "all":
        sync_all_svgs(
            source=args.source,
            ref=args.ref,
            emoji_set=args.emoji_set,
            clean=args.clean,
            generate_aliases=not args.no_aliases,
        )
    else:
        sync_selected_svgs(version=args.version, map_path=pathlib.Path(args.map_path))

    print("Done.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
