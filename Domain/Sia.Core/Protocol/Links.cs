﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Core.Protocol
{
    public class LinksForSerialization
    {
        public LinksMetadata Metadata { get; set; }
        public LinksCollection Links { get; set; }
    }

    public class LinksMetadata
    {
        public PaginationMetadataValues Pagination { get; set; }
    }

    public class PaginationMetadataValues
    {
        public string PageNumber { get; set; }
        public string PageSize { get; set; }
        public string TotalRecords { get; set; }
        public string TotalPages { get; set; }
    }

    public class LinksCollection
    {
        public OperationLinks Operations { get; set; }
        public PaginationLinks Pagination { get; set; }
        public RelationLinks Related { get; set; }
    }

    public class OperationLinks
    {
#pragma warning disable CA1720 // Identifier contains type name
        public SingleOperationLinks Single { get; set; }
#pragma warning restore CA1720 // Identifier contains type name
        public MultipleOperationLinks Multiple { get; set; }
    }

    public class SingleOperationLinks
    {
        public string Get { get; set; }
        public string Post { get; set; }
        public string Put { get; set; }
    }

    public class MultipleOperationLinks
    {
        public string Get { get; set; }
    }

    public class PaginationLinks
    {
        public string Next { get; set; }
        public string Previous { get; set; }
    }

    public class RelationLinks
    {
        public object Children { get; set; }
        public object Parent { get; set; }
    }
}
