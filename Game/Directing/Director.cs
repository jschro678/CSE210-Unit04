using System.Collections.Generic;
using Unit04.Game.Casting;
using Unit04.Game.Services;

namespace Unit04.Game.Directing
{
    /// <summary>
    /// <para>A person who directs the game.</para>
    /// <para>
    /// The responsibility of a Director is to control the sequence of play.
    /// </para>
    /// </summary>
    public class Director
    {
        private KeyboardService keyboardService = null;
        private VideoService videoService = null;
        private int cellSize = 15;
        private int score = 0;
        private int points;

        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService)
        {
            this.keyboardService = keyboardService;
            this.videoService = videoService;
        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame(Cast cast)
        {
            videoService.OpenWindow();
            while (videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast);
                DoOutputs(cast);
            }
            videoService.CloseWindow();
        }

        /// <summary>
        /// Gets directional input from the keyboard and applies it to the robot.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void GetInputs(Cast cast)
        {
            Actor robot = cast.GetFirstActor("robot");
            Point velocity = keyboardService.GetDirection();
            robot.SetVelocity(velocity);
        }

        /// <summary>
        /// Updates the robot's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            Actor banner = cast.GetFirstActor("banner");
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> gems = cast.GetActors("gem");
            List<Actor> rocks = cast.GetActors("rock");

            banner.SetText($"Score: {score}");
            int maxX = videoService.GetWidth();
            int maxY = videoService.GetHeight();
            robot.MoveNext(maxX, maxY);
            Random random = new Random();
            foreach (Actor actor in rocks)
            {
                int y = random.Next(0, 3);
                Point direction = new Point(0, y);
                direction = direction.Scale(cellSize);
                actor.SetVelocity(direction);
                actor.MoveNext(maxX, maxY);
                if (robot.GetPosition().Equals(actor.GetPosition()))
                {
                    FallingObject rock = (FallingObject)actor;
                    points = rock.getPoint();
                    score = points + score;
                    banner.SetText($"{score}");
                }
            }
            foreach (Actor actor in gems)
            {
                int y = random.Next(0, 3);
                Point direction = new Point(0, y);
                direction = direction.Scale(cellSize);
                actor.SetVelocity(direction);
                actor.MoveNext(maxX, maxY);
                if (robot.GetPosition().Equals(actor.GetPosition()))
                {
                    FallingObject gem = (FallingObject)actor;
                    points = gem.getPoint();
                    score = points + score;
                    banner.SetText($"{score}");
                }
            }
        }

        /// <summary>
        /// Draws the actors on the screen.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void DoOutputs(Cast cast)
        {
            List<Actor> actors = cast.GetAllActors();
            videoService.ClearBuffer();
            videoService.DrawActors(actors);
            videoService.FlushBuffer();
        }
    }
}
