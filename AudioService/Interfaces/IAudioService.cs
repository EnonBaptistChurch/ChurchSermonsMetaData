using AudioService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudioService.Interfaces
{
    public interface IAudioService
    {
        /// <summary>
        /// Process an audio file: convert/trim as needed.
        /// Returns the path to the processed audio file.
        /// </summary>
        Task<string> ProcessAsync(string inputPath, AudioProcessingOptions options, CancellationToken ct = default);
    }
}
