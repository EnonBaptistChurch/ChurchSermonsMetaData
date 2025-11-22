using System;
using System.Collections.Generic;
using System.Text;

namespace Transcriber.Models
{
    public record WhisperTranscribeSection
    {
        public TimeSpan Start { get; init; }
        public TimeSpan End { get; init; }
        public string Text { get; init; } = string.Empty;

    }
}
