using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;

namespace pokemon_game.Managers;

public class TransitionManager
{
    private List<TransitionZone> _transitions = new List<TransitionZone>();

    private class TransitionZone
    {
        public Rectangle Bounds { get; set; }
        public string Target { get; set; }
    }

    public void LoadTransitions(TiledMap tiledMap)
    {
        _transitions.Clear();

        var transitionLayer = tiledMap.GetLayer<TiledMapObjectLayer>("Transition");
        if (transitionLayer == null)
            return;

        foreach (var obj in transitionLayer.Objects)
        {
            string target = obj.Properties.ContainsKey("target")
                ? obj.Properties["target"].ToString()
                : null;

            if (!string.IsNullOrEmpty(target))
            {
                var zone = new TransitionZone
                {
                    Bounds = new Rectangle(
                        (int)obj.Position.X,
                        (int)obj.Position.Y,
                        (int)obj.Size.Width,
                        (int)obj.Size.Height
                    ),
                    Target = target,
                };

                _transitions.Add(zone);
            }
        }
    }

    public string CheckTransition(Rectangle playerBounds)
    {
        foreach (var transition in _transitions)
        {
            if (transition.Bounds.Intersects(playerBounds))
            {
                return transition.Target;
            }
        }

        return null;
    }
}
