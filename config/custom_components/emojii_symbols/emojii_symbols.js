(() => {
  "use strict";

  const iconSet = "emojii_symbols";
  const iconListCache = {};
  const iconCache = {};
  let aliasMapCache = null;
  let aliasMapPromise = null;

  let integrationReady = false;
  let readyCheckPromise = null;

  async function checkIntegrationReady(retries = 5, delay = 1000) {
    if (integrationReady) return true;
    if (readyCheckPromise) return readyCheckPromise;

    readyCheckPromise = (async () => {
      for (let i = 0; i < retries; i++) {
        try {
          const testUrl = `/${iconSet}/emo/icons.json`;
          const response = await fetch(testUrl, {
            cache: "no-cache",
            headers: { "Cache-Control": "no-cache" },
          });
          if (response.ok) {
            integrationReady = true;
            console.debug(`Emojii Symbols ready after ${i + 1} attempt(s)`);
            return true;
          }
        } catch (error) {
          console.debug(
            `Ready check attempt ${i + 1} failed:`,
            error?.message ?? error
          );
        }

        if (i < retries - 1) {
          await new Promise((resolve) =>
            setTimeout(resolve, delay * Math.pow(1.5, i))
          );
        }
      }

      console.warn(`Emojii Symbols not ready after ${retries} attempts`);
      return false;
    })();

    return readyCheckPromise;
  }

  async function fetchIconList(set) {
    if (iconListCache[set]) return iconListCache[set];

    const ready = await checkIntegrationReady();
    if (!ready) return [];

    const url = `/${iconSet}/${set}/icons.json`;
    let lastError = null;

    for (let attempt = 1; attempt <= 3; attempt++) {
      try {
        const response = await fetch(url, {
          cache: "no-cache",
          headers: { "Cache-Control": "no-cache" },
        });

        if (!response.ok) {
          throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
          throw new Error(`Unexpected data format for ${set}: expected array`);
        }

        const iconNames = data.map((icon) => icon.name).filter(Boolean);
        iconListCache[set] = iconNames;
        return iconNames;
      } catch (error) {
        lastError = error;
        if (attempt < 3) {
          await new Promise((resolve) => setTimeout(resolve, 500 * attempt));
        }
      }
    }

    console.error(`Failed to fetch icon list for ${set}:`, lastError);
    return [];
  }

  async function fetchAliasMap() {
    if (aliasMapCache) return aliasMapCache;
    if (aliasMapPromise) return aliasMapPromise;

    aliasMapPromise = (async () => {
      const ready = await checkIntegrationReady();
      if (!ready) return {};

      const url = `/${iconSet}/emo/aliases.json`;
      try {
        const response = await fetch(url, {
          cache: "no-cache",
          headers: { "Cache-Control": "no-cache" },
        });

        if (!response.ok) {
          return {};
        }

        const data = await response.json();
        if (!data || typeof data !== "object" || Array.isArray(data)) {
          return {};
        }

        aliasMapCache = data;
        return data;
      } catch (_error) {
        return {};
      } finally {
        aliasMapPromise = null;
      }
    })();

    return aliasMapPromise;
  }

  async function getCombinedIconList(set) {
    const [fileIcons, aliases] = await Promise.all([
      fetchIconList(set),
      fetchAliasMap(),
    ]);

    const out = new Set(fileIcons);
    Object.keys(aliases || {}).forEach((k) => out.add(k));
    return Array.from(out);
  }

  const iconSets = ["emo"];

  function toNumber(value, fallback = 0) {
    const n = Number.parseFloat(value);
    return Number.isFinite(n) ? n : fallback;
  }

  function circleToPath(circleEl) {
    const cx = toNumber(circleEl.getAttribute("cx"), 0);
    const cy = toNumber(circleEl.getAttribute("cy"), 0);
    const r = toNumber(circleEl.getAttribute("r"), 0);
    if (r <= 0) return null;
    // Two arcs to make a full circle.
    return `M ${cx - r} ${cy} a ${r} ${r} 0 1 0 ${2 * r} 0 a ${r} ${r} 0 1 0 ${-2 * r} 0`;
  }

  function ellipseToPath(ellipseEl) {
    const cx = toNumber(ellipseEl.getAttribute("cx"), 0);
    const cy = toNumber(ellipseEl.getAttribute("cy"), 0);
    const rx = toNumber(ellipseEl.getAttribute("rx"), 0);
    const ry = toNumber(ellipseEl.getAttribute("ry"), 0);
    if (rx <= 0 || ry <= 0) return null;
    return `M ${cx - rx} ${cy} a ${rx} ${ry} 0 1 0 ${2 * rx} 0 a ${rx} ${ry} 0 1 0 ${-2 * rx} 0`;
  }

  function rectToPath(rectEl) {
    const x = toNumber(rectEl.getAttribute("x"), 0);
    const y = toNumber(rectEl.getAttribute("y"), 0);
    const w = toNumber(rectEl.getAttribute("width"), 0);
    const h = toNumber(rectEl.getAttribute("height"), 0);
    if (w <= 0 || h <= 0) return null;

    let rx = toNumber(rectEl.getAttribute("rx"), 0);
    let ry = toNumber(rectEl.getAttribute("ry"), 0);
    if (rx > 0 && ry === 0) ry = rx;
    if (ry > 0 && rx === 0) rx = ry;
    rx = Math.min(rx, w / 2);
    ry = Math.min(ry, h / 2);

    if (rx === 0 && ry === 0) {
      return `M ${x} ${y} h ${w} v ${h} h ${-w} Z`;
    }

    // Rounded rectangle using arcs.
    const x0 = x;
    const y0 = y;
    const x1 = x + w;
    const y1 = y + h;
    return [
      `M ${x0 + rx} ${y0}`,
      `H ${x1 - rx}`,
      `A ${rx} ${ry} 0 0 1 ${x1} ${y0 + ry}`,
      `V ${y1 - ry}`,
      `A ${rx} ${ry} 0 0 1 ${x1 - rx} ${y1}`,
      `H ${x0 + rx}`,
      `A ${rx} ${ry} 0 0 1 ${x0} ${y1 - ry}`,
      `V ${y0 + ry}`,
      `A ${rx} ${ry} 0 0 1 ${x0 + rx} ${y0}`,
      "Z",
    ].join(" ");
  }

  function pointsToPath(pointsValue, closePath) {
    if (!pointsValue) return null;
    const tokens = pointsValue
      .trim()
      .replace(/\s+/g, " ")
      .split(/[ ,]/)
      .filter(Boolean);
    const nums = tokens.map((t) => Number.parseFloat(t)).filter((n) => Number.isFinite(n));
    if (nums.length < 4) return null;

    const pairs = [];
    for (let i = 0; i + 1 < nums.length; i += 2) {
      pairs.push([nums[i], nums[i + 1]]);
    }
    if (pairs.length < 2) return null;

    const [x0, y0] = pairs[0];
    const commands = [`M ${x0} ${y0}`];
    for (let i = 1; i < pairs.length; i++) {
      const [x, y] = pairs[i];
      commands.push(`L ${x} ${y}`);
    }
    if (closePath) commands.push("Z");
    return commands.join(" ");
  }

  function extractPathData(svgElement) {
    const dList = [];

    svgElement.querySelectorAll("path").forEach((el) => {
      const d = el.getAttribute("d");
      if (d) dList.push(d);
    });

    svgElement.querySelectorAll("circle").forEach((el) => {
      const d = circleToPath(el);
      if (d) dList.push(d);
    });

    svgElement.querySelectorAll("ellipse").forEach((el) => {
      const d = ellipseToPath(el);
      if (d) dList.push(d);
    });

    svgElement.querySelectorAll("rect").forEach((el) => {
      const d = rectToPath(el);
      if (d) dList.push(d);
    });

    svgElement.querySelectorAll("polygon").forEach((el) => {
      const d = pointsToPath(el.getAttribute("points"), true);
      if (d) dList.push(d);
    });

    svgElement.querySelectorAll("polyline").forEach((el) => {
      const d = pointsToPath(el.getAttribute("points"), false);
      if (d) dList.push(d);
    });

    return dList;
  }

  function sanitizeSvg(svgElement) {
    // Remove scripts
    svgElement.querySelectorAll("script").forEach((el) => el.remove());

    // Remove inline event handlers (on*) from all elements (including root)
    const all = [svgElement, ...Array.from(svgElement.querySelectorAll("*"))];
    all.forEach((el) => {
      Array.from(el.attributes).forEach((attr) => {
        if (attr.name && attr.name.toLowerCase().startsWith("on")) {
          el.removeAttribute(attr.name);
        }
      });
    });

    return svgElement;
  }

  function makeOutlineSvg(svgElement) {
    const outlined = svgElement.cloneNode(true);

    // Ensure the root behaves like an outline icon.
    outlined.setAttribute("fill", "none");
    outlined.setAttribute("stroke", "currentColor");
    outlined.setAttribute("stroke-linecap", "round");
    outlined.setAttribute("stroke-linejoin", "round");
    outlined.setAttribute("stroke-width", "2");

    // Strip per-element fills/strokes so the root styling wins.
    outlined.querySelectorAll("*").forEach((el) => {
      el.removeAttribute("fill");
      el.removeAttribute("stroke");
      el.removeAttribute("stroke-width");
      el.removeAttribute("style");
    });

    return outlined;
  }

  (async () => {
    try {
      const ready = await checkIntegrationReady();
      if (!ready) return;

      await Promise.allSettled(iconSets.map((set) => fetchIconList(set)));

      iconSets.forEach((set) => {
        window.customIcons = window.customIcons || {};
        window.customIcons[set] = {
          getIcon: async (iconName) => {
            const aliases = await fetchAliasMap();
            const resolvedName = aliases?.[iconName] || iconName;

            const key = `${set}:${resolvedName}`;
            if (iconCache[key]) return iconCache[key];

            const ready = await checkIntegrationReady();
            if (!ready) return null;

            const iconUrl = `/${iconSet}/${set}/${resolvedName}.svg`;
            let lastError = null;

            for (let attempt = 1; attempt <= 2; attempt++) {
              try {
                const response = await fetch(iconUrl, {
                  cache: "no-cache",
                  headers: { "Cache-Control": "no-cache" },
                });

                if (!response.ok) {
                  throw new Error(`HTTP ${response.status}: ${response.statusText}`);
                }

                const svgText = await response.text();
                const parser = new DOMParser();
                const svgDoc = parser.parseFromString(svgText, "image/svg+xml");

                if (svgDoc.querySelector("parsererror")) {
                  throw new Error(`SVG parsing error for ${resolvedName} in ${set}`);
                }

                const svgElement = svgDoc.querySelector("svg");
                if (!svgElement) {
                  throw new Error(`No SVG element found for ${resolvedName} in ${set}`);
                }

                const sanitizedSvg = sanitizeSvg(svgElement);
                const paths = extractPathData(sanitizedSvg);
                if (paths.length === 0) {
                  throw new Error(`No valid paths found in SVG for ${resolvedName} in ${set}`);
                }

                const outlineSvg = makeOutlineSvg(sanitizedSvg);

                const iconData = {
                  viewBox: sanitizedSvg.getAttribute("viewBox") || "0 0 24 24",
                  // Fallback: monochrome rendering
                  path: paths.join(" "),
                  // Prefer outline (unfilled) rendering by default.
                  format: "outline",
                  innerSVG: outlineSvg,
                };

                iconCache[key] = iconData;
                return iconData;
              } catch (error) {
                lastError = error;
                if (attempt < 2) {
                  await new Promise((resolve) => setTimeout(resolve, 200));
                }
              }
            }

            console.error(`Failed to fetch icon ${resolvedName} from ${set}:`, lastError);
            return null;
          },
          getIconList: async () => {
            const iconNames = await getCombinedIconList(set);
            return iconNames;
          },
        };
      });

      console.debug("Emojii Symbols: Custom icons defined");
    } catch (error) {
      console.error("Emojii Symbols: Initialisation failed:", error);
    }
  })();

  // Full-color rendering support: inject the original SVG into ha-icon when available.
  customElements.whenDefined("ha-icon").then((HaIcon) => {
    const orig = HaIcon.prototype._setCustomPath;
    HaIcon.prototype._setCustomPath = async function (iconPromise, icon) {
      await orig?.bind(this)?.(iconPromise, icon);

      let iconData;
      try {
        iconData = await iconPromise;
      } catch (_e) {
        return;
      }

      if (icon !== this.icon) return;
      if (!iconData?.innerSVG) return;

      // Wait for shadow DOM to be ready.
      await (this.updateComplete ?? this.UpdateComplete);
      const haSvgIcon = this.shadowRoot?.querySelector("ha-svg-icon");
      await haSvgIcon?.updateComplete;

      // Clear path-based rendering and append full SVG.
      this._path = undefined;
      this._secondaryPath = undefined;

      const svg = haSvgIcon?.shadowRoot?.querySelector("svg");
      if (!svg) return;

      // Avoid stacking multiple injected SVGs on repeated renders.
      while (svg.firstChild) svg.removeChild(svg.firstChild);
      svg.appendChild(iconData.innerSVG.cloneNode(true));
    };
  });

  console.info("Emojii Symbols iconset loaded");
})();
