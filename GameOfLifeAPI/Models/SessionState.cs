using System.ComponentModel;
using System.Text.Json.Serialization;

namespace GameOfLifeAPI.Models
{
    public class SessionState
    {

        [Browsable(false)]
        public Guid BoardId { get; } = Guid.NewGuid(); 

        [JsonPropertyName("boardName")]
        public string BoardName { get; set; } = string.Empty;

        [JsonPropertyName("boardHeight")]
        public int BoardHeight { get; set; } = 800;

        [JsonPropertyName("boardWidth")]
        public int BoardWidth { get; set; } = 800;

        [JsonPropertyName("boardResolution")]
        public int BoardResolution { get; set; } = 10;

        [JsonPropertyName("showGrid")]
        public bool ShowGrid { get; set; } = false;
    }
}