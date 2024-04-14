using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageProcessor;

public record BlobMessage(string OriginalImageBlob, string ResizedImageBlob);