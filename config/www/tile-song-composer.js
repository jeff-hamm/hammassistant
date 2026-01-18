/**
 * Tile Song Composer Card for Home Assistant
 * 
 * A custom Lovelace card that provides a visual interface for composing
 * songs to program to Tile devices.
 */

class TileSongComposerCard extends HTMLElement {
  // Notes and their display
  static NOTES = [
    'C4', 'C#4', 'D4', 'D#4', 'E4', 'F4', 'F#4', 'G4', 'G#4', 'A4', 'A#4', 'B4',
    'C5', 'C#5', 'D5', 'D#5', 'E5', 'F5', 'F#5', 'G5', 'G#5', 'A5', 'A#5', 'B5',
    'C6', 'C#6', 'D6', 'D#6', 'E6', 'F6', 'F#6', 'G6',
  ];

  static DURATIONS = [
    { value: '1/32', label: '1/32', symbol: '𝅘𝅥𝅱' },
    { value: '1/16', label: '1/16', symbol: '𝅘𝅥𝅰' },
    { value: '1/8', label: '1/8', symbol: '♪' },
    { value: 'dotted 1/8', label: '1/8.', symbol: '♪·' },
    { value: '1/4', label: '1/4', symbol: '♩' },
    { value: 'dotted 1/4', label: '1/4.', symbol: '♩·' },
    { value: '1/2', label: '1/2', symbol: '𝅗𝅥' },
    { value: '3/4', label: '3/4', symbol: '𝅗𝅥·' },
    { value: 'whole', label: 'whole', symbol: '𝅝' },
  ];

  static PRESETS = [
    { value: 'simple_scale', label: 'C Major Scale' },
    { value: 'doorbell', label: 'Doorbell' },
    { value: 'alert_beeps', label: 'Alert Beeps' },
    { value: 'happy_tune', label: 'Happy Tune' },
    { value: 'twinkle_twinkle', label: 'Twinkle Twinkle' },
    { value: 'mario_coin', label: 'Mario Coin' },
  ];

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.notes = [];
    this.selectedDuration = '1/8';
    this.tileId = '';
  }

  set hass(hass) {
    this._hass = hass;
    if (!this.shadowRoot.innerHTML) {
      this.render();
    }
    this.updateTileSelector();
  }

  setConfig(config) {
    this._config = config;
    this.tileId = config.tile_id || '';
  }

  getCardSize() {
    return 5;
  }

  render() {
    const style = `
      <style>
        :host {
          display: block;
        }
        .card {
          background: var(--ha-card-background, var(--card-background-color, #fff));
          border-radius: var(--ha-card-border-radius, 4px);
          box-shadow: var(--ha-card-box-shadow, 0 2px 2px rgba(0,0,0,0.14));
          padding: 16px;
        }
        .header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 16px;
        }
        .title {
          font-size: 1.5em;
          font-weight: 500;
          color: var(--primary-text-color);
        }
        .controls {
          display: flex;
          gap: 8px;
          flex-wrap: wrap;
          margin-bottom: 16px;
        }
        .piano-wrapper {
          overflow-x: auto;
          margin-bottom: 16px;
        }
        .piano {
          display: flex;
          height: 120px;
          min-width: 600px;
          position: relative;
        }
        .white-key {
          width: 30px;
          height: 100%;
          background: linear-gradient(to bottom, #fff 0%, #f0f0f0 100%);
          border: 1px solid #ccc;
          border-radius: 0 0 4px 4px;
          cursor: pointer;
          display: flex;
          align-items: flex-end;
          justify-content: center;
          padding-bottom: 4px;
          font-size: 10px;
          color: #666;
          user-select: none;
          transition: background 0.1s;
        }
        .white-key:hover {
          background: linear-gradient(to bottom, #e8f4fc 0%, #d0e8f0 100%);
        }
        .white-key:active, .white-key.pressed {
          background: linear-gradient(to bottom, #4fc3f7 0%, #29b6f6 100%);
        }
        .black-key {
          width: 20px;
          height: 65%;
          background: linear-gradient(to bottom, #333 0%, #000 100%);
          border-radius: 0 0 3px 3px;
          position: absolute;
          z-index: 1;
          cursor: pointer;
          display: flex;
          align-items: flex-end;
          justify-content: center;
          padding-bottom: 4px;
          font-size: 8px;
          color: #999;
          user-select: none;
          transition: background 0.1s;
        }
        .black-key:hover {
          background: linear-gradient(to bottom, #444 0%, #222 100%);
        }
        .black-key:active, .black-key.pressed {
          background: linear-gradient(to bottom, #0277bd 0%, #01579b 100%);
        }
        .duration-selector {
          display: flex;
          gap: 4px;
          flex-wrap: wrap;
        }
        .duration-btn {
          padding: 8px 12px;
          border: 2px solid var(--divider-color, #ccc);
          border-radius: 4px;
          background: var(--secondary-background-color, #f5f5f5);
          color: var(--primary-text-color);
          cursor: pointer;
          font-size: 16px;
          transition: all 0.2s;
        }
        .duration-btn:hover {
          border-color: var(--primary-color);
        }
        .duration-btn.selected {
          background: var(--primary-color);
          color: var(--text-primary-color, #fff);
          border-color: var(--primary-color);
        }
        .notation-display {
          background: var(--secondary-background-color, #f5f5f5);
          border-radius: 4px;
          padding: 12px;
          min-height: 60px;
          margin-bottom: 16px;
          font-family: monospace;
          font-size: 14px;
          overflow-x: auto;
          white-space: pre-wrap;
          word-break: break-word;
        }
        .note-chip {
          display: inline-block;
          background: var(--primary-color);
          color: var(--text-primary-color, #fff);
          padding: 4px 8px;
          border-radius: 12px;
          margin: 2px;
          font-size: 12px;
          cursor: pointer;
        }
        .note-chip.rest {
          background: var(--warning-color, #ffc107);
          color: #000;
        }
        .note-chip:hover {
          opacity: 0.8;
        }
        .action-buttons {
          display: flex;
          gap: 8px;
          flex-wrap: wrap;
        }
        .btn {
          padding: 10px 20px;
          border: none;
          border-radius: 4px;
          cursor: pointer;
          font-size: 14px;
          font-weight: 500;
          transition: opacity 0.2s;
        }
        .btn:hover {
          opacity: 0.9;
        }
        .btn:disabled {
          opacity: 0.5;
          cursor: not-allowed;
        }
        .btn-primary {
          background: var(--primary-color);
          color: var(--text-primary-color, #fff);
        }
        .btn-danger {
          background: var(--error-color, #f44336);
          color: #fff;
        }
        .btn-secondary {
          background: var(--secondary-background-color, #e0e0e0);
          color: var(--primary-text-color);
        }
        .btn-rest {
          background: var(--warning-color, #ffc107);
          color: #000;
        }
        .tile-selector {
          margin-bottom: 16px;
        }
        .tile-selector select {
          width: 100%;
          padding: 10px;
          border: 1px solid var(--divider-color, #ccc);
          border-radius: 4px;
          background: var(--secondary-background-color, #f5f5f5);
          color: var(--primary-text-color);
          font-size: 14px;
        }
        .preset-selector {
          margin-bottom: 16px;
        }
        .preset-selector select {
          padding: 10px;
          border: 1px solid var(--divider-color, #ccc);
          border-radius: 4px;
          background: var(--secondary-background-color, #f5f5f5);
          color: var(--primary-text-color);
          font-size: 14px;
        }
        .section-label {
          font-weight: 500;
          margin-bottom: 8px;
          color: var(--secondary-text-color);
          font-size: 12px;
          text-transform: uppercase;
        }
        .empty-state {
          color: var(--secondary-text-color);
          font-style: italic;
        }
        .stats {
          font-size: 12px;
          color: var(--secondary-text-color);
          margin-top: 8px;
        }
      </style>
    `;

    this.shadowRoot.innerHTML = `
      ${style}
      <div class="card">
        <div class="header">
          <span class="title">🎵 Tile Song Composer</span>
        </div>

        <div class="tile-selector">
          <div class="section-label">Select Tile</div>
          <select id="tile-select">
            <option value="">-- Select a Tile --</option>
          </select>
        </div>

        <div class="preset-selector">
          <div class="section-label">Load Preset</div>
          <select id="preset-select">
            <option value="">-- Load a preset song --</option>
            ${TileSongComposerCard.PRESETS.map(p => 
              `<option value="${p.value}">${p.label}</option>`
            ).join('')}
          </select>
        </div>

        <div class="section-label">Duration</div>
        <div class="duration-selector">
          ${TileSongComposerCard.DURATIONS.map(d => 
            `<button class="duration-btn ${d.value === '1/8' ? 'selected' : ''}" 
                     data-duration="${d.value}" 
                     title="${d.label}">
              ${d.symbol}
            </button>`
          ).join('')}
          <button class="btn btn-rest" id="add-rest">Rest</button>
        </div>

        <div class="section-label" style="margin-top: 16px;">Piano (click to add notes)</div>
        <div class="piano-wrapper">
          <div class="piano" id="piano"></div>
        </div>

        <div class="section-label">Composition</div>
        <div class="notation-display" id="notation">
          <span class="empty-state">Click piano keys to add notes...</span>
        </div>
        <div class="stats" id="stats"></div>

        <div class="action-buttons">
          <button class="btn btn-danger" id="clear-btn">Clear All</button>
          <button class="btn btn-secondary" id="undo-btn" disabled>Undo</button>
          <button class="btn btn-primary" id="program-btn" disabled>Program to Tile</button>
        </div>
      </div>
    `;

    this.renderPiano();
    this.attachEventListeners();
  }

  renderPiano() {
    const piano = this.shadowRoot.getElementById('piano');
    if (!piano) return;

    let html = '';
    let whiteKeyIndex = 0;

    TileSongComposerCard.NOTES.forEach((note, i) => {
      const isBlack = note.includes('#');
      
      if (!isBlack) {
        html += `<div class="white-key" data-note="${note}">${note}</div>`;
        whiteKeyIndex++;
      }
    });

    piano.innerHTML = html;

    // Add black keys with absolute positioning
    let whiteKeyCount = 0;
    TileSongComposerCard.NOTES.forEach((note, i) => {
      const isBlack = note.includes('#');
      
      if (isBlack) {
        const leftPos = (whiteKeyCount * 30) - 10; // Position between white keys
        const blackKey = document.createElement('div');
        blackKey.className = 'black-key';
        blackKey.dataset.note = note;
        blackKey.textContent = note.replace('#', '♯');
        blackKey.style.left = `${leftPos}px`;
        piano.appendChild(blackKey);
      } else {
        whiteKeyCount++;
      }
    });
  }

  attachEventListeners() {
    // Piano keys
    const piano = this.shadowRoot.getElementById('piano');
    piano.addEventListener('click', (e) => {
      const key = e.target.closest('[data-note]');
      if (key) {
        this.addNote(key.dataset.note, this.selectedDuration);
        key.classList.add('pressed');
        setTimeout(() => key.classList.remove('pressed'), 200);
      }
    });

    // Duration selector
    this.shadowRoot.querySelectorAll('.duration-btn').forEach(btn => {
      btn.addEventListener('click', () => {
        this.shadowRoot.querySelectorAll('.duration-btn').forEach(b => 
          b.classList.remove('selected'));
        btn.classList.add('selected');
        this.selectedDuration = btn.dataset.duration;
      });
    });

    // Add rest
    this.shadowRoot.getElementById('add-rest').addEventListener('click', () => {
      this.addNote('Rest', this.selectedDuration);
    });

    // Clear
    this.shadowRoot.getElementById('clear-btn').addEventListener('click', () => {
      this.notes = [];
      this.updateNotation();
    });

    // Undo
    this.shadowRoot.getElementById('undo-btn').addEventListener('click', () => {
      this.notes.pop();
      this.updateNotation();
    });

    // Program
    this.shadowRoot.getElementById('program-btn').addEventListener('click', () => {
      this.programSong();
    });

    // Tile selector
    this.shadowRoot.getElementById('tile-select').addEventListener('change', (e) => {
      this.tileId = e.target.value;
      this.updateButtons();
    });

    // Preset selector
    this.shadowRoot.getElementById('preset-select').addEventListener('change', (e) => {
      if (e.target.value) {
        this.loadPreset(e.target.value);
        e.target.value = '';
      }
    });

    // Click to remove notes
    this.shadowRoot.getElementById('notation').addEventListener('click', (e) => {
      const chip = e.target.closest('.note-chip');
      if (chip && chip.dataset.index !== undefined) {
        this.notes.splice(parseInt(chip.dataset.index), 1);
        this.updateNotation();
      }
    });
  }

  addNote(note, duration) {
    this.notes.push({ note, duration });
    this.updateNotation();
  }

  updateNotation() {
    const container = this.shadowRoot.getElementById('notation');
    const undoBtn = this.shadowRoot.getElementById('undo-btn');
    const statsEl = this.shadowRoot.getElementById('stats');
    
    if (this.notes.length === 0) {
      container.innerHTML = '<span class="empty-state">Click piano keys to add notes...</span>';
      undoBtn.disabled = true;
    } else {
      container.innerHTML = this.notes.map((n, i) => 
        `<span class="note-chip ${n.note === 'Rest' ? 'rest' : ''}" 
               data-index="${i}" 
               title="Click to remove">
          ${n.note === 'Rest' ? '🔇' : n.note}:${n.duration}
        </span>`
      ).join('');
      undoBtn.disabled = false;
    }

    statsEl.textContent = `${this.notes.length} notes`;
    this.updateButtons();
  }

  updateButtons() {
    const programBtn = this.shadowRoot.getElementById('program-btn');
    programBtn.disabled = this.notes.length === 0 || !this.tileId;
  }

  updateTileSelector() {
    if (!this._hass) return;
    
    const select = this.shadowRoot.getElementById('tile-select');
    if (!select) return;

    // Find all tile_tracker device trackers
    const tiles = Object.entries(this._hass.states)
      .filter(([entityId, state]) => 
        entityId.startsWith('device_tracker.tile_') ||
        (state.attributes && state.attributes.tile_uuid))
      .map(([entityId, state]) => ({
        id: state.attributes.tile_uuid || entityId,
        name: state.attributes.friendly_name || entityId,
      }));

    const currentValue = select.value;
    select.innerHTML = `
      <option value="">-- Select a Tile --</option>
      ${tiles.map(t => `<option value="${t.id}">${t.name}</option>`).join('')}
    `;
    
    if (this.tileId) {
      select.value = this.tileId;
    } else if (currentValue) {
      select.value = currentValue;
    }
  }

  toNotation() {
    return this.notes.map(n => 
      n.note === 'Rest' ? `R:${n.duration}` : `${n.note}:${n.duration}`
    ).join(' | ');
  }

  loadPreset(preset) {
    // Program the preset directly
    if (!this.tileId) {
      alert('Please select a Tile first');
      return;
    }

    this._hass.callService('tile_tracker', 'play_preset_song', {
      tile_id: this.tileId,
      preset: preset,
    });
  }

  programSong() {
    if (!this.tileId || this.notes.length === 0) return;

    const notation = this.toNotation();
    
    this._hass.callService('tile_tracker', 'compose_song', {
      tile_id: this.tileId,
      notation: notation,
      song_name: 'Custom Composition',
    });
  }
}

customElements.define('tile-song-composer', TileSongComposerCard);

// Register with HACS / custom cards
window.customCards = window.customCards || [];
window.customCards.push({
  type: 'tile-song-composer',
  name: 'Tile Song Composer',
  description: 'A visual piano interface for composing songs to program to Tile devices',
  preview: true,
});

console.info('%c TILE-SONG-COMPOSER %c loaded ', 
  'background: #3f51b5; color: white; font-weight: bold;',
  'background: #8bc34a; color: white; font-weight: bold;');
