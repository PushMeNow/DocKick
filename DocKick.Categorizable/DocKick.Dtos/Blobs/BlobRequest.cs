﻿using System;

namespace DocKick.Dtos.Blobs
{
    public class BlobRequest
    {
        public Guid? CategoryId { get; set; }

        public string ImageName { get; set; }
    }
}