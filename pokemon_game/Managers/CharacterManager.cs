using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using pokemon_game.Entities;

namespace pokemon_game.Managers;

public class CharacterManager
{
    private readonly List<NPC> _npcs = new List<NPC>();
    private ContentManager _content;
    private CollisionManager _collisionManager;
    private NPC _activeNPC;
    private bool _isPlayerBlocked;
    private List<string> _currentDialog;
    private int _currentDialogIndex;
    private bool _dialogComplete;
    private static SoundEffect _noticeSound;
    private static SoundEffectInstance _overworldMusic;

    public static void SetNoticeSound(SoundEffect sound)
    {
        _noticeSound = sound;
    }

    public static void SetOverworldMusic(SoundEffectInstance music)
    {
        _overworldMusic = music;
    }

    public bool IsPlayerBlocked => _isPlayerBlocked;
    public NPC ActiveNPC => _activeNPC;

    public Vector2 GetActiveNPCPosition()
    {
        return _activeNPC?.Position ?? Vector2.Zero;
    }

    public void LoadCharacters(
        TiledMap tiledMap,
        ContentManager content,
        CollisionManager collisionManager
    )
    {
        _content = content;
        _collisionManager = collisionManager;
        _npcs.Clear();

        var entitiesLayer = tiledMap.GetLayer<TiledMapObjectLayer>("Entities");
        if (entitiesLayer == null)
            return;

        foreach (var obj in entitiesLayer.Objects)
        {
            // Only process Character objects, not Player objects
            if (obj.Name == "Character")
            {
                // Get properties
                string graphicName = obj.Properties.ContainsKey("graphic")
                    ? obj.Properties["graphic"].ToString()
                    : "player";

                string direction = obj.Properties.ContainsKey("direction")
                    ? obj.Properties["direction"].ToString()
                    : "down";

                string characterId = obj.Properties.ContainsKey("character_id")
                    ? obj.Properties["character_id"].ToString()
                    : "";

                float radius = 400f; // Default radius
                if (obj.Properties.ContainsKey("radius"))
                {
                    if (float.TryParse(obj.Properties["radius"].ToString(), out float parsedRadius))
                    {
                        radius = parsedRadius;
                    }
                }

                // Load the character texture
                Texture2D texture;
                try
                {
                    texture = content.Load<Texture2D>($"graphics/characters/{graphicName}");
                }
                catch
                {
                    // Fallback to player texture if character not found
                    texture = content.Load<Texture2D>("graphics/characters/player");
                }

                // Create NPC at the object's position
                var position = new Vector2(obj.Position.X, obj.Position.Y);
                var npc = new NPC(texture, position, direction, characterId, radius);

                _npcs.Add(npc);
            }
        }
    }

    public void Update(GameTime gameTime, Vector2 playerPosition)
    {
        // Update all NPCs
        foreach (var npc in _npcs)
        {
            npc.Update(gameTime);
        }

        // Check for NPC triggering if not already in interaction
        if (_activeNPC == null)
        {
            foreach (var npc in _npcs)
            {
                if (npc.CheckPlayerInView(playerPosition, _collisionManager))
                {
                    // Trigger this NPC
                    _activeNPC = npc;
                    _activeNPC.StartApproaching(playerPosition);
                    _isPlayerBlocked = true;

                    // Play notice sound and stop overworld music
                    _noticeSound?.Play();
                    _overworldMusic?.Pause();

                    break;
                }
            }
        }

        // Check if active NPC reached player
        if (_activeNPC != null && _activeNPC.IsApproaching && _activeNPC.HasReachedPlayer())
        {
            _activeNPC.TriggerDialog();
            _currentDialog = _activeNPC.GetDialog();
            _currentDialogIndex = 0;
            _dialogComplete = false;
        }
    }

    public void AdvanceDialog()
    {
        if (_currentDialog == null || _activeNPC == null)
            return;

        _currentDialogIndex++;

        if (_currentDialogIndex >= _currentDialog.Count)
        {
            // Dialog finished, mark as complete to trigger battle
            _currentDialog = null;
            _currentDialogIndex = 0;
            _dialogComplete = true;
            // Keep _isPlayerBlocked true and _activeNPC set for battle transition
            // Note: Don't resume music here - it will either go to battle or resume in EndDialog
        }
    }

    public string GetCurrentDialogLine()
    {
        if (
            _currentDialog == null
            || _currentDialogIndex < 0
            || _currentDialogIndex >= _currentDialog.Count
        )
            return null;

        return _currentDialog[_currentDialogIndex];
    }

    public bool HasActiveDialog()
    {
        return _currentDialog != null && _currentDialogIndex < _currentDialog.Count;
    }

    public bool ShouldStartBattle()
    {
        // Start battle after dialog completes, but only if NPC can battle
        // NPCs with null biome (like Nurses) don't battle
        return _activeNPC != null
            && _dialogComplete
            && _activeNPC.TrainerData != null
            && _activeNPC.TrainerData.CanBattle;
    }

    public void EndBattle()
    {
        // End battle and mark NPC as defeated
        if (_activeNPC != null)
        {
            _activeNPC.MarkAsDefeated();
            _activeNPC = null;
        }
        _currentDialog = null;
        _currentDialogIndex = 0;
        _dialogComplete = false;
        _isPlayerBlocked = false;
    }

    public void EndDialog()
    {
        // End dialog for non-battle NPCs (like Nurse)
        _activeNPC = null;
        _currentDialog = null;
        _currentDialogIndex = 0;
        _dialogComplete = false;
        _isPlayerBlocked = false;
        // Resume overworld music after dialog
        _overworldMusic?.Resume();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var npc in _npcs)
        {
            npc.Draw(spriteBatch);
        }
    }
}
